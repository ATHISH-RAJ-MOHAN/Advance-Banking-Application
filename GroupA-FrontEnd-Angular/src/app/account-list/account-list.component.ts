import { Component } from '@angular/core';
import { Account } from '../models/account';
import { AccountsService } from '../services/account.service';
import { Router } from '@angular/router';
import { CommonService } from '../services/common.service';
import { CustomerService } from '../services/customer.service';
import { Customer } from '../models/customer';

@Component({
  selector: 'accounts-list',
  templateUrl: './account-list.component.html',
  styleUrls: ['./account-list.component.css'], // Corrected from `styleUrl` to `styleUrls`
})
export class AccountsListComponent {
  accountList2: Account[] = [];
  account: Account | null = null;
  customerID: number | undefined; // Declare the customerID property
  selectedAccountBalance: number = 0; // Holds the balance of the selected account
  selectedAccountId: number = 0; // Holds the ID of the selected account for balance check

  constructor(
    private service: AccountsService,
    private router: Router,
    private commonService: CommonService,
    private customerService: CustomerService
  ) {
    var usr = this.commonService.getUserIdFromSession();
    this.customerService
      .getCustomerByUserId(usr?.toString()!)
      .subscribe((id) => (this.customerID = id.customerId));
  }

  async ngOnInit() {
    await this.setCustomerID(); // Wait for customerID to be fetched
    if (this.customerID != undefined)
      this.getAccountByCustId(); // Then fetch accounts
    else {
      console.error('Customer ID is null.');
    }
  }
  Transaction(accid: number) {
    this.router.navigate([`/accounts/transaction/${accid}`]);
  }
  async setCustomerID() {
    const userId = this.commonService.getUserIdFromSession();
    console.log(`UserId From setCustomerID: ${userId}`);
    try {
      var cust = await this.customerService
        .getCustomerByUserId(userId!.toString())
        .toPromise();
      this.customerID = cust?.customerId;
      console.log(`Customer ID: ${this.customerID}`);
    } catch (error) {
      console.error(error);
      alert(error);
    }
  }

  getAccountByCustId() {
    if (this.customerID !== undefined) {
      console.log(`In getAccounts: ${this.customerID}`);
      this.service.getAccountByCustId(this.customerID).subscribe(
        (accounts) => (this.accountList2 = accounts),
        (error) => {
          console.error('Error fetching accounts:', error);
          this.accountList2 = [];
        }
      );
    } else {
      console.error('Customer ID is undefined.');
      // Handle the scenario where customerID is not available
    }
  }

  goToCreateAccountPage() {
    this.router.navigate(['/accounts/create']); // Navigate to the create-account page
  }

  viewDetails(accountId: number, customerID: number): void {
    console.log(customerID);
    this.router.navigate(['/accounts/details', accountId]);
  }

  applyForCheque(accountId: number): void {
    // Find the account based on accountId
    const account = this.accountList2.find(
      (account) => account.accountID === accountId
    );

    if (!account) {
      console.error('Account not found');
      alert('Account not found');
      return;
    }

    // Check the account type and balance
    if (
      (account.type_id === 1 && account.balance < 5000) ||
      (account.type_id !== 1 && account.balance < 10000)
    ) {
      alert('Account Balance is not enough');
      return;
    }

    const wantToAddCheque = confirm(
      'Do you want to add a checkbook to this account?'
    );
    if (!wantToAddCheque) {
      return;
    }

    // Proceed with applying for cheque
    this.service.applyCheque(accountId).subscribe(
      (response) => {
        // Update the hasCheque property in the frontend
        const updatedAccount = this.accountList2.find(
          (account) => account.accountID === accountId
        );
        if (updatedAccount) {
          updatedAccount.hasCheque = true;
          console.log('You have cheque now');
          alert('Your account now has a checkbook');
        }
      },
      (error) => {
        console.error('Error applying for cheque:', error);
        // Handle error appropriately (e.g., display error message)
      }
    );
  }

  deleteAccount(accountId: number): void {
    if (confirm('Are you sure you want to delete this account?')) {
      this.service.deactivateAccount(accountId).subscribe(
        () => {
          this.accountList2 = this.accountList2.filter(
            (account) => account.accountID !== accountId
          );
        },
        (error) => {
          console.error('Error deleting account:', error);
        }
      );
    }
  }
  getAccountByAccountId(accountId: number): void {
    this.service.getAccountByAccId(accountId).subscribe(
      (account) => {
        console.log('Account fetched successfully:', account);
        this.account = account;
      },
      (error) => {
        console.error('Error fetching account:', error);
        this.account = null;
      }
    );
  }
  checkBalance(accountId: number) {
    this.getAccountByAccountId(accountId);
  }
}
