import { Injectable } from '@angular/core';
import { Observable,  } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { GeofencesService } from './geofences.service';
import { CircleGeofence } from './circle-geofence.model';
@Injectable({
  providedIn: 'root'
})
export class CircleGeofencesService extends GeofencesService{
  list: CircleGeofence[] = [];
  formData: CircleGeofence = new CircleGeofence();

  
  getGeofences(): Observable<any[]> {
    return this.http.get<{ DicOfDT: { CircleGeofences: any[] } }>
    (`${this.baseApiUrl}GetAllCircleGeofences`).pipe(
      map(response => {
      this.fullData = response.DicOfDT.CircleGeofences;
      console.log('Cccccircle Geofences: ',this.fullData);
      return this.fullData;
    }),
    catchError(this.handleError)
  );
}
  }
 

