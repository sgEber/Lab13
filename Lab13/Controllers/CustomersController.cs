using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab13.Models;

namespace Lab13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly StoreContext _context;
        public CustomersController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customers>>> GetCustomers()
        {
            return await _context.Customers.AsNoTracking().ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customers>> GetCustomers(int CustomersID)
        {
            var customers = await _context.Customers.FindAsync(CustomersID);

            if (customers == null)
            {
                return NotFound();
            }

            return customers;
        }

        [HttpPost]
        public async Task<ActionResult<Customers>> PostCustomers(Customers customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostCustomers), new { id = customer.CustomersID }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducts(Customers customers)
        {


            _context.Entry(customers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Customers.Any(e => e.CustomersID == customers.CustomersID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomers(int CustomersID)
        {
            var customers = await _context.Customers.FindAsync(CustomersID);
            if (customers == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customers);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost("RequestPostCustomer")]
        public async Task<ActionResult<Customers>> PostCustomer(Customers model)
        {
            var newCustomer = new Customers
            {
                FirstName = model.FirstName,
                lAstName = model.lAstName,
                DocumentNumber = model.DocumentNumber
            };

            _context.Customers.Add(newCustomer);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetCustomers", new { id = newCustomer.CustomersID }, newCustomer);
        }
    }
}
