using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Machete.Api.Controllers
{
    [ElmahHandleError]
    [Authorize]
    public class EmployerController : ApiController
    {
        private readonly IEmployerService serv;
        private readonly IWorkOrderService woServ;

        public EmployerController(IEmployerService employerService, IWorkOrderService workorderService)
        {
            this.serv = employerService;
            this.woServ = workorderService;
        }
        // GET api/values
        public IEnumerable<Web.ViewModel.Employer> Get()
        {
            var vo = new viewOptions();
            vo.displayLength = 10;
            vo.displayStart = 0;
            dataTableResult<Domain.Employer> list = serv.GetIndexView(vo);
            var result = list.query
                .Select(
                    e => Mapper.Map<Domain.Employer, Web.ViewModel.Employer>(e)
                ).AsEnumerable();
            return result;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}