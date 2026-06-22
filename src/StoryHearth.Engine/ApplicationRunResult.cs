using System;
using System.Diagnostics.CodeAnalysis;

namespace StoryHearth.Engine;

public struct ApplicationRunResult
    : IEquatable<ApplicationRunResult>
{
    // failed run with clean exit
    public static readonly ApplicationRunResult Failure
        = new ApplicationRunResult(ApplicationRunResultValue.Failure);

    // successful run with clean exit
    public static readonly ApplicationRunResult Success
        = new ApplicationRunResult(ApplicationRunResultValue.Success);

    // exiting under unkown conditions; success unspecified (considered failure)
    public static readonly ApplicationRunResult Unknown
        = new ApplicationRunResult(ApplicationRunResultValue.Unknown);

    public bool IsFailure => _value == ApplicationRunResultValue.Failure;
    public bool IsSuccess => _value == ApplicationRunResultValue.Success;
    public bool IsUnknown => _value <= 0 || _value >= ApplicationRunResultValue.UpperBound;

    public static ApplicationRunResult Combine(ApplicationRunResult a,  ApplicationRunResult b)
    {
        if (a.IsUnknown || b.IsUnknown) return Unknown;
        if (a.IsFailure || b.IsFailure) return Failure;

        return Success;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is ApplicationRunResult other ? Equals(other) : false;
    }

    public bool Equals(ApplicationRunResult other)
    {
        return IsUnknown ? other.IsUnknown : _value == other._value;
    }

    public static bool Equals(ApplicationRunResult a, ApplicationRunResult b) => a.Equals(b);

    public override int GetHashCode()
    {
        return
            IsUnknown ?
            ApplicationRunResultValue.Unknown.GetHashCode() :
            _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value switch
        {
            ApplicationRunResultValue.Success => nameof(ApplicationRunResult) + "." + nameof(Success),
            ApplicationRunResultValue.Failure => nameof(ApplicationRunResult) + "." + nameof(Failure),
            _ => nameof(ApplicationRunResult) + "." + nameof(Unknown),
        };
    }

    public static bool operator ==(ApplicationRunResult a, ApplicationRunResult b) => Equals(a, b);
    public static bool operator !=(ApplicationRunResult a, ApplicationRunResult b) => !(a == b);

    private ApplicationRunResult(ApplicationRunResultValue value)
    {
        _value = value;
    }

    private readonly ApplicationRunResultValue _value;

    private enum ApplicationRunResultValue
    {
        Unknown = 0, // default

        Success,
        Failure,

        UpperBound,
    }
}
