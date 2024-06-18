using Microsoft.AspNetCore.Http;

namespace MoviesApiProjeect.Core.DTOs
{
    public class BaseMovieDto
    {
        [MaxLength(250)]
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string StoreLine { get; set; }
        public byte GenreId { get; set; }
    }
}
