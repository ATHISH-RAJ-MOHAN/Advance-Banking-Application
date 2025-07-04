import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Account } from '../models/account';
import { AccountsService } from '../services/account.service';

@Component({
  selector: 'app-accounts-delete',
  templateUrl: './account-delete.component.html',
  styleUrls: ['./account-delete.component.css']
})
export class AccountsDeleteComponent {
  deleteForm: FormGroup;
  isSubmitting = false;
  confirmDelete = false;
  accountId!: number;

  constructor(
    private fb: FormBuilder,
    private accountsService: AccountsService
  ) {
    this.deleteForm = this.fb.group({
      accountId: ['', [Validators.required, Validators.pattern("^[0-9]*$")]]
    });
  }

  onSubmit() {
    if (this.deleteForm.valid) {
      this.accountId = +this.deleteForm.get('accountId')?.value;
      // Directly set the confirmDelete flag to true to show the confirmation dialog
      this.confirmDelete = true;
    }
  }

  confirmDeletion() {
    this.isSubmitting = true;
    this.accountsService.deactivateAccount(this.accountId).subscribe(
      () => {
        alert('Account deactivated successfully.');
        this.isSubmitting = false;
        this.confirmDelete = false;
        this.deleteForm.reset();
      },
      error => {
        console.error("Error deactivating account:", error);
        alert("Error deactivating account.");
        this.isSubmitting = false;
      }
    );
  }

  cancelDeletion() {
    this.confirmDelete = false;
    this.deleteForm.reset();
  }
}
