using KittenApp.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace KittenApp.Web.Models.ViewModels
{
    public class BreedViewModel
    {
        public string Name { get; set; }
        public ICollection<Kitten> Kittens { get; set; }
        public static Expression<Func<Breed, BreedViewModel>> FromBreed =>
            k => new BreedViewModel()
            {
                Name = k.Name,
                Kittens = k.Kittens
            };
    }
}
