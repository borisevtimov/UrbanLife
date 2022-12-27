﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrbanLife.Core.Services;
using UrbanLife.Core.Utilities;
using UrbanLife.Core.ViewModels;
using UrbanLife.Data.Data.Models;
#nullable disable warnings

namespace UrbanLife.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService userService;
        private readonly SignInManager<User> signInManager;
        private readonly IWebHostEnvironment webhostEnvironment;

        public UserController(UserService userService,
            SignInManager<User> signInManager,
            IWebHostEnvironment webHostEnvironment)
        {
            this.userService = userService;
            this.signInManager = signInManager;
            this.webhostEnvironment = webHostEnvironment;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            if (!await userService.SignUserAsync(model))
            {
                model.InvalidProfile = 1;
            }

            ModelState.Clear();
            TryValidateModel(model);

            if (!ModelState.IsValid)
            {
                return View();
            }

            await userService.SignUserAsync(model);

            return Redirect("/");
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel registerViewModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            if (await userService.UserExistsAsync(registerViewModel.Email))
            {
                registerViewModel.EmailAlreadyUsed = 1;
            }

            ModelState.Clear();
            TryValidateModel(registerViewModel);

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (await userService.UserExistsAsync(registerViewModel.Email))
            {
                return View();
            }

            try
            {
                string fileName = await PictureProcessor
                    .DownloadProfilePictureAsync(webhostEnvironment.WebRootPath, registerViewModel.ProfilePicture);

                await userService.AddUserAsync(registerViewModel, fileName);
            }
            catch (IOException)
            {
                return View();
            }

            return Redirect("/");
        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await signInManager.SignOutAsync();
            }

            return Redirect("/");
        }
    }
}
