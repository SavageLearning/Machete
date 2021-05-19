using Machete.Web.ViewModel.Api;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Machete.Web.Controllers.Api
{
    /// <summary>
    /// Adds the ViewModel.Api.PaginationMetaData model to the swagger documentation
    /// </summary>
    public class PaginationMetaDataSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            context.SchemaGenerator.GenerateSchema(typeof(PaginationMetaData), context.SchemaRepository);
        }
    }
}
