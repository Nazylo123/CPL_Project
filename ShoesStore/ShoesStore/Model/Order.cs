using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApi.Models;
using System.Text.Json.Serialization;

namespace ShoesStore.Model
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; } 
        public string Status { get; set; }
	
		public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public AppUser User { get; set; }
    }
}
