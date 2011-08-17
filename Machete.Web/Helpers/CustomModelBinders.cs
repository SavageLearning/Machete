using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Service;
using Machete.Domain;

namespace Machete.Web.Helpers
{
    public class workerRequestBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {

            // see if there is an existing model to update and create one if not
            List<WorkerRequest> model = (List<WorkerRequest>)bindingContext.Model ?? new List<WorkerRequest>();
            // find out if the value provider has the required prefix
            bool hasPrefix = bindingContext.ValueProvider
            .ContainsPrefix(bindingContext.ModelName);
            string searchPrefix = (hasPrefix) ? bindingContext.ModelName + "." : "";
            // Get the raw attempted value from the value provider
            //WorkerService workerService = 
            ValueProviderResult vpr = bindingContext.ValueProvider.GetValue("workerRequests2");
            if (vpr != null)
            {
                var incomingData = vpr.AttemptedValue;
                //return incomingData.Split(new char[1] { ',' }).Select(data => Car.GetCars().FirstOrDefault(o => o.Id == int.Parse(data))).ToList();
                model = incomingData.Split(new char[1] { ',' }).Select(data => new WorkerRequest
                {
                    //workerRequested = workerService.GetWorkerByNum(int.Parse(data)),
                    dwccardnum = int.Parse(data)
                }).ToList();
            }
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, vpr);
            return model;
        }
    }
}