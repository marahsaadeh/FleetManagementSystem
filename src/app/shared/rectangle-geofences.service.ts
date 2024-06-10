import { Injectable } from '@angular/core';
import { RectangleGeofence } from './rectangle-geofence.model';
import { Observable } from 'rxjs';

import { catchError, map } from 'rxjs/operators';

import { GeofencesService } from './geofences.service';
@Injectable({
  providedIn: 'root'
})
export class RectangleGeofencesService  extends GeofencesService{
  list: RectangleGeofence[] = [];
  formData: RectangleGeofence = new RectangleGeofence();

  
  getGeofences(): Observable<any[]> {
    return this.http.get<{ DicOfDT: { RectangleGeofences: any[] } }>
    (`${this.baseApiUrl}GetAllRectangleGeofences`).pipe(
      map(response => {
      this.fullData = response.DicOfDT.RectangleGeofences;
      console.log('fullData Rrrrrrrrrrregtangle',this.fullData);
      return this.fullData;
    }),
    catchError(this.handleError)
  );
}
  }
 

