using System;

// Stub to replace Code Contracts

namespace LyphTEC.Repository;

/// <summary>
/// Use to check method parameters with similar syntax as <see cref="System.Diagnostics.Contracts.Contract"/> class as Code Contracts are not supported
/// </summary>
public static class Contract
{
    public static void Requires<TException>(bool condition) where TException : Exception
    {
        if (condition)
            return;

        Requires<TException>(false, string.Empty);
    }

    public static void Requires<TException>(bool condition, string userMessage) where TException : Exception
    {
        if (condition)
            return;

        var t = typeof (TException);
        var ex = string.IsNullOrWhiteSpace(userMessage)
                     ? Activator.CreateInstance(t)
                     : Activator.CreateInstance(t, userMessage);

        throw (TException)ex;
    }

    public static void IsNotNull(object arg, string paramName = "")
    {
        if (arg is null)
            throw new ArgumentNullException(paramName);

        if (arg is string s && string.IsNullOrWhiteSpace(s))
            throw new ArgumentNullException(paramName);
    }

    public static void IsNotNullOrDefault<T>(T arg, string paramName = "")
    {
        IsNotNull(arg, paramName);

        if (ReferenceEquals(arg, default(T)))
            throw new ArgumentNullException(paramName);
    }

    public static void IsNotNullOrWhiteSpace(string arg, string paramName = "")
    {
        IsNotNull(arg, paramName);
    }
}
