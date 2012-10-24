import collections
import ConfigParser
import smtplib
import string

import MySQLdb
import requests

wanted_field_ids = [id for id in range(1, 17) if id != 12]

field_names = ('sid', 'name', 'address1', 'address2', 'city', 'state',
                'zipcode', 'business', 'phone', 'cellphone', 'email',
                'referredby', 'active', 'blogparticipate', 'notes',
                'referredbyOther')

# Get settings
try:
    _settings = ConfigParser.ConfigParser()
    _settings.read('employer_form.ini')
except ConfigParser.Error as err:
    print err
    raise

db_config = dict(_settings.items('db'))
machete_config = dict(_settings.items('machete'))
smtp_config = dict(_settings.items('smtp'))


# Open connection to Database
db = MySQLdb.connect(db_config['host'],
                     db_config['user'],
                     db_config['pw'],
                     db_config['db_name'])
cursor = db.cursor()

# only grab webform entries without success = true in webform_machete table
id = '2298'
cursor.execute("SELECT * from \
    webform_submitted_data LEFT OUTER JOIN \
    webform_machete ON webform_machete.sid = webform_submitted_data.sid \
    WHERE (webform_machete.success <> 1 OR webform_machete.success IS NULL) \
    AND webform_submitted_data.nid=%s", id)

form_data = cursor.fetchall()

print "------------------- form_data: ", form_data

def cluster_form_submission_data(data):
    '''group form fields by their sid (submission id)'''
    form_submission_data = collections.defaultdict(list)
    for field in data:
        form_submission_data[field[1]].append(field[2:])
    return form_submission_data

def convert_single_form_submission(form_data):
    '''convert a single submission to the format we need'''
    return dict(zip(field_names, [sid,] + [x[2] for x in sorted(form_data)]))

all_submissions = [convert_single_form_submission(x) for sid, x in
                   cluster_form_submission_data(form_data).items()]

print "------------------- all_submissions: ", all_submissions

# Send e-mail
# DOES NOT WORK FROM VM, but works from external server
def mail(subject='error', message='Error occurred'):
    mailServer = smtplib.SMTP(smtp_config['server'], smtp_config['port'])
    mailServer.ehlo()
    mailServer.starttls()
    mailServer.ehlo()
    mailServer.login(smtp_config['user'], smtp_config['pw'])

    body = string.join((
        "From: %s" % smtp_config['user'],
        "To: %s" % smtp_config['to'],
        "Subject: %s" % subject ,
        "",
        message
        ), "\r\n")
    server = smtplib.SMTP('localhost')
    server.sendmail(smtp_config['user'], smtp_config['to'], body)
    server.quit()

# login to machete
s = requests.session()
s.config['keep_alive'] = True

login_response = s.post(url='http://192.168.2.109/Account/Logon',
    data={'UserName': machete_config['user'], 'Password': machete_config['pw']})

if ('Login was unsuccessful' in login_response.text):
    #mail("Machete login failed", "Script could not login to machete")
    print "!````````````` login failed"
    exit()

# Process submissions
for send_data in all_submissions:
    send_data['ID'] = '0'
    try:
        post_response = s.post(url='http://192.168.2.109/Employer/Create',
                                                           data=send_data)
    except:
        print "!````````````` Update failed (MAIL SEND)"
#        mail("Machete post failed",
#            "Posting the form to the Machete URL failed")

    print "--------------- post_response: ", post_response.text

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
                print "!````````````` Update webform_machete table failed"
#                mail("update DB failed", "Updating webform_machete \
#                                                     table failed")
        else:
            try:
                cursor.execute("INSERT INTO webform_machete (sid, success, \
                tries, last_attempt) VALUES('%s', '%s', 1, NOW())",
                (send_data['sid'], post_response.json['jobSuccess']))
                db.commit()
            except:
                print "!````````````` Insert into webform_machete table failed"
#                mail("Insert DB failed", "Inserting into webform_machete
#                                                    table failed")
    else:  #jobSuccess failed
        print "JobFailed!"
        if (existing_entry > 0):
            print "blip 1"
            try:
                cursor.execute("UPDATE webform_machete SET success=0, \
                tries=tries+1, last_attempt=NOW() WHERE sid='%s'",
                (send_data['sid'],))
                db.commit()
            except:
                print "````````````` Update webform_machete table failed"
#                mail("update DB failed", "Updating webform_machete \
#                                                    table failed")
        else:
            print "blip 2"
            try:
                cursor.execute("INSERT INTO webform_machete (sid, success, \
                tries, last_attempt) VALUES('%s', 0, 1, NOW())",
                (send_data['sid'],))
                db.commit()
            except:
                print "!````````````` Insert into webform_machete table failed"
#                mail("Insert DB failed", "Inserting into webform_machete \
#                                                    table failed")

db.close()
