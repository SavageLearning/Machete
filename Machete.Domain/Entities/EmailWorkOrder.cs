namespace Machete.Domain
{
    public class EmailWorkOrder : Record
    {
        public int WorkOrderID { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }

        public int EmailID { get; set; }
        public virtual Email Email { get; set; }

    }
}
