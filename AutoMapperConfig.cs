using AutoMapper;
using slp.light.DTO;
using slp.light.Model;

namespace slp.light
{
    public class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserOutDto>()
                    .ForMember(x => x.Roles, x => x.MapFrom(y => y.Roles.Select(r => r.ToString())));
            });

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}