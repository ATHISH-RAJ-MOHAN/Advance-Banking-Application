import { Component } from '@angular/core';
import { EnquiryDisplay } from '../models/enquiry-display';
import { ManagerServiceService } from '../services/manager.service';
import { CommonService } from '../services/common.service';

@Component({
  selector: 'app-manager-home',
  templateUrl: './manager-home.component.html',
  styleUrl: './manager-home.component.css'
})
export class ManagerHomeComponent {
  enquiriesPending: EnquiryDisplay[] = [];
  enquiriesApproved: EnquiryDisplay[] = [];
  enquiriesRejected: EnquiryDisplay[] = [];
  managerId: number | null = null;
  constructor(private managerService: ManagerServiceService, private commonService: CommonService) 
  {
    
  }
  async ngOnInit() {
    await this.setManagerId()
    if(this.managerId != null)
      this.fetchEnquiries();
    else
      console.error("Manager ID is null");
  }
  
  
  async setManagerId() {
    const userId = this.commonService.getUserIdFromSession()
    try {
      this.managerId = await this.managerService.getManagerIdFromUserId(userId!).toPromise();
      console.log(`Manager ID: ${this.managerId}`);
    } catch (error) {
      console.error(error);
      alert(error);
    }
    //  this.managerService.getManagerIdFromUserId(userId!)
    //   .subscribe(id => {
    //     this.managerId = id
    //     console.log(`In the subscribe: ${this.managerId}`)
    //   },error => {
    //     console.log(error);
    //     alert(error.error);
    //   });
  }
  
  fetchEnquiries()
  { 
    this.managerService.getPendingList(this.managerId!)
      .subscribe(enquiries => {
        this.enquiriesPending = enquiries
      })
      this.managerService.getApprovedList(this.managerId!)
      .subscribe(enquiries => {
        this.enquiriesApproved = enquiries
      })
      this.managerService.getRejectedList(this.managerId!)
      .subscribe(enquiries => {
        this.enquiriesRejected = enquiries
      })
  }
  
}
