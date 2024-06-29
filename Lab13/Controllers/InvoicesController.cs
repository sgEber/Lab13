using Lab13.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace Lab13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly StoreContext _context;

        public InvoicesController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoices>>> GetInvoices()
        {
            return await _context.Invoices.AsNoTracking().ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Invoices>> PostInvoice(Invoices model)
        {
            var newInvoice = new Invoices
            {
                CustomersID = model.CustomersID,
                Date = model.Date,
                InvoicesNumber = model.InvoicesNumber,
                Total = model.Total
            };

            _context.Invoices.Add(newInvoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvoices", new { id = newInvoice.InvoicesID }, newInvoice);
        }

    }
}
