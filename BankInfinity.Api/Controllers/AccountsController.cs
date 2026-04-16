using Microsoft.AspNetCore.Mvc;
using BankInfinity.Api.Services;

namespace BankInfinity.Api.Controllers
{
    // Эти атрибуты говорят, что это не обычный класс, а веб-контроллер
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
            // Обращаемся к нашему сервису, который ты только что написал
            var result = _accountService.GetBalance(id);

            // ==========================================
            // TODO: ЗАДАНИЕ 6. HTTP-ответы
            // ==========================================
            // Напиши условие if-else:
            // Если result.IsSuccess равно true, верни Ok(result.Data);
            // Если false, верни BadRequest(result.ErrorMessage);
            if (result.IsSuccess == true)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }
    }
}