using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenersController : ControllerBase
    {
        
     
        private readonly IGenresService _genresService;

        public GenersController(IGenresService genresService)
        {
            _genresService = genresService;
        }

       

        [HttpGet]

        public async Task< IActionResult> GetAllAsync() 
        
        {
            var geners = await _genresService.GetAllA();

            return Ok(geners);
        }

        [HttpPost]

        public async Task<IActionResult> CreatAsync(CreateGenreDto dto)
        {
            var genre = new Genre
            {
                Name = dto.Name
            };

              await _genresService.Add(genre);
          

            return Ok(genre);   
        }

        [HttpPut(template:"{id}")]

        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CreateGenreDto dto)
        {
            var genre = await _genresService.GetbyId(id);

            if(genre == null)
           
                return NotFound(value:$"No genre was found with Id :{id}");

            genre.Name = dto.Name;

            _genresService.Update(genre);

            return Ok(genre);

        }

        [HttpDelete(template: "{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var genre = await _genresService.GetbyId(id);

            if (genre == null)

                return NotFound(value: $"No genre was found with Id :{id}");

            _genresService.Delete(genre);

           

            return Ok();
        }

    }
}
