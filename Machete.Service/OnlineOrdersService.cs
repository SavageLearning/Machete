﻿using AutoMapper;
using Machete.Data.Infrastructure;
using Machete.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Service
{
    public interface IOnlineOrdersService
    {
        //WorkOrder Get(int id);
        IEnumerable<WorkOrder> GetAll();
        IEnumerable<WorkOrder> GetMany(Func<WorkOrder, bool> where);
        WorkOrder Create(WorkOrder order, string user);
    }

    public class OnlineOrdersService : IOnlineOrdersService
    {
        private readonly IMapper map;
        private readonly IEmployerService eserv;
        private readonly IWorkOrderService woserv;
        private readonly IWorkAssignmentService waserv;
        protected readonly IUnitOfWork uow;

        public OnlineOrdersService(
            IEmployerService eServ,
            IWorkOrderService woServ,
            IWorkAssignmentService waServ,
            IUnitOfWork uow, 
            IMapper map)
        {
            this.map = map;
            this.eserv = eServ;
            this.woserv = woServ;
            this.waserv = waServ;
            this.uow = uow;
        }

        public IEnumerable<WorkOrder> GetAll()
        {
            return woserv.GetAll();
        }

        public IEnumerable<WorkOrder> GetMany(Func<WorkOrder, bool> where)
        {
            return woserv.GetMany(where);
        }

        public WorkOrder Create(WorkOrder order, string user)
        {
            return woserv.Create(order, user);
        }
    }
}
