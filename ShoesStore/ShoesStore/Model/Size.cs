namespace ShoesStore.Model
{
    public class Size
    {
        public int Id { get; set; }
        public string SizeName { get; set; }

        public List<ProductSize> ProductSizes { get; set; }
        public List<ProductSizeStock> ProductSizeStocks { get; set; }
    }

}
