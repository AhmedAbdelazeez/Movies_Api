using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;

namespace MoviesApi.Services
{
    public class MovieService : IMoviesSevices
    {
        private readonly ApplicationDbContext _context;

        public MovieService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Movie> Add(Movie movie)
        {
            await _context.Movies.AddAsync(movie);

            _context.SaveChanges();

            return movie;
            
        }

        public Movie Delete(Movie movie)
        {
            _context.Remove(movie);
            _context.SaveChanges();
            return movie;
        }

        public async Task<IEnumerable<Movie>> GetAll(int genreid = 0)
        {
            return await _context.Movies
                .Where(m => m.GenreId == genreid || genreid == 0)
                .OrderByDescending(x => x.Rate)
                .Include(x => x.Genre)
                .ToListAsync();
         
        }

        public async Task<Movie> GetById(int id)
        {
            return await _context.Movies.Include(x => x.Genre).SingleOrDefaultAsync(x => x.Id==id);
        }

        public Movie Update(Movie movie)
        {
            _context.Update(movie);
            _context.SaveChanges();
            return movie;
        }
    }
}
