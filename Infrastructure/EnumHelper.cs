using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Infrastructure
{
    public static class EnumHelper
    {
        /// <summary>
        /// 获取枚举描述值
        /// </summary>
        /// <returns></returns>
        public static string ToDescription(this Enum enumValue)
        {
            string str = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;
            DescriptionAttribute da = (DescriptionAttribute)objs[0];
            return da.Description;
        }

        public static Dictionary<int, string> ToDictionary(Type enumType)
        {
            Dictionary<int, string> listitem = new Dictionary<int, string>();
            Array vals = Enum.GetValues(enumType);
            foreach (Enum enu in vals)
            {
                listitem.Add(Convert.ToInt32(enu), enu.ToDescription());
            }
            return listitem;
        }
    }
}
