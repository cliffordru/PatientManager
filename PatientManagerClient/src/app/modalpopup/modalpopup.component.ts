import { formatDate } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import * as alertify from 'alertifyjs';
import { catchError, Observable, throwError } from 'rxjs';
import { ApiService } from '../service/patients';

@Component({
  selector: 'app-modalpopup',
  templateUrl: './modalpopup.component.html',
  styleUrls: ['./modalpopup.component.css']
})
export class ModalpopupComponent implements OnInit {

  constructor(private service: ApiService, public dialogref: MatDialogRef<ModalpopupComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  desdata: any;
  respdata: any;
  editdata: any;

  ngOnInit(): void {
    //this.loadDes();
    if (this.data.patientid != null && this.data.patientid != '') {
      this.LoadEditData(this.data.patientid);
    }
  }

  loadDes() {
    this.service.GetDes().subscribe(result => {
      this.desdata = result;
    });
  }

  LoadEditData(id: any) {
    this.service.GetPatientbyId(id).subscribe(item => {
      this.editdata = item;
      this.Reactiveform.setValue({
        id: this.editdata.id, firstName: this.editdata.firstName, lastName: this.editdata.lastName, birthday: formatDate(this.editdata.birthday, "MM/dd/yyyy", "en-US"),
        gender: this.editdata.gender
      })
    });
  }

  Reactiveform = new FormGroup({
    id: new FormControl({ value: 0, disabled: false }),
    firstName: new FormControl("", Validators.compose([Validators.required, Validators.maxLength(35)])),
    lastName: new FormControl("", Validators.compose([Validators.required, Validators.maxLength(35)])),
    birthday: new FormControl("", Validators.compose([Validators.required, Validators.minLength(6), Validators.maxLength(10)])), 
    /*designation: new FormControl(""),*/
    gender: new FormControl("", Validators.compose([Validators.required, Validators.maxLength(10)]))
  });
  SavePatient() {
    if (this.Reactiveform.valid) {
      this.service.Save(this.Reactiveform.value).subscribe(result => {
        this.respdata = result;
        if (this.respdata == '204') {
          alertify.success("Patient saved successfully.")
          this.dialogref.close();
        }
      });
    } else {
      //TODO: better validation messaging
      alertify.error("Please Enter valid data")
    }
  }
}
