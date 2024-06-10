
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';  
import { environment } from '../../environments/environment';
import { Vehicle } from './vehicle.model';
import { Observable } from 'rxjs';
import { throwError } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { catchError, map, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})

export class VehicleService {
  
  url:string=environment.apiBaseUrl+'/Vehicles'
  private apiUrl = 'https://localhost:7062/api/Vehicles';
  public rowCount: number = 0;
  list:Vehicle[]=[];
formDate:Vehicle=new Vehicle()
formSubmittes:boolean=false;

  fullData: any = null; //Postman
  constructor(private http:HttpClient) { }



  getVehicles(): Observable<Vehicle[]> {
    return this.http.get<any>(this.url).pipe(
      map(response => response.DicOfDT.Vehicles),
      tap(vehicles => this.rowCount = vehicles.length),
      finalize(() => console.log('count', this.rowCount)),
      catchError(this.handleError)
    );
  }
  
  refreshList() {
    this.http.get(this.url).subscribe({
      next: res => {
        const data = res as any; 
        this.list = data.DicOfDT.Vehicles as Vehicle[];
        this.fullData = data;//Posyman
       console.log('vehicles Data:',this.fullData )
        this.formSubmittes=false;
      },
      error: err => { console.error('Error fetching vehicles', err); }
    });
  }
  private handleError(error: any) {
    console.error('An error occurred:', error.error.message);
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }
  
  addVehicle(vehicle: any): Observable<any> {
    const headers = new HttpHeaders({'Content-Type': 'application/json'});
    return this.http.post(`${this.apiUrl}/Add`, JSON.stringify(vehicle), { headers })
      .pipe(
        catchError(this.handleError)
      );
  }


  updateVehicle(vehicle: any): Observable<any> {
    const headers = new HttpHeaders({'Content-Type': 'application/json'});
    return this.http.put(`${this.apiUrl}/${vehicle.VehicleID}`, JSON.stringify(vehicle), { headers })
      .pipe(
        catchError(this.handleError)
      );
  }
  
  deleteVehicle(id: number) {
    return this.http.delete(`${this.url}/${id}`);
  }

}
