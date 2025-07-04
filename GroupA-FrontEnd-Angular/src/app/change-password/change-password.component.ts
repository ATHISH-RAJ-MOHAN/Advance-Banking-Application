import { Component } from '@angular/core';
import { CustomerService } from '../services/customer.service';
import { Router } from '@angular/router';
interface PasswordValidation {
  minLength: number;
  minUpperCase: number;
  minLowerCase: number;
  minDigit: number;
  minSpecialChar: number;
}
@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css'
})
export class ChangePasswordComponent {
  oldPassword: string = '';
  newPassword: string = '';
  newPasswordConfirm: string = '';
  oldPasswordFieldType: string = 'password';
  newPasswordFieldType: string = 'password';
  errorMessage: string = '';
  passwordValidation: PasswordValidation = {
    minLength: 6,
    minUpperCase: 1,
    minLowerCase: 1,
    minDigit: 1,
    minSpecialChar: 1
  };

  passwordRequirementsMet: boolean[] = [false, false, false, false, false]; // Array to track met conditions

  
  
  constructor(private customerService: CustomerService, private router: Router) {}

  onPasswordChange(event: Event) {
    const password = (event.target as HTMLInputElement).value;
    this.newPassword = password;
    this.passwordRequirementsMet = [
      password.length >= this.passwordValidation.minLength,
      /[A-Z]/.test(password),
      /[a-z]/.test(password),
      /[0-9]/.test(password),
      /[!@#$%^&*()_+\-=\[\]{};':\\|,.<>\/?`~]/.test(password)
    ];
  }
  onChangePassword() {
    this.errorMessage = '';
    if (this.newPassword !== this.newPasswordConfirm) {
      this.errorMessage = 'New passwords do not match';
      return;
    }
  
    if (this.newPassword.length < 6) {
      this.errorMessage = 'New password must be at least 6 characters long';
      return;
    }
  
    const userid = window.sessionStorage.getItem('userId');
    if (!userid) {
      alert('UserID not found. Please log in again.');
      this.router.navigateByUrl('/login');
      return;
    }
  
    this.customerService.changePassword(userid, this.oldPassword, this.newPassword)
      .subscribe({
        next: (response) => {
          alert('Password changed successfully');
          this.router.navigateByUrl('/login');
        },
        error: (error) => {
          const message = error.error|| error.message || 'Unknown error occurred';
          this.errorMessage = 'Failed to update password: ' + message;
          console.error('Failed to update password:', error);
        }
      });
   }
   validatePassword(password: string): boolean {
    return this.passwordRequirementsMet.every(conditionMet => conditionMet); // Check if all conditions are true
  }
}
