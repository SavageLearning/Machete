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
        void CreateSignin(T signin, string user);
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
        /// <param name="signin"></param>
        /// <param name="user"></param>
        public virtual void CreateSignin(T signin, string user)
        {
            //Search for worker with matching card number
            Worker wfound;
            wfound = wRepo.GetAllQ().FirstOrDefault(s => s.dwccardnum == signin.dwccardnum);
            if (wfound != null)
            {
                signin.WorkerID = wfound.ID;
            }
            //Search for duplicate signin for the same day
            int sfound = 0; ;
            sfound = repo.GetAllQ().Count(s => s.dateforsignin == signin.dateforsignin &&
                                                     s.dwccardnum == signin.dwccardnum);
            if (sfound == 0) Create(signin, user);
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