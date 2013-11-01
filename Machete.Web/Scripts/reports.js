//////////////JSONTOCSV//////////////
//
// JSON2CSV
// Credit and thanks: http://stackoverflow.com/users/64741/zachary
//

//Removing the initial commas because the report data may contain commas
//from the service level. This isn't ideal, but the idea is that they
//get what they see as far as the reports are concerned.
function json2spreadsheet(objArray) {
    var array = typeof objArray !== 'object' ? JSON.parse(objArray) : objArray;

    var str = '';
    var line = '';

    for (var index in array[0]) {
        line += index + ',';
    }

    line = line.slice(0, -1);
    str += line + '\r\n';

    for (var i = 0; i < array.length; i++) {
        //var line = '';

        for (var indices in array[i]) {
            line += array[i][indices];
            line = line.replace(/,/g, '') + 'comma';
        }

        line = line.replace(/comma/g, ',');
        line = line.replace(/\r?\n|\r/g, '');
        str += line + '\r\n';
    }

    return str;
}

function download(strData, strFileName, strMimeType) {
    var D = document,
        A = arguments,
        a = D.createElement("a"),
        d = A[0],
        n = A[1],
        t = A[2] || "text/plain";

    //build download link:
    a.href = "data:" + strMimeType + "," + escape(strData);


    if (window.MSBlobBuilder) { // IE10
        var bb = new MSBlobBuilder();
        bb.append(strData);
        return navigator.msSaveBlob(bb, strFileName);
    } /* end if(window.MSBlobBuilder) */



    if ('download' in a) { //FF20, CH19
        a.setAttribute("download", n);
        a.innerHTML = "downloading...";
        D.body.appendChild(a);
        setTimeout(function () {
            var e = D.createEvent("MouseEvents");
            e.initMouseEvent("click", true, false, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
            a.dispatchEvent(e);
            D.body.removeChild(a);
        }, 66);
        return true;
    } /* end if('download' in a) */



    //do iframe dataURL download: (older W3)
    var f = D.createElement("iframe");
    D.body.appendChild(f);
    f.src = "data:" + (A[2] ? A[2] : "application/octet-stream") + (window.btoa ? ";base64" : "") + "," + (window.btoa ? window.btoa : escape)(strData);
    setTimeout(function () {
        D.body.removeChild(f);
    }, 333);
    return true;
} /* end download() */



//Global variables
function reportTableDefaults(url, lang, date)
{
    var tableDefaults =
        {
            "bPaginate": false, // (reports have fixed size)
            "bAutoWidth": false,
            "bInfo": true,
            "bSort": false,
            "bFilter": false,
            "bServerSide": true,
            "sAjaxSource": url,
            "bProcessing": false,
            "oLanguage": lang,
            "aoColumns": [
                { mDataProp: "weekday" },
                { mDataProp: "date" },
                { mDataProp: "totalSignins" },
                { mDataProp: "totalAssignments" },
                { mDataProp: "weekEstDailyHours" },
                { mDataProp: "weekEstPayment" },
                { mDataProp: "weekHourlyWage" },
                {
                    mDataProp: null,
                    sDefaultContent: '<img src="/Content/dataTables/details_open.png" class="nestedDetailsButton">'
                },
            ],
            "fnServerData": function (sSource, aoData, fnCallback) {
                aoData.push(
                    { "name": "todaysdate", "value": date });
                $.getJSON(sSource, aoData, function (json) {
                    /* Do whatever additional processing you want on the callback, then tell DataTables */
                    fnCallback(json);
                })
            }
        };
    return tableDefaults;
}

function dclTableDefaults(lang, date) 
{
    var dclDefaults = {
        "bPaginate": false, // to enable pagination (this report has fixed size)
        "bAutoWidth": false,
        "bDestroy": false,
        "bInfo": true, //shows information about data being displayed on the page
        "bSort": false, //enable or disable sorting of columns
        "bFilter": false, //enable or disable filtering of data
        "bServerSide": true, //server side processing, req. source
        "sAjaxSource": "/Reports/AjaxDcl", //source for server side processing
        "bProcessing": false, //enable processing indicator
        "oLanguage": lang, //internationalisation
        "aoColumns": [
            { mDataProp: "date" },
            { mDataProp: "dwcList" },
            { mDataProp: "dwcPropio" },
            { mDataProp: "hhhList" },
            { mDataProp: "hhhPropio" },
            { mDataProp: "uniqueSignins" },
            { mDataProp: "totalSignins" },
            { mDataProp: "totalAssignments" },
            { mDataProp: "cancelledJobs" },
        ], //these are the column names in the array; total # must match
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "todaysdate", "value": date });
            $.getJSON(sSource, aoData, function (json) {
                /* Do whatever additional processing you want on the callback, then tell DataTables */
                fnCallback(json);
            });
        }
    };
    return dclDefaults;
}

