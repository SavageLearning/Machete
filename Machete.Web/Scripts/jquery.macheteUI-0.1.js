// File:     jquery.macheteUI
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Web
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
(function ($, window) {

    var mUI = {
        state: {
            changed: function (level) {
                for (chglvl in mUI.state.changeLevel) {
                    if (level <= chglvl) {
                        if (mUI.state.changeLevel[chglvl]) { return true; }
                    }
                }
                return false;
            },
            whatChanged: {},
            changeLevel: {},
            pageTimerSeconds: 2700, //45 minutes on editPage auto-close
            pageTimer: null
        }
    };
    var methods = {
        init: function (options) {
            // THIS
        },
        //
        //
        //
        createTable: function(opt) {
            var myTable = this,
                myTab = opt.tab,
                myOptions = opt.options,
                clickEvent = opt.clickEvent,
                dblclickevent = opt.dblClickEvent,
                tabLabel = opt.tabLabel,
                maxTabs = opt.maxTabs; // Default maxTabs is 2 (list=0,create=1...)
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
                    aoDataConcatenated = aoData.concat(myOptions.fnServerDataExtra());
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

        },
        //
        //
        //
        createTabs: function (opt) {
            var tabdiv = this,
                changeConfirm = opt.changeConfirm,
                changeTitle = opt.changeTitle,
                confirmed = false, // create jQuery tabs with mUI handlers
                maxTabs = opt.maxTabs || 2,
                level = _checkFormLevel(opt.formLevel, "createTabs"), // Error if form level not set correctly
                updateDatatable = opt.updateDatatable || false;
            if (!changeConfirm) throw new Error("mUI.createTabs requires a changeConfirm option");
            if (!changeTitle) throw new Error("mUI.createTabs requires a changeTitle option");

            $(tabdiv).tabs({
                // defaults
                selected: opt.defaultTab || 0,
                idPrefix: opt.prefix || "PREFIX",
                //template to put the ui-icon-close in the tab
                tabTemplate: "<li><a href='#{href}'>#{label}</a> <span class='ui-icon ui-icon-close'>Remove Tab</span></li>",
                //
                //http://forum.jquery.com/topic/ajaxoptions-is-null-problem
                ajaxOptions: {
                    error: function (xhr, status, index, anchor) {
                        $(anchor.hash).html("Couldn't load this tab.");
                    },
                    data: {},
                    success: function (data, textStatus) { }
                },
                //
                // jquery.tabs() select event
                select: function (e, ui) {
                    console.log('select event: changeLevel:' + level +
                                ' state:' + mUI.state.changeLevel[level] +
                                ' confirmed: ' + confirmed);
                    //
                    // detectChanges functionality 
                    if (mUI.state.changed(level)) {
                        if (!confirmed) {
                            jConfirm(changeConfirm, changeTitle, function (r) {
                                if (r === true) {
                                    console.log('confirm ok--changed: ' + mUI.state.changed + ', confirmed: ' + confirmed);
                                    confirmed = true;
                                    $(ui.tab).click();
                                }
                            });
                            e.stopImmediatePropagation();
                            return false;
                        } else {
                            // if confirmed==true, then ignore changed bit
                            mUI.state.changeLevel[level] = false;
                            confirmed = false;
                        }
                    }
                    //                    
                    //if ListTab selected, redraw dataTable
                    if ($(ui.tab).hasClass('ListTab') && updateDatatable) {
                        $(ui.panel).find('.display').dataTable().fnDraw();
                    }
                },
                //
                // jquery.tabs() load event (This event doesn't happen for the list tab)
                load: function () {
                    //$(ui.panel).fadeIn();
                    mUI.state.changeLevel[level] = false;
                    console.log('tab-load--changed: ' + mUI.state.changed() + ', confirmed: ' + confirmed);
                },
                //
                // jquery.tabs() show event
                show: function () {
                    //if ($(ui.tab).hasClass('ListTab')
                    //        || $(ui.tab).hasClass('GeneralTab')) {
                    //    $(ui.panel).fadeIn();
                    //}
                },
                //
                // jquery.tabs() remove event (This event doesn't happen for the list tab)
                remove: function () { }
            });
            //
            // close tab event
            $(tabdiv).find("span.ui-icon-close").live("click", function (e) {
                var trgTabnav = $(e.target).closest('.ui-tabs');
                var index = trgTabnav.children('.ui-tabs-nav').index($(this).parent());
                //
                // check mUI detectChanges
                if (mUI.state.changed(level)) {
                    if (!confirmed) {
                        jConfirm(changeConfirm, changeTitle, function (r) {
                            if (r === true) {
                                console.log('confirm ok--changed: ' + mUI.state.changed + ', confirmed: ' + confirmed);
                                confirmed = true;
                                $(e.target).click();
                            }
                        });
                        e.stopImmediatePropagation();
                        return false;
                    } else {
                        // if confirmed==true, then ignore changed bit
                        mUI.state.changeLevel[level] = false;
                        confirmed = false;
                    }
                }
                //
                //clean up tabs
                trgTabnav.tabs("remove", index);
                trgTabnav.tabs("select", 0);            //select list tab
            });
        },
        //
        // Change Work Order's employer, doubleclick event
        //
        selectEmployerDblClick: function (content) {
            //
            this.dblclick(function (event) {
                var myTr = event.target.parentNode;
                var myID = $(myTr).attr('recordid');
                var orderText = $('li.WO.ui-tabs-selected a').text();
                var employerText = 'EID #: ' + myID + ', ' + $(myTr).find('td:eq(1)').text() + ' @ ' + $(myTr).find('td:eq(2)').text();
                var idPrefix = $('#employerSelectTable').attr('idprefix');
                $.alerts.okButton = content.okButton;
                jConfirm(content.message,
                    content.title + '[' + orderText + '] TO [' + employerText + ']',
                    function (r) {
                        if (r === true) {
                            //
                            // action for doubleclick
                            $('#' + idPrefix + 'EmployerID').val(myID);
                            $('a.ui-dialog-titlebar-close').click();
                            $('#' + idPrefix + 'SaveBtn').submit();
                            $('#' + idPrefix + 'CloseBtn').click();
                            event.preventDefault();
                        }
                    });
                event.preventDefault();
            });
        },
        //
        //  waFormConfiguration: Assignment Create/Edit form configuration
        //
        waFormConfiguration: function (opt) {
            var waForm = this;
            var hrWage = opt.hourlyWage;
            var skillID = opt.skillID;
            var hours = opt.hour;
            opt.waForm = this;
            //
            // Increment tabindex by 20 to offset for employers(0) and orders (10)
            $(waForm).find('[tabindex]').each(function () {
                $(this).attr('tabindex', parseInt($(this).attr('tabindex')) + 20);
            });
            //
            // Run only if hourly wage is 0
            // don't want to override a custom hourly wage on edit
            if ($(hrWage).text() === "0") {
                // update earnings info based on skill
                _waParseSkillsDD(opt);
            }
            // show total estimated earnings for assignment
            _waEstimateEarnings(opt);
            _waFilterHourRange(opt);
            // presets for skill dropdown
            $(skillID).bind('change', function () {
                _waParseSkillsDD(opt);
                _waFilterHourRange(opt);
                _waEstimateEarnings(opt);
            });
            // presets for min/max hour dropdowns
            $(hours).bind('change', function () {
                _waFilterHourRange(opt);
            });
            // if money fields change, recalc total
            $(waForm).find('.earnings-part').bind('change', function () {
                _waEstimateEarnings(opt);
            });
        },
        //
        //
        tabFormSubmit: function (opt) {
            var form = this;
            var parentTab = $(form).closest('.ui-tabs');
            var SelTab = opt.selectTab || 0; // Selects tab "0" if no option selected
            var create = opt.create || null;//true
            var recType = opt.recType || null;//activity
            var exclusiveTab = opt.exclusiveTab || true;
            var closeTab = opt.closeTab || undefined;
            var closeTabBeforeLoad = opt.closeTabBeforeLoad || undefined;
            var preProcess = opt.preProcess || null;
            var postProcess = opt.postProcess || null;
            var callback = opt.callback || null;
            var maxTabs = opt.maxTabs || 2;
            var level = _checkFormLevel(opt.formLevel, "formSubmit"); //1 //Error if form level not set correctly
            var pleaseDontFindMe = opt.pleaseDontFindMe || false;
            //
            //setup button.click to secondary submit
            //  workorder/edit/activate.btn
            if (opt.clickButton) {
                opt.clickButton.click(function () {
                    form.submit();
                });
            }
            form.submit(function (e) {
                //
                e.preventDefault();
                //
                if (preProcess) { preProcess(); }
                //
                // overiding form behavior after instantiation
                // used with duplicate work assignment submit
                if (form.data("SelTab") !== undefined) { SelTab = form.data("SelTab"); }
                if (form.data("exclusiveTab") !== undefined) { exclusiveTab = form.data("exclusiveTab"); }
                if (form.data("create") !== undefined) { create = form.data("create"); }
                //
                //
                if ($(form).valid()) {
            // post create form, open tab for new records
                    if (create) {
            // $(form).ajaxSubmit() breaks exception handling, so 
                        $.post($(form).attr("action"), $(form).serialize(),
            // success:
                        function (data) {
                            if (data.isRedirect) { window.location.href = data.redirectUrl; }
                            else if (data.jobSuccess == false) { alert(data.rtnMessage); }
                            else {
                                add_rectab({
                                    tabref: data.sNewRef, //come from JsonResult
                                    label: data.sNewLabel, // JsonResult
                                    tab: parentTab,
                                    exclusive: exclusiveTab,
                                    recordID: data.iNewID,  //JsonResult
                                    recType: recType,
                                    maxTabs: maxTabs,
                                    pleaseDontFindMe: pleaseDontFindMe
                                });
                                if (callback) { callback(); }
                            }
                        }, "json");
                    } else {
                        //$.post($(form).attr("action"), $(form).serialize());
                        $(form).ajaxSubmit({
                            dataType: 'json', 
                        //$.post(
                          //  $(form).attr("action"), //URL from form object
                            //$(form).serialize(),    //DATA from form object
                            success: function (data) {       //Successful server post callback
                                console.log("got to exception alert in formSubmit, jobSuccess is:" + data.JobSuccess);
                                if (data.jobSuccess == false) {
                                    alert(data.rtnMessage);
                                } else {
                                    if (callback) {
                                        callback();
                                    }
                                }
                            }
                        });
                    }
                    //
                    //
                    if (postProcess) {
                        postProcess();
                    }
                    //
                    // clear changed bit for current form level
                    // clear changed bit for levels downstream
                    for (chglvl in mUI.state.changeLevel) {
                        if (level <= chglvl) {
                            $(form).find('.saveBtn').removeClass('highlightSave');
                            mUI.state.changeLevel[chglvl] = false;
                            console.log("formSubmit: changeLevel[" + chglvl + "]: false");
                        }
                    }
                    //
                    //
                    if (closeTab) {
                        var oTabs = $(form).closest('.ui-tabs').children('.ui-tabs-nav');
                        $(oTabs).find('.ui-state-active').find('span.ui-icon-close').click();
                    }
                    //
                    // Tab behavior after save. change tab or no...
                    if (SelTab >= 0) {
                        $(parentTab).tabs("select", SelTab);
                    }
                }
            });
            //TODO: javascript...need to deal with ajax error
        },
        //
        //
        formSubmit: function (opt) {
            var form = this;
            var preProcess = opt.preProcess || null; //always happens before submit
            var postProcess = opt.postProcess || null; // always happens after submit
            var callback = opt.callback || null; // happens after submit if submit successful
  
            form.submit(function (e) {
                e.preventDefault();
                //
                if (preProcess) { preProcess(); }

                if ($(form).valid()) {
                    //
                    $(form).ajaxSubmit({
                        dataType: 'json',
                        success: function (data) {
                            if (data.jobSuccess == false) {
                                alert(data.rtnMessage);
                            } else {
                                if (callback) {
                                    callback();
                                }
                            }
                        }

                    });
                    //
                    if (postProcess) {
                        postProcess();
                    }
                }
            });
        },
        //
        //
        formClickDuplicate: function (opt) {
            var btn, editForm, dupForm;
            btn = this;
            editForm = opt.editForm;
            if (opt.dupForm) {
                dupForm = opt.dupForm;
            } else {
                throw new Error("No duplicate form to submit");
            }
            btn.click(function () {
                if (editForm) {
                    editForm.data("SelTab", -1);
                    editForm.data("create", null);
                    editForm.submit();
                }
                // duplicate the current edit
                dupForm.data("SelTab", -1);
                dupForm.data("exclusiveTab", false);
                dupForm.submit();
            });
        },
        //
        //
        btnEventImageDelete: function (opt) {
            var btn = this;
            var ok = opt.ok || "OK?!";
            var confirm = opt.confirm || "CONFIRM?!";
            var title = opt.title || "TITLE?!";
            var action = opt.action;
            var params = opt.params;
            var callback = opt.callback;
            if (!action) throw new Error("btnEventImageDelete requires action property.");
            if (!params) throw new Error("btnEventImageDelete requires params property.");
            btn.click(function () {
                $.alerts.okButton = ok;
                jConfirm(confirm,
                         title,
                         function (r) {
                             if (r === true) {
                                 //console.log("btnEventImageDelete: delete submitted");
                                 $.post(action, params, callback);
                             }
                         });
            });

        },
        //
        // Attaches a workorder to an email   
        btnAttachWorkOrder: function (opt) {
            var btn = this;
            var url = opt.url;
            var field = opt.field;
            var reattachBtn = opt.reattach;
            var clearBtn = opt.clear;
            var viewBtn = opt.view;
            var woid = opt.woid;
            if (!url) throw new Error("url is a required property");
            if (!field) throw new Error("field is a required property");

            var attachFunc = function (e) {
                e.preventDefault();
                console.log('attachWO called ' + url);
                //$.get(url, function (data) {
                //    //var encoded = $('<div/>').text(data).html();
                //    console.log(data);
                //    field.val(data);
                //});
                $.ajax({
                    "dataType": 'html',
                    "type": "GET",
                    "url": url,
                    "success": function (result) {
                        if (result.jobSuccess == false) {
                            alert(result.rtnMessage);
                        }
                        else {
                            field.val(result);
                            _applyAttachmentBtnMask(attachElems);

                        }
                    },
                    "failure": function (result) {
                        alert(result);
                    }
                });
                _applyAttachmentBtnMask(attachElems);

            };
            attachElems = {
                attach: btn,
                reattach: reattachBtn,
                view: viewBtn,
                clear: clearBtn,
                field: field,
                woid: woid
            };
            //attach buttons
            btn.click(attachFunc);
            //reattach button
            if (reattachBtn) {
                reattachBtn.click(attachFunc);
            }
            // show the existing attachment
            if (viewBtn) {
                viewBtn.click(function (e) {
                    myWindow = window.open('', '');
                    myWindow.document.write(field.val());
                    myWindow.focus();
                    _applyAttachmentBtnMask(attachElems);

                });
            }
            // clear the attachment
            if (clearBtn) {
                clearBtn.click(function (e) {
                    field.val('');
                    _applyAttachmentBtnMask(attachElems);
                });
            }
            _applyAttachmentBtnMask(attachElems);
        },
        //
        //
        btnSendEmail: function (opt) {
            var btn = this;
            var statusBox = opt.statusBox;
            var emailForm = opt.emailForm;
            var sendStatus = opt.sendStatus;
            console.log("sending email...");
            $(btn).click(function (e) {
                $(statusBox).val(sendStatus);
                $(emailForm).submit();
            });
        },
        formClickDelete: function (opt) {
            var btn = this;
            var ok = opt.ok || "OK?!";
            var confirm = opt.confirm || "CONFIRM?!";
            var title = opt.title || "TITLE?!";
            var form = opt.form;
            var altClose = opt.altClose;
            var postDelete = opt.postDelete;
            //
            if (!form) throw new Error("No Delete Form defined");
            
            // default assumes that formClickDelete called on a MacheteTab object
            _submitAndCloseTab({
                form: form,
                altClose: altClose, // whats called on form.submit below
                postDelete: postDelete
            }); //setup ajax submit action
            btn.click(function () {
                $.alerts.okButton = ok;
                jConfirm(confirm,
                title,
                function (r) {
                    if (r === true) {
                        //alert("delete submitted");
                        form.submit();
                    }
                });
            });
        },
        //
        // Used in ActivityJoinIndex
        postSelectedRows: function (opt) {
            var btn = this;
            var target = opt.targetTable;
            var url = opt.url;
            var personID = $(opt.person).val();
            var tables = opt.refreshTables;
            btn.click(function (e) {
                var selectedTr = $(target).find('tr.row_selected');
                e.preventDefault();
                console.log("personID: " + personID + " activities:" + selectedTr);

                var aData = new Array();
                $(selectedTr).each(function () {
                    var recordid = $(this).attr('recordid');
                    var o = { 'recordid': recordid };
                    aData.push(recordid);
                });
                var postData = { personID: personID, actList: aData };
                $.post(url, $.param(postData, true), function (data) {
                    if (data.jobSuccess == false) {
                        alert(data.rtnMessage);
                    } else {
                        $(tables).each(function () {
                            $(this).dataTable().fnDraw();
                        });
                    }

                });
            });
        },
        //
        // public function for setChangeState
        markChanged: function (opt) {
            var form = this;
            _setChangeState({
                form: form,
                level: opt.level,
                recType: opt.recType
            });
        },
        //
        // Setup bind-change event to detect changes on forms
        formDetectChanges: function (opt) {
            var form = this;
            var changeConfirm = opt.changeConfirm || "CONFIRM?!";
            var changeTitle = opt.changeTitle || "TITLE?!";
            var level = _checkFormLevel(opt.formLevel, "formDetectChanges"); // Error if form level not set correctly
            console.log('formDetectChanges load--changed: ' + mUI.state.changed());
            var recType = opt.recType || Error('formDetectChanges must have a recType');
            //
            // fires when changed AND focus moves away from element
            $(form).find('input[type="text"], select, textarea').bind('change', function () {
                //
                //
                _setChangeState({
                    form: form,
                    level: level,
                    recType: recType
                });
            });
            window.onbeforeunload = function () {
                if (mUI.state.changed(1)) {
                    return changeConfirm;
                } else {
                    return null;
                }
            }
        },
        //
        // Show or hide based on a select element's selected value
        // Used on workorder.englishRequired
        selectToggleOnValue: function (opt) {
            var select = this;
            var showVal = opt.showVal || "yes";
            var target = opt.target;
            if (!target) {
                throw new Error("SelectToggleOnValue not given a target to toggle.");
            }
            //
            $(select).bind('change', function () {
                _toggleDropDown(select, showVal, target);
            });
            _toggleDropDown(select, showVal, target);
        },
        //
        // Enables or disable HTML element with if element's value == enableVal
        // Used on Worker.race/disabled/driverslicense/carinsurance
        selectEnableOnValue: function (opt) {
            var select = this;
            var enableVal = opt.enableVal;
            var target = opt.target;
            if (!enableVal) {
                throw new Error("selectEnableOnValue requires an enableVal property");
            }
            if (!target) {
                throw new Error("selectEnableOnValue requires a target to enable");
            }
            $(select).bind('change', function () {
                EnableOnValue(select, enableVal, target);
            });
            EnableOnValue(select, enableVal, target);
        },
        //
        // Used in config.cshtml. Shows/hides based on class and skill
        // selected in dropdown
        configEnableOnValue: function (opt) {
            var object = this;
            var event = 'change';
            var cfgArray = opt;
            $(object).bind(event, function () {
                _validateOnValue(object, cfgArray);
            });
            _validateOnValue(object, cfgArray);
        },
        //
        //
        selectDDPopulatesField: function (opt) {
            var object = this;
            var event = 'change';
            var cfg = opt;
            $(object).bind(event, function () {
                _insertTemplate(object, cfg);
            });
            //_insertTemplate(object, cfg);
        },
        //
        //
        tabTimer: function (opt) {
            var tab = this;
            this.bind('tabsselect', function (event, ui) {
                if ($(ui.tab).attr('id') == 'activityListTab') {
                    $('body').unbind('mousemove', _resetTimer);
                    $('body').unbind('keydown', _resetTimer);
                    //$('#activeTabs #signinTable').unbind('click',resetTimer);
                    _clearTimer();
                } else {
                    _setTimer();
                    $('body').bind('mousemove', _resetTimer);
                    $('body').bind('keydown', _resetTimer);
                    //$('#activeTabs #signinTable').bind('click',resetTimer);
                }
            });
        }

    };

    $.fn.mUI = function (method) {

        // Method calling logic
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.mUI');
        }

    };

    ////////////////////////////////////////////
    //
    // machete js internal functions
    //
    function _pageTimerEnd() {
        window.location.href = "/Activity";
    }

    function _clearTimer() {
        try {
            clearTimeout(mUI.state.pageTimer);
            console.log('pageTimer cleared: ' + mUI.state.pageTimer);
            mUI.state.pageTimer = null;
        } catch (e) {
            console.log('no pageTimer set');
        }
    }

    function _setTimer() {
        if (mUI.state.pageTimer != null) {
            console.log('pageTimer already exists: ' + mUI.state.pageTimer);
        } else {
            mUI.state.pageTimer = setTimeout(_pageTimerEnd, mUI.state.pageTimerSeconds * 1000);
            console.log('pageTimer set:' + mUI.state.pageTimer);
        }
    }

    function _resetTimer() {
        console.log('resetting pageTimer');
        _clearTimer();
        _setTimer();
    }
    //
    //
    function _insertTemplate(object, cfg) {
        var foo = $(object).find('option:selected').attr(cfg.attrName);
        cfg.target.val(foo);
    }

    //
    //
    function _validateOnValue(object, cfgArray) {
        //Object is the dropdown that is triggering the different validation states
        var myArray = cfgArray;
        if (!myArray) {
            throw new Error("_validateOnValue configuration array empty or null");
        }
        for (var i = 0; i < myArray.length; i++) {
            var entry = myArray[i];
            console.log('_validateOnValue called for value: ' + entry.enableOnValue);
            if ($(object).val() == entry.enableOnValue) {
                $(entry.targets).show();
                $(entry.validators).attr("data-val", true);
            } else {
                $(entry.targets).hide();
                $(entry.validators).attr("data-val", false);
            }
            var myForm = $(object).closest('form');
            $(myForm).removeData('unobtrusiveValidation');
            $(myForm).removeData('validator');
            $.validator.unobtrusive.parse(myForm);
        }
    }
    //  Internal function to record what's changed:
    //       level: refers to the hierarchy level of a given form 
    //              ([1]employer/[2]order/[3]assignment)
    //              the lower the number, the more general the form. 
    //     recType: categorizes what changed
    // whatChanged: stores a reference to the tab that changed
    function _setChangeState(opt) {
        var form = opt.form;
        var level = opt.level || _checkFormLevel(opt.formLevel, "_setChangeState");
        var recType = opt.recType;
        if (!recType) throw new Error("_setChangeState must have a recType");
        //
        var changedTab = $(form).closest('.ui-tabs').children('.ui-tabs-nav').find('.ui-tabs-selected');
        $(form).find('.saveBtn').addClass('highlightSave');
        mUI.state.changeLevel[level] = true;
        mUI.state.whatChanged[recType] = changedTab;
        console.log('_setChangeState-changed: ' + mUI.state.changed(level));

    }
    //
    // Back end for selectToggleOnValue:
    function _toggleDropDown(select, showVal, target) {
        //
        if ($(select).find(':selected').text() === showVal) {
            $(target).show();
        } else {
            $(target).hide();
        }
    }
    //
    //
    function EnableOnValue(select, showVal, target) {
        //
        var selectedVal = $(select).find(':selected').attr('value');
        if (selectedVal === showVal) {
            $(target).removeAttr('disabled', 'disabled');
        } else {
            $(target).attr('disabled', 'disabled');
        }
    }
    //
    //  parse skills drop down
    //  for the create/edit assignment page. 
    //  bound to change event
    //
    function _waParseSkillsDD(opt) {
        var myDD = opt.skillID;
        var myWage = opt.hourlyWage;
        var myHour = opt.hour;
        var myRange = opt.range;
        //
        var myOption = $(myDD).find('option:selected');
        //
        // If custom attributes exist in skill dropdown selected, change fields
        //
        if ($(myOption).attr('wage') !== null) { $(myWage).val($(myOption).attr('wage')); }
        if ($(myOption).attr('minHour') !== null) { $(myHour).val($(myOption).attr('minHour')); }
        if ($(myOption).attr('fixedjob') === "True") { //Disable wage and hours on fixed job
            $(myWage).attr('disabled', 'disabled');
            $(myHour).attr('disabled', 'disabled');
            $(myRange).attr('disabled', 'disabled');
            $(myRange).find('option[value=""]').attr("selected", "selected").show();
        } else {
            $(myWage).removeAttr('disabled', 'disabled');
            $(myHour).removeAttr('disabled', 'disabled');
            $(myRange).removeAttr('disabled', 'disabled');
        }
        _waEstimateEarnings(opt);
    }
    //
    // Estimates earnings boxes on Assignments form
    function _waEstimateEarnings(opt) {
        var waForm = opt.waForm;
        var myWage = opt.hourlyWage;
        var myHour = opt.hour;
        var myTotal = opt.total;
        var myTotalRange = opt.totalRange;
        var myRange = opt.range;
        var myDays = opt.days;
        var rangeVal = $(myRange).val();
        var wageVal = $(myWage).val();
        var hourVal = $(myHour).val();
        var daysVal = $(myDays).find('option:selected').val();
        $(myTotal).attr('disabled', 'disabled');
        $(myTotalRange).attr('disabled', 'disabled');

        if (isNumber(daysVal) &&
            isNumber(hourVal) &&
            isNumber(wageVal)) {
            var total = daysVal * hourVal * wageVal;

            $(myTotal).val("$" + total.toFixed(2));
            if (isNumber(rangeVal)) {
                var range = daysVal * rangeVal * wageVal;
                $(myTotalRange).val("$" + range.toFixed(2));
            } else {
                $(myTotalRange).val("");
            }
        } else {
            $(myTotal).val(opt.errCalcMsg);
            $(myTotalRange).val(opt.errCalcMsg);
        }
    }
    //
    // Filters the Hour range based on what's selected in the base #hours field
    // TODO:2012/01/22: move to subfile with Assignment specific JS
    function _waFilterHourRange(opt) {
        //var myHours = opt.hour;
        var waForm = opt.waForm;
        var hour = $(opt.hour).val();
        var range = $(opt.range).val();
        var totalRange = opt.TotalRange;
        var myRange = opt.range;
        $(myRange).find('option').each(function () {
            var entry = this;
            var entryval = $(this).val();
            if (Number(entryval) <= Number(hour)) {
                $(entry).hide();
            }
            else {
                $(entry).show();
            }

        });
        if (Number(hour) >= Number(range)) {
            $(myRange).find('option[value=""]').attr("selected", "selected").show();
            $(totalRange).val("");
        }
    }
    //
    //
    // Called by formClickDelete -- sets up closing the Tab with the submit
    function _submitAndCloseTab(opt) {
        var form = opt.form;
        var altClose = opt.altClose;
        var postDelete = opt.postDelete;
        // TODO:2012/01/22: Switch (and test) to ajaxSubmit
        form.submit(function (e) {
            e.preventDefault();
            $.post($(this).attr("action"), $(this).serialize(), function (data) {
                if (data.status !== "OK") {
                    $.alerts.okButton = "OK";
                    jConfirm(data.status, "ERROR?!");
                    return false;
                }
                //
                //trigger close even
                if (altClose) {
                    $(altClose).click();
                } else {
                    var tabNav = $(e.target).closest('.ui-tabs').children('.ui-tabs-nav');
                    $(tabNav).find('.ui-state-active').find('span.ui-icon-close').click();
                }
                if (postDelete) {
                    postDelete();
                }
            });

        });
    }
    //
    //
    function _checkFormLevel(level, caller) {
        if (level === null || level === undefined || level < 1) {
            throw new Error(caller + ": formLevel not correctly defined, formlevel: " + level);
        }
        console.log(caller + ": formLevel: " + level);
        return level;
    }

    function _applyAttachmentBtnMask(opt) {
        var btn = opt.attach;
        var reattachBtn = opt.reattach;
        var clearBtn = opt.clear;
        var viewBtn = opt.view;
        var field = opt.field;
        var woid = opt.woid;
        if (field.val())
        {
            $(btn).hide();
            if (woid) {
                $(reattachBtn).show();
            } else {
                $(reattachBtn).hide();
            }
            $(viewBtn).show();
            $(clearBtn).show();
        } else
        {
            if (woid) {
                $(btn).show();
            } else {
                $(btn).hide();
            }
            $(reattachBtn).hide();
            $(viewBtn).hide();
            $(clearBtn).hide();
        }
    }
})(jQuery, window, document);