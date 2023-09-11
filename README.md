# London Stock Exchange

## Problem

The solution is comprised of 2 microservices; the Exchange and the Pricing Engine.

Each service exposes one or more APIs that work together to satisfy the requirements detailed.


![Sequence](/Documentation/Overview.png)

### Exchange Service

Exposes the Trade REST API that allows clients to submit trades.

The trade processing sequence is detailed below. Trades are converted to transactions before sending to the Pricing Engine.

![Sequence](/Documentation/ExchangeSequence.png)

### Pricing Engine Service

1. Exposes the Transaction REST API that is used internally by the Exchange service.
   
_Note: This API is NOT exposed to clients._

2. Exposes the Stock REST API that allows clients to get the average price of stocks.

The transaction processing sequence is detailed below. As transactions arrive, the average price is calculated and the latest price stored. Previous prices are archived to a temporal stock table table.

![Sequence](/Documentation/PricingEngineSequence.png)

## Enhancements

- Faily scalable as the services can be scaled independantly.
- The database will eventually become a bottleneck as it's transactional. Eventually consistent is what we should aim for.
- The current implementation uses REST APIs exclusively. I would perfer to have an asynchronus messaging architecture to improve the latency when mutating data.

## Developer notes

### Tools

- Latest Entity Framework Core Tools ```dotnet tool install --global dotnet-ef```
- Visual Studio 2022 or later

### Build Notes

1. Clone the repository.
2. Open the solution from Visual Studio 2022 or later.
3. Rebuild the solution.
4. Create a local database instance for the Exchange Web Application from the command prompt
```
cd StockExchange\Exchange.Model.MsSqlServer
dotnet ef migrations add Initial --context ExchangeDatabaseContext --project Exchange.Model.MsSqlServer.csproj
dotnet ef database update
```
5. Create a local database instance for the Pricing Engine Web Application from the command prompt
```
cd StockExchange\PricingEngine.Model.MsSqlServer
dotnet ef migrations add Initial --context PricingEngineDatabaseContext --project PricingEngine.Model.MsSqlServer.csproj
dotnet ef database update
```
6. Set the Exchange Web Application & Pricing Engine Web Appication to both start.
7. Run the application.

_Note: Your connection may vary depenending on the edition of SQL Server_