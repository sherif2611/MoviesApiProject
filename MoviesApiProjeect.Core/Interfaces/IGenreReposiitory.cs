
using MoviesApiProjeect.Core.Models;

namespace MoviesApiProjeect.Core.Interfaces
{
    public interface IGenreReposiitory:IBaseRepository<Genre>
    {
        Task<Genre> GetGenreByIdAsync(int id);
        Task DeleteGenreAsync(int id);
    }
}
