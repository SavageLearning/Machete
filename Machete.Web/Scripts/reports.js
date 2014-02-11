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
        "bDestroy": true,
        "bInfo": true,
        "bSort": false,
        "bFilter": false,
        "bServerSide": true,
        "sAjaxSource": url,
        "bProcessing": false,
        "oLanguage": lang,
        "aoColumns": null,
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push(
                { "name": "todaysdate", "value": date });
            $.getJSON(sSource, aoData, function (json) {
                /* Do whatever additional processing you want on the callback, then tell DataTables */
                fnCallback(json);
            });
        },
        "fnFooterCallback": null
    };
    return tableDefaults;
}

function summaryTableDefaults(url, lang, date)
{
    var tableDefaults = reportTableDefaults(url, lang, date);
    tableDefaults.aoColumns = [
    { mDataProp: "date" },
    { mDataProp: "stillHere" },
    { mDataProp: "totalSignins" },
    { mDataProp: "wentToClass" },
    { mDataProp: "dispatched" },
    { mDataProp: "totalHours" },
    { mDataProp: "totalIncome" },
    { mDataProp: "avgIncomePerHour" },
    {
        mDataProp: null,
        sDefaultContent: '<img src="/Content/dataTables/details_open.png" class="nestedDetailsButton">'
    },
    ];
    tableDefaults.fnFooterCallback = function (nRow, aaData, iStart, iEnd, aiDisplay) {
        /*
         * Calculate the total for all numbers this table 
         */
        var nCells = nRow.getElementsByTagName('th');
        var nTr = nRow.parentElement.getElementsByTagName('tr')[1];
        var tCells = nTr.getElementsByTagName('td');

        var iTotal = 0;
        var iWent = 0;
        var iDispatch = 0;
        var iHours = 0;
        var iIncome = 0;
        var iAverage = 0.0;
        var iDontCount = 0.0;

        var iEnrolled = 0;
        var iQuit = 0;
        var myFirstSignin = 0;
        var jornalero = 0;
        var conseguiChamba = 0;
        var myFirstJob = 0;

        //console.log(aaData); aaData is an array of objects, each containing properties and values for each column

        for (var i = 0; i < aaData.length ; i++) {
            //console.log(aaData[i]);
            iTotal += parseInt(aaData[i].totalSignins);
            iWent += parseInt(aaData[i].wentToClass);
            iDispatch += parseInt(aaData[i].dispatched);
            iHours += parseInt(aaData[i].totalHours);
            iIncome += parseFloat(aaData[i].totalIncome.replace('$', '').replace(',', ''));
            iAverage += parseFloat(aaData[i].avgIncomePerHour.replace('$', ''));
            iDontCount += parseFloat(aaData[i].avgIncomePerHour.replace('$', '')) > 0 ? 0 : 1;
            iEnrolled += parseInt(aaData[i].drilldown.newlyEnrolled);
            iQuit += parseInt(aaData[i].drilldown.peopleWhoLeft);
            myFirstSignin += parseInt(aaData[i].drilldown.uniqueSignins);
            jornalero += parseInt(aaData[i].drilldown.tempDispatched);
            conseguiChamba += parseInt(aaData[i].drilldown.permanentPlacements);
            myFirstJob += parseInt(aaData[i].drilldown.undupDispatched);
        }

        nCells[2].innerHTML = iTotal + ' signins';
        nCells[3].innerHTML = iWent + ' students';
        nCells[4].innerHTML = iDispatch + ' dispatched';
        nCells[5].innerHTML = iHours + ' hours';
        nCells[6].innerHTML = '$' + iIncome.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ' earned';
        nCells[7].innerHTML = '$' + (iAverage / (aaData.length - iDontCount)).toFixed(2) + '/hr. avg.';

        tCells[0].innerHTML = iEnrolled + ' signed up, ' + iQuit + ' left';

        tCells[2].innerHTML = '(' + myFirstSignin + ' unique)';

        tCells[4].innerHTML = '(' + myFirstJob + ' unique, ' + jornalero + ' temporary, and ' + conseguiChamba + ' permanent jobs)';
    }

    return tableDefaults;
}

