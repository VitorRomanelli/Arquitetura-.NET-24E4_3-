using QuestionnaireSystem.Application.Services.Interfaces;
using QuestionnaireSystem.Application.Services;
using QuestionnaireSystem.Persistence.Repositories.Interfaces;
using QuestionnaireSystem.Persistence.Repositories;
using QuestionnaireSystem.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connection = builder.Configuration.GetConnectionString("localMySqlConnection")!;
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

builder.Services.AddScoped<IQuestionnaireRepository, QuestionnaireRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserAnswerRepository, UserAnswerRepository>();
builder.Services.AddScoped<IQuestionnaireService, QuestionnaireService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserAnswerService, UserAnswerService>();

builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddXmlSerializerFormatters();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
