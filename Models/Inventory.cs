namespace product_inventory_management.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int StockQuantity { get; set; }
    }
}
