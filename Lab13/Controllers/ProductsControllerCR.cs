using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab13.Models;
using Lab13.Models.Request;

namespace Lab13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsControllerCR : ControllerBase
    {
        private readonly StoreContext _context;
        public ProductsControllerCR(StoreContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Products>> InsertProduct([FromBody] ProductRequest1 requestProduct)
        {
            Products products = new();
            products.Name = requestProduct.Name;
            products.Price = requestProduct.Price;

            if (_context.Products == null)
            {
                return Problem("Entidad 'StoreContext.Products' es nulo .");
            }
            _context.Products.Add(products);
            await _context.SaveChangesAsync();

            return CreatedAtAction("InsertProduct", new { id = products.ProductsID }, products);
        }

        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct([FromBody] ProductRequest2 requestProduct)
        {
            var id = requestProduct.ProductID;

            if (_context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult<Products>> UpdateProductPrice([FromBody] ProductRequest7 productRequest7)
        {
            var products = await _context.Products.FindAsync(productRequest7.ProductID);

            if (products == null)
            {
                return NotFound();
            }

            products.Price = productRequest7.Price;

            _context.Products.Update(products);
            await _context.SaveChangesAsync();

            return CreatedAtAction("UpdateProductPrice", new { id = products.ProductsID }, products);
        }

        [HttpDelete("DeleteProductList")]
        public async Task<IActionResult> DeleteProductList(List<ProductRequest2> productrequest2)
        {

            if (_context.Products == null)
            {
                return NotFound();
            }

            foreach (var productID in productrequest2)
            {
                var product = await _context.Products.FindAsync(productID.ProductID);
                if (product == null)
                {
                    return NotFound();
                }
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
