import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoginModel } from '../models/login-model';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  constructor(private http: HttpClient, private router: Router, public authService: AuthenticationService) {}

  onLogin() {
    const loginData = {
      username: this.username,
      password: this.password,
    };

    this.http
      .post<LoginModel>('https://auth-api-a.azurewebsites.net/api/accounts/login', loginData)
      .subscribe(
        (res) => {
          if (res.token != null) {
            alert('login success'); //api to front end =get;; //front ebd to database = post
            window.sessionStorage.setItem('token', res.token);
            window.sessionStorage.setItem('userId', res.userId.toString());
            window.sessionStorage.setItem('roleid', res.roleId.toString());
            if(res.roleId == 1) 
              { 
                if (res.lastPasswordChange==null)
                   {
                alert("Password change required.");
                this.router.navigate(['/change-password']); // Redirect to password change page
                   }
                else
                  {
                this.router.navigateByUrl('/customer');
                  }
             }
            else if(res.roleId == 2) this.router.navigateByUrl('/manager');
            this.authService.isAuthenticated = true;
          } else {
            alert('Login Unsuccessful, Please check and try again.');
          }
        },
        (error) => {
          console.error('Login error:', error);
          // Handle login error
        }
      );
  }
}
