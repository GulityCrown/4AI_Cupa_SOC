import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { RecipesDTO } from 'src/app/recipes/RecipesDTO'
import { createRecipe } from './app/create-recipe/createRecipe';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RecipesService {
  private recipesURL = this.baseUrl + '/recipes/';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getRecipesList() {
    return this.http.get<RecipesDTO[]>(this.recipesURL)
  }

  getClickedRecipes(Id: number) {
    return this.http.get<RecipesDTO>(this.recipesURL + Id);
  }
  CreateRecipe(RecipesDTO: createRecipe) {
    return this.http.post<createRecipe>(this.baseUrl + '/CreateRecipe', RecipesDTO)
  }
}
