using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Services
{
    public class GenresService : IGenresService
    {
        private readonly ApplicationDbContext _context;

        public GenresService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> Add(Genre genre)
        {
            await _context.Genres.AddAsync(genre);
            _context.SaveChanges();
            return genre;
        }

        public Genre Delete(Genre genre)
        {
            _context.Remove(genre);

            _context.SaveChanges();
            return genre;
        }

        public async Task<IEnumerable<Genre>> GetAllA()
        {
            return  await _context.Genres.OrderBy(b => b.Name).ToListAsync();
        }

        public async Task<Genre> GetbyId(int id)
        {
            return await _context.Genres.SingleOrDefaultAsync(g=> g.Id == id);

        }

        public Task<bool> IsValidegenre(int id)
        {
            return _context.Genres.AnyAsync(e => e.Id ==id);
        }

        public Genre Update(Genre genre)
        {
            _context.Update(genre);

            _context.SaveChanges(); 
            return genre;
        }
    }
}
