import { Component, Input, OnInit , ViewChild, TemplateRef, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { GeofencesService } from '../shared/geofences.service';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { Geofences } from '../shared/geofences.model';
import { AllGeofencesService } from '../shared/all-geofences.service';
import { CircleGeofencesService } from '../shared/circle-geofences.service';
import { RectangleGeofencesService } from '../shared/rectangle-geofences.service';
import { PolygonGeofencesService } from '../shared/polygon-geofences.service';
import { GeofenceServiceFactory } from '../shared/geofence-service-factory.service';

@Component({
  selector: 'app-geofences',
  templateUrl: './geofences.component.html',
  styles: []
})
export class GeofencesComponent implements OnInit {
  public geofences:any[]=[];
  public type!: string;
  public fullData: any; 
  constructor(private allGeofencesService:AllGeofencesService ,private circleGeofencesService: CircleGeofencesService,private polygonGeofencesService: PolygonGeofencesService,private rectangleGeofencesService: RectangleGeofencesService,private geofenceServiceFactory:GeofenceServiceFactory,private router: ActivatedRoute) { }

  ngOnInit(): void {
    this.router.params.subscribe(params => {
      this.type = params['type']; 
      this.loadGeofences(this.type);
    });
  }
 public loadGeofences(type: string): any {

    if(type=='all')this.allGeofencesService.getGeofences().subscribe(data => this.geofences = data);
     
    const service = this.geofenceServiceFactory.getService(type);
    service.getGeofences().subscribe(
      (data: GeofencesService[]) => {
        this.geofences = data;
      },
      (error: any) => {  
        console.error('Error loading geofences:', error);
      }
    );
  }
  
}
