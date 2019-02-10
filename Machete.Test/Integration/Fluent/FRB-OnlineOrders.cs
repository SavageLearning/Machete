namespace Machete.Test.Integration.Fluent
{
    public partial class FluentRecordBase
    {
        public Machete.Web.ViewModel.Api.WorkOrder CloneOnlineOrder()
        {
            var wo = (Machete.Web.ViewModel.Api.WorkOrder)Records.onlineOrder.Clone();
            wo.contactName = RandomString(10);
            return wo;
        }
    }
}
