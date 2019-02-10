using System.Threading;
using AutoMapper;

namespace Machete.Web.Maps.Api
{
    public class MacheteProfile : Profile
    {
        public static string getCI()
        {
            return Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpperInvariant();
        }
    }
}