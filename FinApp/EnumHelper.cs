using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FinApp
{
    public static class EnumHelper
    {
        public static string GetDescription(this Enum value)
        {
            if (value == null) return string.Empty;

            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();

            return attribute?.Description ?? value.ToString();
        }

        public static List<ValueDescription> GetAllValuesAndDescriptions(Type t)
        {

            return Enum.GetValues(t)
                      .Cast<Enum>()
                      .Select(e => new ValueDescription
                      {
                          Value = e,
                          Description = (e as Enum).GetDescription()
                      })
                      .ToList();
        }

        public class ValueDescription
        {

            public object Value { get; set; }
            public object Description { get; set; }
        }
    }
}
