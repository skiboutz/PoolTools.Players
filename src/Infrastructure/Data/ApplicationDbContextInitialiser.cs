using PoolTools.Player.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PoolTools.Player.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Seed Teams
        if(_context.Teams.Any() == false)
        {
            _context.Teams.AddRange(new List<Team>
            {
                new() { Code = "ANA", City = "Anaheim", Name = "Ducks"},
                new() { Code = "ARI", City = "Arizona", Name = "Coyotes"},
                new() { Code = "BOS", City = "Boston", Name = "Bruins"},
                new() { Code = "BUF", City = "Buffalo", Name = "Sabred"},
                new() { Code = "CAR", City = "Carolina", Name = "Hurricanes"},
                new() { Code = "CBJ", City = "Columbus", Name = "Blue Jackets"},
                new() { Code = "CGY", City = "Calgary", Name = "Flames"},
                new() { Code = "CHI", City = "Chicago", Name = "Blackhawks"},
                new() { Code = "COL", City = "Colorado", Name = "Avalanche"},
                new() { Code = "DAL", City = "Dallas", Name = "Stars"},
                new() { Code = "DET", City = "Detroit", Name = "Red Wings"},
                new() { Code = "EDM", City = "Edmonton", Name = "Oilers"},
                new() { Code = "FLA", City = "Florida", Name = "Panthers"},
                new() { Code = "LAK", City = "Los Angeles", Name = "Kings"},
                new() { Code = "MIN", City = "Minnesota", Name = "Wild"},
                new() { Code = "MTL", City = "Montreal", Name = "Canadiens"},
                new() { Code = "NJD", City = "New Jersey", Name = "Devils"},
                new() { Code = "NSH", City = "Nashville", Name = "Predators"},
                new() { Code = "NYI", City = "New York", Name = "Islanders"},
                new() { Code = "NYR", City = "New York", Name = "Rangers"},
                new() { Code = "OTT", City = "Ottawa", Name = "Senators"},
                new() { Code = "PHI", City = "Philadelphia", Name = "Flyers"},
                new() { Code = "PIT", City = "Pittsburgh", Name = "Penguins"},
                new() { Code = "SEA", City = "Seattle", Name = "Kraken"},
                new() { Code = "SJS", City = "San Jose", Name = "Sharks"},
                new() { Code = "STL", City = "St. Louis", Name = "Blues"},
                new() { Code = "TBL", City = "Tampa Bay", Name = "Lightning"},
                new() { Code = "TOR", City = "Toronto", Name = "Maple Leafs"},
                new() { Code = "VAN", City = "Vancouver", Name = "Canucks"},
                new() { Code = "VGK", City = "Las Vegas", Name = "Golden Knights"},
                new() { Code = "WPG", City = "Winnipeg", Name = "Jets"},
                new() { Code = "WSH", City = "Washington", Name = "Capitals"}
            });

            await _context.SaveChangesAsync();
        }
    }
}
