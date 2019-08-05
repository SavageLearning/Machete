using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Helpers
{
    public interface IModelBindingAdaptor {
        Task<bool> TryUpdateModelAsync<TModel>(ControllerBase controller, TModel model) where TModel : class;
    }
    
    public class ModelBindingAdaptor : IModelBindingAdaptor {
        public virtual Task<bool> TryUpdateModelAsync<TModel>(ControllerBase controller, TModel model) where TModel : class {
            return controller.TryUpdateModelAsync(model);
        }
    }
}
