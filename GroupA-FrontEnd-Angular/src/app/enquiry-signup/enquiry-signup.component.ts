import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

interface PasswordValidation {
  minLength: number;
  minUpperCase: number;
  minLowerCase: number;
  minDigit: number;
  minSpecialChar: number;
}

@Component({
  selector: 'app-sign-up',
  templateUrl: './enquiry-signup.component.html',
  styleUrls: ['./enquiry-signup.component.css']
})
export class EnquirySignupComponent  {

  [x: string]: any;

  errorMessage: string = '';
  email: string = '';
  password: string = '';

  passwordValidation: PasswordValidation = {
    minLength: 6,
    minUpperCase: 1,
    minLowerCase: 1,
    minDigit: 1,
    minSpecialChar: 1
  };

  passwordRequirementsMet: boolean[] = [false, false, false, false, false]; // Array to track met conditions

  constructor(private http: HttpClient, private router: Router) { }

  onPasswordChange(event: Event) {
    const password = (event.target as HTMLInputElement).value;
    this.password = password;
    this.passwordRequirementsMet = [
      password.length >= this.passwordValidation.minLength,
      /[A-Z]/.test(password),
      /[a-z]/.test(password),
      /[0-9]/.test(password),
      /[!@#$%^&*()_+\-=\[\]{};':\\|,.<>\/?`~]/.test(password)
    ];
  }


  signup(): void {
    if (!this.validatePassword(this.password)) {
      this.errorMessage = 'Password does not meet requirements.';
      return;
    }

    const signupData = {
      email: this.email,
      password: this.password
    };

    console.log('Signup Data:', signupData); // Check if data is correct

    this.http.post('https://groupa-enquiry-api.azurewebsites.net/api/AddEnquirer', signupData)
      .subscribe(
        (response: any) => {
          console.log('Signup successful:', response);
          this.showAlert();
          this.router.navigate(['/home']);
        },
        (error: any) => {
          console.error('Signup error:', error);

          this.errorMessage = 'Email already exists.';

        }
      )
  }

  validatePassword(password: string): boolean {
    return this.passwordRequirementsMet.every(conditionMet => conditionMet); // Check if all conditions are true
  }

  showAlert(): void {
    alert("You have successfully signed up");
  }

}
