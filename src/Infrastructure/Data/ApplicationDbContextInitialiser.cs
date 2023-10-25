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
                new Team{ Code = "ANA", City = "Anaheim", Name = "Ducks"},
                new Team{ Code = "ARI", City = "Arizona", Name = "Coyotes"},
                new Team{ Code = "BOS", City = "Boston", Name = "Bruins"},
                new Team{ Code = "BUF", City = "Buffalo", Name = "Sabred"},
                new Team{ Code = "CAR", City = "Carolina", Name = "Hurricanes"},
                new Team{ Code = "CBJ", City = "Columbus", Name = "Blue Jackets"},
                new Team{ Code = "CGY", City = "Calgary", Name = "Flames"},
                new Team{ Code = "CHI", City = "Chicago", Name = "Blackhawks"},
                new Team{ Code = "COL", City = "Colorado", Name = "Avalanche"},
                new Team{ Code = "DAL", City = "Dallas", Name = "Stars"},
                new Team{ Code = "DET", City = "Detroit", Name = "Red Wings"},
                new Team{ Code = "EDM", City = "Edmonton", Name = "Oilers"},
                new Team{ Code = "FLA", City = "Florida", Name = "Panthers"},
                new Team{ Code = "LAK", City = "Los Angeles", Name = "Kings"},
                new Team{ Code = "MIN", City = "Minnesota", Name = "Wild"},
                new Team{ Code = "MTL", City = "Montreal", Name = "Canadiens"},
                new Team{ Code = "NJD", City = "New Jersey", Name = "Devils"},
                new Team{ Code = "NSH", City = "Nashville", Name = "Predators"},
                new Team{ Code = "NYI", City = "New York", Name = "Islanders"},
                new Team{ Code = "NYR", City = "New York", Name = "Rangers"},
                new Team{ Code = "OTT", City = "Ottawa", Name = "Senators"},
                new Team{ Code = "PHI", City = "Philadelphia", Name = "Flyers"},
                new Team{ Code = "PIT", City = "Pittsburgh", Name = "Penguins"},
                new Team{ Code = "SEA", City = "Seattle", Name = "Kraken"},
                new Team{ Code = "SJS", City = "San Jose", Name = "Sharks"},
                new Team{ Code = "STL", City = "St. Louis", Name = "Blues"},
                new Team{ Code = "TBL", City = "Tampa Bay", Name = "Lightning"},
                new Team{ Code = "TOR", City = "Toronto", Name = "Maple Leafs"},
                new Team{ Code = "VAN", City = "Vancouver", Name = "Canucks"},
                new Team{ Code = "VGK", City = "Las Vegas", Name = "Golden Knights"},
                new Team{ Code = "WPG", City = "Winnipeg", Name = "Jets"},
                new Team{ Code = "WSH", City = "Washington", Name = "Capitals"}
            });

            await _context.SaveChangesAsync();
        }
    }
}
