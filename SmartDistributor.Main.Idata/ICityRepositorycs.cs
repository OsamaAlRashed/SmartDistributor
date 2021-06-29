using SmartDistributor.Main.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartDistributor.Main.Idata
{
    public interface ICityRepositorycs
    {
        Task<CityDto> GetCity(Guid Id);
        Task<CityDto> GetCityByName(string Name);

    }
}
