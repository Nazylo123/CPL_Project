using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesStore.IRepository;

namespace ShoesStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailRepository emailRepository;
        public EmailController(IEmailRepository emailRepository)
        {
            this.emailRepository = emailRepository;
        }

        [HttpPost]
        public async Task <IActionResult> SendEmail(string receptor,  string subject, string body)
        {
            await emailRepository.SendEmail(receptor, subject, body);
            return Ok();
        }
    }
}
