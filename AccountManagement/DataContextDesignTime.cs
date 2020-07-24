using System;
using AccountManagement.ConfigSection;
using AccountManagement.ConfigSection.ConfigModels;
using AccountManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AccountManagement
{
    public class DataContextDesignTime : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            DbOption selectedDbOption = AppConfigs.SelectedDbOption();

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DataContext>();
            switch (selectedDbOption.DbType)
            {
                case DbTypes.SqlServer:
                    dbContextOptionsBuilder.UseSqlServer(selectedDbOption.ConnectionStr);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            DbContextOptions<DataContext> dbContextOptions = dbContextOptionsBuilder.Options;
            return new DataContext(dbContextOptions);
        }
    }
}