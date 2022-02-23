import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StudentsComponent } from './students/students.component';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [StudentsComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: 'students', component: StudentsComponent }
    ])
  ]
})
export class StudentModule { }
