
using AutoMapper;
using MoviesApiProjeect.Core.Interfaces;
using MoviesApiProjeect.Core.Models;
using MoviesApiProjeect.EF.Data;
using MoviesApiProjeect.EF.Repositories;

namespace MoviesApiProjeect.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        public IGenreReposiitory Genres {  get; private set; }
        public IMovieRepository Movies { get; private set; }

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UnitOfWork(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            Genres=new GenreRepository(context);
            Movies=new MovieRepository(context,mapper);
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }
    }
}