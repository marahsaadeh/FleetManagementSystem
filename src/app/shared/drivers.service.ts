import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';  
import { environment } from '../../environments/environment';
import { Driver } from './driver.model';
import { Observable } from 'rxjs';
import { throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})

export class DriversService {
  url:string=environment.apiBaseUrl+'/Drivers'

  private apiUrl =  'https://localhost:7062/api/Drivers';
  list:Driver[]=[];
  formDate:Driver=new Driver()
  formSubmittes:boolean=false;
  public fullData: any = null; //Postman

  constructor(private http: HttpClient) {}

  getDrivers(): Observable<any[]> {  
    return this.http.get<{DicOfDT: {Driver: any[]}}>(this.apiUrl).pipe(
      map(response => {
        this.fullData = response.DicOfDT.Driver;
        console.log('dddddddddddddddddddddddddddddddddddrivers',this.fullData);
        return response.DicOfDT.Driver;
      }),
      catchError(this.handleError)
    );
  }
  
  private handleError(error: any) {
    console.error('An error occurred:', error.message || error);
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }
  addDriver(driver: any): Observable<any> {
    const headers = new HttpHeaders({'Content-Type': 'application/json'});
    return this.http.post(`${this.apiUrl}/Add`, JSON.stringify(driver), { headers })
      .pipe(
        catchError(this.handleError)
      );
  }


  updateDriver(driver: any): Observable<any> {
    const headers = new HttpHeaders({'Content-Type': 'application/json'});
    return this.http.put(`${this.apiUrl}/${driver.DriverID}`, JSON.stringify(driver), { headers })
      .pipe(
        catchError(this.handleError)
      );
  }
  deleteDriver(id: number) {
    return this.http.delete(`${this.url}/${id}`);
  }
}
