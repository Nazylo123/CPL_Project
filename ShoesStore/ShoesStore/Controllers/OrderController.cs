using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesStore.IRepository;
using ShoesStore.Model;
using ShoesStore.ViewModel;
using ShoesStore.ViewModel.RequestModel;

namespace ShoesStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrderController(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        // Lấy danh sách tất cả các đơn hàng
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> GetOrders()
        {
            try
            {
                var orders = await _ordersRepository.GetAllOrdersAsync();
                if (orders == null || !orders.Any())
                {
                    return NotFound("No orders found.");
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Lấy thông tin đơn hàng theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderViewModel>> GetOrderById(int id)
        {
            try
            {
                var order = await _ordersRepository.GetOrderByIdAsync(id);

                if (order == null)
                {
                    return NotFound($"Order with ID {id} not found.");
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Cập nhật đơn hàng
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderViewModel orderViewModel)
        {
            try
            {
                var existingOrder = await _ordersRepository.GetOrderByIdAsync(id);
                if (existingOrder == null)
                    return NotFound();

                await _ordersRepository.UpdateOrderAsync(orderViewModel);
                return Ok(orderViewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Xóa đơn hàng
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var result = await _ordersRepository.DeleteOrderAsync(id);
                if (result)
                {
                    return Ok("Order deleted successfully.");
                }
                return NotFound($"Order with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
