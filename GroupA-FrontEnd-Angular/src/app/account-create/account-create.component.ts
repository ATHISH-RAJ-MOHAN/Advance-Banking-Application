import { Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountsService } from '../services/account.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'account-create',
  templateUrl: './account-create.component.html',
  styleUrls: ['./account-create.component.css'],
})
export class AccountCreateComponent implements OnDestroy {
  accountForm!: FormGroup;
  minBalance: number = 0;
  hasChequeSubscription: Subscription | undefined;
  accountTypeSubscription: Subscription | undefined;

  constructor(
    private fb: FormBuilder,
    private accountsApiService: AccountsService,
    private router: Router
  ) {
    this.createForm();
  }
  goToAccountsPage() {
    this.router.navigate(['/customer']); // Navigate to the create-account page
  }

  createForm() {
    this.accountForm = this.fb.group({
      accountID: [null],
      balance: ['', Validators.required],
      hasCheque: [null, Validators.required],
      customerId: [null, [Validators.required]],
      accountType: [null, Validators.required],
      branchName: ['', Validators.required],
      wd_quota: [null],
      dp_quota: [null],
    });

    // Subscribe to changes in 'hasCheque' and 'accountType' to update minimum balance
    this.hasChequeSubscription = this.accountForm
      .get('hasCheque')
      ?.valueChanges.subscribe(() => this.updateMinBalance());
    this.accountTypeSubscription = this.accountForm
      .get('accountType')
      ?.valueChanges.subscribe(() => this.updateMinBalance());
  }

  updateMinBalance() {
    const hasCheque = this.accountForm.get('hasCheque')?.value;
    const accountType = this.accountForm.get('accountType')?.value;
    if (hasCheque !== null && accountType !== null) {
      if (hasCheque && accountType === 1) {
        this.minBalance = 5000;
      } else if (hasCheque && accountType === 2) {
        this.minBalance = 10000;
      } else if (!hasCheque && accountType === 1) {
        this.minBalance = 1000;
      } else if (!hasCheque && accountType === 2) {
        this.minBalance = 5000;
      }
    }
  }

  ngOnDestroy() {
    // Unsubscribe from subscriptions to prevent memory leaks
    if (this.hasChequeSubscription) {
      this.hasChequeSubscription.unsubscribe();
    }
    if (this.accountTypeSubscription) {
      this.accountTypeSubscription.unsubscribe();
    }
  }

  onSubmit() {
    const accountType = this.accountForm.get('accountType')?.value;
    const balance = Number(this.accountForm.get('balance')?.value);

    if (
      (accountType === 1 && balance < this.minBalance) ||
      (accountType === 2 && balance < this.minBalance)
    ) {
      alert('Balance not enough for account type.');
      return;
    }

    if (this.accountForm.valid) {
      // Set quotas before submitting
      this.minBalance;
      this.setQuotas();

      const accountData = this.mapFormDataToAccountObject(
        this.accountForm.value
      );
      console.log('Account data:', accountData);
      this.accountsApiService.createAccount(accountData).subscribe(
        (response) => {
          console.log('Account created successfully:', response);
          alert('Account successfully created');
          this.accountForm.reset();
        },
        (error) => {
          alert('Error creating account');
          console.error('Error creating account:', error);
        }
      );
    } else {
      alert('Form is invalid. Please fill all required fields.');
      console.log('Form is invalid. Please fill all required fields.');
    }
  }

  setQuotas() {
    const accountType = this.accountForm.get('accountType')?.value;
    if (accountType !== null) {
      if (accountType === 1) {
        this.accountForm.patchValue({ wd_quota: 10, dp_quota: 5 });
      } else if (accountType === 2) {
        this.accountForm.patchValue({ wd_quota: 999999, dp_quota: 30 });
      }
    }
  }

  mapFormDataToAccountObject(formData: any): any {
    const hasCheque = this.convertToBoolean(formData.hasCheque);
    const accountID = Number(formData.accountID);
    const customerID = formData.customerId
      ? Number(formData.customerId)
      : undefined; // Use the one from session
    const type_id = Number(formData.accountType);
    const balance = Number(formData.balance);
    const accountType = this.accountForm.get('accountType')?.value;

    let minBalance = 0;
    if (hasCheque) {
      if (type_id == 1) minBalance = 5000;
      else minBalance = 10000;
    } else {
      if (type_id == 1) minBalance = 1000;
      else minBalance = 5000;
    }
    console.log(minBalance);
    if (balance < minBalance) {
      alert('Balance is not sufficient');
      return null;
    }

    let wd_quota = 0;
    let dp_quota = 0;
    if (type_id === 1) {
      // Assuming type_id 1 is for savings account

      wd_quota = 10; // Example value for savings account with cheque
      dp_quota = 5; // Example value for savings account with cheque
    } else if (type_id === 2) {
      // Assuming type_id 2 is for current account
      wd_quota = 9990999; // Example value for current account
      dp_quota = 30; // Example value for current account
    }

    const isActive = true;

    return {
      accountID: accountID,
      balance: balance,
      hasCheque: hasCheque,
      wd_quota: wd_quota,
      dp_quota: dp_quota,
      isActive: isActive,
      customerID: customerID,
      type_id: type_id,
      branchID: formData.branchName,
    };
  }

  convertToBoolean(value: any) {
    if (typeof value === 'string') {
      return value.toLowerCase() === 'true';
    }
    return Boolean(value);
  }
}
