using SmartDistributor.SqlServer.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDistributor.Main.Data
{
    public class BaseRepository
    {
        protected BaseRepository(SmartDistributorDbContext context)
        {
            Context = context;
        }

        protected SmartDistributorDbContext Context { get; set; }
    }
}
