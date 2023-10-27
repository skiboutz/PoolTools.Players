using PoolTools.Player.Application.Common.Interfaces;
using PoolTools.Player.Application.Common.Models;

namespace PoolTools.Player.Infrastructure.Identity;

class IdentityService : IIdentityService
{
    public Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        throw new NotImplementedException();
    }

    public Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteUserAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetUserNameAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsInRoleAsync(string userId, string role)
    {
        throw new NotImplementedException();
    }
}