function activityTableDefaults(url, lang, date)
{
    var tableDefaults = reportTableDefaults(url, lang, date);
    tableDefaults.aoColumns = [
    { mDataProp: "date" },
    { mDataProp: "safety" },
    { mDataProp: "skills" },
    { mDataProp: "esl" },
    { mDataProp: "basGarden" },
    { mDataProp: "advGarden" },
    { mDataProp: "finEd" },
    { mDataProp: "osha" },
    {
        mDataProp: null,
        sDefaultContent: '<img src="/Content/dataTables/details_open.png" class="nestedDetailsButton">'
    },
    ];

    return tableDefaults;
}

function workerTableDefaults(url, lang, date) {
    var tableDefaults = reportTableDefaults(url, lang, date);
    tableDefaults.aoColumns = [
        { mDataProp: "date" },
        { mDataProp: "singleAdults" },
        { mDataProp: "familyHouseholds" },
        { mDataProp: "newSingleAdults" },
        { mDataProp: "newFamilyHouseholds" }
    ];
    tableDefaults.fnFooterCallback = function (nRow, aaData, iStart, iEnd, aiDisplay) {
        /*
         * Calculate the total for all numbers this table 
         */
        var nCells = nRow.getElementsByTagName('th');

        var iComplete = 0.0;
        var iSingle = 0;
        var iFamily = 0;
        var iNewSingle = 0;
        var iNewFamily = 0;

        for (var i = 0; i < aaData.length ; i++) {
            iComplete += parseFloat(aaData[i].zipCompleteness);
            iSingle += parseInt(aaData[i].singleAdults);
            iFamily += parseInt(aaData[i].familyHouseholds);
            iNewSingle += parseInt(aaData[i].newSingleAdults);
            iNewFamily += parseInt(aaData[i].newFamilyHouseholds);
        }

        iComplete = Math.round((iComplete / (iSingle + iFamily)) * 10000)/100;

        nCells[0].innerHTML = "Zip Codes: " + iComplete + "%";
        nCells[2].innerHTML = iNewSingle;
        nCells[3].innerHTML = iNewFamily;
    }

    return tableDefaults;
}


function fnFormatWeekDetails(oTable, nTr) {
    var aData = oTable.fnGetData(nTr);
    var jobList = aData.topJobs;
    var sOut = '<table cellpadding="5" cellspacing="0" border="0" class="report-drill">';
    sOut += '<thead><tr><th>Date</th>'; //don't really need date; they already select date for drilldown
    sOut += '<th>Skill</th>';
    sOut += '<th>Count</th>';
    sOut += '</tr></thead><tbody>';
    for (var i in jobList) {
        var job = jobList[i];
        sOut += '<tr><td>' + job.date + '</td><td>' + job.skill + '</td><td>' + job.count + '</td></tr>';
    }
    sOut += '</tbody></table>';
    return sOut;
}

function fnFormatSummaryDetails(oTable, nTr) {
    var aData = oTable.fnGetData(nTr);
    var drillList = aData.drilldown;
    var sOut = '<table cellpadding="5" cellspacing="0" border="0" style="margin-left: 13.5%;background: #90A2B6;border: solid 2px gray;border-collapse: separate;">';
    sOut += '<thead><tr><th>Enrolled</th>';
    sOut += '<th>Exited</th>';
    sOut += '<th>Unique Signin</th>';
    sOut += '<th>Temporary</th>';
    sOut += '<th>Permanent</th>';
    sOut += '<th>Unique Dispatch</th>';
    sOut += '</tr></thead><tbody>';
    sOut += '<tr><td>' + drillList.newlyEnrolled + '</td><td>' + drillList.peopleWhoLeft + '</td><td>' + drillList.uniqueSignins + '</td><td>' + drillList.tempDispatched + '</td><td>' + drillList.permanentPlacements + '</td><td>' + drillList.undupDispatched + '</td></tr>';
    sOut += '</tbody></table>';
    return sOut;
}

function fnFormatActivityDetails(oTable, nTr) {
    var aData = oTable.fnGetData(nTr);
    var activityList = aData.drilldown;
    var sOut = '<table cellpadding="5" cellspacing="0" border="0" class="report-drill">';
    sOut += '<thead><tr><th>Class</th>';
    sOut += '<th>Type</th>';
    sOut += '<th>Count</th>';
    sOut += '</tr></thead><tbody>';
    for (var i in activityList) {
        var act = activityList[i];
        sOut += '<tr><td>' + act.name + '</td><td>' + act.type + '</td><td>' + act.count + '</td></tr>';
    }
    sOut += '</tbody></table>';
    return sOut;
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