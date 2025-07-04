import { Component, Input } from '@angular/core';
import { EnquiryDisplay } from '../models/enquiry-display';

@Component({
  selector: 'app-manager-enquiry-list',
  templateUrl: './manager-enquiry-list.component.html',
  styleUrl: './manager-enquiry-list.component.css'
})
export class ManagerEnquiryListComponent {
  @Input()
  enquiries: EnquiryDisplay[] = [];

  @Input()
  Type: string = "";
}
