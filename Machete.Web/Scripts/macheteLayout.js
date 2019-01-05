///////////////////////////////////////////////////////////////////
///
/// Add_rectab to tabbar
///
///     theref      - The URL reference for thte tab. Passed to tabs()
///     label       - Label used for new tab. Passed to tabs(), used to detect duplicate tab
///     tabObj      - The tab() object
///     exclusiveTab- If true then remove other tabs
///     recID       - 
///
function add_rectab(opt) {
    var theref = opt.tabref;
    if (!theref) { throw new Error("add_rectab requires tabref"); }  //href for tab
    var label = opt.label;
    if (!label) { throw new Error("add_rectab requires label"); } //tab label
    var tabObj = opt.tab;
    var exclusive = opt.exclusive;
    var recID = opt.recordID;
    var recType = opt.recType;
    var maxTabs = opt.maxTabs || 2;
    var pleaseDontFindMe = opt.pleaseDontFindMe;
    //
    //search for tab label--if it's already open, select instead of adding duplicate (datatables error)    
    var foundtab = $(tabObj).children('.ui-tabs-nav').find('li').find('a[recordID=' + recID + ']');
    var tabsize = foundtab.size();
    if (tabsize > 0 && !pleaseDontFindMe) {
        var index1 = $("li", tabObj).index($(foundtab).parent());
        tabObj.tabs("select", index1);
        $(foundtab).val(label);
        return;
    }
    //
    // If true, look for existing tab with same label; remove for re-create
    if (exclusive) {
        var index2 = $(tabObj).children('.ui-tabs-nav').find('li').size() - 1;
        console.log("add_rectab maxTabs value:" + maxTabs);
        if (index2 >= maxTabs) { //Don't blast tab 0 or tab 1 (list and create)
            tabObj.tabs("remove", index2);
        }
    }
    tabObj.tabs("add", theref, label);  // create tab on tabbar    
    //
    //
    var tabIndex = tabObj.tabs('length');
    tabObj.tabs("select", tabIndex - 1);    // select the newly created tab
    var newTab = $(tabObj).find('.ui-tabs-selected');
    $(newTab).attr('ID', recType + recID + '-EditTab');
    $(newTab).find('a').attr('recordID', recID); //Put recID in HTML attribute
    $(newTab).addClass(recType);
    $(newTab).find('span').attr('ID', recType + recID + '-CloseBtn');
    //float the close icon --ui 1.8.6 is overflow: hidden on ui-icon
    $(tabObj).find("span.ui-icon-close").attr('style', 'float: right');

    return false;
}
//////////////////////////////////////////////////////////////////
///
///##Create dataTable
///
function jqrfyTable(o) {
    var myTable = o.table,
        myTab = o.tab,
        myOptions = o.options,
        clickEvent = o.clickEvent,
        dblclickevent = o.dblClickEvent,
        tabLabel = o.tabLabel,
        maxTabs = o.maxTabs; // Default maxTabs is 2 (list=0,create=1...)
    var oTable;
    var origCallback;
    var tableID = $(myTable).attr('ID');
    //
    // insert standard fnRowCallback for mUI row attributes. call original
    //    handler at end
    if ("fnRowCallback" in myOptions) {
       origCallback = myOptions.fnRowCallback;
    }
    myOptions.fnRowCallback = function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
        //
        // custom attributes to create record tabs on doubleclick
        $(nRow).attr('edittabref', aData['tabref']);
        $(nRow).attr('edittablabel', aData['tablabel']);
        $(nRow).attr('recordid', aData['recordid']);
        if (jQuery.browser.mobile) {
            var $foo = $(nRow).find('td:nth-child(1)');
            var footext = $foo.text();
            var btnID = tabLabel + aData['recordid'] + '-Btn';
            $foo.prepend('<input type="button" class="rowButton" value="open" id="' + btnID + '"></input>');
        }
        // call original handler
        if (origCallback != undefined) {
            return origCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull);
        } else {
            return nRow;
        }
    }

    myOptions.fnServerData = function (sSource, aoData, fnCallback) {
        var aoDataConcatenated = aoData;
        if (myOptions.fnServerDataExtra) {
            aoDataConcatenated= aoData.concat(myOptions.fnServerDataExtra());
        }
        $.ajax({
            "dataType": 'json',
            "type": "GET",
            "url": sSource,
            "data": aoDataConcatenated,
            "success": function (result) {
                if (result.jobSuccess == false) {
                    alert(result.rtnMessage);
                }
                else {
                    fnCallback(result);
                }
            },
            "failure": function (result) {
                alert(result);
            }
        });
    }

    //
    // create datatable
    oTable = $(myTable).dataTable(myOptions).fnSetFilteringDelay(400);
    //
    // Add unique ID for testing hook
    $('#' + tableID + '_filter input').attr('ID', tableID + '_searchbox');    
    ////////////////////////////////////////////////////////////////
    //
    // table click event -- highlight row
    //
    if (!clickEvent) {
        // remove row_selected from all; add to event.target (only 1 selected)
        clickEvent = function (event) {
            $(oTable.fnSettings().aoData).each(function () {
                $(this.nTr).removeClass('row_selected');
            });
            $(event.target.parentNode).addClass('row_selected');
        }
    }
    $(myTable).find('tbody').click(clickEvent);
    ////////////////////////////////////////////////////////////////
    //
    // table doubleclick event 
    //
    if (!dblclickevent) {
        dblclickevent = function (event) {
            console.log("default dblclick event");
            var exclusiveTab = $(event.target).closest('.ui-tabs').hasClass('ExclusiveTab');
            var myTr = $(event.target).closest('tr');
            //
            // add new tab
            add_rectab({
                tabref: $(myTr).attr('edittabref'),
                label: $(myTr).attr('edittablabel'),
                tab: myTab,
                exclusive: exclusiveTab,
                recordID: $(myTr).attr('recordid'),
                recType: tabLabel,
                maxTabs: maxTabs
            });
        }
    }
    if (!jQuery.browser.mobile) {
        $(myTable).find('tbody').dblclick(dblclickevent);
    } else {
        $('.rowButton').live('click', dblclickevent);
    } 

    return oTable;
}
//////////////////////////////////////////////////////////////////
///
///##open google map, default to casa latina origin
///
function openGoogleMap(destAddr, origAddr) {
    if (origAddr == null) {
        origAddr = "317+17th+Avenue+South,+Seattle,+WA+98144";
    }
    var myStr = "http://maps.google.com/maps?f=d&source=s_d&saddr=" + origAddr + "&daddr=" + destAddr + "&hl=en&ie=UTF8";
    window.open(myStr);
}
////////////////////////////////////////////
//
// add new html elements
//
//$.fn.addItems = function (data) {
//    var elSel = this[0];
//    var i;
//    if (data == null) {
//        return;
//    }
//    for (i = elSel.length; i >= 0; i--) {
//        elSel.remove(i);
//    }
//    for (i = 0; i < data.length; i++) {
//        var text = data[i].Text;
//        var value = data[i].Value;

