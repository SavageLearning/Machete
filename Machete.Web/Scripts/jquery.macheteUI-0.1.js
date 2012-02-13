/*
* File:        jquery.macheteUI
* Version:     0.1
* Description: 
* Author:      
* Created:     
* Language:    
* License:     
* Project:     
* Contact:     
* 
* Copyright 2011 Savage Learning, LLC., all rights reserved.
*
* This source file is free software, under either the GPL v2 license or a
* BSD style license, as supplied with this software.
* 
* This source file is distributed in the hope that it will be useful, but 
* WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
* or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
* 
* For details please refer to:
*/
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
            changeLevel: {}
        }
    };
    var methods = {
        init: function (options) {
            // THIS
        },
        //
        //
        //
        createTabs: function (opt) {
            var tabdiv = this,
                changeConfirm = opt.changeConfirm,
                changeTitle = opt.changeTitle,
                confirmed = false, // create jQuery tabs with mUI handlers
                level = _checkFormLevel(opt.formLevel, "createTabs"); // Error if form level not set correctly
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
                    //
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
                    if ($(ui.tab).hasClass('ListTab')) {
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
        waFormConfiguration: function () {
            var waForm = this;
            var hrWage = $(waForm).find('#hourlyWage');
            var skillID = $(waForm).find('#skillID');
            var hours = $(waForm).find('#hours');
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
                parseSkillsDD(waForm);
            }
            // show total estimated earnings for assignment
            waEstimateEarnings(waForm);
            _waFilterHourRange(waForm);
            // presets for skill dropdown
            $(skillID).bind('change', function () {
                parseSkillsDD(waForm);
                _waFilterHourRange(waForm);
                waEstimateEarnings(waForm);
            });
            // presets for min/max hour dropdowns
            $(hours).bind('change', function () {
                _waFilterHourRange(waForm);
            });
            // if money fields change, recalc total
            $(waForm).find('.earnings-part').bind('change', function () {
                waEstimateEarnings(waForm);
            });
        },
        //
        //
        formSubmit: function (opt) {
            var form = this;
            var parentTab = $(form).closest('.ui-tabs');
            var SelTab = opt.selectTab || 0;
            var create = opt.create || null;
            var recType = opt.recType || null;
            var exclusiveTab = opt.exclusiveTab || true;
            var preProcess = opt.preProcess || null;
            var closeTab = opt.closTab || undefined;
            var postProcess = opt.postProcess || null;
            var callback = opt.callback || null;
            var level = _checkFormLevel(opt.formLevel, "formSubmit"); // Error if form level not set correctly
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
                    //
                    // post create form, open tab for new records
                    if (create) {
                        //
                        // post create form, open tab for new records
                        //$.post($(form).attr("action"), $(form).serialize(),
                        $(form).ajaxSubmit({
                            success: function (data) {
                                add_rectab({
                                    tabref: data.sNewRef, //come from JsonResult
                                    label: data.sNewLabel, // JsonResult
                                    tab: parentTab,
                                    exclusive: exclusiveTab,
                                    recordID: data.iNewID,  //JsonResult
                                    recType: recType
                                });
                                if (callback) {
                                    callback();
                                }
                            }
                        });
                    } else {
                        //$.post($(form).attr("action"), $(form).serialize());
                        $(form).ajaxSubmit({
                            success: callback
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
        formClickDuplicate: function (opt) {
            var btn, editForm, dupForm;
            btn = this;
            if (opt.editForm) {
                editForm = opt.editForm;
            } else {
                throw new Error("No edit form to submit");
            }
            if (opt.dupForm) {
                dupForm = opt.dupForm;
            } else {
                throw new Error("No duplicate form to submit");
            }
            btn.click(function () {
                editForm.data("SelTab", -1);
                editForm.data("create", null);
                editForm.submit();
                // duplicate the current edit
                dupForm.data("SelTab", -1);
                dupForm.data("exclusiveTab", false);
                dupForm.submit();
            });
        },

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
        //        
        formClickDelete: function (opt) {
            var btn = this;
            var ok = opt.ok || "OK?!";
            var confirm = opt.confirm || "CONFIRM?!";
            var title = opt.title || "TITLE?!";
            var form = opt.form;
            var altClose = opt.altClose;
            var postDelete = opt.postDelete;
            if (!form) throw new Error("No employer Delete Form defined");
            _submitAndCloseTab({
                form: form,
                altClose: altClose,
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
        selectToggleOnValue: function (opt) {
            var select = this;
            var showVal = opt.showVal || "yes";
            var target = opt.target;
            if (!target) {
                throw new Error("SelectToggleOnValue not given a target to toggle.");
            }
            //
            //
            $(select).bind('change', function () {
                toggleDropDown(select, showVal, target);
            });
            toggleDropDown(select, showVal, target);
        },
        //
        // Enables a target element with a select elment selects enableVal
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

    //
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
    //
    function toggleDropDown(select, showVal, target) {
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
    function parseSkillsDD(myForm) {
        var myDD = $(myForm).find('#skillID');
        var myWage = $(myForm).find('#hourlyWage');
        var myHour = $(myForm).find('#hours');
        var myRange = $(myForm).find('#hourRange');
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
            $(myForm).find('#hourRange option[value=""]').attr("selected", "selected").show();
        } else {
            $(myWage).removeAttr('disabled', 'disabled');
            $(myHour).removeAttr('disabled', 'disabled');
            $(myRange).removeAttr('disabled', 'disabled');
        }
        waEstimateEarnings(myForm);
    }
    //
    // Estimates earnings boxes on Assignments form
    function waEstimateEarnings(waForm) {
        var myWage = $(waForm).find('#hourlyWage').val();
        var myHours = $(waForm).find('#hours').find('option:selected').val();
        var myEarnings = $(waForm).find('#total');
        var myRangeEarnings = $(waForm).find('#totalRange');
        var myRange = $(waForm).find('#hourRange').val();
        $(myEarnings).attr('disabled', 'disabled');
        $(myRangeEarnings).attr('disabled', 'disabled');
        var myDays = $(waForm).find('#days').find('option:selected').val();
        if (isNumber(myDays) &&
        isNumber(myHours) &&
        isNumber(myWage)) {
            var total = myDays * myHours * myWage;

            $(myEarnings).val("$" + total.toFixed(2));
            if (isNumber(myRange)) {
                var range = myDays * myRange * myWage;
                $(myRangeEarnings).val("$" + range.toFixed(2));
            } else {
                $(myRangeEarnings).val("");
            }
        } else {
            $(myEarnings).val("@(Machete.Web.Resources.Shared.notcalculable)");
            $(myRange).val("@(Machete.Web.Resources.Shared.notcalculable)");
        }
    }
    //
    // Filters the Hour range based on what's selected in the base #hours field
    // TODO:2012/01/22: move to subfile with Assignment specific JS
    function _waFilterHourRange(waForm) {
        var myHours = $(waForm).find('#hours');
        var hour = $(myHours).val();
        var range = $(waForm).find('#hourRange').val();
        $(waForm).find('#hourRange option').each(function () {
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
            $(waForm).find('#hourRange option[value=""]').attr("selected", "selected").show();
            $(waForm).find('#totalRange').val("");
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
                    jConfirm(data.status, "ERROR");
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
})(jQuery, window, document);