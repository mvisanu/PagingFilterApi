using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagingCustomer.Contexts;
using PagingCustomer.Filter;
using PagingCustomer.Helpers;
using PagingCustomer.Models;
using PagingCustomer.Services;
using PagingCustomer.Wrappers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PagingCustomer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUriService _uriService;
        public CustomersController(ApplicationDbContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Customers
                .Where(e => e.FirstName.Contains(filter.SearchText ?? String.Empty)
                || e.LastName.Contains(filter.SearchText ?? String.Empty)
                || e.Email.Contains(filter.SearchText ?? String.Empty)
               )
               .OrderBy(e => e.LastName)
               .ThenBy(e => e.FirstName)
               .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
               .Take(validFilter.PageSize)
               .ToListAsync()
               
               
               ;
            var totalRecords = await _context.Customers.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Customer>(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _context.Customers.Where(a => a.Id == id).FirstOrDefaultAsync();
            return Ok(new Response<Customer>(customer));
        }
    }
}
