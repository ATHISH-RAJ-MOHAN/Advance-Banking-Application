import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Account } from '../models/account'; // Import your account model

@Injectable({
  providedIn: 'root'
})
export class AccountsService {
  private apiUrl = 'https://groupa-cust-acc-trans-api.azurewebsites.net/api/Accounts'; // Replace this with your actual API URL

  constructor(private http: HttpClient) { }

  getAccountByAccId(accountId: number): Observable<Account> {
    const url = `${this.apiUrl}/Accid:${accountId}`;
    return this.http.get<Account>(url);
  }

  getAccountByCustId(customerId: number): Observable<Account[]> {
    const url = `${this.apiUrl}/${customerId}`;
    return this.http.get<Account[]>(url);
  }

  createAccount(accountData: Account): Observable<Account> {
    const url = `${this.apiUrl}/Create`;
    return this.http.post<Account>(url, accountData);
  }
  
  deactivateAccount(accountId: number): Observable<any> {
    const url = `${this.apiUrl}/Delete?AccId=${accountId}`;
    return this.http.delete<any>(url);
  }
  applyCheque(accountId: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/ApplyCheque/${accountId}`, {});
  }
  getAccountBalance(accountId: number): Observable<number> {
    // Assuming your backend API provides an endpoint to fetch the balance of an account
    const url = `${this.apiUrl}/accounts/${accountId}/balance`;
    return this.http.get<number>(url);
  }
}
