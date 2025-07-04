import { Component } from '@angular/core';
import { CommonService } from '../services/common.service';
import { Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-site-header',
  templateUrl: './site-header.component.html',
  styleUrl: './site-header.component.css'
})
export class SiteHeaderComponent{
  constructor(private router: Router, public authService: AuthenticationService) { 
  }

  onSignOut() {
    console.log("going here");
    window.sessionStorage.clear();
    alert('You are signed out');
    this.router.navigate(['/home']);
  }
}
