using Lab13.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Lab13.Models;
using Lab13.Models.Request;

namespace Lab13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesControllerCR : ControllerBase
    {
        private readonly StoreContext _context;
        public InvoicesControllerCR(StoreContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Invoices>> InsertInvoice([FromBody] InvoicesRequest5 invoicesRequest5)
        {
            var customer = await _context.Customers.FindAsync(invoicesRequest5.CustomersID);

            if (customer == null)
            {
                return NotFound();
            }

            Invoices invoices = new();
            invoices.CustomersID = invoicesRequest5.CustomersID;
            invoices.Date = invoicesRequest5.Date;
            invoices.InvoicesNumber = invoicesRequest5.InvoicesNumber;
            invoices.Total = invoicesRequest5.Total;
            invoices.Customer = customer;

            _context.Invoices.Add(invoices);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(InsertInvoice), new { id = invoices.InvoicesID }, invoices);
        }

        [HttpPost]
        public async Task<ActionResult<List<Invoices>>> InsertInvoiceList([FromBody] InvoicesRequest8 invoicesRequest8)
        {
            try
            {
                var customers = await _context.Customers.FindAsync(invoicesRequest8.CustomersID);

                if (customers == null)
                {
                    return NotFound(new { message = "Customer not found" });
                }

                List<Invoices> invoiceList = invoicesRequest8.ListInvoice.Select(x => new Invoices
                {
                    Date = x.Date,
                    Total = x.Total,
                    InvoicesNumber = x.InvoicesNumber,
                    CustomersID = customers.CustomersID,
                }).ToList();

                _context.Invoices.AddRange(invoiceList);
                await _context.SaveChangesAsync();

                return CreatedAtAction("InsertInvoiceList", new { message = "Invoices inserted correctly" }, invoiceList);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, title: "An error occurred while inserting invoices");
            }
        }


    }
}
