using AutoMapper;
using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Repositories.Implementation
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Cuenta, CuentaCreacionViewModel>();
        }
    }
}
