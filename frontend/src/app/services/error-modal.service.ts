import { Injectable } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ErrorModalComponent } from '../components/error-modal/error-modal.component';
@Injectable({
  providedIn: 'root',
})
export class ErrorModalService {
  constructor(private modalService: NgbModal) {}

  openErrorModal(errorMessage: string) {
    const modalRef = this.modalService.open(ErrorModalComponent);
    modalRef.componentInstance.errorMessage = errorMessage;
  }
}

