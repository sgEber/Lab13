using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab13.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext _context;
        public ProductsController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProducts(int ProductsID)
        {
            var products = await _context.Products.FindAsync(ProductsID);

            if (products == null)
            {
                return NotFound();
            }

            return products;
        }

        [HttpPost]
        public async Task<ActionResult<Products>> PostProducts(Products product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostProducts), new { id = product.ProductsID }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducts(Products products)
        {
       

            _context.Entry(products).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.ProductsID == products.ProductsID))
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
        public async Task<IActionResult> DeleteProducts(int ProductsID)
        {
            var products = await _context.Products.FindAsync(ProductsID);
            if (products == null)
            {
                return NotFound();
            }

            _context.Products.Remove(products);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}