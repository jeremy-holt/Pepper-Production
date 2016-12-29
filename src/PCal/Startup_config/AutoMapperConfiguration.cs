using AutoMapper;
using PCal.Models;

namespace PCal.Startup_config
{
    public static class AutoMapperConfiguration
    {
        public static void CreateMapper()
        {
            Mapper.Initialize(c => { c.CreateMap<FarmProduct, FarmProduct>(); });
        }
    }
}