using Microsoft.AspNetCore.Http;

namespace MoviesApiProjeect.Core.DTOs
{
    public class UpdateMovieDto:BaseMovieDto
    {
        public IFormFile ?Poster { get; set; }
    }
}