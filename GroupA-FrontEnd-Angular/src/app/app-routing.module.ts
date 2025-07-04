import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginGuard } from './guard/login.guard';
import { ManagerHomeComponent } from './manager-home/manager-home.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { NotFoundComponent } from './ui/not-found.component';
import { ManagerViewEnquiryComponent } from './manager-view-enquiry/manager-view-enquiry.component';
import { EnquirySignupComponent } from './enquiry-signup/enquiry-signup.component';
import { EnquiryLoginComponent } from './enquiry-login/enquiry-login.component';
import { EnquiryHomeComponent } from './enquiry-home/enquiry-home.component';
import { EnquiryFormComponent } from './enquiry-form/enquiry-form.component';
import { AccountsListComponent } from './account-list/account-list.component';
import { AccountsDetailsComponent } from './account-details/account-details.component';
import { AccountCreateComponent } from './account-create/account-create.component';
import { AccountsDeleteComponent } from './account-delete/account-delete.component';
import { CustomerHomeComponent } from './customer-home/customer-home.component';
import { CustomerDetailsComponent } from './customer-details/customer-details.component';
import { TransactionHistoryComponent } from './transaction-history/transaction-history.component';
import { TransactionCreateComponent } from './transaction-create/transaction-create.component';
import { ChangePasswordComponent } from './change-password/change-password.component';

const routes: Routes = [

  // LOGIN PATH
  {path: 'home', component: LandingPageComponent},

  // CUSTOMER PATHS
  { path: 'customer', canActivate:[LoginGuard], component: CustomerHomeComponent },

  { path: 'customer/details', canActivate:[LoginGuard], component:CustomerDetailsComponent },

  { path: 'change-password', canActivate:[LoginGuard], component: ChangePasswordComponent},

  // ACCOUNTS PATHS
  { path: 'accounts', canActivate:[LoginGuard], component: AccountsListComponent},
  
  { path: 'accounts/delete', canActivate:[LoginGuard], component: AccountsDeleteComponent},
  
  { path: 'accounts/create', canActivate:[LoginGuard], component: AccountCreateComponent},

  { path: 'accounts/details/:id', component: AccountsDetailsComponent },

  // TRANSACTION PATHS
  { path: 'accounts/transaction/:id', canActivate:[LoginGuard], component: TransactionHistoryComponent},

  { path: 'accounts/transfer', canActivate:[LoginGuard], component: TransactionCreateComponent},

  // ENQUIRY PATHS
  { path: 'enquiry', component: EnquiryHomeComponent },

  { path: 'enquiry/signup', component: EnquirySignupComponent },

  { path: 'enquiry/login', component: EnquiryLoginComponent },

  { path: 'enquiry/enquiryForm', component: EnquiryFormComponent },

  // MANAGER PATHS
  { path: 'manager/:id', canActivate:[LoginGuard], component: ManagerViewEnquiryComponent},

  { path: 'manager', canActivate:[LoginGuard], component: ManagerHomeComponent },

  // EDGE CASES
  { path: '', redirectTo:'/home', pathMatch:'full'},

  { path: '**', component: NotFoundComponent }
];



@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
