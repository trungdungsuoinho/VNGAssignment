using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using VNGAssignment.Entities;
using VNGAssignment.Migrations;
using VNGAssignment.Models;

namespace VNGAssignment.Services
{
    public class BookService(MyDbContext dbContext, IMapper mapper) : IBookService
    {
        private MyDbContext _context = dbContext;
        private IMapper _mapper = mapper;

        public async Task<Book?> GetById(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Book>> GetAll()
        {
            return await _context.Books.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<List<Book>> Search(SearchBookRequest model)
        {
            return await _context.Books
                .Where(c =>
                    string.IsNullOrWhiteSpace(model.SearchText) ||
                    c.Id.ToString().Contains(model.SearchText) ||
                    c.Title.Contains(model.SearchText, StringComparison.OrdinalIgnoreCase) ||
                    c.Author.Contains(model.SearchText, StringComparison.OrdinalIgnoreCase) ||
                    c.PublishedYear.Contains(model.SearchText, StringComparison.OrdinalIgnoreCase)
                )
                .OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<Book> Create(AddBookRequest model)
        {
            var entity = _mapper.Map<Book>(model);
            await _context.Books.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Update(UpdateBookRequest model)
        {
            var entity = await GetById(model.Id);
            if (entity != null)
            {
                _context.Entry(entity).CurrentValues.SetValues(model);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await GetById(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
