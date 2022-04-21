using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;

namespace RandomizerHost.Extensions
{
    public static class EnumExtensions
    {
        public static String GetDescription(this Enum in_Value)
        {
            FieldInfo fieldInfo = in_Value?.GetType().GetField(in_Value.ToString());
            Attribute attribute = fieldInfo?.GetCustomAttribute(typeof(DescriptionAttribute));

            if (attribute is DescriptionAttribute descriptionAttribute)
            {
                return descriptionAttribute.Description;
            }
            else
            {
                return null;
            }
        }
    }
}
