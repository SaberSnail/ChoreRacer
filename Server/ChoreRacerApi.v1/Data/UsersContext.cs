using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChoreRacerApi.v1.Data
{
	public sealed class UsersContext : IdentityDbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			options.UseSqlite(GetConnectionString());
		}

		private static string GetConnectionString()
		{
			return $"Data Source={c_dbFileName}";
		}

		const string c_dbFileName = "users.db";
	}
}