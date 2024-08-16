using Azure;
using VNGAssignment.Entities;
using VNGAssignment.Models;

namespace VNGAssignment.Services
{
    public interface IBookService
    {
        public Task<Book?> GetById(int id);
        public Task<List<Book>> GetAll();
        public Task<List<Book>> Search(SearchBookRequest model);
        public Task<Book> Create(AddBookRequest model);
        public Task<bool> Update(UpdateBookRequest model);
        public Task<bool> Delete(int id);
    }
}
