using AutoMapper;

namespace BowTie.BLL.Infrastructure
{
    /// <summary>
    /// Automapper configuration.
    /// </summary>
    public static class AutoMapperServicesConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });
        }
    }
}
