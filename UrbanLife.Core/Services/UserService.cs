using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UrbanLife.Core.ViewModels;
using UrbanLife.Data.Data;
using UrbanLife.Data.Data.Models;
#nullable disable warnings

namespace UrbanLife.Core.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public UserService(ApplicationDbContext dbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            this.dbContext = dbContext;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task CreateRoleAsync(string roleName)
        {
            await roleManager.CreateAsync(new IdentityRole { Name = roleName });
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await dbContext.Users
                .FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpper()) != null;
        }

        public async Task AddUserAsync(RegisterViewModel model, string fileName)
        {
            User user = new()
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PasswordHash = HashPassword(model.Password),
                ProfileImageName = fileName
            };

            IdentityResult result = await userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception("Error creating a user!");
            }

            if (!await userManager.IsInRoleAsync(user, "User"))
            {
                await userManager.AddToRoleAsync(user, "User");
            }

            await signInManager.SignInAsync(user, isPersistent: false);
        }

        public async Task<bool> SignUserAsync(LoginViewModel model)
        {
            User? user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.NormalizedEmail == model.Email.ToUpper()
                                        && HashPassword(model.Password) == u.PasswordHash);

            if (user != null)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
            }

            return user != null;
        }

        private static string HashPassword(string password)
        {
            byte[] bytePassword = Encoding.UTF8.GetBytes(password);

            using SHA256 hash = SHA256.Create();
            return Convert.ToBase64String(hash.ComputeHash(bytePassword));
        }
    }
}