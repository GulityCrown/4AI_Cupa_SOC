﻿using AspNetCoreAPI.Data;
using AspNetCoreAPI.Migrations;
using AspNetCoreAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AspNetCoreAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _context;


    

        public RecipesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<RecipesDTO> GetRecipesList()
        {
            IEnumerable<Recipe> dbRecipes = _context.Recipes;

            return dbRecipes.Select(dbRecipe =>
                new RecipesDTO
                {
                    Id = dbRecipe.Id,
                    Name = dbRecipe.Name,
                    Postup = dbRecipe.Postup,
                    Difficulty = dbRecipe.Difficulty,
                    ImageURL = dbRecipe.ImageURL,
                    CheckID = dbRecipe.CheckID,
                    userID = dbRecipe.userID,
                    Ingrediencie = dbRecipe.Ingrediencie,
                    Veganske = dbRecipe.Veganske,
                    Vegetarianske = dbRecipe.Vegetarianske,
                    NizkoKaloricke = dbRecipe.NizkoKaloricke,
                    Cas = dbRecipe.Cas,
                    imageId = dbRecipe.ImageId
                });
        }
        [HttpGet("{id:int}")]
        public RecipesDTO GetRecipes(int id)
        {
            var recipe = _context.Recipes.Single(savedId => savedId.Id == id);
            return new RecipesDTO
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Postup = recipe.Postup,
                Difficulty = recipe.Difficulty,
                ImageURL = recipe.ImageURL,
                CheckID = recipe.CheckID,
                userID = GetCurrentUser().Id,
                Ingrediencie = recipe.Ingrediencie,
                Veganske = recipe.Veganske,
                Vegetarianske = recipe.Vegetarianske,
                NizkoKaloricke = recipe.NizkoKaloricke,
                Cas = recipe.Cas,
                imageId = recipe.ImageId
            };
           
        }

        [HttpGet("/getImage/{id:int}")]
        public ImagesDTO GetImage(int id)
        {
            var image = _context.Images.Single(savedId => savedId.Id == id);
            return new ImagesDTO
            {
                Id = image.Id,
                image = image.image
            };

        }


        [HttpGet("/getAllImages")]
        public ActionResult<List<ImagesDTO>> GetImages()
        {
            var images = _context.Images.ToList();
            var imagesDTO = images.Select(image => new ImagesDTO
            {
                Id = image.Id,
                image = image.image
            }).ToList();

            return Ok(imagesDTO);

        }



        [HttpPost("/CreateRecipe")]
        public RecipesDTO CreateRecipe(RecipesDTO receptik)
        {
            var user = GetCurrentUser();

            var nReceptik = new Recipe()
            {
                Id = receptik.Id,
                Name = receptik.Name,
                Postup = receptik.Postup,
                Difficulty = receptik.Difficulty,
                ImageURL = receptik.ImageURL,
                CheckID = receptik.CheckID,
                userID = GetCurrentUser().Id,
                Ingrediencie = receptik.Ingrediencie,
                Veganske = receptik.Veganske,
                Vegetarianske = receptik.Vegetarianske,
                NizkoKaloricke = receptik.NizkoKaloricke,
                Cas = receptik.Cas,
                ImageId = receptik.imageId

            };
            nReceptik.CheckID = user?.Id;
            _context.Add(nReceptik);
            _context.SaveChanges();
            return receptik;
        }

        [Route("upload")]
        [HttpPost]
        public int SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                var filename = postedFile.FileName;

                byte[] fileBytes;
                using (var memoryStream = new MemoryStream())
                {
                    postedFile.CopyTo(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }


                var images = new Models.Images()
                {
                    image = fileBytes
                };
         
                _context.Images.Add(images);
                _context.SaveChanges();

                return images.Id;
            }
            catch (Exception)
            {
                return 0;
            }

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


        /*   [HttpPost("upload")]
           public IActionResult UploadImage()
           {
               return Ok(new { message = "Image uploaded successfully" });
           }
           */
        [HttpPut("Editujem")]
        public IActionResult Edit(EditDTO receptik)
        {
            var nReceptik = _context.Recipes.FirstOrDefault(x => x.Id == receptik.Id);

            nReceptik.Name = receptik.Name;
            nReceptik.Ingrediencie = receptik.Ingrediencie;
            nReceptik.Postup = receptik.Postup;
            nReceptik.ImageURL = receptik.ImgURL;
            nReceptik.Cas = receptik.Cas;
            _context.SaveChanges();
            return Ok();
        }
        [HttpPost("PridajRecenziu")]
        public RecensionDTO AddRecension(RecensionDTO nRecenzia)
        {
            Recensions recenzia = new Recensions();
            //var recenzia = _context.Recensions.FirstOrDefault(x => x.UserId == GetCurrentUser().Id && x.RecipeId == nRecenzia.RecipesID);
            recenzia.RecipeId = nRecenzia.RecipesID;
            recenzia.Content = nRecenzia.Content;
            recenzia.Datetime = nRecenzia.Datetime;
            recenzia.UserImage = nRecenzia.UserImage;
            recenzia.UserName = GetCurrentUser().UserName;
            recenzia.UserId = GetCurrentUser().Id;
            _context.Add(recenzia);
            _context.SaveChanges();
            var vraciam = new RecensionDTO()
            {
                Id = recenzia.Id,
                RecipesID = nRecenzia.RecipesID,
                Content = nRecenzia.Content,
                UserName = GetCurrentUser().UserName,
                
            };
            return vraciam;
            
        }
        [HttpGet("recenzie/{id:int}")]
        public IEnumerable<RecensionDTO> GetRecensions([FromRoute] int id)
        {
            IEnumerable<Recensions> dbRecensions = _context.Recensions.Where(x => x.RecipeId == id).ToArray();


            return dbRecensions.Select(dbRecension =>
                new RecensionDTO
                {
                    Id = dbRecension.Id,
                    Content = dbRecension.Content,
                    Datetime = dbRecension.Datetime,
                    UserImage = dbRecension.UserImage,
                    UserID = dbRecension.UserId,
                    UserName = dbRecension.UserName,
                    AmountOfLikes = dbRecension.AmountOfLikes,
                    AmountOfDisslikes = dbRecension.AmountOfDisslikes,
                    CheckID = GetCurrentUser().Id
                }); 
        }
        [HttpPost("likeRecension/{id:int}")]
        public RecensionDTO LikeRecension([FromBody] int RecensionId)
          {
            var dLike = _context.Recensions.Where(x => x.Id == RecensionId).Single<Recensions>();
            var existuje = _context.LikeRecensions.Any(x => x.RecenziaId == RecensionId && x.UserId == GetCurrentUser().Id && x.IsLiked == true);
            var jeDisslikenuty = _context.LikeRecensions.Any(x => x.RecenziaId == RecensionId && x.UserId == GetCurrentUser().Id && x.IsLiked == false);


            if (existuje)
            {
                var jeLiknuty = _context.LikeRecensions.Where(x => x.RecenziaId == RecensionId && x.UserId == GetCurrentUser().Id && x.IsLiked == true).Single<LikeRecensions>();
                _context.LikeRecensions.Remove(jeLiknuty);
                dLike.AmountOfLikes -= 1;
                _context.SaveChanges();
                
            }
            else if (jeDisslikenuty)
            {
                var jeDissLiknuty = _context.LikeRecensions.Where(x => x.RecenziaId == RecensionId && x.UserId == GetCurrentUser().Id && x.IsLiked == false).Single<LikeRecensions>();
                jeDissLiknuty.IsLiked = true;
                dLike.AmountOfLikes += 1; 
                dLike.AmountOfDisslikes -= 1;
                _context.SaveChanges();
            }
            else if (!existuje)
            {
                LikeRecensions nLike = new LikeRecensions()
                {
                    User = GetCurrentUser(),
                    Recenzia = dLike,
                    IsLiked = true,
                    
                    RecenziaId = RecensionId,
                    UserId = GetCurrentUser().Id
                };
                dLike.AmountOfLikes += 1;

                _context.LikeRecensions.Add(nLike);
                _context.SaveChanges();

            }
            var pseudoSignaly = new RecensionDTO()
            {
                RecipesID = dLike.RecipeId,
                UserName = dLike.UserName,
                Content = dLike.Content,
                Id = dLike.Id,
                AmountOfLikes = dLike.AmountOfLikes,
                AmountOfDisslikes = dLike.AmountOfDisslikes
            };
            return pseudoSignaly;
        }
        [HttpGet("/Homepage/returnRandomRecipe")]
        public IEnumerable<RecipesDTO> ReturnRandomRecipe()
        {
            List<Recipe> filtrovany = new List<Recipe>();
            int pocet = _context.Recipes.Count();
            if (pocet != 0)
            {
                Random rng = new Random();
                List<Recipe> dbRecipes = _context.Recipes.ToList();
                List<int> randomIds = new List<int>();
                int rng2;
             for (int i = 0; i < pocet - pocet/2 ; i++)
            {
                rng2 = rng.Next(1, pocet);
                if (randomIds.Contains(rng2))
                {
                    continue;
                }


                else
                {
                    filtrovany.Add(dbRecipes[rng2]);

                }
                randomIds.Add(rng2);

            }




            return filtrovany.Select(dbRecipe =>
                new RecipesDTO
                {
                    Id = dbRecipe.Id,
                    Name = dbRecipe.Name,
                    Postup = dbRecipe.Postup,
                    Difficulty = dbRecipe.Difficulty,
                    ImageURL = dbRecipe.ImageURL,
                    CheckID = dbRecipe.CheckID,
                    userID = dbRecipe.userID,
                    Ingrediencie = dbRecipe.Ingrediencie,
                    Veganske = dbRecipe.Veganske,
                    Vegetarianske = dbRecipe.Vegetarianske,
                    NizkoKaloricke = dbRecipe.NizkoKaloricke,
                    Cas = dbRecipe.Cas,
                    imageId = dbRecipe.ImageId
                }).Reverse();
        }
            else {
                return null;
                }

            }
    /*int pocet = _context.Recipes.Count();


    if (pocet != 0) {
        Random rng = new Random();
        int dalsi = rng.Next(1, pocet);
        List<Recipe> dbRecipes = _context.Recipes.Where(x => x.Id == dalsi).ToList();
        int rng2;
        List<int> randomIds = new List<int>();
        rng.Next(1, pocet);
        for (int i = 0; i < pocet - (pocet / 2); i++)
        {

            rng2 = rng.Next(1, pocet);
            if (randomIds.Contains(rng2))
            {
                continue;
            }


            else
            {
                var pridaj = _context.Recipes.Where(x => x.Id == rng2).Single<Recipe>();

                dbRecipes.Add(pridaj);
            }
            randomIds.Add(rng2);

        }




        return dbRecipes.Select(dbRecipe =>
            new RecipesDTO
            {
                Id = dbRecipe.Id,
                Name = dbRecipe.Name,
                Postup = dbRecipe.Postup,
                Difficulty = dbRecipe.Difficulty,
                ImageURL = dbRecipe.ImageURL,
                CheckID = dbRecipe.CheckID,
                userID = dbRecipe.userID,
                Ingrediencie = dbRecipe.Ingrediencie,
                Veganske = dbRecipe.Veganske,
                Vegetarianske = dbRecipe.Vegetarianske,
                NizkoKaloricke = dbRecipe.NizkoKaloricke,
                Cas = dbRecipe.Cas,
                imageId = dbRecipe.ImageId
            }).Reverse();
    }
    else {
        return null;
        }*/
        [HttpPost("disslikeRecension/{id:int}")]
        public RecensionDTO DisslikeRecension([FromBody] int RecensionId)
        {
            var dLike = _context.Recensions.Where(x => x.Id == RecensionId).Single<Recensions>();
            var existuje = _context.LikeRecensions.Any(x => x.RecenziaId == RecensionId && x.UserId == GetCurrentUser().Id && x.IsLiked == false);
            var jeLikenuty = _context.LikeRecensions.Any(x => x.RecenziaId == RecensionId && x.UserId == GetCurrentUser().Id && x.IsLiked == true);

            if (existuje)
            {
                var jeLiknuty = _context.LikeRecensions.Where(x => x.RecenziaId == RecensionId && x.UserId == GetCurrentUser().Id && x.IsLiked == false).Single<LikeRecensions>();
                _context.LikeRecensions.Remove(jeLiknuty);
                dLike.AmountOfDisslikes -= 1;

                _context.SaveChanges();

            }
            else if (jeLikenuty)
            {

                var jeDissLiknuty = _context.LikeRecensions.Where(x => x.RecenziaId == RecensionId && x.UserId == GetCurrentUser().Id && x.IsLiked == true).Single<LikeRecensions>();
                jeDissLiknuty.IsLiked = false;
                dLike.AmountOfLikes -= 1;
                dLike.AmountOfDisslikes += 1;
                _context.SaveChanges();
            }
            else if (!existuje)
            {
                LikeRecensions nLike = new LikeRecensions()
                {
                    User = GetCurrentUser(),
                    Recenzia = dLike,
                    IsLiked = false,

                    RecenziaId = RecensionId,
                    UserId = GetCurrentUser().Id
                };
                dLike.AmountOfDisslikes += 1;

                _context.LikeRecensions.Add(nLike);
                _context.SaveChanges();

            }
            var pseudoSignaly = new RecensionDTO()
            {
                RecipesID = dLike.RecipeId,
                UserName = dLike.UserName,
                Content = dLike.Content,
                Id = dLike.Id,
                AmountOfLikes = dLike.AmountOfLikes,
                AmountOfDisslikes = dLike.AmountOfDisslikes
            };
            return pseudoSignaly;
        }
        [HttpDelete("removeRecension/{id:int}")]
        public RecensionDTO RemoveRecension([FromRoute]int id)
        {
            var vymaz = _context.Recensions.Where(x => x.Id == id).Single();
            _context.Remove(vymaz);
            _context.SaveChanges();
            return null;

            

        }


    }


}
