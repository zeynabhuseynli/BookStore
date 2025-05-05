using BookStore.Domain.Entities.Users;
using System.Security.Claims;

namespace BookStore.Application.Interfaces.IManagers.Helper;
public interface IClaimManager
{
    int GetCurrentUserId();
    IEnumerable<Claim> GetUserClaims(User user);
}

