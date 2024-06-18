
using Microsoft.AspNetCore.Authorization;

namespace MoviesApiProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GenresController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenresController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _unitOfWork.Genres.GetAllAsync();
            return Ok(genres.OrderBy(g => g.Name));
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenreDto dto)
        {
            Genre genre = new()
            {
                Name = dto.Name
            };
            await _unitOfWork.Genres.AddAsync(genre);
            _unitOfWork.Complete();
            return Ok(genre);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]GenreDto dto)
        {
            var genre=await _unitOfWork.Genres.GetGenreByIdAsync(id);
            if(genre == null) 
                return NotFound($"No genre was found with ID: {id}");
            genre.Name = dto.Name;
            _unitOfWork.Genres.Update(genre);
            _unitOfWork.Complete();
            return Ok(genre);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var genre = await _unitOfWork.Genres.GetGenreByIdAsync(id);
            if (genre == null)
                return NotFound($"No genre was found with ID: {id}");

            await _unitOfWork.Genres.DeleteGenreAsync(id);
            _unitOfWork.Complete(); 
            return Ok(genre);
        }
    }
}