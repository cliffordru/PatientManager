import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import * as alertify from 'alertifyjs';
import { ModalpopupComponent } from '../modalpopup/modalpopup.component';
import { Patient } from '../model/patient';
import { ApiService } from '../service/patients';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {
  title = 'Patient Manager';
  displayedColumns: string[] = ['id', 'firstName', 'lastName', 'birthday', 'gender', 'action'];
  dataSource: any;
  patientdata: any;
  errorMessage = '';

  @ViewChild(MatPaginator) paginator !: MatPaginator;
  @ViewChild(MatSort) sort !: MatSort;

  constructor(private service: ApiService, private dialog: MatDialog) {

  }
  ngOnInit(): void {
    this.GetAll();
    this.service.RequiredRefresh.subscribe(r => {
      this.GetAll();
    });
  }

  /* TODO: error handling
  ,
      error: (err: HttpErrorResponse) => 
      {
        this.progress = 0;
        this.errorMessage = 'Upload failed.';
        console.log(err)
      }
      */
  GetAll() {
    this.service.GetPatients().subscribe(result => {
      this.patientdata = result;

      this.dataSource = new MatTableDataSource<Patient>(this.patientdata)
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }
  Filterchange(event: Event) {
    const filvalue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filvalue;
  }
  getrow(row: any) {
    //console.log(row);
  }
  FunctionEdit(id: any) {
    this.OpenDialog('1000ms', '600ms', id)
  }
  FunctionDelete(id: any) {
    alertify.confirm("Delete Patient", "Are you sure you want to permanently delete this Patient?", () => {
      this.service.Remove(id).subscribe(result => {
        this.GetAll();
        alertify.success("Patient successfully deleted.")
      });

    }, function () {

    }).set('labels', {ok:'Permanently Delete', cancel:'Cancel', });

  }

  OpenDialog(enteranimation: any, exitanimation: any, id: any) {

    this.dialog.open(ModalpopupComponent, {
      enterAnimationDuration: enteranimation,
      exitAnimationDuration: exitanimation,
      width: "50%",
      data: {
        patientid: id
      }
    })
  }


}

