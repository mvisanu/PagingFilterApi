using Microsoft.EntityFrameworkCore;
using PagingCustomer.Models;

namespace PagingCustomer.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }

       
    }
}
