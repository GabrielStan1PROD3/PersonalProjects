using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple =false,Inherited =true)]
    public class KeyIdentifierAttribute : Attribute
    {
    }
}
