import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { VehicleInformation } from '../../shared/vehicle-information.model';
import { ActivatedRoute } from '@angular/router';
import { Component, Input, OnInit , ViewChild, TemplateRef, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { DriversService } from '../../shared/drivers.service';
import { VehicleInformationService } from '../../shared/vehicle-information.service';
import { DriversComponent } from '../../drivers/drivers.component';
import { FormGroup,FormControl,Validators, NgForm } from '@angular/forms';
import { VehicleInformationComponent} from '../vehicle-information.component';
import { VehicleFormComponent } from '../../vehicles/vehicle-form/vehicle-form.component';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';
import { Driver } from '../../shared/driver.model';
@Component({
  selector: 'app-vehicle-information-form',

  templateUrl: './vehicle-information-form.component.html',
  styles: []
})
export class VehicleInformationFormComponent implements OnInit {

  public vehicleInformations: VehicleInformation[] = []; 
  drivers: any[] = [];
  public fullData: any; 

  selectedVehicleId?: number;  
@Input() isSidebarOpen: boolean = true;
vehicleInfoForm!: FormGroup;
@ViewChild('updateVehicleInformationModal') private updateVehicleInformationModalRef!: TemplateRef<any>;

@Output() vehicleInfAdded = new EventEmitter<VehicleInformation>();


  constructor( private toastr:ToastrService,private router: Router,private modalService: NgbModal,public vehicleInfoService: VehicleInformationService , public driverService:DriversService) { }

  openModal(vehicleInformation:VehicleInformation) {
    this.vehicleInfoService.formDate=Object.assign({},vehicleInformation);
    this.modalService.open(this.updateVehicleInformationModalRef);

    this.loadDrivers(); 

  }

  
  updateDriverList(vehicleInf: any) {
    this.vehicleInfoService.list.push(vehicleInf); 
  }
  
  resetForm(form?: NgForm) {
    if (form != null) {
      form.resetForm();
    }
    this.vehicleInfoService.formDate = new VehicleInformation();
  }
  
  openNewVehicleInfForm() {
    this.resetForm();  
  }
  
  closeModal() {
    this.modalService.dismissAll();
  }
  onSubmit(form: NgForm) {
    if (this.vehicleInfoService.formDate.VehicleInformationID) {
//update
this.updateVehicleInfo(form);
    }
    else{
//add
this.addVehicleInfo(form);
    }
  }

  addVehicleInfo(form: NgForm) {
    this.vehicleInfoService.addVehicleInformation(form.value).subscribe(
      response => {
        this.toastr.success('Vehicle Information added successfully!');
        this.vehicleInfAdded.emit(response);
        this.closeModal();
        this.modalService.dismissAll();  
        this.vehicleInfoService.getVehicleInformations();
       this.resetForm(form);
       alert(`Vehicle Information added successfully!\n\nResponse:\n${JSON.stringify(response, null, 2)}`);

      },
      error => {
        this.toastr.error('Failed to add vehicle');
        console.error('Error:', error);
      }
    );
  }

  updateVehicleInfo(form: NgForm) {
    this.vehicleInfoService.updateVehicleInformation(this.vehicleInfoService.formDate).subscribe(
      data => {
          this.toastr.info('Vehicle updated successfully!');  
        this.vehicleInfAdded.emit(data);
        this.closeModal();
        this.modalService.dismissAll();
        this.vehicleInfoService.getVehicleInformations();
      this.resetForm(form);
      alert(`Vehicle Information updated successfully!\n\nResponse:\n${JSON.stringify(data, null, 2)}`);

    
      },
      error => {
        this.toastr.error('Failed to update vehicle');
        console.error('ErrorForm:', error);
      }
    );
  }




  loadDrivers(): void {
    this.driverService.getDrivers().subscribe(data => {
      this.drivers = data;
    });
  }

  ngOnInit(): void {
    this.vehicleInfoService.getVehicleInformations().subscribe(data => {
      this.vehicleInformations = data;
      this.fullData = this.vehicleInfoService.fullData;  
      console.log('FullData', this.fullData);
    });

  }
  deleteVehicleInfo(id: number) {
    if (confirm("Are you sure you want to delete this vehicle information?")) {
      this.vehicleInfoService.deleteVehicleInformation(id).subscribe({
        next: (response) => { 
          this.toastr.success('Vehicle information deleted successfully');
          this.vehicleInfoService.getVehicleInformations(); 
  

          alert(`Vehicle information deleted successfully!\n\nResponse:\n${JSON.stringify(response, null, 2)}`);
        },
        error: (error: HttpErrorResponse) => {
          console.error('Error deleting vehicle information', error);
          alert(`Failed to delete vehicle information: ${error.status} ${error.message}`);
        }
      });
    }
  }
  
}