using DoAnWebNangCao.Constants;
using DoAnWebNangCao.Models;
using Microsoft.AspNetCore.Identity;

namespace DoAnWebNangCao.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider serviceProvider)
        {
            var userMgr = serviceProvider.GetService<UserManager<User>>();
            var roleMgr = serviceProvider.GetService<RoleManager<IdentityRole>>();
            await roleMgr.CreateAsync(new IdentityRole(Role.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Role.User.ToString()));
            var admin = new User
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
            };
            var userInDb = await userMgr.FindByEmailAsync(admin.Email);
            if (userInDb is null)
            {
                await userMgr.CreateAsync(admin, "Admin@123");
                await userMgr.AddToRoleAsync(admin, Role.Admin.ToString());
            }
            var admin21 = new User
            {
                UserName = "admin2",
                Email = "admin2@gmail.com",
                EmailConfirmed = true,
            };
            var userInDb2 = await userMgr.FindByEmailAsync(admin21.Email);
            if (userInDb2 is null)
            {
                await userMgr.CreateAsync(admin21, "Admin2@123");
                await userMgr.AddToRoleAsync(admin21, Role.Admin.ToString());
            }
        }
    }
}
