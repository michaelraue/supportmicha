namespace SupportMicha.WpfUi.Shared;

using System.Globalization;
using System.Windows.Data;
using SupportMicha.ApiInterface;

/// <summary>
/// Converts an enumeration value to its display name defined by the <see cref="DisplayNameAttribute"/>
/// applied to the enumeration field. If no such attribute is found, the original name of the enumeration
/// value is used.
/// </summary>
public class EnumDisplayNameConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }

        var type = value.GetType();
        var name = Enum.GetName(type, value);
        if (name == null)
        {
            return name;
        }

        var field = type.GetField(name);
        if (field == null)
        {
            return name;
        }

        var attr = (DisplayNameAttribute?)field.GetCustomAttributes(typeof(DisplayNameAttribute), false)
            .FirstOrDefault();
        return attr != null ? attr.DisplayName : name;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new InvalidOperationException();
    }
}