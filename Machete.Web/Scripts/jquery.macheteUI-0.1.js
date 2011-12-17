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

(function ($, window, document) {
    var methods = {
        init: function (options) {
            // THIS 
        },
        selectEmployerDblClick: function (content) {
            //
            this.dblclick(function (event) {
            var myTr = event.target.parentNode;
            var myID = $(myTr).attr('requestedID');
            var orderText = $('li.WO.ui-tabs-selected a').text();
	        var employerText = 'EID #: ' + myID + ', ' + $(myTr).find('td:eq(1)').text() + ' @ ' + $(myTr).find('td:eq(2)').text();
            var idPrefix = $('#employerSelectTable').attr('idprefix');
            $.alerts.okButton = content.okButton;
            jConfirm(content.message,
                content.title + '[' + orderText +'] TO [' + employerText+ ']',
                function (r) {
                    if (r == true) {
                        // 
                        // action for doubleclick
                        $('#' + idPrefix + 'EmployerID').val(myID);
                        $('a.ui-dialog-titlebar-close').click();
                        $('#' + idPrefix + 'SaveBtn').submit();
                        $('#' + idPrefix + 'CloseBtn').click();
                        event.preventDefault();
                    }
                }
            );
            event.preventDefault();
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
})(jQuery, window, document);