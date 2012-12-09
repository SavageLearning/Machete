import collections
import ConfigParser
import smtplib
import string
from datetime import datetime

import MySQLdb
import requests

DEBUG = True

if DEBUG:
    import pprint

NOT_A_FIELD = -1000

fields = {
    NOT_A_FIELD: 'sid',
    2: 'business',
    3: 'name',
    4: 'address1',
    5: 'address2',
    6: 'city',
    7: 'state',
    8: 'zipcode',
    9: 'phone',
    10: 'cellphone',
    11: 'email',
    12: 'referredBy',
    13: 'referredbyOther',
    14: 'blogparticipate',
    15: 'returnCustomer',
    16: 'notes',
    18: 'contactName',
    19: 'workSiteAddress1',
    20: 'workSiteAddress2',
    21: 'wo_city',
    22: 'wo_state',
    23: 'wo_zipcode',
    24: 'wo_phone',
    25: 'typeOfWorkID',
    26: 'englishRequired',
    27: 'englishRequiredNote',
    28: 'lunchSupplied',
    29: 'description',
    30: 'date_needed',
    31: 'time_needed',
    32: 'timeFlexible',
    33: 'transportMethodID',
    43: 'receiveUpdates'
}

field_names = list(fields[key] for key in sorted(fields))

# Get settings
try:
    _settings = ConfigParser.ConfigParser()
    _settings.read('employer_form.ini')
except ConfigParser.Error as err:
    raise

db_config = dict(_settings.items('db'))
machete_config = dict(_settings.items('machete'))
smtp_config = dict(_settings.items('smtp'))
webform_config = dict(_settings.items('webform'))

# Open connection to Database
db = MySQLdb.connect(db_config['host'],
                     db_config['user'],
                     db_config['pw'],
                     db_config['db_name'])
cursor = db.cursor()

# only grab webform entries without success = true in webform_machete table
cursor.execute("SELECT * from \
    webform_submitted_data LEFT OUTER JOIN \
    webform_machete ON webform_machete.sid = webform_submitted_data.sid \
    WHERE (webform_machete.success <> 1 OR webform_machete.success IS NULL) \
    AND webform_submitted_data.nid=%s", webform_config['id'])

form_data = cursor.fetchall()

entry_count = len(form_data) / (len(field_names) -1)
print "=================> entry_count: ", entry_count

if DEBUG:
    print "------------------- form_data: ", pprint.pformat(form_data,
                                                            indent=4)

def cluster_form_submission_data(data):
    '''group form fields by their sid (submission id)'''
    form_submission_data = collections.defaultdict(list)
    for field in data:
        form_submission_data[field[1]].append(field[2:])
    if DEBUG:
        print "----------- form_submission_data: ", pprint.pformat(
            form_submission_data, indent=4)
    return form_submission_data


def convert_single_form_submission(sid, form_data):
    '''convert a single submission to the format we need'''
    return dict(zip(field_names, [sid, ] + [x[2] for x in sorted(form_data)]))


all_submissions = [convert_single_form_submission(sid, x) for sid, x in
                   cluster_form_submission_data(form_data).items()]

if DEBUG:
    print "------------------- all_submissions: ", pprint.pformat(
        all_submissions, indent=4)


# Logging
log_file = open('employer_combined.log', 'a')
now = datetime.now()

def log_entry(entry):
    try:
        log_file.write(now.strftime("%Y-%m-%d %I:%M%p :: ") + entry + "\n")
    except:
        mail('Machete log error', 'Not able to write to log file.')


# Send e-mail
def mail(subject='error', message='Error occurred'):
    mailServer = smtplib.SMTP(smtp_config['server'], smtp_config['port'])
    mailServer.ehlo()
    mailServer.starttls()
    mailServer.ehlo()
    mailServer.login(smtp_config['user'], smtp_config['pw'])

    body = string.join((
        "From: %s" % smtp_config['user'],
        "To: %s" % smtp_config['to'],
        "Subject: %s" % subject,
        "",
        message
    ), "\r\n")
    server = smtplib.SMTP('localhost')
    server.sendmail(smtp_config['user'], smtp_config['to'], body)
    server.quit()

