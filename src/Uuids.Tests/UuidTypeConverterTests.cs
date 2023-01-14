using System;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Reflection;
using NUnit.Framework;

namespace Uuids.Tests;

public class UuidTypeConverterTests
{
    [TestCase(typeof(string))]
    [TestCase(typeof(InstanceDescriptor))]
    public void CanConvertToCorrect(Type type)
    {
        UuidTypeConverter converter = new();
        Assert.True(converter.CanConvertTo(type));
    }

    [Test]
    public void CanConvertFromCorrect()
    {
        UuidTypeConverter converter = new();
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Test]
    public void CanConvertFromInt32()
    {
        UuidTypeConverter converter = new();
        Assert.False(converter.CanConvertFrom(typeof(int)));
    }

    [Test]
    public void ConvertNotUuidToStringWillCallOverrideToString()
    {
        string expectedValue = "133742";
        NotUuid notUuid = new(133742);
        UuidTypeConverter converter = new();

        object? actualValue = converter.ConvertTo(notUuid, typeof(string));

        Assert.NotNull(actualValue);
        Assert.IsInstanceOf<string>(actualValue);
        Assert.AreEqual(expectedValue, (string?) actualValue);
        Assert.AreEqual(1, notUuid.ToStringCalls);
    }

    [Test]
    public void ConvertToString()
    {
        string expectedValue = "28d2b480b9e743f48ee32ecf03247ad1";
        Uuid uuid = new("28d2b480-b9e7-43f4-8ee3-2ecf03247ad1");
        UuidTypeConverter converter = new();

        object? actualValue = converter.ConvertTo(uuid, typeof(string));

        Assert.NotNull(actualValue);
        Assert.IsInstanceOf<string>(actualValue);
        Assert.AreEqual(expectedValue, (string?) actualValue);
    }

    [Test]
    public void ConvertToInstanceDescriptor()
    {
        ConstructorInfo? uuidCtor = typeof(Uuid)!.GetConstructor(new[] { typeof(string) });
        InstanceDescriptor expectedValue = new(uuidCtor, new object[] { "ee753afdd98a45678de9740de0441987" });
        Uuid uuid = new("ee753afd-d98a-4567-8de9-740de0441987");
        UuidTypeConverter converter = new();

        object? actualValue = converter.ConvertTo(uuid, typeof(InstanceDescriptor));

        Assert.NotNull(actualValue);
        Assert.IsInstanceOf<InstanceDescriptor>(actualValue);
        InstanceDescriptor? actualDescriptor = (InstanceDescriptor?) actualValue;
        Assert.AreEqual(expectedValue.MemberInfo, actualDescriptor?.MemberInfo);
        Assert.AreEqual(expectedValue.IsComplete, actualDescriptor?.IsComplete);
        Assert.AreEqual(expectedValue.Arguments, actualDescriptor?.Arguments);
    }

    [Test]
    public void ConvertToInt32()
    {
        Uuid uuid = new("28d2b480-b9e7-43f4-8ee3-2ecf03247ad1");
        UuidTypeConverter converter = new();

        Assert.Throws<NotSupportedException>(() =>
        {
            object? _ = converter.ConvertTo(uuid, typeof(int));
        });
    }

    [Test]
    public void ConvertFromString()
    {
        Uuid expectedValue = new("28d2b480-b9e7-43f4-8ee3-2ecf03247ad1");
        UuidTypeConverter converter = new();

        object? actualValue = converter.ConvertFrom("28d2b480b9e743f48ee32ecf03247ad1");

        Assert.NotNull(actualValue);
        Assert.IsInstanceOf<Uuid>(actualValue);
        Assert.AreEqual(expectedValue, (Uuid) actualValue!);
    }

    [Test]
    public void ConvertFromValidInstanceDescriptor()
    {
        Uuid expectedValue = new("b28d9df8-fd78-429f-89c7-c669e82eb604");
        UuidTypeConverter converter = new();
        ConstructorInfo? uuidCtor = typeof(Uuid)!.GetConstructor(new[] { typeof(string) });
        InstanceDescriptor descriptor = new(uuidCtor, new object[] { "b28d9df8fd78429f89c7c669e82eb604" });

        object? actualValue = converter.ConvertFrom(descriptor);

        Assert.NotNull(actualValue);
        Assert.IsInstanceOf<Uuid>(actualValue);
        Assert.AreEqual(expectedValue, (Uuid) actualValue!);
    }

    [Test]
    public void ConvertFromInvalidInstanceDescriptor()
    {
        UuidTypeConverter converter = new();
        ConstructorInfo? guidCtor = typeof(Guid)!.GetConstructor(new[] { typeof(string) });
        InstanceDescriptor descriptor = new(guidCtor, new object[] { "b28d9df8fd78429f89c7c669e82eb604" });

        Assert.Throws<NotSupportedException>(() =>
        {
            object? _ = converter.ConvertFrom(descriptor);
        });
    }

    [Test]
    public void ConvertFromInt32()
    {
        UuidTypeConverter converter = new();

        Assert.Throws<NotSupportedException>(() =>
        {
            object? _ = converter.ConvertFrom(42);
        });
    }

    private sealed class NotUuid
    {
        public NotUuid(int id)
        {
            Id = id;
        }

        private int Id { get; }

        public int ToStringCalls { get; private set; }

        [DebuggerStepThrough]
        public override string ToString()
        {
            ToStringCalls++;
            return Id.ToString("D");
        }
    }
}