function wecChildDefaults(lang, date) {
    var wecDefaults = {
        "bPaginate": false, // (this report has fixed size)
        "bAutoWidth": false,
        "bDestroy": false,
        "bInfo": true,
        "bSort": false,
        "bFilter": false,
        "bServerSide": true,
        "sAjaxSource": "/Reports/AjaxJobs",
        "bProcessing": false,
        "oLanguage": lang,
        "aoColumns": [
            { mDataProp: "date" },
            { mDataProp: "skill" },
            { mDataProp: "count" }
        ],
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push(
                //THIS HAS TO BE THE CHILD DATE NOT THE FORM DATE
                { "name": "todaysdate", "value": date });
            $.getJSON(sSource, aoData, function (json) {
                /* Do whatever additional processing you want on the callback, then tell DataTables */
                fnCallback(json);
            });
        }
    };
    return wecDefaults;
}

function mwdTableDefaults(lang, date) 
{
    var mwdDefaults = {
        "bPaginate": false, // this report has fixed size
        "bAutoWidth": false,
        "bDestroy": false,
        "bInfo": true,
        "bSort": false, 
        "bFilter": false,
        "bServerSide": true, //server side processing
        "sAjaxSource": "/Reports/AjaxMwd",
        "bProcessing": false, 
        "oLanguage": lang, 
        "aoColumns": [
            { mDataProp: "date" },
            { mDataProp: "totalSignins" },
            { mDataProp: "uniqueSignins" },
            { mDataProp: "totalDWCSignins" },
            { mDataProp: "totalHHHSignins" },
            { mDataProp: "dispatchedDWCSignins" },
            { mDataProp: "dispatchedHHHSignins" },
            { mDataProp: "totalHours" },
            { mDataProp: "totalIncome" },
            { mDataProp: "avgIncomePerHour" },
        ], // column names; must match # of cols. in table
        "fnServerData": function ( sSource, aoData, fnCallback ) {
            aoData.push({ "name": "todaysdate", "value": date });
            $.getJSON(sSource, aoData, function (json) {
                /* Do whatever additional processing you want on the callback, then tell DataTables */
                fnCallback(json);
            });
        }
    };
    return mwdDefaults;
}

function jzcTableDefaults(lang, date) 
    {
        var jzcDefaults = {
        "bPaginate": false, // to enable pagination (this report has fixed size)
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bDestroy": false,
        "bInfo": true, //shows information about data being displayed on the page
        "bSort": false, //enable or disable sorting of columns
        "bFilter": false, //enable or disable filtering of data
        "bServerSide": true, //server side processing, req. source
        "sAjaxSource": "/Reports/AjaxJobsZipCodes", //source for server side processing
        "bProcessing": false,
        "oLanguage": lang,
        "aoColumns": [
            { mDataProp: "date" },
            { mDataProp: "topZips" },
            { mDataProp: "topZipsCount" },
            { mDataProp: "topJobs" },
            { mDataProp: "topJobsCount" }
        ], //these are the column names in the array; total # must match
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "todaysdate", "value": date });
            $.getJSON(sSource, aoData, function (json) {
                /* Do whatever additional processing you want on the callback, then tell DataTables */
                fnCallback(json);
            });
        }
    };
    return jzcDefaults;
}


