using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Вбудована підтримка OpenAPI в .NET 9
builder.Services.AddOpenApi();

// Реєструємо наші сервіси СППР
builder.Services.AddSingleton<InvestmentDecisionDss.Data.DssContext>();
builder.Services.AddTransient<InvestmentDecisionDss.Services.IDssEngine, InvestmentDecisionDss.Services.DssEngineService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Генерує специфікацію API
    app.MapOpenApi();
    
    // Підключає сучасний графічний інтерфейс (заміна Swagger)
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("СППР Інвестицій API");
    });
}

app.UseAuthorization();
app.MapControllers();

app.Run();