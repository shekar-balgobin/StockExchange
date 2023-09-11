using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Tyl.StockExchange.PricingEngine.Model.MsSqlServer;

AppDomain.CurrentDomain.Load(assemblyString: "Tyl.StockExchange.PricingEngine");

var webApplicationBuilder = WebApplication.CreateBuilder(args);

var connectionString = webApplicationBuilder.Configuration.GetConnectionString("SQLServer");
var serviceCollection = webApplicationBuilder.Services;
serviceCollection.AddControllers();
serviceCollection.AddDbContext<PricingEngineDatabaseContext>(dcob => dcob.UseSqlServer(connectionString))
	.AddEndpointsApiExplorer()
	.AddMediatR(msc => msc.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()))
	.AddSwaggerGen(sgo => {
		sgo.SwaggerDoc(name: "v1", new OpenApiInfo {
			Contact = new OpenApiContact {
				Email = "shekar.balgobin@gmail.com",
				Name = "Shekar Balgobin",
			},
			Description = "Pricing Engine API",
			Title = "Stock Exchange - Take Home Task",
			Version = "v1"
		});

		sgo.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
	});

var webApplication = webApplicationBuilder.Build();
if (webApplication.Environment.IsDevelopment()) {
	webApplication.UseSwagger();
	webApplication.UseSwaggerUI();
}

webApplication.UseHttpsRedirection().UseAuthorization();
webApplication.MapControllers();
webApplication.Run();
