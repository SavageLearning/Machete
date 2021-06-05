using AutoMapper;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;

namespace Machete.Service
{
    public interface ITransportVehiclesService : IService<TransportVehicle>
    {

    }
    public class TransportVehiclesService : ServiceBase2<TransportVehicle>, ITransportVehiclesService
    {
        private readonly IMapper map;

        public TransportVehiclesService(IDatabaseFactory db, IMapper map) : base(db)
        {
            this.map = map;
            this.logPrefix = "TV";
        }
    }
}
