using System.Linq;
using System.Threading.Tasks;
using ChoreRacerApi.v1.Data;
using ChoreRacerApi.v1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ChoreRacerApi.v1
{
	public static class DatabaseInitializer
	{
		public static async Task InitializeIfNeeded(this UsersContext usersContext, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IConfiguration configuration)
		{
			usersContext.Database.EnsureCreated();

			if (usersContext.Roles.Any(x => x.Name == RoleKind.Admin))
				return;

			await roleManager.CreateAsync(new IdentityRole(RoleKind.Admin));

			var adminEmail = configuration["DefaultAdminEmail"];
			await userManager.CreateAsync(new IdentityUser
			{
				UserName = adminEmail,
				Email = adminEmail,
			}, configuration["DefaultAdminPassword"]);
			await userManager.AddToRoleAsync(await userManager.FindByNameAsync(adminEmail), RoleKind.Admin);
		}

		public static void InitializeIfNeeded(this ChoresContext choresContext)
		{
			choresContext.Database.EnsureCreated();

			if (choresContext.Chores.Any())
				return;

			choresContext.Chores.Add(new ChoreDto
			{
				Title = "Clean my room",
				Description = "Make sure all toys and clothes are put away and the floor is clean before bed.",
			});

			choresContext.SaveChanges();
		}
	}
}
