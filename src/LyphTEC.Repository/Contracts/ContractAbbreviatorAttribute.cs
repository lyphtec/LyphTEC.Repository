using System;
using System.Diagnostics;

namespace LyphTEC.Repository.Contracts
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [Conditional("CONTRACTS_FULL")]
    public sealed class ContractAbbreviatorAttribute : Attribute
    {
    }
}
