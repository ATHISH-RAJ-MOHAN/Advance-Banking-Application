import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EnquiryDataService {

  userEmail: string = '';

  constructor() {}

  setUserEmail(email: string): void {
    this.userEmail = email;
  }

  getUserEmail(): string {
    return this.userEmail;
  }

  loginResponse: any | null = null;

  setLoginResponse(data: any) {
    this.loginResponse = data;
  }

  getLoginResponse(): any {
    return this.loginResponse;
  }


  loginDocumentResponse: any | '111';

  setloginDocumentResponse(data: any) {
    this.loginDocumentResponse = data;
  }

  getloginDocumentResponse(): any {
    return this.loginDocumentResponse;
  }
}