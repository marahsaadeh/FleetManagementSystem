import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable, of } from 'rxjs';
import { throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { Geofences } from './geofences.model';
@Injectable({
  providedIn: 'root'
})
export abstract class GeofencesService {

  protected baseApiUrl = 'https://localhost:7062/api/Geofence/';
   formSubmittes:boolean=false;
   
   public fullData: any = null; //Postman


  constructor(protected http: HttpClient) {}
  

  abstract getGeofences(): Observable<any[]>; 

  
  public handleError(error: any) {
    console.error('An error occurred:', error.message || error);
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }
}
