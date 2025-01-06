﻿using System.Security.Cryptography.X509Certificates;

namespace AspNetCoreAPI
{
    public class RecipesDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Postup { get; set; }
        public string? ImageURL { get; set; }
        public string? Difficulty { get; set; }
        public string? CheckID { get; set; }
        public string? Ingrediencie { get; set; }
        public int? Cas { get; set; }
        public bool? Veganske { get; set; }
        public bool? Vegetarianske { get; set; }
        public bool? NizkoKaloricke { get; set; }
        public string? userID {  get; set; }
        public List<string>? Postupicky { get; set; }

        public int? imageId { get; set; }   

}
}
