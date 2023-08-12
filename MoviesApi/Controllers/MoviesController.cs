using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MoviesApi.Dtos;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMapper _Mapper;
        private readonly IMoviesSevices _movieService;
        private readonly IGenresService _GenreService;

        public MoviesController(IMoviesSevices movieService, IGenresService genreService, IMapper mapper)
        {
            _movieService = movieService;
            _GenreService = genreService;
           
            _Mapper = mapper;
        }

        private new List<string> _allowExtenstions = new List<string> { ".jpg", ".png" };

        private long _maxAllowedPosterSize = 1048576 ;

       
       
        [HttpGet]

        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _movieService.GetAll();

            var data = _Mapper.Map < IEnumerable<MoviesDetailsSto>>(movies);
            return Ok(data);
        }
        [HttpGet(template: "{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            var Movie = await _movieService.GetById(id);
               
            if(Movie==null)
                return NotFound();



            var dto = _Mapper.Map < MoviesDetailsSto > (Movie);
            return Ok(dto);
        }

        [HttpGet(template: "GetByGenreId")]

        public async Task<IActionResult> GetByGenreIdAsync(int genreid)
        {
            var movies = await _movieService.GetAll(genreid);
            var data = _Mapper.Map<IEnumerable<MoviesDetailsSto>>(movies);
            return Ok(data);

        }
        [HttpPost]

        public async Task <IActionResult> CreateAsync([FromForm]MoviesDto dto)
        {
            if (dto.Poster == null)
                return BadRequest(error: "poster is required");
            if (!_allowExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
            {
                return BadRequest(error: "Only .png and .jpg Imaages are allowed");
            }

            if(dto.Poster.Length> _maxAllowedPosterSize)
            {
                return BadRequest(error: "Only 1MB");
            }

            var isValidGenre = await _GenreService.IsValidegenre(dto.GenreId);

            if(!isValidGenre)
                return BadRequest(error: "Invalid genre Id");


            using var datastraem = new MemoryStream();
            await dto.Poster.CopyToAsync(datastraem);
            var move = _Mapper.Map<Movie>(dto);
            move.Poster = datastraem.ToArray();
            _movieService.Add(move);
            return Ok(move);

            
        }

        [HttpPut(template:"{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] MoviesDto dto)
        {
                var Movie = await _movieService.GetById(id);

                if (Movie == null)
                    return NotFound(value: $"No Movie was found with Id :{id}");
            var isValidGenre = await _GenreService.IsValidegenre(dto.GenreId);

            if (!isValidGenre)
                return BadRequest(error: "Invalid genre Id");

            if(dto.Poster != null)
            {
                if (!_allowExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                {
                    return BadRequest(error: "Only .png and .jpg Imaages are allowed");
                }

                if (dto.Poster.Length > _maxAllowedPosterSize)
                {
                    return BadRequest(error: "Only 1MB");
                }
                using var datastraem = new MemoryStream();
                await dto.Poster.CopyToAsync(datastraem);
                Movie.Poster = datastraem.ToArray();
            }
            Movie.Title = dto.Title;
                Movie.Year=dto.Year;
                Movie.StoreLine= dto.StoreLine;
                Movie.Rate= dto.Rate;
               Movie.GenreId= dto.GenreId;
           
            _movieService.Update(Movie);
            return Ok(Movie);

        }
        [HttpDelete(template:"{id}")]
        public async Task<IActionResult> DeleteMovies(int id)
        {
            var Movie = await _movieService.GetById(id);
            if(Movie==null)
                return NotFound(value: $"No Movie was found with Id :{id}");

             _movieService.Delete(Movie);
           
            return Ok(Movie);
        }
    }
}
