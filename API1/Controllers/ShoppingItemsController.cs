#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API1.Models;

namespace API1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingItemsController : ControllerBase
    {
        private readonly ShoppingItemContext _context;

        public ShoppingItemsController(ShoppingItemContext context)
        {
            _context = context;
        }

        
        [HttpGet("grandtotal")]
        public async Task<ActionResult<Double>> GetGrandTotal()
        {
            Object locker = new object();
            var shoppingItems = _context.ShoppingItems.AsEnumerable<ShoppingItem>;

            var sum = 0.0;
            await _context.ShoppingItems.ForEachAsync(item =>
            {
                sum += item.ItemTotalCost;
            });

            return sum;
        }

        // GET: api/ShoppingItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingItem>> GetShoppingItem(int id)
        {
            var shoppingItem = await _context.ShoppingItems.FindAsync(id);

            if (shoppingItem == null)
            {
                return NotFound();
            }

            return shoppingItem;
        }

        [HttpGet(Name = "GetShoppingList")]
        public IEnumerable<ShoppingItem> GetShoppingItems()
        {
            return _context.ShoppingItems;
        }

        /*[HttpGet("grandtotal")]
        public async IEnumerable<ShoppingItem> GetGrandTotal()
        {
            var total_cost = await _context.ShoppingItems.SumAsync(x => x.ItemTotalCost);
            return total_cost;
        }*/

        // PUT: api/ShoppingItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShoppingItem(int id, ShoppingItem shoppingItem)
        {
            if (id != shoppingItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(shoppingItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoppingItemExists(id))
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

        // POST: api/ShoppingItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ShoppingItem>> PostShoppingItem(ShoppingItem shoppingItem)
        {
            _context.ShoppingItems.Add(shoppingItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetShoppingItem), new { id = shoppingItem.Id }, shoppingItem);
        }

        // DELETE: api/ShoppingItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoppingItem(int id)
        {
            var shoppingItem = await _context.ShoppingItems.FindAsync(id);
            if (shoppingItem == null)
            {
                return NotFound();
            }

            _context.ShoppingItems.Remove(shoppingItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShoppingItemExists(int id)
        {
            return _context.ShoppingItems.Any(e => e.Id == id);
        }
    }
}
