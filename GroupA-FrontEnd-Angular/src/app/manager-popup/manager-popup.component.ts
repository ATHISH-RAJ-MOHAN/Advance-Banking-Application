import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-popup',
  templateUrl: './manager-popup.component.html',
  styleUrl: './manager-popup.component.css'
})
export class ManagerPopupComponent {
  inputValue: string = '';

  @Output() close = new EventEmitter<any>();
  @Output() onCancel = new EventEmitter<void>();

  closePopup() {
    console.log(this.inputValue)
    this.close.emit(this.inputValue); // Pass data back to parent component if needed
  }
  ResetPopup(){
    this.onCancel.emit();
  }
}