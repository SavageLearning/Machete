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
function weeklyTableDefaults(url, lang, date)
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

function dailyTableDefaults(url, lang, date) 
{
    var dclDefaults = {
        "bPaginate": false, // to enable pagination (this report has fixed size)
        "bAutoWidth": false,
        "bDestroy": false,
        "bInfo": true, //shows information about data being displayed on the page
        "bSort": false, //enable or disable sorting of columns
        "bFilter": false, //enable or disable filtering of data
        "bServerSide": true, //server side processing, req. source
        "sAjaxSource": url, //source for server side processing
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

function monthlyTableDefaults(url, lang, date) 
{
    var mwdDefaults = {
        "bPaginate": false, // this report has fixed size
        "bAutoWidth": false,
        "bDestroy": false,
        "bInfo": true,
        "bSort": false, 
        "bFilter": false,
        "bServerSide": true, //server side processing
        "sAjaxSource": url,
        "bProcessing": false, 
        "oLanguage": lang, 
        "aoColumns": [
            { mDataProp: "date" },
            { mDataProp: "totalSignins" },
            { mDataProp: "uniqueSignins" },
            { mDataProp: "dispatchedDWCList" },
            { mDataProp: "dispatchedHHHList" },
            { mDataProp: "dispatchedDWCPropio" },
            { mDataProp: "dispatchedHHHPropio" },
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

function yearlyTableDefaults(url, lang, date)
{
    var yearDefaults = {
        "bPaginate": false, // this report has fixed size
        "bAutoWidth": false,
        "bDestroy": false,
        "bInfo": true,
        "bSort": false,
        "bFilter": false,
        "bServerSide": true, //server side processing
        "sAjaxSource": url,
        "bProcessing": false,
        "oLanguage": lang,
        "aoColumns": [
                { mDataProp: "date" },
                { mDataProp: "tempJobs" },
                { mDataProp: "safetyTraining" },
                { mDataProp: "skillsTraining" },
                { mDataProp: "eslAssessed" },
                { mDataProp: "basicGarden" },
                { mDataProp: "advGarden" },
                { mDataProp: "finEd" },
                {
                    mDataProp: null,
                    sDefaultContent: '<img src="/Content/dataTables/details_open.png" class="nestedDetailsButton">'
                },
        ], // column names; must match # of cols. in table
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "todaysdate", "value": date });
            $.getJSON(sSource, aoData, function (json) {
                /* Do whatever additional processing you want on the callback, then tell DataTables */
                fnCallback(json);
            });
        }
    };
    return yearDefaults;
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

function newTableDefaults(lang, date) {
    var newDefaults = {
        "bPaginate": false, // to enable pagination (this report has fixed size)
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bDestroy": false,
        "bInfo": true, //shows information about data being displayed on the page
        "bSort": false, //enable or disable sorting of columns
        "bFilter": false, //enable or disable filtering of data
        "bServerSide": true, //server side processing, req. source
        "sAjaxSource": "/Reports/AjaxNewWorkers", //source for server side processing
        "bProcessing": false,
        "oLanguage": lang,
        "aoColumns": [
            { mDataProp: "date" },
            { mDataProp: "singles" },
            { mDataProp: "families" },
            { mDataProp: "newSingles" },
            { mDataProp: "newFamilies" }
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

//tested on jsFiddle, working ok
function dailySigninPie(objArray) {
    var array = typeof objArray !== 'object' ? JSON.parse(objArray) : objArray;

    var dwc = 0;
    var dwcPropio = 0;
    var hhh = 0;
    var hhhPropio = 0;
    var unique = 0;
    var total = 0;
    var totalAssigned = 0;

    var pieData = [];

    for (var i = 0; i < array.aaData.length; i++) {
        dwc += array.aaData[i].dwcList;
        dwcPropio += array.aaData[i].dwcPropio;
        hhh += array.aaData[i].hhhList;
        hhhPropio += array.aaData[i].hhhPropio;
        unique += array.aaData[i].uniqueSignins;
        total += array.aaData[i].totalSignins;
        totalAssigned += array.aaData[i].totalAssignments;
    }

    pieData = [
        ['DWC List', dwc],
        ['Propio (DWC)', dwcPropio],
        ['HHH List', hhh],
        ['Propio (HHH)', hhhPropio]
    ];

    return pieData;
}

function monthlySigninPie(objArray) {
    var array = typeof objArray !== 'object' ? JSON.parse(objArray) : objArray;

    var total = 0;
    var unique = 0;
    var dwcList = 0;
    var hhhList = 0;
    var dwcPropio = 0;
    var hhhPropio = 0;
    var totalHours = 0;
    var totalIncome = 0;
    var avgIncome = 0;

    var notDispatched = 0;

    var pieData = [];

    for (var i = 0; i < array.aaData.length; i++) {
        total += parseInt(array.aaData[i].totalSignins) || 0;
        unique += parseInt(array.aaData[i].uniqueSignins) || 0;
        dwcList += parseInt(array.aaData[i].dispatchedDWCList) || 0;
        hhhList += parseInt(array.aaData[i].dispatchedHHHList) || 0;
        dwcPropio += parseInt(array.aaData[i].dispatchedDWCPropio) || 0;
        hhhPropio += parseInt(array.aaData[i].dispatchedHHHPropio) || 0;
        totalHours += parseInt(array.aaData[i].totalHours) || 0;
        totalIncome += parseInt(array.aaData[i].totalIncome) || 0;
        avgIncome += parseInt(array.aaData[i].avgIncomePerHour) || 0;
    }

    notDispatched = total - (dwcList + hhhList + dwcPropio + hhhPropio);

    pieData = [
        ['DWC List', dwcList],
        ['HHH List', hhhList],
        ['DWC Propio Patron', dwcPropio],
        ['HHH Propio Patron', hhhPropio],
        ['Not Dispatched', notDispatched]
    ];

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

    var totalNot = 0;

    var pieData = [];

    for (var i = 0; i < array.aaData.length; i++) {
        dwc += array.aaData[i].dwcList;
        dwcPropio += array.aaData[i].dwcPropio;
        hhh += array.aaData[i].hhhList;
        hhhPropio += array.aaData[i].hhhPropio;
        unique += array.aaData[i].uniqueSignins;
        total += array.aaData[i].totalSignins;
        totalAssigned += array.aaData[i].totalAssignments;
    }

    totalNot = total - totalAssigned;

    pieData = [
        ['Total Not Assigned', totalNot],
        ['Total Assigned', totalAssigned]
        ];
    
    return pieData;
}

function pieFilling(ajaxUrl, data, pieType) {
    var jstring = '';
    var pieObject;
    var json = $.getJSON(ajaxUrl, data, function (ajax) {
        jstring = JSON.stringify(ajax.aaData);
    });

    if (pieType == 'dailySigninPie')
    {
        pieObject = dailySigninPie(json);
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