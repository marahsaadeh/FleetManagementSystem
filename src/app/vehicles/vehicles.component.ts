

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Component, Input, OnInit , ViewChild, TemplateRef, Output, EventEmitter} from '@angular/core';
import { FormGroup,FormControl,Validators, NgForm } from '@angular/forms';
import { Vehicle } from '../shared/vehicle.model';
import { VehicleService } from '../shared/vehicle.service';
import { HttpErrorResponse } from '@angular/common/http'; 
import { VehicleFormComponent } from './vehicle-form/vehicle-form.component'; // تأكد من أن المسار صحيح
import { Router } from '@angular/router';
import { VehicleInformationService } from '../shared/vehicle-information.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-vehicles',
  templateUrl: './vehicles.component.html',
  styles: []
})
export class VehiclesComponent implements OnInit {

  vehicles: Vehicle[] = [];
  selectedVehicleId?: number; 

  @Input() isSidebarOpen: boolean = true;
  vehicleForm!: FormGroup;
  @ViewChild('updateVehicleModal') private updateVehicleModalRef!: TemplateRef<any>;
  @Output() showMore = new EventEmitter<Vehicle>();

  constructor(public service: VehicleService , private toastr:ToastrService,private modalService: NgbModal,private router: Router,private vehicleInfoService: VehicleInformationService ) {}
 


openModal(vehicle: Vehicle) {
  this.service.formDate = Object.assign({}, vehicle); 
  this.modalService.open(this.updateVehicleModalRef); 
}


closeModal() {
  this.modalService.dismissAll();
}
showVehicleInfo(vehicleId: number): void {
  console.log("Vehicle ID clicked:", vehicleId);
  this.router.navigate(['/vehicle-information', vehicleId])
    .then(success => {
      console.log('Navigation Success?', success);
      console.log('Vehicle information ID:', vehicleId);
    })
    .catch(err => console.log('Navigation Error:', err));
}



  ngOnInit(): void {
    this.service.refreshList();
    this.vehicleForm = new FormGroup({
      vehicleNumber: new FormControl('', Validators.required),
      vehicleType: new FormControl('', Validators.required)
    });
    this.loadVehicles();
  }
  loadVehicles(): void  {
    this.service.getVehicles().subscribe(
      (data: Vehicle[]) => this.vehicles = data,
      (error: HttpErrorResponse) => {
        console.error('Error fetching vehicles:', error.message);
        this.toastr.error('Error fetching vehicles: ' + error.message);
      }
    );
  }

  
  onVehicleAdded(vehicle: Vehicle) {
    if (Array.isArray(this.vehicles)) {
      this.vehicles.push(vehicle);
      console.log('vehicles before push:', this.vehicles);

    } else {
      console.error('vehicles is not an array', this.vehicles);
    }  
  }
 

  deleteVehicle(id: number) {
    if (confirm("Are you sure you want to delete this vehicle?")) {
        this.service.deleteVehicle(id).subscribe({
            next: (response) => {  
                this.toastr.success('Vehicle deleted successfully');
                this.service.refreshList();
              
                alert(`Vehicle deleted successfully!\n\nResponse:\n${JSON.stringify(response, null, 2)}`);
            },
            error: (error: HttpErrorResponse) => {
                console.error('Error deleting vehicle', error);
                alert(`Failed to delete vehicle: ${error.status} ${error.message}`);
            }
        });
    }
}

  }


