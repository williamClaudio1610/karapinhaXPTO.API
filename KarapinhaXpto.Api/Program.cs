using KarapinhaXpto.DAL;
using KarapinhaXpto.DAL.Repositories;
using KarapinhaXpto.Model;
using KarapinhaXpto.Services;
using KarapinhaXpto.Shared.IRepositories;
using KarapinhaXpto.Shared.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAllOrigins",
		builder =>
		{
			builder.AllowAnyOrigin()
				   .AllowAnyMethod()
				   .AllowAnyHeader();
		});
});

// Configurar Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Registrar dependências
builder.Services.AddTransient<ICategoriaRepositorio, CatriaRepositorio>();
builder.Services.AddTransient<ICategoriaServices, CategoriaService>();
builder.Services.AddTransient<IUtilizadorServices, UtilizadorService>();
builder.Services.AddTransient<IUtilizadorRepositorio, UtilizadorRepositorio>();
builder.Services.AddTransient<EmailService>();
builder.Services.AddScoped<IPasswordHasher<Utilizador>, PasswordHasher<Utilizador>>();
builder.Services.AddTransient<ITokenServices, TokenService>();
builder.Services.AddTransient<IProfissionaisServices, ProfissionaisService>();
builder.Services.AddTransient<IProfissionaisRepositorio, ProfissionaisRepositorio>();
builder.Services.AddTransient<IServicoServices, ServicoService>();
builder.Services.AddTransient<IServicoRepositorio, ServicoRepositorio>();
builder.Services.AddTransient<IHorarioService, HorarioService>();
builder.Services.AddTransient<IHorarioRepositorio, HorarioRepositorio>();
builder.Services.AddScoped<HorarioService>(); // Registrar o serviço HorarioService

builder.Services.AddTransient<IMarcacoesServices, MarcacaoService>();
builder.Services.AddTransient<IMarcacaoRepositorio<ServicoMarcacao>, MarcacoesRepositorio<ServicoMarcacao>>();
builder.Services.AddTransient<IMarcacaoRepositorio<Marcacao>, MarcacoesRepositorio<Marcacao>>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	options.JsonSerializerOptions.Converters.Add(new KarapinhaXpto.Shared.Responses.TimeOnlyConverter());
	options.JsonSerializerOptions.Converters.Add(new KarapinhaXpto.Shared.Responses.DateOnlyConverter());
});

// Configurar DbContext
var conn = builder.Configuration.GetConnectionString("Conn");
builder.Services.AddDbContext<KarapinhaXptoDbContext>(options =>
	options.UseSqlServer(conn, sqlOptions =>
		sqlOptions.MigrationsAssembly(typeof(KarapinhaXptoDbContext).Assembly.FullName)));

// Adicionar autenticação JWT
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];
var key = Encoding.UTF8.GetBytes(jwtSecretKey);

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.RequireHttpsMetadata = false;
	options.SaveToken = true;
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(key),
		ValidateIssuer = false,
		ValidateAudience = false
	};
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configurar o pipeline de requisição HTTP.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseDeveloperExceptionPage();
}

app.UseCors("AllowAllOrigins");

app.UseRouting();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
