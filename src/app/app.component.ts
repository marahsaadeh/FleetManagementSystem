import { Component } from '@angular/core';
import { Vehicle } from './shared/vehicle.model';


@Component({
  selector: 'app-root',


  templateUrl: './app.component.html',
  styles: []
})
export class AppComponent {
  title = 'FleetManagementApp';
  isSidebarOpen = true;  
  isVehicleInfoVisible = false; 
  selectedVehicle: Vehicle | null = null; 

  showVehicleInfo(vehicle: Vehicle) {
    this.selectedVehicle = vehicle;
    this.isVehicleInfoVisible = true;
  }

  hideVehicleInfo(): void {
    this.isVehicleInfoVisible = false;
    this.selectedVehicle = null;  
  }

  toggleSidebar(open: boolean): void {
    this.isSidebarOpen = open;
  }

}
