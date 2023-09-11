using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Tyl.StockExchange.PricingEngine.Model.MsSqlServer;

[ExcludeFromCodeCoverage]
internal sealed class PricingEngineDesignTimeDbContextFactory :
	IDesignTimeDbContextFactory<PricingEngineDatabaseContext> {
	public PricingEngineDatabaseContext CreateDbContext(string[] args) {
		var configurationRoot = new ConfigurationBuilder().AddJsonFile(path: "appsettings.json", optional: false).Build();
		var connectionString = configurationRoot.GetConnectionString("SQLServer");
		var serviceCollection = new ServiceCollection().AddDbContext<PricingEngineDatabaseContext>((sp, dcob) =>
			dcob.UseInternalServiceProvider(sp).UseSqlServer(connectionString)).AddEntityFrameworkSqlServer();

		return serviceCollection.BuildServiceProvider().GetService<PricingEngineDatabaseContext>()!;
	}
}
