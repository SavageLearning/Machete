using System.Collections.Generic;
using Machete.Domain;

namespace Machete.Test.Integration.HttpClientUtil
{
    public class ListResponseModel<T> where T : class

    {
        public List<T> data { get; set; }
    }

    public class ItemResponseModel<T> where T : class
    {
        public T data { get; set; }
    }
}
