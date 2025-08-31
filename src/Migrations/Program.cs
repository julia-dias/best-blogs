using DbUp;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Reflection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName)
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var connectionString = configuration.GetConnectionString("DB_BESTBLOGS");
await RetryEnsureDB(connectionString);

var upgrader = DeployChanges.To
        .MySqlDatabase(connectionString)
        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
        .LogToConsole()
        .Build();

var result = upgrader.PerformUpgrade();

if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.ResetColor();
    return -1;
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(result.Successful);
Console.ResetColor();
return 0;

async Task RetryEnsureDB(string con, int retryCount = 20)
{
    try
    {
        EnsureDatabase.For.MySqlDatabase(connectionString);
    }
    catch (MySqlException)
    {
        if (retryCount > 0)
        {
            await Task.Delay(5000);
            await RetryEnsureDB(con, retryCount - 1);
        }
        else
        {
            throw;
        }
    }
}
