import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Enquiry } from '../models/enquiry';
import { ManagerServiceService } from '../services/manager.service';
import { EnquiryDataService } from '../services/enquiry-data.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-manager-view-enquiry',
  templateUrl: './manager-view-enquiry.component.html',
  styleUrl: './manager-view-enquiry.component.css'
})
export class ManagerViewEnquiryComponent implements OnInit{
  id:number=0;
  feedback:string = "";
  isPending:boolean = false;
  isLoading:boolean = true;
  isPopupVisible:boolean = false;
  needsCheque:string="No";
  EnquiryDetais:Enquiry = <Enquiry>{};
  documentPhotoFile: File | null = null;
  documentAadharFile: File | null = null;
  documentPanCardFile: File | null = null;
  imageSrc1: string | null = null;
  imageSrc2: string | null = null;
  imageSrc3: string | null = null;

  constructor(
    public apiservices:ManagerServiceService,
    public enquiryDataService: EnquiryDataService,
    private currentRoute:ActivatedRoute,
    private spinner:NgxSpinnerService,
    private router:Router
  ) {
    this.spinner.show();

   }

  ngOnInit(): void {
    this.id = this.currentRoute.snapshot.params["id"];
    
    this.apiservices.getEnquiryDetails(this.id).subscribe(
      (data)=>{
        console.log(data.status);
        if(data.status=='1' && data.feedback==''){
          this.isPending=true;
        }
        if(data.wantsCheque==true){
          this.needsCheque="Yes"
        }
        console.log(this.isPending);
        this.EnquiryDetais=data;
        this.imageSrc1 = 'data:image/png;base64,' + data.photo;
        this.imageSrc2 = 'data:image/png;base64,' + data.aadhaar;
        this.imageSrc3 = 'data:image/png;base64,' + data.panCard;
    
        this.documentPhotoFile = this.base64ToFile(data.photo, 'photo.png');
        this.documentAadharFile = this.base64ToFile(data.aadhaar, 'aadhar.png');
        this.documentPanCardFile = this.base64ToFile(data.panCard, 'pancard.png');

        this.previewFile(this.documentPhotoFile, this.imageSrc1);
        this.previewFile(this.documentAadharFile, this.imageSrc2);
        this.previewFile(this.documentPanCardFile, this.imageSrc3);

        while(this.EnquiryDetais.emailAddress == null);
        this.spinner.hide();
      }   
    )
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

  onApprove():void{
    console.log("Approve button pressed");
    this.apiservices.approveEnquiry(this.id).subscribe(
      (Res)=>{
        console.log('Successful');
        alert('Approved and mail sent successfully!!');
        this.router.navigate(['/manager']);
      }, error => {
        console.log(error);
        alert(error.error);
      }
      
    );
  }
  onReject():void{
    console.log('Reject Button Pressed')
    this.isPopupVisible = true;
  }
  ResetPopup(){
    this.isPopupVisible = false;
  }
  closePopup(data: any) {
    this.isPopupVisible = false;
    // Handle data passed back from the popup if needed
    this.apiservices.rejectEnquiry(this.id, data).subscribe(
      (res)=>{
        this.router.navigate(['/manager']);
      }, error => {
        console.log(error);
        alert(error.error);
      }
    )
  }
}