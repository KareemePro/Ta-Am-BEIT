using Firebase.Auth;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoodDelivery.Enums;

//public class EnumConverter : JsonConverter<Enum>
//{
//    public override Enum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//    {
//        throw new NotImplementedException();
//    }

//    public override void Write(Utf8JsonWriter writer, Enum value, JsonSerializerOptions options)
//    {
//        writer.WriteStringValue(value.ToEnumString());
//    }
//}

//public class EnumSchemaFilter : ISchemaFilter
//{
//    public void Apply(OpenApiSchema model, SchemaFilterContext context)
//    {
//        if (context.Type.IsEnum)
//        {
//            model.Enum.Clear();
//            var names = Enum.GetNames(context.Type);
//            foreach (var name in names)
//            {
//                model.Enum.Add(new OpenApiString(name));
//            }
//        }
//    }


//}
