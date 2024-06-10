
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable, of } from 'rxjs';
import { throwError } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { VehicleInformation } from './vehicle-information.model';

@Injectable({
  providedIn: 'root'
})

export class VehicleInformationService {
  url:string=environment.apiBaseUrl+'/VehicleInformation'
  private apiUrl =  'https://localhost:7062/api/VehicleInformation';
  list:VehicleInformation[]=[];
  formDate:VehicleInformation=new VehicleInformation()
  formSubmittes:boolean=false;
  public fullData: any = null; //Postman
  public rowCount: number = 0;
  constructor(private http: HttpClient) {}
  
  getVehicleInformation(vehicleId: number): Observable<VehicleInformation | null> {
    console.log('Fetching information for vehicle ID:', vehicleId);
    return this.http.get<any>(this.apiUrl).pipe(
      map(response => {

        var testVehicleInfoArray=response.DicOfDT.VehicleInformations as VehicleInformation[];
        testVehicleInfoArray.forEach(v => console.log('Checking Vehicle ID:', v.VehicleID, typeof v.VehicleID, v.VehicleID === vehicleId));
     

        console.log('Body Postman',testVehicleInfoArray);
        const vehicleInfo = testVehicleInfoArray.find(v => v.VehicleID === vehicleId);

        console.log('The vehicle Information is  :', vehicleInfo);

        return vehicleInfo || null;  // This will now correctly return the single VehicleInformation or null if not found
      }),
      catchError(this.handleError)
    );
  }
  getVehicleInformations(): Observable<any[]> {  // Adjust return type as needed
    return this.http.get<{DicOfDT: {VehicleInformations: any[]}}>(this.apiUrl).pipe(
      map(response => {
        this.fullData = response.DicOfDT.VehicleInformations;
        return response.DicOfDT.VehicleInformations;
      }),
      tap(vehicleInformations => {
        this.rowCount = vehicleInformations.length; // تعيين rowCount إلى عدد معلومات المركبات المسترجعة
        console.log('عدد معلومات المركبات: ', this.rowCount);
      }),
      catchError(this.handleError)
    );
  }
  addVehicleInformation(vehicleInfo: any): Observable<any> {
    const headers = new HttpHeaders({'Content-Type': 'application/json'});
    return this.http.post(`${this.apiUrl}/Add`, JSON.stringify(vehicleInfo), { headers })
      .pipe(
        catchError(this.handleError)
      );
  }


  updateVehicleInformation(vehicleInfo: any): Observable<any> {
    const headers = new HttpHeaders({'Content-Type': 'application/json'});
    return this.http.put(`${this.apiUrl}/${vehicleInfo.VehicleInformationID}`, JSON.stringify(vehicleInfo), { headers })
      .pipe(
        catchError(this.handleError)
      );
  }

  deleteVehicleInformation(id: number) {
    return this.http.delete(`${this.url}/${id}`);
  }
  private handleError(error: any) {
    console.error('An error occurred:', error.message || error);
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }
}
