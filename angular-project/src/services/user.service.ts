import { Inject, Injectable, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserDTO } from 'src/app/user-profile/UserDTO';
import { RecipesDTO } from 'src/app/recipes/RecipesDTO';
import {CreatorDTO} from "../app/recipes/CreatorDTO";
import { RecensionsDTO } from 'src/app/recipes-details/recensions-dto';
import { PasswordDTO } from 'src/app/user-profile/PasswordDTO';
import { Action } from 'rxjs/internal/scheduler/Action';


@Injectable({
  providedIn: 'root'
})
export class UserService {
  private httpClient = inject(HttpClient);


  constructor(@Inject('BASE_URL') private baseUrl: string, private route: ActivatedRoute) { }
  private userUrl = this.baseUrl + '/userProfile';
  private clickedUserUrl = this.baseUrl + '/clickedUserProfile/';
  private anotherUserURL = this.baseUrl + '/user';
  getCurrentUser(){
    return this.httpClient.get<UserDTO>(this.userUrl);
  }
  userProfile(userName: string){
    return this.httpClient.get<UserDTO>(this.clickedUserUrl + userName);
  }

  getAllCreatorImages(): Observable<CreatorDTO[]> {
    return this.httpClient.get<CreatorDTO[]>(`${this.baseUrl}/getUserCreators`);
  }



  usersRecipes(userName: string){
    return this.httpClient.get<RecipesDTO[]>(this.userUrl + '/usersRecipes/' + userName);
  }
  getFavourites(){
    return this.httpClient.get<RecipesDTO[]>(this.userUrl + '/usersFavRecipes');
  }

  deleteImage(): Observable<any> {
    return this.httpClient.delete<any>(this.anotherUserURL + '/deleteImage');
  }
  getUsersRecensions(userName: string){
    return this.httpClient.get<RecensionsDTO[]>(this.clickedUserUrl + 'myRecensions/'+ userName);
  }
  changePassword(newPassword: PasswordDTO){
    return this.httpClient.post<boolean>(this.userUrl + "/changePassword", newPassword)
  }
}
