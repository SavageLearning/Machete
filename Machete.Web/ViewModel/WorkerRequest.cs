namespace Machete.Web.ViewModel
{
    public class WorkerRequest : Record
    {
        public WorkerRequest()
        {
            //idString = "wkrRequest";
        }

        public int WorkOrderID { get; set; }
        public virtual WorkOrder workOrder { get; set; }
        public int WorkerID { get; set; }
        public virtual Worker workerRequested { get; set; }
    }
}
