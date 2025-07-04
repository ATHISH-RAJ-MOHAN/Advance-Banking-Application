import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  private customerId: number =-1;

  setCustomerId(id: number) {
    this.customerId = id;
  }

  getCustomerId(): number {
    return this.customerId;
  }
}