import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from 'src/services/user.service';
import { Subject, take, takeUntil } from 'rxjs';
import { NakupnyZoznam } from '../DTOs/NakupnyZoznamDTO';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import {
  GoogleGenerativeAI, HarmBlockThreshold, HarmCategory 
} from '@google/generative-ai';
import { MatIcon } from '@angular/material/icon';

export const environment = {
  API_KEY: "AIzaSyDsaNk1Gesqy1mFhA5Maj-w83uUClWzr-8",
};

@Component({
  selector: 'app-nakupny-zoznam',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, MatIcon],
  templateUrl: './nakupny-zoznam.component.html',
  styleUrl: './nakupny-zoznam.component.css'
})

export class NakupnyZoznamComponent {


  genAI = new GoogleGenerativeAI(environment.API_KEY);
  generationConfig = {
   safetySettings: [
     {
       category: HarmCategory.HARM_CATEGORY_HARASSMENT,
       threshold: HarmBlockThreshold.BLOCK_LOW_AND_ABOVE,
     },
   ],
   temperature: 0.9,
   top_p: 1,
   top_k: 32,
   maxOutputTokens: 100, // limit output
 };
  model = this.genAI.getGenerativeModel({
   model: 'gemini-2.0-flash', // or 'gemini-pro-vision'
   ...this.generationConfig,
 });

 ceny:string[] = [];
 async TestGeminiPro() {
  // Model initialisation missing for brevity

  
  const prompt = `
  Prosím, uveď orientačné ceny NÁSLEDUJÚCICH potravín v slovenských obchodoch. Chcem len cenu v eurách (€) vedľa názvu každej potraviny. Bez zbytočných úvodov alebo poznámok, prosím.
  
  Formát: Názov (Hmotnosť/Objem) Cena €
  
  Potraviny:
  ${this.nIngrediencie}
  
  Nakoniec, uveď JEDNU RIADKU s odhadovanou CELKOVOU CENOU všetkých potravín DOKOPY.
  `;
  
  const result = await this.model.generateContent(prompt);
  const response = await result.response;

  this.ceny = response.text().split("\n").filter(x => x != "");
  console.log(this.ceny);
}



  ingrediencie = signal<NakupnyZoznam[]>([]);
  nIngrediencie: string[] = [];
  route = inject(ActivatedRoute)
  userService = inject(UserService);
  addIngr = new FormControl('');
  private destroy$ = new Subject<void>();
  pIsChecked = new FormControl(null)
  
  ngOnInit(){
    const day = this.route.snapshot.paramMap.get('name');

    this.userService.getList(day)
    .pipe(takeUntil(this.destroy$))
    .subscribe(result => {
      this.ingrediencie.set(result);
      this.nIngrediencie = result.map(ingredient => ingredient.name);
      if(this.ingrediencie().length > 0){
      this.TestGeminiPro();
      }
    });
  }
  day: string = "";
  addIngredient(){
    this.day = this.route.snapshot.paramMap.get('name');
    this.nIngrediencie.push(this.addIngr.value)
    this.userService.pridatDoNakupnehoZoznamu({
      name: this.addIngr.value,
      isChecked: this.pIsChecked.value,
      day: this.day
    }).pipe(takeUntil(this.destroy$))
    .subscribe(result => {this.ingrediencie.update(x => [...x, result])
      this.nIngrediencie = this.ingrediencie().map(x => x.name)
        this.TestGeminiPro();

    })
    this.addIngr.reset;
    

  }

  checkOwned(id: number){
    this.userService.checkOwned(id)
    .pipe(takeUntil(this.destroy$))
    .subscribe();
  }
  DeleteSpecific(){
    this.day = this.route.snapshot.paramMap.get('name');
    this.userService.deleteSelected(this.day)
    .pipe(takeUntil(this.destroy$))
    .subscribe(res => this.ingrediencie.set(null));
    this.ceny = [];
  }
  deleteItem(id: number){
    this.userService.deleteItemInsideShoppingList(id)
    .pipe(takeUntil(this.destroy$))
    .subscribe(res => this.ingrediencie.update(items => items.filter(item => item.id !== id)));
    this.nIngrediencie = this.ingrediencie().filter(x => x.id != id).map(ingredient => ingredient.name);
    this.TestGeminiPro();
  }
  
  

}
