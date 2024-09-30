using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;

namespace RestApiServer.Utils
{
    public class DbContextUtils
    {
        public readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        public DbContextUtils(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<AppDbContext> GetDbContextAsync()
        {
            return await _dbContextFactory.CreateDbContextAsync();
        }

        public AppDbContext GetDbContext()
        {
            return _dbContextFactory.CreateDbContext();
        }
        public static AppDbContext CreateInstance()
        {
            return new AppDbContext();
        }
    }
}