using ChoreRacerApi.v1.Models;
using Microsoft.EntityFrameworkCore;

namespace ChoreRacerApi.v1.Data
{
	public sealed class ChoresContext : DbContext
	{
		public DbSet<ChoreDto> Chores { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			options.UseInMemoryDatabase(c_dbName);
		}

		const string c_dbName = "Chores";
	}
}
