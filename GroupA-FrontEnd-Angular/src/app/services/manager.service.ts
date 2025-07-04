import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Enquiry } from '../models/enquiry';

@Injectable({
  providedIn: 'root'
})
export class ManagerServiceService {
  constructor( private http: HttpClient) { }

  private baseUrl = 'https://groupa-manager-api.azurewebsites.net/api/manager';

  approveEnquiry(id: number): Observable<any> {
    console.log("From api service");
    return this.http.get(`${this.baseUrl}/Approve/${id}`,);
  }

  rejectEnquiry(id: number, feedback: string): Observable<any> {
    var obj = { id: id, feedback: feedback}
    console.log(obj)
    return this.http.post(`${this.baseUrl}/Reject`, JSON.stringify(obj), { headers: { 'Content-Type': 'application/json'}});
  }

  getPendingList(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/PendingList/${id}`);
  }

  getRejectedList(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/RejectedList/${id}`);
  }

  getApprovedList(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/ApprovedList/${id}`);
  }
  
  getEnquiryDetails(id: number): Observable<Enquiry> {
    return this.http.get<Enquiry>(`${this.baseUrl}/EnquiryDetails/${id}`);
  }
  
  getDocuments(id:number): Observable<any> {
    return this.http.get(`${this.baseUrl}/Documents/${id}`);
  }

  getManagerIdFromUserId(id:number): Observable<any> {
    console.log(`id in api call: ${id}`)
    return this.http.get(`${this.baseUrl}/ManagerId/${id}`);
  }
}
