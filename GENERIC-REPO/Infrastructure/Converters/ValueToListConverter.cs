using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Converters
{
    public class ValueToListConverter<TValue> : ValueConverter<ICollection<TValue>, string>
    {
        public ValueToListConverter() : base
            (
            v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
            v => JsonSerializer
            .Deserialize<ICollection<TValue>>
           (v, new JsonSerializerOptions())
                        ??
             new List<TValue>())
        { }
           
        


    }
}
