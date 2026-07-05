using System;

namespace StoryHearth;

/// <summary>
///     Use in conjunction with the
///     <see cref="UnsupportedAccessAttribute">UnsupportedAccess attribute</see>
///     to designate the intended clients of the unsupported interface. This
///     attribute is intended for readability and for use by future tooling.
/// </summary>
[AttributeUsage(AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
public class IntendedForUseByAttribute : Attribute
{
    public string ClassName { get; set; }

    public IntendedForUseByAttribute(string className)
    {
        ClassName = className;
    }
}
