import { ChangeDetectorRef, Component, inject, Inject, OnInit } from '@angular/core';
import { IngredienceDTO } from '../../DTOs/IngredienceDTO';
import { MAT_DIALOG_DATA, MatDialogContent, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { DialogOverviewExampleDialog } from 'src/app/user-profile/user-profile.component';
import { UserDTO } from 'src/app/DTOs/UserDTO';
import { UserService } from 'src/services/user.service';
import { HttpClient } from '@angular/common/http';
import { IngredientService } from '../IngredientService';
import { CommonModule, NgForOf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatLabel } from '@angular/material/form-field';
import { getBaseUrl } from 'src/main';

@Component({
  selector: 'app-dialog',
  standalone: true,
  imports: [ FormsModule,
    MatDialogTitle,
    MatDialogContent,
    NgForOf,
    MatButtonModule,
    MatLabel],
  templateUrl: './dialog.component.html',
  styleUrl: './dialog.component.css'
})
export class DialogComponent implements OnInit{
ingredience: IngredienceDTO = {Name: ''};
  inputString: string = '';
  ingrediences: any = [];
  BASE_URL= getBaseUrl();
  //@Output() newItemEvent = new EventEmitter<string>();
  constructor(
    public dialogRef: MatDialogRef<DialogOverviewExampleDialog>,
    @Inject(MAT_DIALOG_DATA) public data: UserDTO, private userService: UserService, private httpClient: HttpClient, private ingredientService: IngredientService, private cdr: ChangeDetectorRef) {
  }

  ngOnInit() {
    this.httpClient.get<any>(this.BASE_URL + "/getIngredience").subscribe(value =>
      this.ingrediences = value,
    )
    
  }
  selectedIngredients: string = '';
/*  onCheckboxChange(ingredient: any, event: any) {
    if (event.target.checked) {
      this.selectedIngredients += ingredient.name + ', ';
    } else {
      this.selectedIngredients = this.selectedIngredients.replace(ingredient.name + ', ', '');
    }
    console.log(this.selectedIngredients)
  }*/

  onCheckboxChange(ingredient: any, event: any) {
    if (event.target.checked) {
      this.ingredientService.selectedIngredients += ingredient.name + ', ';
    } else {
      this.ingredientService.selectedIngredients = this.ingredientService.selectedIngredients.replace(ingredient.name + ', ', '');
    }
  }


  sendIngredience() {
    this.ingredience.Name = this.inputString;
    this.httpClient.post(this.BASE_URL + '/addIngredience', this.ingredience).subscribe(response => {
        console.log(response);
        // Add the new ingredient to the list without refreshing
        this.dialogRef.close({ data: this.inputString })
        this.ingrediences.push({ name: this.ingredience.Name });
        //this.newItemEvent.emit(this.inputString);
        // Clear the input field
        this.inputString = '';
        // Trigger change detection
        this.cdr.detectChanges();
      }, error => {
        console.error('Error adding ingredient:', error);
      }
    );
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  addIngredience($event: any) {
    this.ingrediences.add($event);

  }
}
