using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrbanLife.Core.Services;
using UrbanLife.Core.Utilities;
using UrbanLife.Core.ViewModels;
using UrbanLife.Data.Data.Models;

namespace UrbanLife.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserService userService;
        private readonly PaymentService paymentService;
        private readonly UserManager<User> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AccountController(UserService userService,
            PaymentService paymentService,
            UserManager<User> userManager,
            IWebHostEnvironment webHostEnvironment)
        {
            this.userService = userService;
            this.paymentService = paymentService;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Settings(UpdateProfileViewModel model)
        {
            User user = await userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                string? fileName = null;

                if (model.ProfilePicture != null && user.ProfileImageName != "guest.png")
                {
                    fileName = await PictureProcessor
                        .DownloadProfilePictureAsync(webHostEnvironment.WebRootPath, model.ProfilePicture);

                    PictureProcessor.DeleteProfilePicture(webHostEnvironment.WebRootPath, user.ProfileImageName);
                }
                else if (model.ProfilePicture != null && user.ProfileImageName == "guest.png")
                {
                    fileName = await PictureProcessor
                        .DownloadProfilePictureAsync(webHostEnvironment.WebRootPath, model.ProfilePicture);
                }

                await userService.UpdateProfileAsync(user, model, fileName);
            }
            catch (IOException)
            {
                return View();
            }

            return Redirect("/");
        }

        public async Task<IActionResult> Payments()
        {
            UrbanLife.Data.Data.Models.User user = await userManager.GetUserAsync(User);
            List<PaymentMethodViewModel> payments = await paymentService.GetPaymentsForAccountPageAsync(user.Id);

            PaymentInfoViewModel paymentInfoViewModel = new()
            {
                AccountFirstName = user.FirstName,
                UserId = user.Id,
                Payments = payments
            };

            return View(paymentInfoViewModel);
        }

        public IActionResult AddPayment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment(BankCardViewModel model)
        {
            UrbanLife.Data.Data.Models.User user = await userManager.GetUserAsync(User);
            List<Payment> paymentsByUser = await paymentService.GetPaymentsByUserAsync(user.Id);
            Payment payment = await paymentService.GetPaymentByNumberAsync(model.CardNumber);

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                if (payment != null)
                {
                    await paymentService.AddUserPaymentAsync(user.Id, payment.Id, model.IsDefault);
                }
                else
                {
                    await paymentService.AddPaymentMethodAsync(user.Id, model);
                }

            }
            catch (Exception)
            {
                return Redirect("/user/account/payments");
            }

            return Redirect("/user/account/payments");
        }

        public async Task<IActionResult> SetDefault(string paymentId)
        {
            UrbanLife.Data.Data.Models.User user = await userManager.GetUserAsync(User);

            await paymentService.SetDefaultPaymentAsync(user.Id, paymentId);

            return Redirect("/user/account/payments");
        }

        public async Task<IActionResult> Delete(string paymentId)
        {
            UrbanLife.Data.Data.Models.User user = await userManager.GetUserAsync(User);

            try
            {
                await paymentService.DeletePaymentAsync(user.Id, paymentId);
            }
            catch (InvalidOperationException)
            {
                return Redirect("/user/account/payments");
            }

            return Redirect("/user/account/payments");
        }

        public IActionResult Subscriptions()
        {
            return View();
        }

        public async Task<JsonResult> PasswordAlreadyUsed(string email, string password)
        {
            if (password == null)
            {
                return Json(false);
            }
            if (email == null)
            {
                UrbanLife.Data.Data.Models.User user = await userManager.GetUserAsync(User);
                email = user.Email;
            }

            return Json(await userService.PasswordExistsAsync(email, password));
        }

        public async Task<JsonResult> CardCredentialsAreValid(string cardNumber, DateTime expireDate, string firstName,
            string lastName, decimal amount, string cvc)
        {
            Payment payment = await paymentService.GetPaymentByNumberAsync(cardNumber);

            if (payment != null)
            {
                if (payment.ExpireDate != expireDate ||
                payment.FirstName != firstName.ToUpper() ||
                payment.LastName != lastName.ToUpper() ||
                payment.Amount != amount ||
                payment.CVC != cvc)
                {
                    return Json(false);
                }
            }

            return Json(true);
        }

        public async Task<JsonResult> CardAlreadyAdded(string cardNumber)
        {
            UrbanLife.Data.Data.Models.User user = await userManager.GetUserAsync(User);
            List<Payment> paymentsByUser = await paymentService.GetPaymentsByUserAsync(user.Id);

            if (paymentsByUser.FirstOrDefault(p => p.Number == cardNumber) != null)
            {
                return Json(true);
            }

            return Json(false);
        }
    }
}
