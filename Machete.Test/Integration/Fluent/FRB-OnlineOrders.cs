namespace Machete.Test.Integration.Fluent
{
    public partial class FluentRecordBase
    {
        public Api.ViewModel.WorkOrder CloneOnlineOrder()
        {
            var wo = (Api.ViewModel.WorkOrder)Records.onlineOrder.Clone();
            wo.contactName = RandomString(10);
            return wo;
        }
    }
}
