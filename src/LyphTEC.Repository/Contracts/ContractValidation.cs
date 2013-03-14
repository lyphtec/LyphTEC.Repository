using System;
using System.Diagnostics.Contracts;

namespace LyphTEC.Repository.Contracts
{
    public static class ContractValidation
    {
        [ContractAbbreviator]
        public static void IsNotNullOrWhiteSpace(string parameter)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(parameter));
        }

        [ContractAbbreviator]
        public static void IsNotNull(object parameter)
        {
            Contract.Requires<ArgumentNullException>(parameter != null);
        }
    }
}
