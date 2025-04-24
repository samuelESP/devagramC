using devagramC.Models;
using devagramC.repository;
using devagramC.repository.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//Conexão com o banco
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DevagramContext>(option => option.UseSqlServer(connectionString));



builder.Services.AddScoped<IUsuarioRepository, UsuarioRepositoryImpl>();

//Chave criptografada
var chaveCriptografia = Encoding.ASCII.GetBytes(ChaveJWT.ChaveSecreta);
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(autenticacao =>
{
    autenticacao.RequireHttpsMetadata = false;
    autenticacao.SaveToken = true;
    autenticacao.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(chaveCriptografia),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
    



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
