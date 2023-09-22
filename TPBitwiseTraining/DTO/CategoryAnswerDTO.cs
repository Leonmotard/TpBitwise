using TPBitwiseTraining.Models;

namespace TPBitwiseTraining.DTO
{
    public class CategoryAnswerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public HashSet<ProductBrandDTO> Products { get; set; } = new HashSet<ProductBrandDTO>();
    }
}