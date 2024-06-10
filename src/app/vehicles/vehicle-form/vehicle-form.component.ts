
import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { VehicleService } from '../../shared/vehicle.service';
import { Vehicle } from '../../shared/vehicle.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styles: []
})
export class VehicleFormComponent {
  @Output() vehicleAdded = new EventEmitter<Vehicle>();
  @ViewChild('updateVehicleModal') updateVehicleModal: any;

  constructor(public service: VehicleService, private toastr: ToastrService, private modalService: NgbModal) {}
  openUpdateModal() {
    this.modalService.open(this.updateVehicleModal);
  }
  closeModal() {
    this.modalService.dismissAll();
  }
  onSubmit(form: NgForm) {
  
    if (this.service.formDate.VehicleID) {
//update

this.updateVehicle(form);
    }
    else{
//add

this.addVehicle(form);
    }
  }

  addVehicle(form: NgForm) {
    this.service.addVehicle(form.value).subscribe(
      response => {
        this.toastr.success('Vehicle added successfully!');
        this.vehicleAdded.emit(response);
        this.closeModal();
        this.modalService.dismissAll(); 
        this.service.refreshList();
       this.resetForm(form);
       alert(`Vehicle added successfully!\n\nResponse:\n${JSON.stringify(response, null, 2)}`);

      },
      error => {
        this.toastr.error('Failed to add vehicle');
        console.error('Error:', error);
      }
    );
  }

  updateVehicle(form: NgForm) {
    this.service.updateVehicle(this.service.formDate).subscribe(
      data => {
          this.toastr.info('Vehicle updated successfully!');  
        this.vehicleAdded.emit(data);
        this.closeModal();
        this.modalService.dismissAll();
        this.service.refreshList();
      this.resetForm(form);
      alert(`Vehicle updated successfully!\n\nResponse:\n${JSON.stringify(data, null, 2)}`);

      },
      error => {
        this.toastr.error('Failed to update vehicle');
        console.error('ErrorForm:', error);
      }
    );
  }

  openModal() {
    this.modalService.open(this.updateVehicleModal);
  }

  
  updateVehicleList(vehicle: any) {

    this.service.list.push(vehicle); 
  }
  
  resetForm(form?: NgForm) {
    if (form != null) {
      form.resetForm();
    }
    this.service.formDate = new Vehicle();
  }
  
  openNewVehicleForm() {
    this.resetForm();  
  }
  
  
  
}
