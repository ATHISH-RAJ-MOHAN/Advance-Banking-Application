import { Component } from '@angular/core';
import { TransactionService } from '../services/transaction.service';
import { ActivatedRoute, Router } from '@angular/router'; // Import Router service

@Component({
  selector: 'app-transfer-funds',
  templateUrl: './transaction-create.component.html',
  styleUrls: ['./transaction-create.component.css']
})
export class TransactionCreateComponent {
  sourceAccount: number=0 ;
  destinationAccount: number = 0;
  amount: number = 0;
  message: string = '';

  constructor(
    private transferService: TransactionService,
    private router: Router // Inject Router service
  ) { }
  
  goBack(): void {
    this.router.navigate(['/customer']);
  }
  
  onSubmit(): void {
    if(this.amount<=0){
      alert("please enter valid amount");
      return;
    }
    this.transferService.transferFunds(this.sourceAccount, this.destinationAccount, this.amount).subscribe(
      response => {
        alert("Transaction Successful");
        this.message = `Transfer successful: ${response.message}`;
        
        // Clear the fields after successful transaction
        this.sourceAccount = 0;
        this.destinationAccount = 0;
        this.amount = 0;
      },
      error => {
        alert("Transaction Failed! Check the accountIds and Try Again");
        this.message = `Transfer failed: ${error.error.message || error.message}`;
      }
    );
  }  
}
