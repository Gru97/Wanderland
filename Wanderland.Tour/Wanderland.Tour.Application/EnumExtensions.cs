using System.ComponentModel;

namespace Wanderland.Tour.Application;

public static class EnumExtensions
{
    public static string GetDescription(this Enum enumValue)
    {
        var descriptionAttribute = enumValue.GetType()
            .GetField(enumValue.ToString())
            .GetCustomAttributes(false)
            .SingleOrDefault(attr => attr.GetType() == typeof(DescriptionAttribute)) as DescriptionAttribute;

        // return description
        return descriptionAttribute?.Description ?? "";
    }
}