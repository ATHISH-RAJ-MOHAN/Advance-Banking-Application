import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { EnquiryDataService } from '../services/enquiry-data.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-enquiry-login',
  templateUrl: './enquiry-login.component.html',
  styleUrl: './enquiry-login.component.css'
})
export class EnquiryLoginComponent {
  email: string = '';
  password: string = '';
  errorMessage: string = '';
  loginResponse: any | null = null;


  constructor(
    private http: HttpClient,
    private router: Router, // Inject Router
    private enquiryDataService: EnquiryDataService
  ) {}

  login(): void {
    // Check if email and password are provided
    if (!this.email || !this.password) {
      this.errorMessage = 'Please provide both email and password.';
      return;
    }
  
    // Make HTTP POST request to login API
    const url = 'https://groupa-enquiry-api.azurewebsites.net/api/GetEnquirer';
    const body = { email: this.email, password: this.password };
    const body1={email:this.email}
  
    // First HTTP POST request to login API
    this.http.post<any>(url, body).subscribe(
      response => {
        // Handle successful login response
        console.log('Login successful:', response);
        this.enquiryDataService.setLoginResponse(response);
        this.enquiryDataService.setUserEmail(this.email);
  
        // Clear error message
        this.errorMessage = '';
  
        // Make second HTTP POST request to get documents
        const url2 = 'https://groupa-enquiry-api.azurewebsites.net/api/GetDocuments';
       // const body2 = { email: this.email };
  
        this.http.post<any>(url2, body1).subscribe(
          response2 => {
            // Handle successful documents response
            console.log('Documents retrieved:', response2);
            this.enquiryDataService.setloginDocumentResponse(response2);
  
            // Clear error message
            this.errorMessage = '';
  
            // Redirect to EnquiryFormComponent upon successful login
            this.router.navigate(['/enquiry/enquiryForm']); // Specify the route path
          },
          error => {
            // Handle documents retrieval error
            console.error('Failed to retrieve documents:', error);
            this.errorMessage = 'Failed to retrieve documents. Please check your credentials.';
          }
        );
      },
      error => {
        // Handle login error
        console.error('Login failed:', error);
        this.errorMessage = 'Login failed. Please check your credentials.';
      }
    );
  }
  
    

  }