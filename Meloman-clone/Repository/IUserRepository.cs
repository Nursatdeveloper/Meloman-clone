using Meloman_clone.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Meloman_clone.Repository
{
    public interface IUserRepository
    {
        User FindByEmail(string email);
        bool RegisterUser(User user);
        List<Claim> GetClaims(User user, string role);
        List<Claim> GetAdminClaims();
        List<User> GetUsers();

    }
}
