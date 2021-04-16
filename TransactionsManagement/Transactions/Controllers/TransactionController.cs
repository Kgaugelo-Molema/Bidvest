using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Transactions.Services;

namespace Transactions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private ITransactionsService _transactionsService;
        public TransactionController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        /// <summary>
        /// List all transactions
        /// </summary>
        [HttpGet]
        public IActionResult GetAll()
        {
            var transactions = _transactionsService.GetAll();
            return Ok(transactions);
        }
    }
}