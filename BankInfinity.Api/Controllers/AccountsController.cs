using Microsoft.AspNetCore.Mvc;
using BankInfinity.Api.Services;

namespace BankInfinity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountsController(AccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: api/accounts/5/balance
        [HttpGet("{id}/balance")]
        public IActionResult GetBalance(int id)
        {
  
            var result = _accountService.GetBalance(id);

            // TODO:  Проверь результат:
            // Если result.IsSuccess == true, верни Ok(result.Data) (это HTTP 200)
            // Иначе верни BadRequest(result.ErrorMessage) (это HTTP 400)
            
            return BadRequest("Эндпоинт в разработке");
        }
    }
}