using Meloman_clone.Data;
using Meloman_clone.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Meloman_clone.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;
        public UserRepository(UserContext context)
        {
            _context = context;
        }
        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User FindByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(e => e.Email == email);
            if(user == null)
            {
                return null;
            }
            return user;
        }
        public User FindById(int id)
        {
            var user = _context.Users.FirstOrDefault(e => e.UserId == id);
            return user;
        }

        public List<Claim> GetAdminClaims()
        {
            List<Claim> adminClaims = new List<Claim>
            {
                new Claim("IsAdmin", "True")
            };
            return adminClaims;
        }

        public List<Claim> GetClaims(User user, string role)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Role", role));
            claims.Add(new Claim("Email", user.Email));
            claims.Add(new Claim(ClaimTypes.Name, user.Name));
            claims.Add(new Claim("Account", "Exist"));
            claims.Add(new Claim("Name", user.Name));
            claims.Add(new Claim("UserId", user.UserId.ToString()));

            return claims;
        }

        public bool RegisterUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
