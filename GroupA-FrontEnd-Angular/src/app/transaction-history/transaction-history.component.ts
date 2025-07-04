import { Component } from '@angular/core';
import { TransactionService } from '../services/transaction.service';
import { Transaction } from '../models/transaction';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-transaction-list',
  templateUrl: './transaction-history.component.html',
  styleUrls: ['./transaction-history.component.css'],
})
export class TransactionHistoryComponent {
  transactions: Transaction[] = [];
  accountId: number = 0;

  constructor(
    private transactionService: TransactionService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Use customerId as needed

    this.accountId = parseInt(this.route.snapshot.paramMap.get('id')!);
    this.fetchTransactions();
  }
  goBack(): void {
    this.router.navigate(['/customer']);
  }
  fetchTransactions(): void {
    if (!this.accountId || this.accountId <= 0) {
      alert('Please enter a valid account ID');
      return;
    }

    this.transactionService.getTransactions(this.accountId).subscribe(
      (data) => {
        this.transactions = data;

        if (this.transactions.length === 0) {
          alert('No transactions found for this account ID');
        }
      },
      (error) => {
        console.error('Error fetching transactions', error);
        alert('Check AccountId and try again');
        this.transactions = [];
      }
    );
  }
  downloadFile() {
    this.transactionService.generateAndSave(this.accountId).subscribe(
      (data) => {
        // On successful generation and save, initiate download
        this.transactionService
          .downloadFile(this.accountId)
          .subscribe((blob) => {
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = `Report_${this.accountId}.xlsx`;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
          });
      },
      (error) => {
        console.log(error);
      }
    );
  }

  clear(): void {
    this.accountId = 0;
    this.transactions = [];
  }
}
