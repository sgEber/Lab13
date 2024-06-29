using Lab13.Models;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("InsertInvoice")]
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


        [HttpPost("InsertInvoiceList")]
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

                return CreatedAtAction(nameof(InsertInvoiceList), new { message = "Invoices inserted correctly" }, invoiceList);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, title: "An error occurred while inserting invoices");
            }
        }

        [HttpPost("InsertInvoiceDetail")]
        public async Task<ActionResult<List<Details>>> InsertInvoiceDetail([FromBody] InvoicesRequest10 invoicesRequest10)
        {
            try
            {
                var invoice = await _context.Invoices.FindAsync(invoicesRequest10.InvoicesID);

                if (invoice == null)
                {
                    return NotFound(new { message = "Invoice not found" });
                }

                var details = invoicesRequest10.InvoiceDetail.ToList();
                List<Details> newDetails = new();

                foreach (var invoiceDetail in details)
                {
                    Details detail = new()
                    {
                        InvoicesID = invoice.InvoicesID
                    };

                    int idProduct = invoiceDetail.ProductsID;
                    var product = await _context.Products.FindAsync(idProduct);

                    if (product == null)
                    {
                        return Problem(detail: "Product not found", title: "An error occurred while inserting invoice details");
                    }

                    detail.ProductsID = invoiceDetail.ProductsID;
                    detail.Price = product.Price;
                    detail.Amount = invoiceDetail.Amount;
                    detail.Subtotal = detail.Amount * detail.Price;

                    _context.Details.Add(detail);
                    newDetails.Add(detail);
                    invoice.Total += detail.Subtotal;
                }

                _context.Invoices.Update(invoice);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(InsertInvoiceDetail), new { id = invoice.InvoicesID }, newDetails);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, title: "An error occurred while inserting invoice details");
            }
        }

    }
}
