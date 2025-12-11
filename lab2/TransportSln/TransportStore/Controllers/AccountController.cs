using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TransportStore.Models.ViewModels;

namespace TransportStore.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInMgr)
        {
            userManager = userMgr;
            signInManager = signInMgr;
        }

        public ViewResult Login(string returnUrl)
        {
            return View(new LoginModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser? user = await userManager.FindByNameAsync(loginModel.Name);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    if ((await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
                    {
                        return Redirect(loginModel?.ReturnUrl ?? "/");
                    }
                }
                ModelState.AddModelError("", "Невірне ім'я або пароль");
            }
            return View(loginModel);
        }

        public ViewResult Register(string returnUrl = "/")
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser { UserName = model.Name, Email = model.Email };
                IdentityResult result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (model.Name.ToLower().Contains("admin"))
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                    else 
                    {
                        await userManager.AddToRoleAsync(user, "User");
                    }

                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            IdentityUser? user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            return View(new ProfileModel
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                PhoneNumber = user.PhoneNumber ?? ""
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profile(ProfileModel model)
        {
            IdentityUser? user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var resultPass = await userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if (!resultPass.Succeeded)
                {
                    foreach (var error in resultPass.Errors) ModelState.AddModelError("", error.Description);
                    return View(model);
                }
            }

            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                ViewBag.Message = "Профіль оновлено успішно!";
                return View(model);
            }

            foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
            return View(model);
        }
    }
}