#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt.Models;

namespace Projekt.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly ILogger<RegisterModel> _logger;

		public RegisterModel(
	        UserManager<ApplicationUser> userManager,
	        IUserStore<ApplicationUser> userStore,
	        SignInManager<ApplicationUser> signInManager,
	        ILogger<RegisterModel> logger)
		{
			_userManager = userManager;
			_userStore = userStore;
			_signInManager = signInManager;
			_logger = logger;
		}

		[BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
			[Required(ErrorMessage = "Nazwa użytkownika jest wymagana")]
			[Display(Name = "Nazwa użytkownika")]
			public string UserName { get; set; }

			[Required(ErrorMessage = "Hasło jest wymagane")]
			[StringLength(100, ErrorMessage = "{0} musi mieć od {2} do {1} znaków.", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "Hasło")]
			public string Password { get; set; }

			[Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
			[DataType(DataType.Password)]
			[Display(Name = "Potwierdź hasło")]
			[Compare("Password", ErrorMessage = "Hasła muszą być takie same")]
			public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = CreateUser();

				await _userStore.SetUserNameAsync(user, Input.UserName, CancellationToken.None);

				var result = await _userManager.CreateAsync(user, Input.Password);

				if (result.Succeeded)
				{
					_logger.LogInformation("Utworzono nowe konto użytkownika.");

					await _signInManager.SignInAsync(user, isPersistent: false);
					return LocalRedirect(returnUrl);
				}

				foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}
