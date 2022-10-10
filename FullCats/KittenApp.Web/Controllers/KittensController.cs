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
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using static System.Net.Mime.MediaTypeNames;
    using System.Xml.Linq;

    public class KittensController : BaseController
    {
        private const int KittensPerPage = 3;

        [HttpGet]
        [PreAuthorize]
        public IActionResult Add()
        {
            this.Model.Data["error"] = string.Empty;
            var sb = new StringBuilder();
            using (this.Context)
            {
                sb.AppendLine($@"<select id=""breed"" name=""Breed"" class=""input-xlarge form-control"">");
                if (!this.Context.Breeds.Any())
                {
                    this.Model.Data["error"] = "No breeds found. Please create!";
                }
                var records = this.Context.Breeds.Select(x =>
                $@"<option value=""{x.Name}"">{x.Name}</option>");
                sb.AppendJoin(Environment.NewLine, records);
                sb.AppendLine($@"</select>");
            }
            this.Model.Data["breeds"] = sb.ToString();
            return this.View();
        }

        [HttpPost]
        public IActionResult Add(KittenAddingModel model)
        {
            using (this.Context)
            {
                var breed = this.Context.Breeds
                .FirstOrDefault(b => b.Name == model.Breed);
                if (breed == null)
                {
                    this.Model.Data["error"] = "Not existing breed!";
                    return this.View();
                }
                if (this.Context.Kittens.Count(x => x.Name == model.Name) != 0)
                {
                    this.Model.Data["error"] = "Cat with such name exists!";
                    return this.View();
                }

                var kitten = new Kitten()
                {
                    Name = model.Name,
                    Age = model.Age,
                    Breed = breed
                };

                    this.Context.Kittens.Add(kitten);
                this.Context.SaveChanges();

                return this.RedirectToAction("/kittens/all");
            }
        }

        [HttpGet]
        [PreAuthorize]
        public IActionResult All()
        {
            var kittens = this.Context.Kittens
                .Include(k => k.Breed)
                .Select(KittenViewModel.FromKitten)
                .Select(vm =>
                    $@"
                    <div class=""col-4"">
                        <img class=""img-thumbnail"" src=""https://images.pexels.com/photos/20787/pexels-photo.jpg?auto=compress&cs=tinysrgb&h=350"" alt=""{vm.Name}'s photo"" />
                        <div>
                            <h5>Name: {vm.Name}</h5>
                            <h5>Age: {vm.Age}</h5>
                            <h5>Breed: {vm.Breed}</h5>
                            
                         <div class=""control-group"">
                        <form action=""/kittens/details"" method=""GET"">
<input type=""hidden"" name=""Name"" value=""{vm.Name}"" />
                        <div class=""controls"">
                            <button class=""btn btn-success"">Comments</button>
                        </div>
                        </form>
                    </div>
                        </div>
                    </div>")
                .ToList();

            var kittensResult = new StringBuilder();
            kittensResult.Append(@"<div class=""row text-center"">");
            for (int i = 0; i < kittens.Count; i++)
            {
                kittensResult.Append(kittens[i]);

                if (i % KittensPerPage == KittensPerPage - 1)
                {
                    kittensResult.Append(@"</div><div class=""row text-center"">");
                }
            }

            kittensResult.Append("</div>");

            this.Model.Data["kittens"] = kittensResult.ToString();
            return this.View();
        }
        [HttpGet]
        [PreAuthorize]
        public IActionResult Details()
        {
            this.Model.Data["error"] = string.Empty;

            var kittenName = Request.UrlParameters["Name"];
            var kittenId = this.Context.Kittens.FirstOrDefault(x => x.Name == kittenName).Id;
            var comments = this.Context.Comments.Where(x => x.KittenId == kittenId).Select(x => $@"<h5>{x.User.Username} commented: <span style=""font-weight:normal"">{x.CommentContent}</span></h5>").ToArray();
            var kitten = this.Context.Kittens
                .Where(x => x.Name == kittenName)
                .Include(k => k.Breed)
                .Select(KittenViewModel.FromKitten)
                .Select(vm =>
                    $@"
                    <div class=""col-4"">
                        <img class=""img-thumbnail"" src=""https://images.pexels.com/photos/20787/pexels-photo.jpg?auto=compress&cs=tinysrgb&h=350"" alt=""{vm.Name}'s photo"" />
                        <div>
                            <h5>Name: {vm.Name}</h5>
                            <h5>Age: {vm.Age}</h5>
                            <h5>Breed: {vm.Breed}</h5>
                            </br>
                            <h5>Comments:</br>" + String.Join("", comments)
            + $@"  </br></br> </br><div class=""control-group"">
                        <form action=""/kittens/addComment"" method=""POST"">
<input type=""hidden"" name=""Name"" value=""{vm.Name}"" />
<div class=""controls"">
                            <label for=""comment"">Write comment: </label> <input type=""text"" id=""comment""  name=""Comment""  maxlength=""35""> <button class=""btn btn-success"">Add comment</button>
                        </div>
                        </form>
                    </div>
                        </div>
                    </div>")
                .ToList();
            var sb = new StringBuilder();
            sb.Append(@"<div align=""center"">");
            sb.Append(kitten[0]);
            sb.Append(@"</div><div class=""row text-center""></div>");

            this.Model.Data["comments"] = sb.ToString();
            return this.View();
        }
        [HttpPost]
        public IActionResult AddComment()
        {
            var commentContent = Request.FormData["Comment"];
            var kittenName = Request.FormData["Name"];
            
             using (this.Context)
             {
                 var kitten = this.Context.Kittens
                 .FirstOrDefault(b => b.Name == kittenName);
                var user = this.Context.Users
                .FirstOrDefault(b => b.Username == this.User.Name);
                var comment = new Comment()
                 {
                     CommentContent = commentContent,
                     Kitten = kitten,
                     User = user
                };
                 this.Context.Add(comment);
                 this.Context.SaveChanges();
             }
            return this.RedirectToAction($"/kittens/details?Name={kittenName}");

        }
    }
}
