using CustomerWebApi.Data;
using CustomerWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerWebApi.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {

        private readonly CustomerWebApi.Data.CustomerDbContext _customerDbContext;

        public CustomerController(CustomerWebApi.Data.CustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext;
        }



        [HttpGet]
        // GET: CustomerController
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            return _customerDbContext.Customers;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetbyId(int id)
        {
            var customer = await _customerDbContext.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }



        [HttpPost]
        public async Task<ActionResult<Customer>> Create(Customer customer)
        {
            await _customerDbContext.Customers.AddAsync(customer);
            await _customerDbContext.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        [HttpPut("{id}")]   
        public async Task<IActionResult> Update(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            _customerDbContext.Entry(customer).State = EntityState.Modified;
            await _customerDbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerDbContext.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _customerDbContext.Customers.Remove(customer);
            await _customerDbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
