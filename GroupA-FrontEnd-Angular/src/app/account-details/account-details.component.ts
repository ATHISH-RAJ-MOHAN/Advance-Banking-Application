import { Component } from '@angular/core';
import { Account } from '../models/account';
import { AccountsService } from '../services/account.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from '../services/shared.service';
@Component({
  selector: 'accounts-details',
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.css'],
})
export class AccountsDetailsComponent {
  account: Account | null = null;
  customerId: number = -1; // Declare the customerId property

  constructor(
    private service: AccountsService,
    private sharedService: SharedService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.customerId = this.sharedService.getCustomerId();
    // Use customerId as needed

    const accountId = this.route.snapshot.paramMap.get('id');
    if (accountId) {
      const accid = parseInt(accountId, 10);
      if (!isNaN(accid)) {
        this.getAccountByAccountId(accid);
      }
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

  goBack(): void {
    this.router.navigate(['/customer']);
  }
}
