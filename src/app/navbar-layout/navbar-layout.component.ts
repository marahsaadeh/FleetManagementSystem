import { Component, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-navbar-layout',
  templateUrl: './navbar-layout.component.html'
})
export class NavbarLayoutComponent {
  @Output() sidebarToggled = new EventEmitter<boolean>(); 
  isSidebarOpen = true; 

  toggleSidebar(): void {
    this.isSidebarOpen = !this.isSidebarOpen;
    this.sidebarToggled.emit(this.isSidebarOpen); 
    document.body.classList.toggle('sb-sidenav-toggled', !this.isSidebarOpen);
  }
}

