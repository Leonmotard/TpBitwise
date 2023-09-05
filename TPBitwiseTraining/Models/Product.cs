namespace TPBitwiseTraining.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Brand Brand { get; set; } = null!;
        public double Price { get; set; }
        public Category Category { get; set; } = null!;
        
    }
}
