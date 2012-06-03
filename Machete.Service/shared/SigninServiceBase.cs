using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using System.Data.Objects.SqlClient;
using System.Globalization;
using System.Data.Objects;

namespace Machete.Service
{
    public interface ISigninService<T> : IService<T> where T : Signin
    {
        Image getImage(int dwccardnum);        
    }
    public abstract class SigninServiceBase<T> : ServiceBase<T> where T : Signin
    {
        protected readonly IWorkerRepository wRepo;
        protected readonly IWorkerRequestRepository wrRepo;
        protected readonly IImageRepository iRepo;
        //
        //
        protected SigninServiceBase(IRepository<T> repo,
                                   IWorkerRepository wRepo,
                                   IImageRepository iRepo,
                                   IWorkerRequestRepository wrRepo,
                                   IUnitOfWork uow)
            : base(repo, uow)
        {
            this.wRepo = wRepo;
            this.wrRepo = wrRepo;
            this.iRepo = iRepo;
            this.logPrefix = "SigninServiceBase";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardrequest"></param>
        /// <returns></returns>
        public Image getImage(int cardrequest)
        {
            Worker w_query = wRepo.GetAllQ().Where(w => w.dwccardnum == cardrequest).AsEnumerable().FirstOrDefault();
            if (w_query == null) return null;
            if (w_query.ImageID != null)
            {
                return iRepo.Get(i => i.ID == w_query.ImageID);
            }
            return null;
        } 
    }
}