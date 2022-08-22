namespace Catalog.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public string Image { get; set; } = "";
    }
}
