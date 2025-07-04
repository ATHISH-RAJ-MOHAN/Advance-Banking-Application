import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Customer } from '../models/customer';
import { CustomerService } from '../services/customer.service';
import { CommonService } from '../services/common.service';

@Component({
  selector: 'app-view-profile',
  templateUrl: './customer-details.component.html',
  styleUrls: ['./customer-details.component.css']
})
export class CustomerDetailsComponent {
  CustomerDetails: Customer = <Customer>{}
  public photo?: string;
  public aadhar?: string;
  public panCard?: string;
  //public customerId:number;
  public photoFile: File | null = null;
  public aadharFile: File | null = null;
  public panCardFile: File | null = null;
  customerId: any;
  emailAddress: any;
  constructor(private http: HttpClient, private router: Router, public apiService: CustomerService, public commonService: CommonService) {
    const uid = this.commonService.getUserIdFromSession();
    if(uid !== null) {
      this.apiService.getCustomerByUserId(uid.toString()).subscribe(
       // (data) => this.CustomerDetails = data
        (data:any)=> {this.CustomerDetails= data
          this.emailAddress=data.emailAddress
          this.apiService.getDocuments(data.emailAddress,data).subscribe(
            (docs:any)=>{
              console.log(docs)
              console.log("docs came")
              this.photo = `data:image/jpeg;base64,${docs.basePhoto}`;
              this.aadhar = `data:image/jpeg;base64,${docs.baseAadhar}`;
              this.panCard = `data:image/jpeg;base64,${docs.basePanCard}`;
              this.CustomerDetails.documentPhotoFile= 'data:image/png;base64,'+docs.basePhoto;         
              this.CustomerDetails.documentAadharFile='data:image/png;base64,'+docs.baseAadhar
              this.CustomerDetails.documentPanCardFile='data:image/png;base64,'+docs.basePanCard
            }
          )
        }
      );
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
  onFileChange(event: Event, type: string): void {
    const element = event.target as HTMLInputElement;
    const file = element.files ? element.files[0] : null;
    if (file) {
      switch (type) {
        case 'photo':
          this.photoFile = file;
          console.log('Photo file set:', file.name);
          break;
        case 'aadhar':
          this.aadharFile = file;
          console.log('Aadhar file set:', file.name);
          break;
        case 'panCard':
          this.panCardFile = file;
          console.log('PAN card file set:', file.name);
          break;
      }
    }
}

private loadDocuments(emailAddress: string, data:any): void {
  this.apiService.getDocuments(emailAddress, data).subscribe({
    next: (doc:any) => {
      this.photo = `data:image/jpeg;base64,${doc.basePhoto}`;
      this.aadhar = `data:image/jpeg;base64,${doc.baseAadhar}`;
      this.panCard = `data:image/jpeg;base64,${doc.basePanCard}`;
    },
    error: (error) => {
      console.error('Failed to load documents', error);
      alert('Failed to load documents: ' + error.message);
    }
  });
}


onUpdateDocument(): void {
  if (this.emailAddress==null) {
    alert('Customer ID is undefined.');
    return;
  }

  const formData = new FormData();
  if (this.photoFile) {
    formData.append('photo', this.photoFile);
    console.log('Appending photo to formData');
  }
  if (this.aadharFile) {
    formData.append('aadhar', this.aadharFile);
    console.log('Appending Aadhar to formData');
  }
  if (this.panCardFile) {
    formData.append('panCard', this.panCardFile);
    console.log('Appending PAN Card to formData');
  }

  console.log('Sending update request for customer ID:', this.customerId);

  this.apiService.updateDocuments(this.CustomerDetails.emailAddress, formData).subscribe({
    next: () => {
      console.log('Documents updated successfully.');
      alert('Documents updated successfully.');
      this.loadDocuments(this.CustomerDetails.emailAddress, formData); // Reload documents
    },
    error: (error) => {
      console.error('Failed to update documents:', error);
      alert('Failed to update documents: ' + error.message);
    }
  });
}


onUpload(model:any){
  console.log(model)
  //console.log(model.documentAadharFile)
 // console.log(this.base64ToFile(model.documentPhotoFile,"image"))
  const updatedModel={
    //CustomerId: model.customerId,
    EmailAddress: model.emailAddress,
    Photo: model.documentPhotoFile,
    Aadhar: model.documentAadharFile,
    PanCard: model.documentPanCardFile,
  }
  console.log(updatedModel)
  const formData = new FormData();
  if (this.photoFile) {
    formData.append('photo', this.photoFile);
    console.log('Appending photo to formData');
  }
  if (this.aadharFile) {
    formData.append('aadhar', this.aadharFile);
    console.log('Appending Aadhar to formData');
  }
  if (this.panCardFile) {
    formData.append('panCard', this.panCardFile);
    console.log('Appending PAN Card to formData');
  }
  this.apiService.updateDocuments(this.emailAddress, updatedModel).subscribe(
    (data:any)=>{
      alert("uploaded Successfully")  
    })
  
}

  onEdit(){
    
     
    this.apiService.update(this.CustomerDetails).subscribe(
      (data:any)=>{
        alert("updated Successfully")
      })
    // this.apiService.update().subscribe(
    //   (data)=>this.CustomerDetails= data
    // )
   // this.router.navigateByUrl('/edit-profile')
  }
  
  ngOnInit(): void {
    const userId = this.commonService.getUserIdFromSession();
    if (userId !== null) {
      this.apiService.getCustomerByUserId(userId.toString()).subscribe(
        (data: Customer) => {
          console.log(data);
          this.CustomerDetails = data;
        },
        error => {
          console.error('Error fetching customer data:', error);
        }
      );
    }
  }

  goToEdit(): void {
    this.router.navigateByUrl('/edit-profile');
  }

  goBack(): void {
    this.router.navigateByUrl('/customer');
  }
}
