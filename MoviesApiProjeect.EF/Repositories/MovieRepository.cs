using Microsoft.EntityFrameworkCore;
using MoviesApiProjeect.Core.DTOs;
using MoviesApiProjeect.Core.Interfaces;
using MoviesApiProjeect.Core.Models;
using MoviesApiProjeect.EF.Data;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;

namespace MoviesApiProjeect.EF.Repositories
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public MovieRepository(ApplicationDbContext context,IMapper mapper):base(context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<MovieDetailsDto>> GetFromMoviesAsync(Expression<Func<Movie, bool>> excepression = null)
        {
            IQueryable<Movie> query = _context.Movies.OrderByDescending(m => m.Rate).Include(m => m.Genre);

            if (excepression != null)
            {
                query = query.Where(excepression);
            }

            var movies = await query.ToListAsync();
            var moviesDetailsDto = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);
            return moviesDetailsDto;
        }

        //public async Task<MovieDetailsDto> GetMovieByIdAsync(int id)
        //        => await _context.Movies.Include(m => m.Genre)
        //            .Select(m => new MovieDetailsDto
        //            {
        //                GenreId = m.GenreId,
        //                GenreName = m.Genre.Name,
        //                Id = m.Id,
        //                Poster = m.Poster,
        //                Rate = m.Rate,
        //                StoreLine = m.StoreLine,
        //                Title = m.Title,
        //                Year = m.Year
        //            })
        //            .SingleOrDefaultAsync(m => m.Id == id);

    }
}