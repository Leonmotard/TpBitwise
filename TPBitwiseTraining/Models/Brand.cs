namespace TPBitwiseTraining.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public HashSet<Product> Products { get; set; } = new HashSet<Product>();
    }
}