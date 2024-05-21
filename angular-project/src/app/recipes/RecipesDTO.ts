export class RecipesDTO{
    id: number;
    name: string;
    postup: string;
    ingrediencie: string;
    difficulty: string;
    imageURL: string;
    pictureURL: string;
    checkID: string;
    userID: string;
    recipesID?: number;
    cas?: number;
    veganske?: boolean;
    vegetarianske?: boolean;
    nizkoKaloricke?: boolean;

  }