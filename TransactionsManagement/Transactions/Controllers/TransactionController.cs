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
        /// <param name="amount">
        /// ZAR
        /// </param>
        [HttpGet]
        public IActionResult GetAll()
        {
            var transactions = _transactionsService.GetAll();
            return Ok(transactions);
        }

        /// <summary>
        /// Add transaction
        /// </summary>
        /// <param name="amount">
        /// ZAR
        /// </param>
        [HttpPost("add")]
        public IActionResult Add([FromQuery]string description, [FromQuery]decimal amount)
        {
            try
            {
                // validation
                if (string.IsNullOrWhiteSpace(description))
                    throw new AppException("Description is required");

                // add transaction
                _transactionsService.Add(new TransactionsModel { Description = description, Amount = amount });
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Save transactions
        /// </summary>
        /// <param name="amount">
        /// ZAR
        /// </param>
        [HttpPost("save")]
        public IActionResult Save([FromBody]IEnumerable<TransactionsModel> data)
        {
            try
            {
                return Ok(data);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Edit transaction
        /// </summary>
        /// <param name="id">
        /// The Id is required
        /// </param>
        [HttpPut("edit")]
        public IActionResult Edit([FromQuery]int id, [FromQuery]string description, [FromQuery]decimal amount)
        {
            try
            {
                if (id == 0)
                    throw new AppException("ID is required");

                // edit transaction
                _transactionsService.Add(new TransactionsModel { Id = id, Description = description, Amount = amount });
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