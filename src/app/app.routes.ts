import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { VehicleInformationComponent } from './vehicle-information/vehicle-information.component';
import { VehicleInformationFormComponent } from './vehicle-information/vehicle-information-form/vehicle-information-form.component';
import { RouteHistoryComponent } from './route-history/route-history.component';
import { DriversComponent } from './drivers/drivers.component';
import { GeofencesComponent } from './geofences/geofences.component';
import { VehiclesComponent } from './vehicles/vehicles.component';
const routes: Routes = [
    { path: 'vehicles', component: VehiclesComponent,pathMatch:'full' },
    { path: 'vehicle-information', component: VehicleInformationFormComponent,pathMatch:'full' },
    {path:'route-histories',component:RouteHistoryComponent,pathMatch:'full'},
    {path:'drivers',component:DriversComponent,pathMatch:'full'},
    { path: 'vehicle-information/:id', component: VehicleInformationComponent,pathMatch:'full' },
    { path: 'geofences/:type', component: GeofencesComponent },

    { path: '', redirectTo: 'vehicles', pathMatch: 'full' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
  })
  export class AppRoutingModule {}