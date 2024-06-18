
using Microsoft.EntityFrameworkCore;
using MoviesApiProjeect.Core.Interfaces;
using MoviesApiProjeect.Core.Models;
using MoviesApiProjeect.EF.Data;

namespace MoviesApiProjeect.EF.Repositories
{
    public class GenreRepository : BaseRepository<Genre>, IGenreReposiitory
    {
        private readonly ApplicationDbContext _context;
        public GenreRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task DeleteGenreAsync(int id)
        {
            _context.Remove(await GetGenreByIdAsync(id));
        }

        public async Task<Genre> GetGenreByIdAsync(int id)=> await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
    }
}