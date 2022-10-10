namespace KittenApp.Web.Controllers
{
    using KittenApp.Models;
    using KittenApp.Web.Models.BindingModels;
    using KittenApp.Web.Models.ViewModels;
    using Microsoft.EntityFrameworkCore;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Attributes.Security;
    using SimpleMvc.Framework.Interfaces;
    using System;
    using System.Linq;
    using System.Text;

    public class BreedController : BaseController
    {
        private const int BreedsPerPage = 3;
        [HttpGet]
        [PreAuthorize]
        public IActionResult Add()
        {
            this.Model.Data["error"] = string.Empty;
            return this.View();
        }

        [HttpPost]
        public IActionResult Add(BreedAddingModel model)
        {
            using (this.Context)
            {
                var breed = this.Context.Breeds
                .FirstOrDefault(b => b.Name == model.Name);
                if (breed != null)
                {
                    this.Model.Data["error"] = "Existing breed";
                    return this.View();
                }

                var newBreed = new Breed()
                {
                    Name = model.Name
                };

                this.Context.Breeds.Add(newBreed);
                this.Context.SaveChanges();

                return this.RedirectToAction("/breed/all");
            }
        }

        [HttpGet]
        [PreAuthorize]
        public IActionResult All()
        {
            var breeds = this.Context.Breeds
                .Select(BreedViewModel.FromBreed)
                .Select(vm =>
                    $@"<div class=""col-4"">
                        <div>
                            <h5>Breed Name: {vm.Name}</h5>
                            <h5>Kittens: {String.Join(", ", vm.Kittens.Select(x => x.Name))}</h5>
                        </div>
                    </div>")
                .ToList();

            var breedsResult = new StringBuilder();
            breedsResult.Append(@"<div class=""row text-center"">");
            for (int i = 0; i < breeds.Count; i++)
            {
                breedsResult.Append(breeds[i]);

                if (i % BreedsPerPage == BreedsPerPage - 1)
                {
                    breedsResult.Append(@"</div><div class=""row text-center"">");
                }
            }

            breedsResult.Append("</div>");

            this.Model.Data["breeds"] = breedsResult.ToString();
            return this.View();
        }

    }
}
