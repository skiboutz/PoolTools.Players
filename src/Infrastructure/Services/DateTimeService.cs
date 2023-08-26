using PoolTools.Player.Application.Common.Interfaces;

namespace PoolTools.Player.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;

    public DateTime Today => DateTime.Today;
}
