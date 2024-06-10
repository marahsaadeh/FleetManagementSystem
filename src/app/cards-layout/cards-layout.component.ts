import { Component, Input, OnInit } from '@angular/core';
import { VehicleService } from '../shared/vehicle.service';
import { VehicleInformationService } from '../shared/vehicle-information.service';
import { RouteHistoryService } from '../shared/route-history.service';
import { HttpClient } from '@angular/common/http';
import { AllGeofencesService } from '../shared/all-geofences.service';

@Component({
  selector: 'app-cards-layout',
  templateUrl: './cards-layout.component.html',
  styles: []
})
export class CardsLayoutComponent implements OnInit {
  @Input() isSidebarOpen: boolean = true;

  totalVehicles: number = 0;
  totalVehicleInformation: number = 0;
  totalRouteHistories: number = 0;
  totalGeofences: number = 0;

  constructor(
    private http: HttpClient,
    private vehicleService: VehicleService,
    private vehicleInformationService: VehicleInformationService,
    private geofencesService: AllGeofencesService,
    private routeHistoryService: RouteHistoryService
  ) { }

  ngOnInit(): void {
    this.loadData();
  }
  loadData(): void {
    this.vehicleService.getVehicles().subscribe(
      () => {
        this.totalVehicles = this.vehicleService.rowCount;
      },
      error => {
  
        console.error('Failed to load vehicles', error);
      }
    );

    this.geofencesService.getGeofences().subscribe(
      () => {
        this.totalGeofences =   this.geofencesService.rowCount;
      },
      error => {
   
        console.error('Failed to load vehicles', error);
      }
    );
    this.routeHistoryService.getRouteHistories().subscribe(
      () => {
        this.totalRouteHistories =   this.routeHistoryService.rowCount;
      },
      error => {
 
        console.error('Failed to load vehicles', error);
      }
    );
    this.vehicleInformationService.getVehicleInformations().subscribe(
      () => {
        this.totalVehicleInformation =   this.vehicleInformationService.rowCount;
      },
      error => {
 
        console.error('Failed to load vehicles', error);
      }
    );
  }
  

  
}
