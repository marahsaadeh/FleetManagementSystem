import { Injectable } from '@angular/core';
import { PolygonGeofence } from './polygon-geofence.model';
import { Observable } from 'rxjs';

import { catchError, map } from 'rxjs/operators';

import { GeofencesService } from './geofences.service';
@Injectable({
  providedIn: 'root'
})
export class PolygonGeofencesService  extends GeofencesService{
  list: PolygonGeofence[] = [];
  formData: PolygonGeofence = new PolygonGeofence();

  
  getGeofences(): Observable<any[]> {
    return this.http.get<{ DicOfDT: { PolygonGeofences: any[] } }>
    (`${this.baseApiUrl}GetAllPolygonGeofences`).pipe(
      map(response => {
      this.fullData = response.DicOfDT.PolygonGeofences;
      console.log('fullData Pppppppolyg',this.fullData);
      return this.fullData;
    }),
    catchError(this.handleError)
  );
}
  }
 

