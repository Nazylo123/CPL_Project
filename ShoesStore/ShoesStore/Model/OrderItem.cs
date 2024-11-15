using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Model
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("Size")]
        public int SizeId { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; } // Giá tại thời điểm mua

        public Order Order { get; set; }
        public Product Product { get; set; }
        public Size Size { get; set; }
    }
}
