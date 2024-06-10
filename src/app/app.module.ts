import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ModalModule } from 'ngx-bootstrap/modal';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { VehiclesComponent } from './vehicles/vehicles.component';
import { VehicleFormComponent } from './vehicles/vehicle-form/vehicle-form.component';
import { VehicleInformationComponent } from './vehicle-information/vehicle-information.component';
import { VehicleInformationFormComponent } from './vehicle-information/vehicle-information-form/vehicle-information-form.component';
import { HttpClientModule } from '@angular/common/http';
import { NavbarLayoutComponent } from './navbar-layout/navbar-layout.component';
import { SidebarLayoutComponent } from './sidebar-layout/sidebar-layout.component';
import { CardsLayoutComponent } from './cards-layout/cards-layout.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouteHistoryComponent } from './route-history/route-history.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ToastrModule } from 'ngx-toastr';
import { GeofencesComponent } from './geofences/geofences.component';
import { AppRoutingModule } from './app.routes';
import { DriversComponent } from './drivers/drivers.component';
import { DriversFormComponent } from './drivers/drivers-form/drivers-form.component';
@NgModule({
  declarations: [AppComponent,VehiclesComponent,VehicleFormComponent,NavbarLayoutComponent,SidebarLayoutComponent,CardsLayoutComponent
    ,VehicleInformationComponent,VehicleInformationFormComponent,RouteHistoryComponent,GeofencesComponent,DriversComponent,DriversFormComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    ModalModule.forRoot(),
    HttpClientModule,
    ReactiveFormsModule,
    BrowserAnimationsModule, 
    ToastrModule.forRoot(),
    NgbModule,
    AppRoutingModule,
  
    FormsModule
    //RouterModule.forRoot([])  

  ],
  providers:[],
  bootstrap:[AppComponent]
})
export class AppModule { }
