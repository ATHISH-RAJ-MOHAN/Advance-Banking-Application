import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CustomerService } from '../services/customer.service';
import { CommonService } from '../services/common.service';
import { Customer } from '../models/customer';
import { AccountsDetailsComponent } from '../account-details/account-details.component';
import { AccountsService } from '../services/account.service';


@Component({
  selector: 'app-dashboard',
  templateUrl: './customer-home.component.html',
  styleUrls: ['./customer-home.component.css']
})
export class CustomerHomeComponent implements OnInit {
  customerSummary: Partial<Customer> = {};

  constructor(
    private router: Router,
    private customerService: CustomerService,
    private commonService: CommonService
  ) {}

  ngOnInit(): void {
    const userId = this.commonService.getUserIdFromSession();
    if (userId) {
      // Convert userId to string before passing to getCustomerByUserId
      const userIdString: string = userId.toString();
      this.customerService.getCustomerByUserId(userIdString).subscribe(
        (data) => {
          this.customerSummary = data;
        },
        (error) => {
          console.error('Error fetching customer summary:', error);
        }
      );
    }
  }

  onViewProfile(): void {
    this.router.navigateByUrl('/customer/details');
  }
  onTransferFunds(): void {
    this.router.navigateByUrl('/accounts/transfer');
  }
  onShowTransaction(): void {
    this.router.navigateByUrl('/accounts/transaction');
  }
  onPasswordChange(): void {
    this.router.navigateByUrl('/change-password');
  }

  onSubmitSignOut(): void {
    console.log("going here");
    window.sessionStorage.clear();
    alert('You are signed out');
    this.router.navigateByUrl('/login');
  }
}
