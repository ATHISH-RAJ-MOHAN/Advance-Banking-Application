import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'appenquiryhome',
  templateUrl: './enquiry-home.component.html',
  styleUrl: './enquiry-home.component.css'
})
export class EnquiryHomeComponent {
  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  goToLogin(): void {
    this.router.navigate(['/enquiry/login']);
  }

  goToSignup(): void {
    this.router.navigate(['/enquiry/signup']);
  }
}
