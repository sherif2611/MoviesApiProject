using MoviesApiProjeect.Core.DTOs;
using MoviesApiProjeect.Core.Models;
using System.Linq.Expressions;

namespace MoviesApiProjeect.Core.Interfaces
{
    public interface IMovieRepository:IBaseRepository<Movie>
    {
        Task<IEnumerable<MovieDetailsDto>> GetFromMoviesAsync(Expression<Func<Movie, bool>> excepression=null);
        //Task<MovieDetailsDto> GetMovieByIdAsync(int id);
    }
}
