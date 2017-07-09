using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Machete.Api.Maps
{
    public class MacheteProfile : Profile
    {
        public static string getCI()
        {
            return Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpperInvariant();
        }
    }
}