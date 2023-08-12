namespace MoviesApi.Services
{
    public interface IMoviesSevices
    {
        Task<IEnumerable<Movie>> GetAll(int genreid=0);

        Task<Movie> GetById(int id);

        Task<Movie> Add(Movie movie);

        Movie Update(Movie movie);

        Movie Delete(Movie movie);


    }
}
