import { Component, Input, OnInit , ViewChild, TemplateRef, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { DriversService } from '../shared/drivers.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup,FormControl,Validators, NgForm } from '@angular/forms';
import { Driver } from '../shared/driver.model';
import { HttpErrorResponse } from '@angular/common/http';
@Component({
  selector: 'app-drivers',

  templateUrl: './drivers.component.html',
  styles: []
})
export class DriversComponent implements OnInit {
 drivers :Driver[]=[];
public fullData: any; 
 
@Input() isSidebarOpen: boolean = true;
driverForm!: FormGroup;
@ViewChild('updateDriverModal') private updateDriverModalRef!: TemplateRef<any>;
@Output() showMore = new EventEmitter<Driver>();


constructor(private driversService:DriversService , private toastr:ToastrService,private modalService: NgbModal,private router: Router) { }
openModal(vehicle: Driver) {
  this.driversService.formDate = Object.assign({}, vehicle); 
  this.modalService.open(this.updateDriverModalRef); 
}

closeModal() {
  this.modalService.dismissAll();
}
ngOnInit(): void {
  this.driversService.getDrivers().subscribe(data => {
    this.drivers = data;
    this.fullData = this.driversService.fullData;  
    console.log('FullData Ddddddrivers', this.fullData);
  });
}
onDriverAdded(driver: Driver) {
  if (Array.isArray(this.drivers)) {
    this.drivers.push(driver);
    console.log('vehicles before push:', this.drivers);

  } else {
    console.error('vehicles is not an array', this.drivers);
  }  
}
deleteDriver(id: number) {
  if (confirm("Are you sure you want to delete this driver?")) {
    this.driversService.deleteDriver(id).subscribe({
      next: (response) => { 
        this.toastr.success('Driver deleted successfully');
        this.driversService.getDrivers();

        alert(`Driver deleted successfully!\n\nResponse:\n${JSON.stringify(response, null, 2)}`);
      },
      error: (error: HttpErrorResponse) => {
        console.error('Error deleting driver', error);

        alert(`Failed to delete driver: ${error.status} ${error.message}`);
      }
    });
  }
}

}
