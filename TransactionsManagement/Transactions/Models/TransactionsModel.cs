using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transactions.Models
{
    public class TransactionsModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }

    public class TransactionsDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
    }
}
