using TPBitwiseTraining.Models;

namespace TPBitwiseTraining.DTO
{
    public class ProductCreationDTO
    {
        public string Name { get; set; } 
        public string Description { get; set; }
        public int BrandId { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public int Stock { get; set; }
    }
}
