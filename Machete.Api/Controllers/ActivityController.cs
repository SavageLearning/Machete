using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Machete.Service;
using Machete.Api.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Activity = Machete.Domain.Activity;
using DTO = Machete.Service.DTO;

namespace Machete.Api.Controllers
{
    public class ActivityController : MacheteApiController<Activity, ActivityVM, ActivityListVM>
    {
        private new readonly IActivityService service;

        public ActivityController(
            IActivityService serv,
            IMapper map
        ) : base(serv, map)
        {
            service = serv;
        }

        [HttpGet, Authorize(Roles = "Administrator, Manager, Teacher, Check-in")]
        public new ActionResult<IEnumerable<ActivityListVM>> Get([FromQuery] ApiRequestParams apiRequestParams)
        {
            return base.Get(apiRequestParams);
        }

        [HttpGet("{id}"), Authorize(Roles = "Administrator, Manager, Phonedesk")]
        public new ActionResult<ActivityVM> Get(int id)
        {
            return base.Get(id);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public new ActionResult<JsonResult> Post([FromBody] ActivityVM activity)
        {
            ActivityVM newActivity = null;
            if (activity.dateEnd < activity.dateStart)
            {
                return Ok(new { jobSuccess = false, rtnMessage = "End date must be greater than start date." });
            }

            activity.notes = activity.notes ?? "";
            activity.firstID = activity.id;

            var domain = map.Map<ActivityVM, Activity>(activity);

            try
            {
                newActivity = map.Map<Activity, ActivityVM>(service.Create(domain, UserEmail));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            return Ok(new { data = newActivity });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<ActionResult> CreateMany(ActivityScheduleVM actSched)
        {
            var instances = actSched.stopDate.Subtract(actSched.dateStart).Days;
            if (!await TryUpdateModelAsync(actSched) || instances == 0)
            {

                return StatusCode(400, new Exception("Select an appropriate length of time for these events."));
            }

            var length = actSched.dateEnd.Subtract(actSched.dateStart).TotalMinutes;

            for (var i = 1; i <= instances; i++)
            {
                var currentDate = DateTime.UtcNow.AddDays(i);
                var day = currentDate.DayOfWeek;

                switch (day)
                {
                    case DayOfWeek.Sunday when !actSched.sunday:
                    case DayOfWeek.Monday when !actSched.monday:
                    case DayOfWeek.Tuesday when !actSched.tuesday:
                    case DayOfWeek.Wednesday when !actSched.wednesday:
                    case DayOfWeek.Thursday when !actSched.thursday:
                    case DayOfWeek.Friday when !actSched.friday:
                    case DayOfWeek.Saturday when !actSched.saturday:
                        continue;
                    default:
                        {
                            var activity = new Activity
                            {
                                nameID = actSched.name,
                                typeID = actSched.type,
                                dateStart = currentDate,
                                dateEnd = currentDate.AddMinutes(length),
                                recurring = true,
                                firstID = actSched.firstID,
                                teacher = actSched.teacher,
                                notes = actSched.notes ?? ""
                            };

                            service.Create(activity, UserEmail);
                            break;
                        }
                }
            }

            return Ok();
        }

        /// <summary>
        /// POST: /Activity/Edit/5
        /// </summary>
        [HttpPut]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public ActionResult<ActivityVM> Put(ActivityVM activity)
        {
            if (activity.dateEnd < activity.dateStart)
            {
                return StatusCode(400, new Exception("End date must be greater than start date."));
            }

            activity.firstID = activity.id;
            activity.notes = activity.notes ?? "";
            return base.Put(activity.id, activity);
        }

        /// <summary>
        /// Delete /Activity/Delete/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public new ActionResult Delete(int id) { return base.Delete(id); }

        // Activities had CreateMany and DeleteMany functionality that was tightly
        // coupled to our MVC behavior. Removing controller method for now, until
        // we implement the UI and see what we need.

        // POST /Activity/Assign/5
        [HttpPost]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public ActionResult Assign(int personID, List<int> actList)
        {
            if (actList == null) throw new Exception("Activity List is null");
            service.AssignList(personID, actList, UserEmail);

            return Ok();
        }
        // POST /Activity/Unassign/5
        [HttpPost]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public ActionResult Unassign(int personID, List<int> actList)
        {
            if (actList == null) throw new Exception("Activity List is null");
            service.UnassignList(personID, actList, UserEmail);

            return Ok();
        }
    }
}
