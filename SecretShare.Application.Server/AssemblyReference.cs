using System.Reflection;

namespace SecretShare.Application.Server;

internal class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}