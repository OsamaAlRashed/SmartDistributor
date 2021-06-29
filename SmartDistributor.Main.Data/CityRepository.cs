using SmartDistributor.Main.Dto;
using SmartDistributor.Main.Idata;
using SmartDistributor.SqlServer.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SmartDistributor.Main.Data
{
    public class CityRepository : BaseRepository , ICityRepositorycs
    {
        public CityRepository(SmartDistributorDbContext context):base(context)
        {

        }

        public async Task<CityDto> GetCity(Guid Id)
        {
            try
            {
                var result = await Context.Cities.SingleOrDefaultAsync(c => c.Id == Id);
                var resultDto = new CityDto
                {
                    Id = result.Id,
                    Name = result.Name,
                    StateName = result.Name
                };
                return resultDto;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public async Task<CityDto> GetCityByName(string Name)
        {
            try
            {
                var result = await Context.Cities.FirstOrDefaultAsync(c => c.Name == Name);
                var resultDto = new CityDto
                {
                    Id = result.Id,
                    Name = result.Name,
                    StateName = result.Name
                };
                return resultDto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
