using AutoMapper;

namespace ica.website.Infrastructure.Mapping
{
    interface IHaveCustomMappings
    {
        void CreateMappings(IConfiguration configuration);
    }
}
