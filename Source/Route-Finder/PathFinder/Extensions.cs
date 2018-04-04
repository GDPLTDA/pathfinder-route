using System;
using System.ComponentModel;
using System.Linq;

namespace PathFinder
{
    public static class Extensions
    {
        public static string GetDescription(this Enum @enum) =>
             @enum.GetType().GetMember(@enum.ToString())
                    .First()
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() is DescriptionAttribute atributoDescricao ?
                                atributoDescricao.Description : @enum.ToString();

    }
}
