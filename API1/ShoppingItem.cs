namespace API1.Models
{
    public class ShoppingItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UnitCostDollars { get; set; }
        public int UnitCostCents { get; set; }
        public int Quantity { get; set; }
        public double ItemTotalCost => ((UnitCostDollars * 100 + UnitCostCents) * Quantity) / 100.0;
    }
}
