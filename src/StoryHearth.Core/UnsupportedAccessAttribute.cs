using System;

namespace StoryHearth;

/// <summary>
///     Designates that an interface is unsupported for general use, that it is
///     not part of the implementing type's public contract, and that using the
///     interface may be risky (though not necessarily). Changing the interface
///     is not considered a breaking change as it is not considered to be a part
///     of the implementing type's public contract.
/// </summary>
/// <remarks>
///     <para>
///         Interfaces marked with the
///         <see cref="UnsupportedAccessAttribute">UnsupportedAccess
///         attribute</see> should be named with the `<c>UnsupportedAccess</c>`
///         suffix so that it is clear that they are unsupported without looking
///         at their metadata. To avoid confusion, types implementing them
///         should use explicit interface members so that those members can't be
///         accessed except through the interface, and won't appear in
///         Intellisense and similar systems. Exceptions can be made for members
///         that are also a part of the public interface (as the unsupported
///         interface may have a mix of supported and unsupported methods for
///         the convenience of its clients, but usage through the interface is
///         always considered unsupported).
///     </para>
///     <para>
///         Clients should avoid using unsupported interfaces unless essential
///         to the client's behavior. Usage is strictly at the client's own
///         risk. Automated testing is recommended to ensure that the
///         implementing type continues to behave as expected across versions.
///     </para>
///     <para>
///         <b>Important Note</b>: This attribute supports a design philosophy
///         that it is unnecessary to protect a type's internals from clients in
///         a trusted environment, but that it is necessary to define what the
///         public contract is (in this case, what is expected of the client).
///         When exposed to untrusted clients, other methods can be employed.
///         This philosophy makes development and testing easier at no cost
///         within a trusted environment.
///     </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
public class UnsupportedAccessAttribute : Attribute
{
}
