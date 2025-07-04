import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { EnquiryDataService } from '../services/enquiry-data.service';
import { HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-enquiry-form',
  templateUrl: './enquiry-form.component.html',
  styleUrls: ['./enquiry-form.component.css']
})
export class EnquiryFormComponent {
  enquiryForm: FormGroup;
  maxDate: string;
  documentPhotoFile: File | null = null;
  documentAadharFile: File | null = null;
  documentPanCardFile: File | null = null;
  imageSrc1: string | null = null;
  imageSrc2: string | null = null;
  imageSrc3: string | null = null;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    public enquiryDataService: EnquiryDataService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.enquiryForm = this.fb.group({
      fName: ['', Validators.required],
      lName: [''],
      address1: [''],
      address2: [''],
      address3: [''],
      phoneNumber: ['', [Validators.required, this.validatePhoneNumber]],
      email: [''],
      DOB: ['', Validators.required],
      city: [''],
      country: [''],
      pincode: [''],
      wantsCheque: [false],
      accountType: ['', Validators.required],
      balance: [''],
      documentPhotoFile: [null, Validators.required],
      documentAadharFile: [null, Validators.required],
      documentPanCardFile: [null, Validators.required],
    });
    const today = new Date();
    const eighteenYearsAgo = new Date(today.getFullYear() - 18, today.getMonth(), today.getDate());
    this.maxDate = eighteenYearsAgo.toISOString().split('T')[0];
  }

  ngOnInit(): void {
    const userEmail = this.enquiryDataService.getUserEmail();
    const loginResponse = this.enquiryDataService.getLoginResponse();
    const loginDocumentResponse = this.enquiryDataService.getloginDocumentResponse();

    console.log('Login Response:', loginResponse);
    console.log('Login Document Response:', loginDocumentResponse);

    if (userEmail) {
      this.enquiryForm.controls['email'].setValue(userEmail);
    }

    if (loginDocumentResponse) {
      this.imageSrc1 = 'data:image/png;base64,' + loginDocumentResponse.basePhoto;
      this.imageSrc2 = 'data:image/png;base64,' + loginDocumentResponse.baseAadhar;
      this.imageSrc3 = 'data:image/png;base64,' + loginDocumentResponse.basePanCard;

      this.documentPhotoFile = this.base64ToFile(loginDocumentResponse.basePhoto, 'photo.png');
      this.documentAadharFile = this.base64ToFile(loginDocumentResponse.baseAadhar, 'aadhar.png');
      this.documentPanCardFile = this.base64ToFile(loginDocumentResponse.basePanCard, 'pancard.png');
    }

    this.previewFile(this.documentPhotoFile, this.imageSrc1);
    this.previewFile(this.documentAadharFile, this.imageSrc2);
    this.previewFile(this.documentPanCardFile, this.imageSrc3);

    if (loginResponse) {
      const accountType = loginResponse.accountType === 1 ? 'Savings' : (loginResponse.accountType === 2 ? 'Current' : '');
      const dob = new Date(loginResponse.dob);
      const year = dob.getFullYear();
      const month = String(dob.getMonth() + 1).padStart(2, '0');
      const day = String(dob.getDate()).padStart(2, '0');
      var formattedDob = `${year}-${month}-${day}`;
      if(formattedDob=="1-01-01")
      {
        formattedDob='';
      }
      if (loginDocumentResponse.basePhoto == "110=" || loginDocumentResponse.basePhoto == "111") {
        this.documentPhotoFile = null;
      }
      if (loginDocumentResponse.baseAadhar == "110=" || loginDocumentResponse.baseAadhar == "111") {
        this.documentAadharFile = null;
      }
      if (loginDocumentResponse.basePanCard == "110=" || loginDocumentResponse.basePanCard == "111") {
        this.documentPanCardFile = null;
      }
      if (loginResponse.pincode == 0) {
        loginResponse.pincode = ''
      }
      this.enquiryForm.patchValue({
        fName: loginResponse.firstName || '',
        lName: loginResponse.lastName || '',
        address1: loginResponse.address1 || '',
        address2: loginResponse.address2 || '',
        address3: loginResponse.address3 || '',
        phoneNumber: loginResponse.phoneNumber || '',
        DOB: formattedDob,
        //DOB: loginResponse.dob instanceof Date ? (Date)loginResponse.dob : loginResponse.dob,
        city: loginResponse.city || '',
        country: loginResponse.country || '',
        pincode: loginResponse.pincode || '',
        wantsCheque: loginResponse.wantsCheque,
        accountType: accountType,
        balance: loginResponse.balance || '',
        documentPhotoFile: this.documentPhotoFile || null,
        documentAadharFile: this.documentAadharFile || null,
        documentPanCardFile: this.documentPanCardFile || null

      });
    }
  }

  private base64ToFile(base64: string, filename: string): File {
    const byteCharacters = atob(base64);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
      byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    const blob = new Blob([byteArray], { type: 'image/png' });
    return new File([blob], filename, { type: 'image/png' });
  }

  onSave(): void {
    this.postFormData('https://groupa-enquiry-api.azurewebsites.net/api/SaveEnquiry');

  }

  onSubmit(): void {
    this.postFormData('https://groupa-enquiry-api.azurewebsites.net/api/CreateEnquiry');

  }



  private async postFormData(apiUrl: string) {
    const formData = new FormData();
    const pincodeValue = this.enquiryForm.value.pincode ? parseInt(this.enquiryForm.value.pincode, 10) : 0;
    const dateOfBirthString = this.enquiryForm.value.DOB;
    let dateOfBirth: Date;

    if (dateOfBirthString === "dd-mm-yyyy" || !dateOfBirthString) {
      // If the date is the placeholder or empty, use today's date
      dateOfBirth = new Date();
    } else {
      // Convert the string to a Date object (assuming the format is dd-mm-yyyy)
      const [day, month, year] = dateOfBirthString.split('-').map(Number);
      dateOfBirth = new Date(year, month - 1, day);
    }

    const formattedDateOfBirth = dateOfBirth.toISOString().split('T')[0];
    if (this.enquiryForm.value.pincode == '') {
      this.enquiryForm.value.pincode = 0
    }
    formData.append('FirstName', this.enquiryForm.value.fName);
    formData.append('LastName', this.enquiryForm.value.lName);
    formData.append('Address1', this.enquiryForm.value.address1);
    formData.append('Address2', this.enquiryForm.value.address2);
    formData.append('Address3', this.enquiryForm.value.address3);
    formData.append('PhoneNumber', this.enquiryForm.value.phoneNumber);
    formData.append('Email', this.enquiryForm.value.email);
    formData.append('DOB', formattedDateOfBirth);
    formData.append('City', this.enquiryForm.value.city);
    formData.append('Country', this.enquiryForm.value.country);
    formData.append('Pincode', pincodeValue.toString());
    formData.append('WantsCheque', this.enquiryForm.value.wantsCheque.toString());
    formData.append('AccountType', this.enquiryForm.value.accountType === 'Savings' ? '1' : '2');
    formData.append('Balance', this.enquiryForm.value.balance.toString());

    const model = {
      FirstName: this.enquiryForm.value.fName,
      LastName: this.enquiryForm.value.lName,
      Address1: this.enquiryForm.value.address1,
      Address2: this.enquiryForm.value.address2,
      Address3: this.enquiryForm.value.address3,
      PhoneNumber: this.enquiryForm.value.phoneNumber,
      Email: this.enquiryForm.value.email,
      DOB: this.enquiryForm.value.DOB,
      City: this.enquiryForm.value.city,
      Country: this.enquiryForm.value.country,
      Pincode: this.enquiryForm.value.pincode,
      WantsCheque: this.enquiryForm.value.wantsCheque,
      AccountType: this.enquiryForm.value.accountType === 'Savings' ? '1' : '2',
      Balance: this.enquiryForm.value.balance.toString()


    }

    const body = JSON.stringify(model)
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    // Show a loading toast
    const loadingSnackbarRef = this.snackBar.open('Submitting data, please wait...', 'Close', {
      duration: 0 // 0 means it will stay until dismissed
    });


    this.http.post(apiUrl, body, { headers: headers })
      .subscribe(
        response => {
          console.log('API response:', response)

          // loadingSnackbarRef.dismiss(); // Dismiss the loading toast
          // this.snackBar.open('Data submitted successfully!', 'Close', {
          //   duration: 3000
          // });

          this.postFormData2('https://groupa-enquiry-api.azurewebsites.net/api/CreateDocuments');

        },
        error => {
          console.error('API error:', error);
          // loadingSnackbarRef.dismiss(); // Dismiss the loading toast
          // this.snackBar.open('Failed to submit data. Please try again.', 'Close', {
          //   duration: 3000
          // });
        }
      );
  }

  private async postFormData2(apiUrl: string) {
    const formData = new FormData();
    formData.append('Email', this.enquiryForm.value.email);
    if (this.documentPhotoFile) formData.append('Photo', this.documentPhotoFile);
    if (this.documentAadharFile) formData.append('Aadhar', this.documentAadharFile);
    if (this.documentPanCardFile) formData.append('Pancard', this.documentPanCardFile);
    if (this.imageSrc1) formData.append('basePhoto', this.imageSrc1 as string);
    if (this.imageSrc2) formData.append('baseAadhar', this.imageSrc2 as string);
    if (this.imageSrc3) formData.append('basePanCard', this.imageSrc3 as string);

    // Show a loading toast
    const loadingSnackbarRef = this.snackBar.open('Submitting documents, please wait...', 'Close', {
      duration: 0 // 0 means it will stay until dismissed
    });

    this.http.post(apiUrl, formData)
      .subscribe(
        response => {
          console.log('API response:', response);
          loadingSnackbarRef.dismiss(); // Dismiss the loading toast
          this.snackBar.open('Enquiry submitted successfully!', 'Close', {
            duration: 3000
          });
          this.router.navigate(['/home']);
        },
        error => {
          console.error('API error:', error);
          loadingSnackbarRef.dismiss(); // Dismiss the loading toast
          this.snackBar.open('Failed to submit documents. Please try again.', 'Close', {
            duration: 3000
          });
        }
      );
  }

  onPhotoChange(event: any): void {
    this.documentPhotoFile = event.target.files[0];
    this.previewFile(this.documentPhotoFile, 'imageSrc1');
  }

  onAadharChange(event: any): void {
    this.documentAadharFile = event.target.files[0];
    this.previewFile(this.documentAadharFile, 'imageSrc2');
  }

  onPanCardChange(event: any): void {
    this.documentPanCardFile = event.target.files[0];
    this.previewFile(this.documentPanCardFile, 'imageSrc3');
  }

  isSaveDisabled() {
    return (
      !this.enquiryForm.get('fName')?.valid ||
      !this.enquiryForm.get('DOB')?.valid ||
      !this.enquiryForm.get('phoneNumber')?.valid ||
      !this.enquiryForm.get('documentPhotoFile')?.valid ||
      !this.enquiryForm.get('documentAadharFile')?.valid ||
      !this.enquiryForm.get('documentPanCardFile')?.valid

    );
  }

  onAccountTypeChange(): void {
    const accountTypeControl = this.enquiryForm.get('accountType');
    const balanceControl = this.enquiryForm.get('balance');

    if (accountTypeControl && balanceControl) {
      const selectedAccountType = accountTypeControl.value;

      if (selectedAccountType === 'Savings') {
        balanceControl.setValidators([Validators.required, Validators.min(5000)]);
      } else if (selectedAccountType === 'Current') {
        balanceControl.setValidators([Validators.required, Validators.min(10000)]);
      }

      balanceControl.updateValueAndValidity();
    }
  }



  getMinBalanceErrorMessage(): string {
    const balanceControl = this.enquiryForm.get('balance');

    if (balanceControl?.errors?.['required']) {
      return 'Balance is required.';
    }
    if (balanceControl?.errors?.['min']) {
      const selectedAccountType = this.enquiryForm.get('accountType')?.value;
      return selectedAccountType === 'Savings' ? 'Minimum balance for Savings account is 5000.' : 'Minimum balance for Current account is 10000.';
    }

    return '';
  }

  validatePhoneNumber(control: AbstractControl): ValidationErrors | null {
    // Regular expression to match a valid phone number (adjust as needed)
    const phoneNumberRegex = /^[1-9][0-9]{9}$/; // Assuming a 10-digit phone number format

    if (control.value && !phoneNumberRegex.test(control.value)) {
      return { invalidPhoneNumber: true }; // Validation error if phone number doesn't match pattern
    }

    return null; // Return null if validation passes
  }

  //  pincodeValidator(): ValidatorFn {
  //   return (control: AbstractControl): ValidationErrors | null => {
  //     const pincode = control.value as string;
  //     const pincodeRegex = /^[1-9][0-9]{5}$/; // Regex for 6 digits with no leading zero

  //     if (!pincodeRegex.test(pincode)) {
  //       return { invalidPincode: true };
  //     }

  //     return null; // Validation passed
  //   };
  // }

  onPincodeChange(): void {
    const pincodeControl = this.enquiryForm.get('pincode');

    const pincode = pincodeControl?.value.toString();
    const pincodeRegex = /^[1-9][0-9]{5}$/; // Regex for 6 digits with no leading zero

    if (!pincodeRegex.test(pincode)) {
      // Invalid pincode, handle accordingly (e.g., show error message)
      console.log('Invalid pincode entered.');
      // You can also update error state or perform other actions here
    }

  }




  private previewFile(file: File | null, imageSrcProperty: string | null): void {
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        if (typeof reader.result === 'string') {
          // Assign the image source URL to the corresponding property

          if (imageSrcProperty == 'imageSrc1') {
            imageSrcProperty = reader.result;
            this.imageSrc1 = imageSrcProperty;
          }
          else if (imageSrcProperty == 'imageSrc2') {
            imageSrcProperty = reader.result;
            this.imageSrc2 = imageSrcProperty
          }
          else {
            imageSrcProperty = reader.result;
            this.imageSrc3 = imageSrcProperty
          }

        } else {
          console.error('Unexpected result type while reading file.');
        }
      };
      reader.readAsDataURL(file);
    } else {
      // Clear the image source URL if the file is null
      imageSrcProperty = null;
    }
  }




}