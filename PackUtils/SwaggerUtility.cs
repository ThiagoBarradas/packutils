using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;

namespace PackUtils
{
    public class SnakeEnumSchemaFilter : Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Enum?.Count > 0)
            {
                IList<IOpenApiAny> results = new List<IOpenApiAny>();
                var enumValues = Enum.GetValues(context.Type);
                foreach (var enumValue in enumValues)
                {
                    results.Add(new OpenApiString(enumValue.ToString().ToSnakeCase()));
                }

                schema.Enum = results;
            }
        }
    }

    public class CamelEnumSchemaFilter : Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Enum?.Count > 0)
            {
                IList<IOpenApiAny> results = new List<IOpenApiAny>();
                var enumValues = Enum.GetValues(context.Type);
                foreach (var enumValue in enumValues)
                {
                    results.Add(new OpenApiString(enumValue.ToString().ToCamelCase()));
                }

                schema.Enum = results;
            }
        }
    }

    public class LowerEnumSchemaFilter : Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Enum?.Count > 0)
            {
                IList<IOpenApiAny> results = new List<IOpenApiAny>();
                var enumValues = Enum.GetValues(context.Type);
                foreach (var enumValue in enumValues)
                {
                    results.Add(new OpenApiString(enumValue.ToString().ToLowerCase()));
                }

                schema.Enum = results;
            }
        }
    }
}
