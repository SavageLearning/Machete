namespace Machete.Web.ViewModel.Api
{
    public class ScheduleRule : BaseModel
    {
        public int day { get; set; }
        public int leadHours { get; set; }
        public int minStartMin { get; set; }
        public int maxEndMin { get; set; }

    }
}