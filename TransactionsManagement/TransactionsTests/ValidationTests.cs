using System;
using System.Collections.Generic;
using System.Text;
using Transactions.Services;
using Xunit;

namespace TransactionsTests
{
    public class ValidationTests
    {
        [Theory]
        [InlineData(7, false)]
        [InlineData(10, true)]
        [InlineData(15, true)]
        public void GivenAmountValidateDenomination(decimal amount, bool isValid)
        {
            bool result = TransactionValidator.ValidateDenomination(null, amount);
            Assert.Equal(isValid, result);
        }
    }
}
