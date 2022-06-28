using Machete.Api.ViewModel;

namespace Machete.Test.Integration.Fluent
{
    public partial class FluentRecordBase
    {
        public WorkOrderVM CloneOnlineOrder()
        {
            var wo = (WorkOrderVM)Records.onlineOrder.Clone();
            wo.contactName = RandomString(10);
            return wo;
        }
    }
}
