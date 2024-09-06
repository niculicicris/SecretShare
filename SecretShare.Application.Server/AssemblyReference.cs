using System.Reflection;

namespace SecretShare.Application.Server;

public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}