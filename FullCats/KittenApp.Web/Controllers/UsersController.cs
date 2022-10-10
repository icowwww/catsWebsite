namespace KittenApp.Web.Controllers
{
    using System.Linq;
    using KittenApp.Models;
    using KittenApp.Web.Models.BindingModels;
    using SimpleMvc.Common;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Interfaces;
    using System.Text.RegularExpressions;

    public class UsersController : BaseController
    {
        [HttpGet]
        public IActionResult Register()
        {
            this.Model.Data["error"] = string.Empty;
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisteringModel model)
        {
           /* if (!this.IsValidModel(model))
            {
                return this.View();
            }*/
            if (model.Username.Length < 5)
            {
                this.Model.Data["error"] = "Username minimum length is 5 characters.";
                return this.View();
            }
            /*   var email = model.Email;
               var pattern = "@\"^[A-Za-z0-9](([_\\.\\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\\.\\-‌​]?[a-zA-Z0-9]+)*)\\.([A-Za-z]{2,})$";
               if (!Regex.IsMatch(email, pattern))
               {
                   this.Model.Data["error"] = "Email does not match the correct format.";
                   return this.View();
               }*/
            var user = new User()
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = PasswordUtilities.GetPasswordHash(model.Password)
            };

            using (this.Context)
            {
                this.Context.Users.Add(user);
                this.Context.SaveChanges();
                this.SignIn(user.Username, user.Id);
            }

            return this.RedirectToHome();
        }

        [HttpGet]
        public IActionResult Login()
        {
            this.Model.Data["error"] = string.Empty;
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(UserLoggingInModel model)
        {
            var user = this.Context.Users
                .FirstOrDefault(u => u.Username == model.Username);
            if (user == null)
            {
                this.Model.Data["error"] = "Invalid username and / or password";
                return this.View();
            }

            string passwordHash = PasswordUtilities.GetPasswordHash(model.Password);
            if (user.PasswordHash != passwordHash)
            {
                this.Model.Data["error"] = "Invalid username and / or password";
                return this.View();
            }

            this.SignIn(user.Username, user.Id);
            return this.RedirectToHome();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            this.SignOut();
            return this.RedirectToHome();
        }
    }
}
