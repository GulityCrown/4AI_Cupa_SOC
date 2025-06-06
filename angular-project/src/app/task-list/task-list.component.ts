import {LiveAnnouncer} from '@angular/cdk/a11y';
import {AfterViewInit, Component, ViewChild, inject, numberAttribute, signal} from '@angular/core';
import {MatSort, Sort, MatSortModule} from '@angular/material/sort';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import  { TaskDTO } from '../DTOs/TaskDTO'
import { TaskService } from 'src/services/task.service';
import { Subject, Subscription, takeUntil } from 'rxjs';
import { CommonModule, DatePipe } from '@angular/common';
import { ActivatedRoute, Router, RouterLink, RouterModule } from '@angular/router';

import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatRadioModule } from '@angular/material/radio';
import { MatIconAnchor } from '@angular/material/button';

import {MatCardModule} from '@angular/material/card';
import {MatFormField, MatFormFieldModule, MatLabel} from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';



@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [MatTableModule, MatSortModule, CommonModule, DatePipe,MatIconModule,RouterLink,RouterModule,MatProgressSpinnerModule  ],
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.css'
})

export class TaskListComponent {
  isDataLoaded$: Subscription;
  isActive(url: string): boolean {
    return this.router.url === url;
  }
  constructor(){}
  router = inject(Router)
  dataSource = new MatTableDataSource<TaskDTO>();
  getMeTasks(){
    return this.taskService.getTasks()
    .pipe(takeUntil(this.destroy$))
    .subscribe(result => {this.tasks = result;
      this.dataSource.data = result;
    });
   }
   changeToFinishedOrUnfinished(id: number){

    this.taskService.changeToFinishedOrUnfinished(id)
    .pipe(takeUntil(this.destroy$))
    .subscribe(result => {
      console.log('result.id:', result.id);
      console.log('dataSource.data:', this.dataSource.data);
      console.log(id);

      // Ensure both values are numbers before comparison
      const deleteAt = this.dataSource.data.findIndex(task => Number(task.id) === Number(result.id));

      if (deleteAt === -1) {
        console.warn(`Task with id ${result.id} not found in dataSource.`);
        return; 
      }

      this.dataSource.data.splice(deleteAt, 1);
      this.dataSource.data = [...this.dataSource.data]; 

      console.log(deleteAt);
    });
   }

  tasks: TaskDTO[];
   

    private destroy$ = new Subject<void>();
  taskService = inject(TaskService);
 ngOnInit(){
  this.isDataLoaded$ = this.getMeTasks();

 }

navigateToTaskDetail(id: number){
  this.router.navigate(['TaskDetail', id])
}
  private _liveAnnouncer = inject(LiveAnnouncer);

  displayedColumns: string[] = ['priority', 'name', 'startTime', 'deadLine', 'button'];


  @ViewChild(MatSort) sort: MatSort;

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }

  /** Announce the change in sort state for assistive technology. */
  announceSortChange(sortState: Sort) {
    // This example uses English messages. If your application supports
    // multiple language, you would internationalize these strings.
    // Furthermore, you can customize the message to add additional
    // details about the values being sorted.
    if (sortState.direction) {
      this._liveAnnouncer.announce(`Sorted ${sortState.direction}ending`);
    } else {
      this._liveAnnouncer.announce('Sorting cleared');
    }
  }
  
}
export interface PeriodicElement {
  name: string;
  position: number;
  weight: number;
  symbol: string;
}
const ELEMENT_DATA: PeriodicElement[] = [
  {position: 1, name: 'Hydrogen', weight: 1.0079, symbol: 'H'},
  {position: 2, name: 'Helium', weight: 4.0026, symbol: 'He'},
  {position: 3, name: 'Lithium', weight: 6.941, symbol: 'Li'},
  {position: 4, name: 'Beryllium', weight: 9.0122, symbol: 'Be'},
  {position: 5, name: 'Boron', weight: 10.811, symbol: 'B'},
  {position: 6, name: 'Carbon', weight: 12.0107, symbol: 'C'},
  {position: 7, name: 'Nitrogen', weight: 14.0067, symbol: 'N'},
  {position: 8, name: 'Oxygen', weight: 15.9994, symbol: 'O'},
  {position: 9, name: 'Fluorine', weight: 18.9984, symbol: 'F'},
  {position: 10, name: 'Neon', weight: 20.1797, symbol: 'Ne'},
];

