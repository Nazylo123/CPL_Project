using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Model
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Cart")]
        public int CartId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("Size")]
        public int SizeId { get; set; }

        public int Quantity { get; set; }

        public Cart Cart { get; set; }
        public Product Product { get; set; }
        public Size Size { get; set; }
    }
}