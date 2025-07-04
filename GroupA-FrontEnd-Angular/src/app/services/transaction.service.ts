import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Transaction } from '../models/transaction';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  private apiUrl = 'https://groupa-cust-acc-trans-api.azurewebsites.net/api/'; 

  constructor(private http: HttpClient) { }

  transferFunds(sourceAccountId: number, destinationAccountId: number, amount: number): Observable<any> {
    const body = { sourceAccountId, destinationAccountId, amount };
    return this.http.post<any>(`${this.apiUrl}/Transaction/Transfer`, body);
  }

  getTransactions(accountId: number): Observable<Transaction[]> {
    return this.http.get<Transaction[]>(`${this.apiUrl}/Transaction/${accountId}`);
  }

  generateAndSave(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/export/GenerateAndSave/${id}`, { responseType: 'text' });
  }

  downloadFile(id: number): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/export/DownloadFile/${id}`, { responseType: 'blob' });
  }
}
