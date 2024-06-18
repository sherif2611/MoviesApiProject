using Microsoft.AspNetCore.Http;

namespace MoviesApiProjeect.Core.DTOs
{
    public class CreateMovieDto:BaseMovieDto
    {
        public IFormFile Poster { get; set; }
    }
}