import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { Driver } from '../../shared/driver.model';
import { DriversService } from '../../shared/drivers.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';


@Component({
  selector: 'app-drivers-form',

  templateUrl: './drivers-form.component.html',
  styles: []
})
export class DriversFormComponent {
  @Output() driverAdded = new EventEmitter<Driver>();
  @ViewChild('updateDriverModal') updateDriverModal: any;

  constructor(public service:DriversService, private toastr: ToastrService, private modalService: NgbModal) {}
  openUpdateModal() {
    this.modalService.open(this.updateDriverModal);
  }
  closeModal() {
    this.modalService.dismissAll();
  }
  onSubmit(form: NgForm) {
    if (this.service.formDate.DriverID) {
//update
this.updateDriver(form);
    }
    else{
//add
this.addDriver(form);
    }
  }

  addDriver(form: NgForm) {
    this.service.addDriver(form.value).subscribe(
      response => {
        this.toastr.success('driver added successfully!');
        this.driverAdded.emit(response);
        this.closeModal();
        this.modalService.dismissAll();  
        this.service.getDrivers();
       this.resetForm(form);
      
       alert(`Driver added successfully!\n\nResponse:\n${JSON.stringify(response, null, 2)}`);

      },
      error => {
        this.toastr.error('Failed to add vehicle');
        console.error('Error:', error);
      }
    );
  }

  updateDriver(form: NgForm) {
    this.service.updateDriver(this.service.formDate).subscribe(
      data => {
          this.toastr.info('Vehicle updated successfully!');  
        this.driverAdded.emit(data);
        this.closeModal();
        this.modalService.dismissAll();
        this.service.getDrivers();
      this.resetForm(form);
      alert(`Driver updated successfully!\n\nResponse:\n${JSON.stringify(data, null, 2)}`);

    
      },
      error => {
        this.toastr.error('Failed to update vehicle');
        console.error('ErrorForm:', error);
      }
    );
  }

  openModal() {
    this.modalService.open(this.updateDriverModal);
  }

  
  updateDriverList(driver: any) {

    this.service.list.push(driver); 
  }
  
  resetForm(form?: NgForm) {
    if (form != null) {
      form.resetForm();
    }
    this.service.formDate = new Driver();
  }
  
  openNewDriverForm() {
    this.resetForm();  
  }
  
  
  
}
