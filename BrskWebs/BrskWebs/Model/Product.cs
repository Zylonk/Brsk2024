namespace BrskWebs.Model
{
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public int? StockQuantity { get; set; }
    }
}
