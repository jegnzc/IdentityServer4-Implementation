import { Student } from '../../_interfaces/student.model';
import { RepositoryService } from '../../shared/services/repository.service';
import { AuthService } from '../../shared/services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-students',
  templateUrl: './students.component.html',
  styleUrls: ['./students.component.css']
})
export class StudentsComponent implements OnInit {
  public students: Student[];

  constructor(private repository: RepositoryService, private auth: AuthService) { }

  ngOnInit(): void {
    this.getStudents();
  }

  public getStudents = () => {
    console.log(this.auth.getAccessToken());
    console.log(this.auth.getCurrentUser());
    if (this.auth.isAuthenticated()) {
      const apiAddress: string = "student";
      this.repository.getData(apiAddress)
        .subscribe(res => {
          this.students = res as Student[];
        })
    }

  }
}
