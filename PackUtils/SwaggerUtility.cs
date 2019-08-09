using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace PackUtils
{
    public class SnakeEnumSchemaFilter : Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter
    {
        public void Apply(Swashbuckle.AspNetCore.Swagger.Schema model, SchemaFilterContext context)
        {   
            model.Enum = model.Enum?.Select(p => p.ToString().ToSnakeCase()).ToArray();
        }
    }

    public class CamelEnumSchemaFilter : Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter
    {
        public void Apply(Swashbuckle.AspNetCore.Swagger.Schema model, SchemaFilterContext context)
        {
            model.Enum = model.Enum?.Select(p => p.ToString().ToCamelCase()).ToArray();
        }
    }

    public class LowerEnumSchemaFilter : Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter
    {
        public void Apply(Swashbuckle.AspNetCore.Swagger.Schema model, SchemaFilterContext context)
        {
            model.Enum = model.Enum?.Select(p => p.ToString().ToLowerCase()).ToArray();
        }
    }
}