//        var elOptNew = document.createElement('option');
//        elOptNew.text = text;
//        elOptNew.value = value;
//        elSel.add(elOptNew, null);
//    }
//};

// http://stackoverflow.com/questions/18082/validate-numbers-in-javascript-isnumeric/174921#174921
function isNumber(n) {
  return !isNaN(parseFloat(n)) && isFinite(n);
}

datatable_lang_en = {
    "oPaginate": {
        "sFirst": "",
        "sLast": "",
        "sNext": "",
        "sPrevious": ""
    },
    "sEmptyTable": "No data available in table",
    "sInfo": "Showing (_START_ to _END_) out of _TOTAL_ entries",
    "sInfoEmpty": "No entries to show",
    "sInfoFiltered": " - filtered from _MAX_ total entries",
    "sInfoPostFix": "",
    "sLengthMenu": "Show _MENU_ entries",
    "sProcessing": "Processing request...",
    "sSearch": "Search:"
};

datatable_lang_es = {
    "oPaginate": {
        "sFirst": "",
        "sLast": "",
        "sNext": "",
        "sPrevious": ""
    },
    "sEmptyTable": "No hay datos disponibles en el cuadro",
    "sInfo": "Mostrando a cabo (_START_ a _END_) de _TOTAL_ entradas",
    "sInfoEmpty": "No hay entradas para mostrar",
    "sInfoFiltered": " - filtrado de _MAX_ registros",
    "sInfoPostFix": "",
    "sLengthMenu": "Mostrar _MENU_ registros",
    "sProcessing": "Procesamiento de solicitudes...",
    "sSearch": "Filtrar los registros:"
};

/**
* jQuery.browser.mobile (http://detectmobilebrowser.com/)
*
* jQuery.browser.mobile will be true if the browser is a mobile device
*
**/
(function (a) { jQuery.browser.mobile = /iPad|android|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|e\-|e\/|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|xda(\-|2|g)|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4)) })(navigator.userAgent || navigator.vendor || window.opera);