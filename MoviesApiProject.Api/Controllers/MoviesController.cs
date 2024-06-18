using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MoviesApiProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly List<String> _allowedExtentions = new()
        {
            ".jpg",".png"
        };
        private readonly long _maxAllowedPosterSize = 1048576;
        public MoviesController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies= await _unitOfWork.Movies.GetFromMoviesAsync();
            return Ok(movies);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _unitOfWork.Movies.GetFromMoviesAsync(m => m.Id == id);
            if ( movie == null||movie.Count()==0)
                return NotFound();
            return Ok(movie);
        }
        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
        {
            var movies =await _unitOfWork.Movies.GetFromMoviesAsync(m=>m.GenreId==genreId);
            return Ok(movies);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateMovieDto dto)
        {
            if (!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");
            if(dto.Poster.Length>_maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 1MB!");

            if(await _unitOfWork.Genres.GetGenreByIdAsync(dto.GenreId)==null)
                return BadRequest("Invalid genre ID!");

            using var dataStream=new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);

            var movie =_mapper.Map<Movie>(dto);
            movie.Poster=dataStream.ToArray();
            await _unitOfWork.Movies.AddAsync(movie);
            _unitOfWork.Complete();
            return Ok(movie);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,UpdateMovieDto dto)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);
            if (movie == null)
                return NotFound($"No movie with ID: {id}!");
            
            _mapper.Map<Movie>(dto);
            if(dto.Poster!= null)
            {
                if (!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed!");
                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB!");

                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                movie.Poster = dataStream.ToArray();
            }
            _unitOfWork.Movies.Update(movie);
            _unitOfWork.Complete();
            return Ok(movie);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie=await _unitOfWork.Movies.GetByIdAsync(id);
            if(movie==null) 
                return NotFound($"No movie with ID: {id}!");
            await _unitOfWork.Movies.DeleteAsync(id);
            _unitOfWork.Complete();
            return Ok(movie);
        }
    }
}