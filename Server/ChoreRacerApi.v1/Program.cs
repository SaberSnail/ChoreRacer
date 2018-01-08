using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChoreRacerApi.v1.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChoreRacerApi.v1
{
	public sealed class Program
	{
		public static async Task Main(string[] args)
		{
			var host = BuildWebHost(args);

			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
					var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
					var configuration = services.GetRequiredService<IConfiguration>();

					await services.GetRequiredService<UsersContext>().InitializeIfNeeded(roleManager, userManager, configuration);
					services.GetRequiredService<ChoresContext>().InitializeIfNeeded();
				}
				catch (Exception e)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(e, "An error occurred while seeding the database.");
				}
			}

			host.Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.Build();
	}
}
