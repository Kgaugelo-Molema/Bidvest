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
using System.Linq.Expressions;
using MoreLinq;

namespace Transactions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private ITransactionsService _transactionsService;
        private List<object> _results = new List<object>();

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

            var dtos = new List<TransactionsDto>();
            IMapper mapper;
            foreach (var transaction in transactions)
            {
                // map model to dto
                mapper = this.GetMapper<TransactionsModel, TransactionsDto>();
                var dto = mapper.Map<TransactionsDto>(transaction);
                dtos.Add(dto);
            }
            return Ok(dtos);
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
                var transaction = _transactionsService.Add(new TransactionsModel { Description = description, Amount = amount });
                return Ok(transaction);
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
        public IActionResult Save([FromBody]IEnumerable<TransactionsDto> data)
        {
            try
            {
                foreach (var t in data)
                {
                    if (!decimal.TryParse(t.Amount, out var amount))
                    {
                        logResult(BadRequest("Invalid amount entered"));
                        continue;
                    }

                    if (t.Id == 0)
                    {
                        var result = Add(t.Description, amount);
                        logResult(result);
                    }
                    else
                    {
                        var result = Edit(t.Id, t.Description, amount);
                        logResult(result);
                    }
                }
                return Ok(_results.DistinctBy(m => m?.ToString()));
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        private void logResult(IActionResult result)
        {
            if (result is OkObjectResult)
                _results.Add((result as OkObjectResult)?.Value);
            else
                _results.Add((result as BadRequestObjectResult)?.Value);
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