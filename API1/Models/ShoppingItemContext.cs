using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace API1.Models
{
    public class ShoppingItemContext : DbContext
    {
        public ShoppingItemContext(DbContextOptions<ShoppingItemContext> options) : base(options) { }
        public DbSet<ShoppingItem> ShoppingItems { get; set; } = null!;
    }
}
