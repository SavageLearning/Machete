using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Api.Helpers;
using Machete.Api.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Email = Machete.Domain.Email;
using System.Collections.Generic;

namespace Machete.Api.Controllers
{
    public class EmailController : MacheteApiController<Email, EmailVM, EmailListVM>
    {
        private readonly IEmailService serv;

        public EmailController(
            IEmailService serv,
            IMapper map
            ) : base(serv, map)
        { }

        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public new ActionResult<IEnumerable<EmailListVM>> Get(
            [FromQuery] ApiRequestParams apiRequestParams)
        {
            return base.Get(apiRequestParams);
        }

        [HttpPost, Authorize(Roles = "Administrator, Manager, Teacher")]
        public async Task<ActionResult> Create(EmailViewVM emailview, string userName)
        {
            Email newEmail;
            if (await TryUpdateModelAsync(emailview))
            {
                var email = map.Map<EmailViewVM, Email>(emailview);
                if (emailview.attachment != null)
                {
                    email.attachment = HttpUtility.UrlDecode(emailview.attachment);
                    email.attachmentContentType = MediaTypeNames.Text.Html;
                }
                if (emailview.woid.HasValue)
                {
                    newEmail = serv.Create(email, userName, emailview.woid);
                }
                else
                {
                    newEmail = serv.Create(email, userName);
                }

                return Ok(new
                {
                    iNewID = newEmail.ID,
                });
            }
            else
            {
                return StatusCode(400, "Failed to update EmailViewVM model");
            }
        }

        [HttpPost, Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Duplicate(int id, int? woid, string userName)
        {
            var duplicate = serv.Duplicate(id, woid, userName);
            return Ok(new
            {
                iNewID = duplicate.ID
            });
        }
        /// <summary>
        /// POST: /Email/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(EmailViewVM emailview, FormCollection collection)
        {
            //UpdateModel(emailview);
            var email = serv.Get(emailview.id);
            var newemail = map.Map(emailview, email);
            if (emailview.attachment != null)
            {
                newemail.attachment = HttpUtility.HtmlDecode(emailview.attachment);
                newemail.attachmentContentType = MediaTypeNames.Text.Html;
            }
            serv.Save(newemail, UserEmail);
            return Ok();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>s
        /// <returns></returns>
        [HttpDelete, Authorize(Roles = "Administrator, Manager")]
        public ActionResult<EmailVM> Delete(int id, string userName)
        {
            return base.Delete(id);
        }
    }
}