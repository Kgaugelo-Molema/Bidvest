using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Transactions.Models;
using Transactions.Services;
using AutoMapper;
using Transactions.Helpers;

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

        /// <summary>
        /// Add transaction
        /// </summary>
        [HttpPost("add")]
        public IActionResult Add([FromQuery]string description, [FromQuery]decimal amount)
        {
            try
            {
                // create user
                _transactionsService.Add(new TransactionsModel { Description = description, Amount = amount });
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}