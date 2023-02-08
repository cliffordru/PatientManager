import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, Subject, tap, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Patient } from '../model/patient';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  apiurl = `${environment.apiBaseUrl}/patients`;

  private _refreshrequired = new Subject<void>();
  get RequiredRefresh() {
    return this._refreshrequired;
  }

  constructor(private http: HttpClient) {

  }
  GetPatients(): Observable<Patient> {
    return this.http.get<Patient>(this.apiurl);
  }
  GetPatientbyId(id: any) {
    return this.http.get(this.apiurl + '/' + id);
  }
  Remove(id: any) {
    return this.http.delete(this.apiurl + '/' + id);
  }
  Save(inputdata: any) {
    //const body = { firstName:  inputdata.firstName};
    //const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };

    //const body = JSON.stringify(inputdata.firstName);
    return this.http.put(`${this.apiurl}/${inputdata.id}`, inputdata).pipe(
      tap(() => {
        this.RequiredRefresh.next();
      }), catchError(this.handleError)
    );
  }

  //TODO: move URI to config and wire up time permitting
  GetDes() {
    return this.http.get('https://localhost:5001/api/designation');
  }

  private handleError(err: HttpErrorResponse): Observable<never> {
    // in a real world app, we may send the server to some remote logging infrastructure
    // instead of just logging it to the console
    let errorMessage = '';
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
    }
    console.error(errorMessage);
    return throwError(() => errorMessage);
  }
}
