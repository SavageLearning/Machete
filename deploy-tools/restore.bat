REM this scrips exists in c:\temp directory on macheteprod.cloudapp.net

copy C:\temp\test.connections.config C:\inetpub\wwwroot\machete-test\connections.config 
copy C:\temp\alabama.connections.config C:\inetpub\wwwroot\machete-alabama\connections.config 
copy C:\temp\casa.connections.config C:\inetpub\wwwroot\machete-casa\connections.config
copy C:\temp\centrohumanitario.connections.config C:\inetpub\wwwroot\machete-centrohumanitario\connections.config 
copy C:\temp\concord.connections.config C:\inetpub\wwwroot\machete-concord\connections.config 
copy C:\temp\congreso.connections.config C:\inetpub\wwwroot\machete-congreso\connections.config 
copy C:\temp\donbosco.connections.config C:\inetpub\wwwroot\machete-donbosco\connections.config
copy C:\temp\elcentro.connections.config C:\inetpub\wwwroot\machete-elcentro\connections.config 
copy C:\temp\graton.connections.config C:\inetpub\wwwroot\machete-graton\connections.config 
copy C:\temp\mtnview.connections.config C:\inetpub\wwwroot\machete-mtnview\connections.config 
copy C:\temp\nice.connections.config C:\inetpub\wwwroot\machete-nice\connections.config 
copy C:\temp\pasadena.connections.config C:\inetpub\wwwroot\machete-pasadena\connections.config 
copy C:\temp\pomona.connections.config C:\inetpub\wwwroot\machete-pomona\connections.config 
copy C:\temp\sanfrancisco.connections.config C:\inetpub\wwwroot\machete-sanfrancisco\connections.config
copy C:\temp\santacruz.connections.config C:\inetpub\wwwroot\machete-santacruz\connections.config 
copy C:\temp\southside.connections.config C:\inetpub\wwwroot\machete-southside\connections.config 
copy C:\temp\voz.connections.config C:\inetpub\wwwroot\machete-voz\connections.config 
copy C:\temp\wjp.connections.config C:\inetpub\wwwroot\machete-wjp\connections.config 

        function OpenModal() {
            $("#divModal").dialog({
                autoOpen: false, modal: true, title: 'Modal', width: 'auto', height: 'auto'
                , buttons: { "Cancel": function () { $(this).dialog("close"); } },
            }).dialog('open');
            return false;
        };


+++++++++++++++++++++++++++++++++++++++++++++++++++++++



http://stackoverflow.com/questions/3057873/how-to-write-a-simple-html-dropdownlistfor
http://www.asp.net/mvc/overview/older-versions/working-with-the-dropdownlist-box-and-jquery/using-the-dropdownlist-helper-with-aspnet-mvc

RSPEC
https://robots.thoughtbot.com/how-to-stub-external-services-in-tests
https://robots.thoughtbot.com/validating-json-schemas-with-an-rspec-matcher

