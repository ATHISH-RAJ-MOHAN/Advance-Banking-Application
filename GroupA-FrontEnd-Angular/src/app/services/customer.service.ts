import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Customer } from '../models/customer';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  private baseUrl = 'https://groupa-cust-acc-trans-api.azurewebsites.net/api/Customer';
  private documentsUrl = 'https://groupa-enquiry-api.azurewebsites.net/api';

  constructor(private http: HttpClient) { }

  //emailAddress
  // Method to get customer details by ID
  getDetails(id: number): Observable<Customer> {
    const token = window.sessionStorage.getItem('token'); // Retrieve access token from sessionStorage
    const headers = new HttpHeaders().set('Authorization',`Bearer ${token}`);
    return this.http.get<Customer>(`${this.baseUrl}/${id}`, { headers });
  }

  // Method to get customer by user ID
  getCustomerByUserId(userId: string): Observable<Customer> {
    const id = parseInt(userId, 10);
    const url = `${this.baseUrl}/userid/${id}`;
    return this.http.get<Customer>(url);
  }

  // Method to update customer details
  update(customer: Customer): Observable<Customer> {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };
    return this.http.put<Customer>(this.baseUrl, customer, options);
  }

  // Method to get documents by customer ID
  getDocuments(emailAddress: string, model: any): Observable<any> {
    return this.http.post<any>(`${this.documentsUrl}/GetDocuments`,{"email": emailAddress});
  }

  // Method to update documents
  updateDocuments(emailAddress: string, model: any): Observable<any> {
    return this.http.post<any>(`${this.documentsUrl}/CreateDocuments?Email=${emailAddress}`, model);
  }
  changePassword(userid: string, oldPassword: string, newPassword: string): Observable<any> {
    const changePasswordData = { UserID: userid, OldPassword: oldPassword, NewPassword: newPassword };
    return this.http.put<any>(`${this.baseUrl}/change`, changePasswordData);
  }
}