function dailySigninPie(objArray) {
    var array = typeof objArray !== 'object' ? JSON.parse(objArray) : objArray;

    var dwc = 0;
    var dwcPropio = 0;
    var hhh = 0;
    var hhhPropio = 0;
    var unique = 0;
    var total = 0;
    var totalAssigned = 0;

    var pieData = '';

    for (var i = 0; i < array.aaData.length; i++) {
        dwc += array.aaData[i].dwcList;
        dwcPropio += array.aaData[i].dwcPropio;
        hhh += array.aaData[i].hhhList;
        hhhPropio += array.aaData[i].hhhPropio;
        unique += array.aaData[i].uniqueSignins;
        total += array.aaData[i].totalSignins;
        totalAssigned += array.aaData[i].totalAssignments;
    }

    pieData += '[\'DWC List\', ' + dwc + '],[\'Propio (DWC)\', ' + dwcPropio + '],[\'HHH List\', ' + hhh + '],[\'Propio (HHH)\',' + hhhPropio + ']';

    return pieData;
}

function monthlySigninPie(objArray) {
    var array = typeof objArray !== 'object' ? JSON.parse(objArray) : objArray;

    var dwc = 0;
    var dwcPropio = 0;
    var hhh = 0;
    var hhhPropio = 0;
    var unique = 0;
    var total = 0;
    var totalAssigned = 0;

    var pieData = '';

    for (var i = 0; i < array.aaData.length; i++) {
        dwc += array.aaData[i].dwcList;
        dwcPropio += array.aaData[i].dwcPropio;
        hhh += array.aaData[i].hhhList;
        hhhPropio += array.aaData[i].hhhPropio;
        unique += array.aaData[i].uniqueSignins;
        total += array.aaData[i].totalSignins;
        totalAssigned += array.aaData[i].totalAssignments;
    }

    pieData += '[\'DWC List\', ' + dwc + '],[\'Propio (DWC)\', ' + dwcPropio + '],[\'HHH List\', ' + hhh + '],[\'Propio (HHH)\',' + hhhPropio + ']';

    return pieData;
}

function gotWorkPie(objArray) {
    var array = typeof objArray !== 'object' ? JSON.parse(objArray) : objArray;

    var dwc = 0;
    var dwcPropio = 0;
    var hhh = 0;
    var hhhPropio = 0;
    var unique = 0;
    var total = 0;
    var totalAssigned = 0;

    var pieData = '';

    for (var i = 0; i < array.aaData.length; i++) {
        dwc += array.aaData[i].dwcList;
        dwcPropio += array.aaData[i].dwcPropio;
        hhh += array.aaData[i].hhhList;
        hhhPropio += array.aaData[i].hhhPropio;
        unique += array.aaData[i].uniqueSignins;
        total += array.aaData[i].totalSignins;
        totalAssigned += array.aaData[i].totalAssignments;
    }

    total = total - totalAssigned;

    pieData += '[\'Total Signed In\', ' + total + '],[\'Total Assigned\', ' + totalAssigned + ']';

    return pieData;
}

function getPieOptions(showLabels) {
    var pieOptions = {
        seriesDefaults: {
            renderer: jQuery.jqplot.PieRenderer, //jerry-rig; this should be passed in as an arg
            rendererOptions: {
                showDataLabels: showLabels
            }
        },
        legend: { show: true, location: 'e' }
    };

    return pieOptions;
}


function pieFilling(ajaxUrl, data, pieType) {
    var jstring = '';
    var pieObject;
    var json = $.getJSON(ajaxUrl, data, function (ajax) {
        jstring = JSON.stringify(ajax.aaData);
    });

    if (pieType == 'dailySigninPie')
    {
        pieObject = dailySigninPie(jstring);
        return pieObject;
    }
    else if (pieType == 'gotWorkPie')
    {
        pieObject = gotWorkPie(jstring);
        return pieObject;
    }
    else if (pieType == 'monthlySigninPie')
    {
        pieObject = monthlySigninPie(jstring);
        return pieObject;
    }
    else
    {
        return pieObject;
    }
}