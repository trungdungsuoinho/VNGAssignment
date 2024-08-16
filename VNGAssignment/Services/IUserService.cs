using Azure;
using VNGAssignment.Entities;
using VNGAssignment.Models;

namespace VNGAssignment.Services
{
    public interface IUserService
    {
        public Task<User?> GetById(int id);
        public Task<User?> GetByUsername(string username);
        public Task<bool> VerifyPassword(LoginModel model);
    }
}