https://gist.github.com/alex-zige/5795358
http://www.emilsoman.com/blog/2013/05/18/building-a-tested/
http://stackoverflow.com/questions/3297048/403-forbidden-vs-401-unauthorized-http-responses/6937030#6937030
http://rspec.info/about/

        $('#removeWABtn-@(Model.ID)').click(function () {
        
        }

        <!-- TODO: Disable
        <li><a href="/HirerWorkOrder/View/1" id="hirerProfileTab" class="ListTab">@Machete.Web.Resources.WorkOrders.hirerProfile</a> </li>
        <li><a href="/HirerWorkOrder/Create" id="paypalTab" class="ListTab">@Machete.Domain.Resources.WorkOrder.paypal</a> </li>
        <li><a href="/HirerWorkOrder/List" id="hirerAccountTab" class="ListTab">@Machete.Web.Resources.WorkOrders.workOrders</a> </li>
        @* @if(ViewBag.employerId) *@
        @*{ *@
            <li><a href="/HirerAccount/Edit/ViewBag.employerId" id="hirerProfileTab" class="ListTab">@Machete.Web.Resources.WorkOrders.hirerAccount</a> </li>
        @*} *@
        -->

Look from Index line 137
    <h3>@Employers.boxlegend: @Model.Employer.name</h3>

        // Business name toggle - ON
        $('#@(idPrefix)business').mUI('selectToggleOnValue', {
            target: $('#@(idPrefix)businessNameRow'),
            showVal: "@(Shared.yes)"
        });

        // Employer name toggle - OFF
        $('#@(idPrefix)business').mUI('selectToggleOnValue', {
            target: $('#@(idPrefix)employerNameRow'),
            showVal: "@(Shared.no)"
        });


            var workerCenter = @System.Web.Configuration.WebConfigurationManager.AppSettings["OrganizationName"];
            if (workerCenter != "Casa Latina") {
                $("#invalidTransportDate").hide();
                return;
            }

    <!-- add key="ValidationSettings:UnobtrusiveValidationMode" value="None" / -->

        // TODO: add new dialog modal boxes
        /*
            var passwordChangeDialog = $("#passwordChange").dialog({
                autoOpen: false,
                height: 280,
                width: 300,
                modal: true,
                resizable: false,
                closeOnEscape: false,
                buttons: {
                    "@Machete.Web.Resources.ValidationStrings.Change": changePassword
                }
            });
            var messageDialog = $("#messageDialog").dialog({
                autoOpen: false,
                height: 200,
                width: 300,
                modal: true,
                closeOnEscape: true,
                buttons: {
                    "Ok": function () { $(this).dialog("close"); }
                }
            });
    function showMessage(messageText) {
        $("#messageDialogText").text(messageText);
        messageDialog.dialog("open");
    }

    function changePassword() {
        var newpassword = $("#newpassword")[0].value,
            confirmnewpassword = $("#confirmnewpassword")[0].value; 
        if (newpassword != confirmnewpassword) // verifying that new password and confirmed passwords are the same
        {
            showMessage("@Machete.Web.Resources.ValidationStrings.PasswordCompare");
            return;
        }
        if (newpassword === $('#Password')[0].value) { //#Password found using inspect element on Log on Machete window Checking that new password is not same as current password
            showMessage("Password cannot be the same as existing password.");
            return;
        }
    }

        */

                // workRequest Dialog -doubleclick on row to select
        // create event to handle worker selection from dialog
        $('#workerTable-@(Model.ID)').find('tbody').dblclick(function (event) {            
            var myTr = event.target.parentNode;
            var myID = $(myTr).attr('recordid');  
            var myLabel =  $(myTr).find('td:eq(0)').text() + ' '+
                                              $(myTr).find('td:eq(2)').text() + ' '+
                                              $(myTr).find('td:eq(4)').text();
          // handler function created by addRequestionBtn create event
          //M_workerRequestHandler_@(Model.ID)(myID, myLabel);
              $('#workerRequests2_WO-@(Model.ID)').append(
                    $('<option></option>').val(myID).html(myLabel)                
                );
          $('a.ui-dialog-titlebar-close').click();
        });
        //////////////////////////////////////////////////
        //
        //
        $('#workerDialog-@(Model.ID)').hide();
        $('#workerTable-@(Model.ID)').hide();
        //$("#wophone").mask("999-999-9999", { placeholder: " " });

        //////////////////////////////////////////////////
        //
        //
        $('#addRequestBtn-@(Model.ID)').click(function () {
            $('#workerTable-@(Model.ID)').dataTable().fnDraw();
            $('#workerTable-@(Model.ID)').show();
            //
            //Anon function to handle doubleclick of record selector
            //M_workerRequestHandler_@(Model.ID) = function (myID, myLabel){
            //    $('#workerRequests2_WO-@(Model.ID)').append(
            //        $('<option></option>').val(myID).html(myLabel)                
            //    );
            //}
            $("#workerDialog-@(Model.ID)").dialog({
                    height: 340,
                    width: 1000,
                    modal: true
            });
            
            $('#workerDialog-@(Model.ID)').show();
        });
        //////////////////////////////////////////////////
        //
        //


=======================================================

CAUSES JQUERY CODE TO BREAK: added the code to create.cshtml instead
        function checkTransportationMethodDate() {
            if ($("#@(idPrefix)transportMethodID").val() == "") {
                return;
            }

            // Note: this is logic that is specific to Casa Latina only
            var workerCenter = @System.Web.Configuration.WebConfigurationManager.AppSettings["OrganizationName"];
            if (workerCenter != "Casa Latina") {
                $("#invalidTransportDate").hide();
                return;
            }

            var userInput = $("#@(idPrefix)dateTimeofWork").val();
            var userInputInMs = new Date(userInput);

            if (isNaN(userInputInMs.valueOf())) {
                $("#invalidDate").text("The date must be in a valid date form (e.g. 03/14/2015 14:00)").show();
                return;
            } else {
                $("#invalidDate").hide();
            }

            var dayOfWeek = userInputInMs.getDay();

            if (dayOfWeek == 0) { // Sunday
                // Note: only valid transportation method on Sunday is Pickup
                // TODO: this value is hard-coded, should be replaced
                if ((transportMethod == 26) || (transportMethod == 29)) { // Worker buses = 26
                    $("#invalidTransportDate").text("Workers can only be picked up for work on Sunday").show();
                } else {
                    $("#invalidTransportDate").hide();
                }
            }

        };




@if (ViewBag.isEmployerOnlineProfileCreated) {
    @Html.HiddenFor(model => model.Employer.isOnlineProfileComplete)
    @Html.HiddenFor(model => model.Employer.onlineSigninID)
    @Html.HiddenFor(model => model.Employer.onlineSource)
    @Html.HiddenFor(model => model.Employer.active)
}

@if (ViewBag.isEmployerOnlineProfileCreated) {

        <div class="tb-table">
            <h3>@Machete.Web.Resources.WorkOrders.hirerProfile</h3>
            <div class="tb-row">
                @Html.Label(Machete.Web.Resources.WorkOrders.isBusiness)
                <div class="tb-field business_@(Model.ID)">
                    @Html.mUIDropDownYesNoFor(model => model.Employer.business, new { tabindex = "10", id = idPrefix + "business" }) 
                </div>
            </div>
            <!-- Note: Field only appears in table when englishRequired = FALSE -->
            <div class="tb-row employerNameRow" id="@(idPrefix)employerNameRow">
                @Html.mUITableLabelAndTextBoxFor(model => model.Employer.name, new { tabindex = "11", id = idPrefix + "employerName" })
            </div>
            <!-- Note: Field only appears in table when englishRequired = TRUE -->
            <div class="tb-row businessNameRow" id="@(idPrefix)businessNameRow">
                @Html.Label(Machete.Web.Resources.WorkOrders.businessName)
                @Html.mUITableTextBoxFor(model => model.Employer.businessname, new { tabindex = "12", id = idPrefix + "businessname" })
            </div>
            <div class="tb-row">
                <a href="#" id="employerAddress1Modal">@Html.mUITableLabelFor(model => model.Employer.address1)</a>
                @Html.mUITableTextBoxFor(model => model.Employer.address1, new { tabindex = "13", id = idPrefix + "employerAddress1" })
            </div>
            <div class="tb-row">
                @Html.mUITableLabelAndTextBoxFor(model => model.Employer.address2, new { tabindex = "14", id = idPrefix + "employerAddress2" })
            </div>
            <div class="tb-row">
                @Html.mUITableLabelAndTextBoxFor(model => model.Employer.city, new { tabindex = "15", id = idPrefix + "employerCity" })
            </div>
            <div class="tb-row">
                @Html.mUITableLabelAndTextBoxFor(model => model.Employer.state, new { tabindex = "16", id = idPrefix + "employerState" })
                @Html.ValidationMessageFor(model => model.Employer.state)
            </div>
            <div class="tb-row">
                @Html.mUITableLabelAndTextBoxFor(model => model.Employer.zipcode, new { tabindex = "17", id = idPrefix + "employerZipcode" })
            </div>
            <div class="tb-row">
                @Html.mUITableLabelFor(model => model.Employer.phone)
                @Html.mUITableTextBoxFor(model => model.Employer.phone, new { tabindex = "18", id = idPrefix + "employerPhone" })
            </div>
            <div class="tb-row">
                @Html.mUITableLabelAndTextBoxFor(model => model.Employer.cellphone, new { tabindex = "19", id = idPrefix + "employerCellphone" })
            </div>
            <div class="tb-row">
                @Html.mUITableLabelAndTextBoxFor(model => model.Employer.email, new { tabindex = "20", id = idPrefix + "email" })
            </div>
            <div class="tb-row">
                <div>
                    @Html.Label(Machete.Web.Resources.Employers.referredBy)
                </div>
                <div class="tb-field referredby_@(Model.ID)">
                    @Html.mUIDropDownListFor(model => model.Employer.referredby,
                                new SelectList(Lookups.getSelectList(Machete.Domain.LCategory.emplrreference), "Value", "Text"),
                                new { tabindex = "21", id = idPrefix + "referredby" })
                </div>
            </div>
            <!-- Note: Field only appears in table when referredBy = "Other" -->
            <!-- TODO: Need to make sure that grabbing the english text to do the string comparison  -->
            <div class="tb-row referredbyOtherRow" id="@(idPrefix)referredbyOtherRow">
                @Html.mUITableLabelAndTextBoxFor(model => model.Employer.referredbyOther, new { tabindex = "22", id = idPrefix + "referredbyOther" })
            </div>
            <div class="tb-row">
                <!-- Display Previously hired information - worker center specific -->
                @if (CI.TwoLetterISOLanguageName.ToUpperInvariant() == "EN") {
                    @Html.Label(System.Web.Configuration.WebConfigurationManager.AppSettings["PreviouslyHired_EN"])
                } else {
                    @Html.Label(System.Web.Configuration.WebConfigurationManager.AppSettings["PreviouslyHired_ES"])
                }
                @Html.mUIDropDownYesNoFor(model => model.Employer.returnCustomer, new { tabindex = "23", id = idPrefix + "returnCustomer" }) 
            </div>
            <div class="tb-row">
                <!-- Display Receive Updates information - worker center specific -->
                @if (CI.TwoLetterISOLanguageName.ToUpperInvariant() == "EN") {
                    @Html.Label(System.Web.Configuration.WebConfigurationManager.AppSettings["ReceiveUpdates_EN"])
                } else {
                    @Html.Label(System.Web.Configuration.WebConfigurationManager.AppSettings["ReceiveUpdates_ES"])
                }
                @Html.mUIDropDownYesNoFor(model => model.Employer.receiveUpdates, new { tabindex = "24", id = idPrefix + "receiveUpdates" }) 
            </div>
        </div><!--tb-table main workorder table-->
}
