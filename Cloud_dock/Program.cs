using Npgsql;

var builder = WebApplication.CreateBuilder(args);

//стркоа вкл. подлучается из переменных окруж
var connectionString = builder.Configuration.GetConnectionString("Default") ?? "Host=localhost;Username=postgres;Password=postgres;Database=clouddesk";
builder.Services.AddSingleton<NpgsqlDataSource>(NpgsqlDataSource.Create(connectionString));

var app = builder.Build();

//хэлфчек
app.MapGet("/health", async (NpgsqlDataSource dataSource) => {
    try {
        await using var conn = await dataSource.OpenConnectionAsync();
        return Results.Ok(new { status = "healthy", database = "connected" });
    } 
    catch {
        return Results.Problem("Database connection failed", statusCode: 500);
    }
});

//маршрут для получения всех и всея заявочек:
app.MapGet("/api/tickets", async (NpgsqlDataSource dataSource) => {
    await using var conn = await dataSource.OpenConnectionAsync();
    await using var cmd = new NpgsqlCommand("SELECT * FROM tickets ORDER BY created_at DESC", conn);
    await using var reader = await cmd.ExecuteReaderAsync();
    
    var tickets = new List<object>();
    while (await reader.ReadAsync()) {
        tickets.Add(new {
            id = reader.GetInt32(0),
            title = reader.GetString(1),
            description = reader.GetString(2),
            priority = reader.GetInt32(3),
            status = reader.GetString(4),
            created_at = reader.GetDateTime(5)
        });
    }
    return Results.Ok(tickets);
});

app.Run();