if entry_count > 0:
    log_entry("Script started")
    log_entry("Entry Count: %s" % entry_count)

# login to machete
s = requests.session()
s.config['keep_alive'] = True
try:
    login_response = s.post(url=machete_config['base_url'] + '/Account/Logon',
                        data={'UserName': machete_config['user'],
                        'Password': machete_config['pw']}, verify=False,
                        cert=(machete_config['cert'], machete_config['key']))
except:
    print "!-------------- machete connection rejected"
    mail("Machete connection rejected", "Exception raised while trying to connect to machete")
    log_entry("Machete connection rejected")
    exit()

if ('Login was unsuccessful' in login_response.text):
    print "!-------------- login failed"
    log_entry("Failed to login to Machete")
    mail("Machete login failed", "Script could not login to machete")
    exit()

# Process submissions
for send_data in all_submissions:
    send_data['ID'] = '0'
    dt_str = send_data['date_needed'] + " " + send_data['time_needed']
    dt_obj = datetime.strptime(dt_str, '%Y-%m-%d %H:%M:%S')
    send_data['dateTimeofWork'] = dt_obj.strftime("%m-%d-%Y %I:%M:%S %p")

    try:
        post_response = s.post(
            url=machete_config['base_url'] + '/Employer/CreateCombined',
                data=send_data, verify=False, 
                cert=(machete_config['cert'], machete_config['key']))
    except:
        print "!-------------- Update failed (MAIL SEND)"
        log_entry("Failed post to Machete")
        mail("Machete post failed",
            "Posting the form to the Machete URL failed")

    print "-------------- post_response: ", post_response.text

    cursor.execute("SELECT sid FROM webform_machete WHERE sid='%s'",
                   send_data['sid'])
    existing_entry = cursor.rowcount

    print "send_data[sid]:", send_data['sid']
    print "existing_entry: ", existing_entry
    if (not 'an error occurred' in post_response.text and
            post_response.json['jobSuccess']):
        print "JobSuccess!"
        if (existing_entry > 0):
            try:
                cursor.execute("UPDATE webform_machete SET success='1', \
                tries=tries+1, last_attempt=NOW() WHERE sid='%s'",
                               (send_data['sid'],))
                db.commit()
            except:
                print "!-------------- Update webform_machete table failed"
                log_entry("Failed to update webform_machete table")
                mail("update DB failed", "Updating webform_machete \
                                                     table failed")
        else:
            try:
                cursor.execute("INSERT INTO webform_machete (sid, success, \
                tries, last_attempt) VALUES('%s', '%s', 1, NOW())",
                               (send_data['sid'],
                                post_response.json['jobSuccess']))
                db.commit()
            except:
                print "!-------------- Insert into webform_machete table failed"
                log_entry("Failed to insert into webform_machete table")
                mail("Insert DB failed", "Inserting into webform_machete \
                                                    table failed")
    else:  # jobSuccess failed
        print "JobFailed!"
        if (existing_entry > 0):
            print "existing entry"
            try:
                cursor.execute("UPDATE webform_machete SET success=0, \
                tries=tries+1, last_attempt=NOW() WHERE sid='%s'",
                               (send_data['sid'],))
                db.commit()
            except:
                print "-------------- Update webform_machete table failed"
                log_entry("Failed to update webform_machete table")
                mail("update DB failed", "Updating webform_machete \
                                                    table failed")
        else:
            print "no existing entry"
            try:
                cursor.execute("INSERT INTO webform_machete (sid, success, \
                tries, last_attempt) VALUES('%s', 0, 1, NOW())",
                               (send_data['sid'],))
                db.commit()
            except:
                print "!-------------- Insert into webform_machete table failed"
                log_entry("Failed to insert into webform_machete table")
                mail("Insert DB failed", "Inserting into webform_machete \
                                                    table failed")

db.close()

if entry_count > 0:
    log_entry("Script finished")

log_file.close()
