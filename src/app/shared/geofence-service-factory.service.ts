import { Injectable } from '@angular/core';
import { CircleGeofencesService } from './circle-geofences.service';
import { RectangleGeofencesService } from './rectangle-geofences.service';
import { PolygonGeofencesService } from './polygon-geofences.service';
import { GeofencesService } from './geofences.service';
import { AllGeofencesService } from './all-geofences.service';
@Injectable({
  providedIn: 'root'
})
export class GeofenceServiceFactory {
  constructor(private allGeofencesService: AllGeofencesService, 
    private circleGeofencesService: CircleGeofencesService,
    private rectangleGeofencesService: RectangleGeofencesService,
    private polygonGeofencesService: PolygonGeofencesService,


) {}

    getService(type: string): any {
  
      if (type == null) {
        return null;
      }
   
      
      switch (type) {
        case 'all':
        return this.allGeofencesService;
        case 'circle':
        return this.circleGeofencesService;
        case 'rectangle':
        return this.rectangleGeofencesService;
        case 'polygon':
        return this.polygonGeofencesService;
        default:
        throw new Error('Unknown geofence type');
        }
        }
    }
  

