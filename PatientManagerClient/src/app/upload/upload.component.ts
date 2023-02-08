import { HttpClient, HttpErrorResponse, HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit {
  progress!: number;
  message!: string;
  errorMessage = '';
  @Output() public onUploadFinished = new EventEmitter();

  constructor(private http: HttpClient) { }

  ngOnInit() {

  }

  uploadFile = (files: any) => {
    this.errorMessage = '';
    this.message = '';
    this.progress = 0;
    if (files.length === 0) {
      return;
    }

    const apiurl = `${environment.apiBaseUrl}/patients`;

    //TODO: Add client side validation & messaging
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    //TODO: move URI to config
    this.http.post(apiurl, formData, { reportProgress: true, observe: 'events' })
      .subscribe({
        next: (event) => {
          if (event.type === HttpEventType.UploadProgress && event?.loaded && event?.total)
            this.progress = Math.round(100 * event.loaded / event.total);
          else if (event.type === HttpEventType.Response) {
            this.message = `Upload was successful, ${event.body} patients added. Return home to view patients.`;
            this.onUploadFinished.emit(event.body);
          }
        },
        error: (err: HttpErrorResponse) => {
          this.progress = 0;
          this.errorMessage = 'Upload failed.';
          console.log(err)
        }
      });
  }
}