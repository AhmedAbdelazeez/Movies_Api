using AutoMapper;

namespace MoviesApi
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MoviesDetailsSto>();
            CreateMap<MoviesDto, Movie>()
                .ForMember(src => src.Poster, opt => opt.Ignore());
        }
    }
}
