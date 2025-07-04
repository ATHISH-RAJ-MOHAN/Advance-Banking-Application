import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  constructor( private http: HttpClient) { }

  getUserIdFromSession(): number | null {
    const userId = sessionStorage.getItem('userId');
    return userId ? Number(userId) : null;
  }

  getRoleIdFromSession(): number | null {
    const userId = sessionStorage.getItem('roleid');
    return userId ? Number(userId) : null;
  }
}
