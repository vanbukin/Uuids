using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Uuids;

/// <summary>
///     Converter that used to convert between <see cref="Uuid" /> structure and another data types.
/// </summary>
public class UuidTypeConverter : TypeConverter
{
    private static readonly ConstructorInfo UuidStringCtor = typeof(Uuid)
        .GetTypeInfo()
        .DeclaredConstructors
        .Single(x =>
        {
            ParameterInfo[] parameters = x.GetParameters();
            return parameters.Length == 1 && parameters[0].ParameterType == typeof(string);
        });

    /// <inheritdoc />
    public override bool CanConvertFrom(
        ITypeDescriptorContext? context,
        Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    /// <inheritdoc />
    public override bool CanConvertTo(
        ITypeDescriptorContext? context,
        Type? destinationType)
    {
        return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
    }

    /// <inheritdoc />
    public override object? ConvertFrom(
        ITypeDescriptorContext? context,
        CultureInfo? culture,
        object value)
    {
        return value switch
        {
            string text => new Uuid(text),
            InstanceDescriptor descriptor when descriptor.MemberInfo == UuidStringCtor => descriptor.Invoke(),
            InstanceDescriptor => throw GetConvertFromException(value),
            _ => base.ConvertFrom(context, culture, value)
        };
    }

    /// <inheritdoc />
    public override object? ConvertTo(
        ITypeDescriptorContext? context,
        CultureInfo? culture,
        object? value,
        Type destinationType)
    {
        if (value is not Uuid uuidValue)
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }

        if (destinationType == typeof(string))
        {
            return uuidValue.ToString("N", null);
        }

        if (destinationType == typeof(InstanceDescriptor))
        {
            return new InstanceDescriptor(UuidStringCtor, new object[] { uuidValue.ToString("N", null) });
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}
