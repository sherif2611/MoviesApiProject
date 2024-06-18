using MoviesApiProjeect.Core.DTOs;
using MoviesApiProjeect.Core.Models;
using AutoMapper;
namespace MoviesApiProjeect.EF.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieDetailsDto>();
        }
    }
}
