using Lab13.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Lab13.Models.Request;

namespace Lab13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersControllerCR : ControllerBase
    {
        private readonly StoreContext _context;
        public CustomersControllerCR(StoreContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Customers>> InsertCustomer(CustomersRequest3 customersRequest3)
        {
            Customers customers = new();
            customers.FirstName = customersRequest3.FirstName;
            customers.DocumentNumber = customersRequest3.DocumentNumber;
            customers.lAstName = customersRequest3.LastName;

            if (_context.Customers == null)
            {
                return Problem("Entity set 'MarketContext.Customers'  is null.");
            }
            _context.Customers.Add(customers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("InsertCustomer", new { id = customers.CustomersID }, customers);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(CustomersRequest4 customersRequest4)
        {
            var id = customersRequest4.CustomersID;

            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPut]
        public async Task<ActionResult<Customers>> UpdateCustomerDocumentNumberAndEmail([FromBody] CustomersRequest6 customersRequest6)
        {
            var customer = await _context.Customers.FindAsync(customersRequest6.CustomersID);

            if (customer == null)
            {
                return NotFound();
            }

            customer.DocumentNumber = customersRequest6.DocumentNumber;

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("UpdateCustomerDocumentNumberAndEmail", new { id = customer.CustomersID }, customer);
        }
    }
}
