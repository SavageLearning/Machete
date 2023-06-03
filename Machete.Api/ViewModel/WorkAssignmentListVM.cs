namespace Machete.Api.ViewModel
{
    /// <summary>
    /// Class for /WorkAssignment/Index view.
    /// </summary>
    public class WorkAssignmentListVM : ListVM
    {
        public string todaysdate { get; set; }
        public string dwccardnum { get; set; }
        public int status { get; set; }
        public bool wa_grouping { get; set; }
        public int typeofwork_grouping { get; set; }
        public bool assignedWorker_visible { get; set; }
        public bool signin_visible { get; set; }
        public bool requestedWorkers_visible { get; set; }
        public WorkerSigninVM _wsi { get; set; }
    }
}