namespace TPBitwiseTraining.DTO
{
    public class BrandAnswerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public HashSet<ProductBrandDTO>  Products { get; set; } = new HashSet<ProductBrandDTO>();
    }
}
