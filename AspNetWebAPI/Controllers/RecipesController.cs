﻿using AspNetCoreAPI.Data;
using AspNetCoreAPI.Migrations;
using AspNetCoreAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AspNetCoreAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecipesController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public IEnumerable<RecipesDTO> GetRecipesList()
        {
            IEnumerable<Recipe> dbRecipes = _context.Recipes;

            return dbRecipes.Select(dbRecipe =>
                new RecipesDTO
                {
                    Id = dbRecipe.Id,
                    Name = dbRecipe.Name,
                    Description = dbRecipe.Description,
                    Difficulty = dbRecipe.Difficulty,
                    ImageURL = dbRecipe.ImageURL,
                    CheckID = dbRecipe.CheckID,
                    userID = GetCurrentUser().Id,
                });
        }
        [HttpGet("{id:int}")]
        public RecipesDTO GetRecipes(int id)
        {
            var recipe = _context.Recipes.Single(savedId => savedId.Id == id);
            return new RecipesDTO
            {
                Name = recipe.Name,
                Description = recipe.Description,
                Difficulty = recipe.Difficulty,
                ImageURL = recipe.ImageURL,
                CheckID = recipe.CheckID,
                userID = GetCurrentUser().Id,
            };

        }
        [HttpPost("/CreateRecipe")]
        public RecipesDTO CreateRecipe(RecipesDTO receptik)
        {
            var user = GetCurrentUser();

            var nReceptik = new Recipe()
            {
                Name = receptik.Name,
                Description = receptik.Description,
                Difficulty = receptik.Difficulty,
                ImageURL = receptik.ImageURL,
                
            };
            nReceptik.CheckID = user?.Id;
            _context.Add(nReceptik);
            _context.SaveChanges();
            return receptik;
        }
        

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteRecipe(int id)
        {
            var recipe = _context.Recipes.FirstOrDefault(savedId => savedId.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            _context.SaveChanges();
            return NoContent();
        }

        protected ApplicationUser? GetCurrentUser()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);

            return _context.Users.SingleOrDefault(user => user.UserName == userName);
        }

    }
}
