import { Injectable } from '@angular/core';
import { GeofencesService } from './geofences.service';
import { Geofences } from './geofences.model';
import { Observable } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class AllGeofencesService extends GeofencesService{
 
  list: Geofences[] = [];
  formData: Geofences = new Geofences();
  public rowCount: number = 0;

  getGeofences(): Observable<any[]> {
    console.log('input to claa all geofance');
    return this.http.get<{ DicOfDT: { Geofences: any[] } }>(
      `${this.baseApiUrl}GetAllGeofences`
    ).pipe(
      map(response => {
        this.fullData = response.DicOfDT.Geofences;
        console.log('All all alll allll Geofences Data: ', this.fullData);
        return this.fullData;
      }),
      tap(geofences => {
        this.rowCount = geofences.length;
        console.log('عدد الجغرافيات: ', this.rowCount);
      }),
      catchError(this.handleError)
    );
  }



  }
