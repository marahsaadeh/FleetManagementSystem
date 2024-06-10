import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { Component, Input, OnInit , ViewChild, TemplateRef, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { Vehicle } from '../shared/vehicle.model';
import { VehicleInformationService } from '../shared/vehicle-information.service';
@Component({
  selector: 'app-vehicle-information',
  templateUrl: './vehicle-information.component.html',
  styles: []
})
export class VehicleInformationComponent implements OnInit{
  @Input() vehicle: Vehicle | null = null;  
  @Output() close = new EventEmitter<void>();
  vehicleId?: number;
  vehicleInfo: any;
  isLoading: boolean = false;

  constructor(private vehicleInfoService: VehicleInformationService,private route: ActivatedRoute,private cdr: ChangeDetectorRef) { }

  
  closePanel(): void {
    console.log('fun1');
    this.close.emit();
  }
  ngOnInit(): void {
    console.log('VehicleInformationComponent initialized');
    this.route.params.subscribe(params => {
      this.vehicleId = +params['id'];
      console.log('Received vehicle ID:', this.vehicleId);
      this.loadVehicleData();
    });
  }
  
  private loadVehicleData(): void {
    console.log('fun3');
    if (this.vehicleId) {
      this.isLoading = true; 
      this.vehicleInfoService.getVehicleInformation(this.vehicleId).subscribe({
        next: (data) => {
          this.vehicleInfo = data;
          console.log('Vehicle Information Loaded:', this.vehicleInfo);  
          this.isLoading = false;  
          this.cdr.detectChanges();
        },
        error: (error) => {
          console.error('Error fetching vehicle information:', error);
          this.isLoading = false;  
        }
      });
    }
  }

}
