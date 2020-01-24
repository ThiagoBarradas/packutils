using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace PackUtils
{
    public class SnakeEnumSchemaFilter : Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            schema.Enum = schema.Enum.Select(r => OpenApiAnyFactory.CreateFor(schema, r.ToString().ToSnakeCase())).ToArray();
        }
    }

    public class CamelEnumSchemaFilter : Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            schema.Enum = schema.Enum.Select(r => OpenApiAnyFactory.CreateFor(schema, r.ToString().ToCamelCase())).ToArray();
        }
    }

    public class LowerEnumSchemaFilter : Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            schema.Enum = schema.Enum.Select(r => OpenApiAnyFactory.CreateFor(schema, r.ToString().ToLowerCase())).ToArray();
        }
    }
}
