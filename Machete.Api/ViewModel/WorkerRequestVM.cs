namespace Machete.Api.ViewModel
{
    public class WorkerRequestVM : RecordVM
    {
        public int WorkOrderID { get; set; }
        public virtual WorkOrderVM workOrder { get; set; }
        public int WorkerID { get; set; }
        public virtual WorkerVM workerRequested { get; set; }
    }
}
