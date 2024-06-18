
using MoviesApiProjeect.Core.Models;

namespace MoviesApiProjeect.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IGenreReposiitory Genres {  get; }
        IMovieRepository Movies { get; }
        int Complete();
    }
}