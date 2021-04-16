using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactions.Helpers;
using Transactions.Models;

namespace Transactions.Services
{
    public class TransactionsService : ITransactionsService
    {
        private SqlDataContext _context;

        public TransactionsService (SqlDataContext context)
        {
            _context = context;
        }

        public IEnumerable<TransactionsModel> GetAll()
        {
            return _context.Transactions;
        }
    }
}
