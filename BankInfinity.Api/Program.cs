using BankInfinity.Api.Data;
using BankInfinity.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Подключаем поддержку контроллеров
builder.Services.AddControllers();

// ==========================================
// TODO: ЗАДАНИЕ 7. Подключение базы данных
// ==========================================
// EF Core должен знать, какую базу мы используем. 
// Раскомментируй этот блок, чтобы сказать приложению: "Используй SQLite и создай файл bankinfinity.db".

 builder.Services.AddDbContext<BankDbContext>(options =>
     options.UseSqlite("Data Source=bankinfinity.db"));


// ==========================================
// TODO: ЗАДАНИЕ 8. Внедрение зависимостей (DI)
// ==========================================
// Наш AccountsController требует AccountService в своем конструкторе.
// Мы должны зарегистрировать сервис, чтобы ASP.NET сам его создавал и передавал.
// Допиши метод: нужно использовать AddScoped.

// РАСКОММЕНТИРУЙ И ДОПИШИ:
builder.Services.AddScoped<AccountService>();


// Настройки Swagger (красивый интерфейс для тестирования API в браузере)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Включаем Swagger для режима разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Включаем маршрутизацию к нашим контроллерам
app.MapControllers();

// Запускаем сервер!
app.Run();