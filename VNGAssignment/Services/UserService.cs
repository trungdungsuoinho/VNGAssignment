using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using VNGAssignment.Entities;
using VNGAssignment.Helpers;
using VNGAssignment.Migrations;
using VNGAssignment.Models;

namespace VNGAssignment.Services
{
    public class UserService(MyDbContext dbContext) : IUserService
    {
        private MyDbContext _context = dbContext;

        public async Task<User?> GetById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<User?> GetByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(c => c.Username == username);
        }

        public async Task<bool> VerifyPassword(LoginModel model)
        {
            var user = await GetByUsername(model.Username);
            if (user is null)
            {
                return false;
            }
            return PasswordHelper.VerifyPassword(model.Password, user.Password);
        }
    }
}
