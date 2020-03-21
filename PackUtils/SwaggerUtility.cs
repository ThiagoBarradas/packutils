using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PackUtils
{
    public static class SwaggerEnum
    {
        public static List<string> Enums;

        public static void Apply(OpenApiSchema schema, SchemaFilterContext context, string jsonSerializerCase)
        {
            if (schema.Enum?.Count > 0)
            {
                IList<IOpenApiAny> results = new List<IOpenApiAny>();
                var enumValues = Enum.GetValues(context.Type);
                foreach (var enumValue in enumValues)
                {
                    var enumValueString = enumValue.ToString().ToCase(jsonSerializerCase);
                    if (Enums?.Contains(enumValueString) == true)
                    {
                        continue;
                    }

                    results.Add(new OpenApiString(enumValueString));
                }

                schema.Type = "string";
                schema.Format = null;
                schema.Enum = results;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class SwaggerExcludeAttribute : Attribute
    {
    }

    public class SwaggerExcludeFilter : Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null || context.Type == null)
                return;

            var excludedProperties = context.Type.GetProperties()
                 .Where(t => t.GetCustomAttributes().Any(r => r.GetType() == typeof(SwaggerExcludeAttribute)));

            foreach (var excludedProperty in excludedProperties)
            {
                if (schema.Properties.ContainsKey(excludedProperty.Name))
                {
                    schema.Properties.Remove(excludedProperty.Name);
                }
            }
        }
    }

    public class SnakeEnumSchemaFilter : Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            SwaggerEnum.Apply(schema, context, "snakecase");
        }
    }

    public class CamelEnumSchemaFilter : Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            SwaggerEnum.Apply(schema, context, "camelcase");
        }
    }

    public class LowerEnumSchemaFilter : Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            SwaggerEnum.Apply(schema, context, "lowercase");
        }
    }
}
