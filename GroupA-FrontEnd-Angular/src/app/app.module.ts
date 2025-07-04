import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { NgxSpinnerModule } from 'ngx-spinner';

import { AppComponent } from './app.component';
import { SiteHeaderComponent } from './ui/site-header.component';
import { ManagerHomeComponent } from './manager-home/manager-home.component';
import { ManagerEnquiryListComponent } from './manager-home/manager-enquiry-list.component';
import { ManagerViewEnquiryComponent } from './manager-view-enquiry/manager-view-enquiry.component';
import { EnquiryFormComponent } from './enquiry-form/enquiry-form.component';
import { EnquiryLoginComponent } from './enquiry-login/enquiry-login.component';
import { EnquirySignupComponent } from './enquiry-signup/enquiry-signup.component';
import { EnquiryHomeComponent } from './enquiry-home/enquiry-home.component';
import { AccountsListComponent } from './account-list/account-list.component';
import { AccountsDetailsComponent } from './account-details/account-details.component';
import { AccountCreateComponent } from './account-create/account-create.component';
import { AccountsDeleteComponent } from './account-delete/account-delete.component';
import { ManagerPopupComponent } from './manager-popup/manager-popup.component';
import { LoginComponent } from './login/login.component';
import { CustomerDetailsComponent } from './customer-details/customer-details.component';
import { CustomerHomeComponent } from './customer-home/customer-home.component';
import { TransactionHistoryComponent } from './transaction-history/transaction-history.component';
import { TransactionCreateComponent } from './transaction-create/transaction-create.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { SiteFooterComponent } from './ui/site-footer/site-footer.component';
import { ChangePasswordComponent } from './change-password/change-password.component';

@NgModule({
  declarations: [
    SiteHeaderComponent,
    AppComponent,
    AccountsListComponent,
    AccountsDetailsComponent,
    AccountCreateComponent,
    AccountsDeleteComponent,
    ManagerHomeComponent,
    ManagerEnquiryListComponent,
    ManagerViewEnquiryComponent,
    EnquiryFormComponent,
    EnquiryLoginComponent,
    EnquirySignupComponent,
    EnquiryHomeComponent,
    ManagerPopupComponent,
    LoginComponent,
    CustomerDetailsComponent,
    CustomerHomeComponent,
    TransactionHistoryComponent,
    TransactionCreateComponent,
    LandingPageComponent,
    SiteFooterComponent,
    ChangePasswordComponent,
  ],
  imports: [
    BrowserModule, 
    BrowserAnimationsModule,
    NgxSpinnerModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    MatSnackBarModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
