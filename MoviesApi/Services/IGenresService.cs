namespace MoviesApi.Services
{
    public interface IGenresService
    {
        Task<IEnumerable<Genre>> GetAllA();

        Task<Genre> GetbyId(int id);

        Task<Genre> Add(Genre genre);

       Genre Update(Genre genre);

        Genre Delete(Genre genre);

        Task<bool> IsValidegenre(int id);

    }
}
