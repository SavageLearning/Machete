namespace Machete.Api.ViewModel
{
    public class TransportProviderAvailabilityVM : RecordVM
    {
        public int day { get; set; }
        public bool available { get; set; }
    }

    public class TransportProviderAvailabilityListVM : ListVM
    {
        public int day { get; set; }
        public bool available { get; set; }
    }
}