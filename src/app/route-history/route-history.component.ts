import { Component, Input, OnInit , ViewChild, TemplateRef, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { RouteHistoryService } from '../shared/route-history.service';
import { Router } from '@angular/router';
import { RouteHistory } from '../shared/route-history.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup,FormControl,Validators, NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { VehicleService } from '../shared/vehicle.service';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-route-history',
  templateUrl: './route-history.component.html',
  styles: []
})
export class RouteHistoryComponent {
public routeHistories:any[]=[];
public vehiclesData: any[] = []; 
public fullData: any; 
constructor(private toastr:ToastrService,private modalService: NgbModal ,private cdr: ChangeDetectorRef,private router: Router,public routeHisService:RouteHistoryService,public vehicleService:VehicleService  ) { }


@Output() routeHistoryAdded = new EventEmitter<RouteHistory>();
@ViewChild('updaterouteHistoryModal') updaterouteHistoryModal: any;

openUpdateModal() {
  this.modalService.open(this.updaterouteHistoryModal);
  
}
closeModal() {
  this.modalService.dismissAll();
}
loadVehicles() {
  this.vehicleService.refreshList(); 
}
ngOnInit(): void {
  this.routeHisService.getRouteHistories().subscribe(data => {
    this.routeHistories = data;
    this.sortRouteHistories();
    this.vehicleService.refreshList();
    console.log('Route Histories loaded:', this.routeHistories);
    
  }, error => {
    console.error('Failed to load route histories:', error);
  });

 

}


onSubmit(form: NgForm) {

//add
this.addRouteHistory(form);
  
}

addRouteHistory(form: NgForm) {
  this.loadVehicles();
  this.routeHisService.addRouteHistory(form.value).subscribe(
    response => {
      this.toastr.success('Route History added successfully!');
      this.routeHistories.push(response);  
      this.sortRouteHistories();         
      this.routeHistoryAdded.emit(response); 
      this.onRouteHistoryAdded(response);  
      this.closeModal();
      this.resetForm(form);
      this.cdr.detectChanges();
      alert(`Route History added successfully!\n\nResponse:\n${JSON.stringify(response, null, 2)}`);
    },
    error => {
      this.toastr.error('Failed to add route history');
      console.error('Error:', error);
    }
  );
}

openModal() {
  this.modalService.open(this.updaterouteHistoryModal );
 // this.vehicleService.getVehicles
}


resetForm(form?: NgForm) {
  if (form != null) {
    form.resetForm();
  }
  this.routeHisService.formDate = new RouteHistory();
}

openNewForm() {
  this.resetForm();  
}
onRouteHistoryAdded(routeHistory: RouteHistory) {
  if (Array.isArray(this.routeHistories)) {
    this.routeHistories.push(routeHistory);
    console.log('Route Histories after push:', this.routeHistories);
  } else {
    console.error('routeHistories is not an array', this.routeHistories);
  }  
}


sortRouteHistories() {
  this.routeHistories.sort((a, b) => {
    const dateA = new Date(a.GPSTime).getTime();
    const dateB = new Date(b.GPSTime).getTime();
    return dateA - dateB;
  });
}
}
