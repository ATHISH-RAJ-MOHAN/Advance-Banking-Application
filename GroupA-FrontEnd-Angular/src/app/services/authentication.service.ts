import { Injectable } from '@angular/core';
import { CommonService } from './common.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  constructor(private common: CommonService) {}
  isAuthenticated : boolean = false;

  getStatus(): boolean {
    return this.common.getUserIdFromSession()==null ? false : true;
  }
}
