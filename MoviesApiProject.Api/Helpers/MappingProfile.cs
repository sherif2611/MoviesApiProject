using AutoMapper;

namespace MoviesApiProject.Api.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateMovieDto, Movie>()
                .ForMember(src => src.Poster, opt => opt.Ignore());
            CreateMap<UpdateMovieDto, Movie>()
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(src => src.Poster, opt => opt.Ignore());
        }
    }
}