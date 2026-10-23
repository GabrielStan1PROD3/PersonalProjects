using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Converters
{
    public class EnumToStringConverter<TEnum> : ValueConverter<TEnum, string>
       where TEnum :struct, Enum
    {
        public EnumToStringConverter() : base
        (
           value => value.ToString(),
           value => GetValue(value)
           //value => (TEnum)Enum.Parse(typeof(TEnum), value)
        )

        {

        }

        private static TEnum GetValue(string value) 
        {
            if (Enum.TryParse<TEnum>(value,ignoreCase:true, out var result)) 
            {
                return result;
            }

            throw new DbConversionException("We couldnt Convert your value ");
        }
    }
}

