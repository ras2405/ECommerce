import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from 'src/app/services/session.service';
import { UserService } from 'src/app/services/user.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ErrorModalService } from 'src/app/services/error-modal.service';
import { HttpErrorResponse } from '@angular/common/http';
import { PaymentType, Purchase } from 'src/app/Interfaces/purchase';
import { Product } from 'src/app/Interfaces/product';
import { PromotionType } from 'src/app/Interfaces/promotion';
import { User } from 'src/app/Interfaces/user';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  user: User = {};
  editMode = false;
  errorResponse?: HttpErrorResponse["error"];
  shoppingHistory: Purchase[] = [];
  modalRef: NgbModalRef | undefined;

  constructor(private userService: UserService, private router: Router, private login: LoginService, private modalService: NgbModal, private errorModalService: ErrorModalService) { }
  @ViewChild('confirmLogoutModal') confrimLogoutModal: any;

  ngOnInit() {
    this.userService.getUserData().subscribe(
      (data) => {
        this.user = data;
        this.errorResponse = null;
      },
      (error: HttpErrorResponse) => {
        if (error.status == 0 || error.status == 500){
          this.errorModalService.openErrorModal('Oops! The server is experiencing some difficulties. Please try again later');
        }
        console.error('An error has ocurred getting the logged users data', error);
        this.errorResponse = error.error;
      }
    );

    this.userService.getBuyersShoppingHistory().subscribe(
      (data) => {
        this.shoppingHistory = data;
      },
      (error: HttpErrorResponse) => {
        if (error.status == 0 || error.status == 500){
          this.errorModalService.openErrorModal('Oops! The server is experiencing some difficulties. Please try again later');
        }
        console.error('An error has ocurred getting the logged users shopping history', error);
      }
    );
  }

  logout(): void {
    this.login.logout().subscribe(
      () => {
        localStorage.removeItem('user');
        localStorage.removeItem('cart');
        this.router.navigate(['/products']);
      },
      (error: HttpErrorResponse) => {
        if (error.status == 0 || error.status == 500){
          this.errorModalService.openErrorModal('Oops! The server is experiencing some difficulties. Please try again later');
        }
        console.error('Error logging out:', error);
      }
    );
  }

  openConfirmLogoutModal(): void {
    this.modalRef = this.modalService.open(this.confrimLogoutModal, { centered: true , size: 'lg'});
  }

  editModeOn() {
    this.editMode = true;
  }

  saveChanges() {
    this.userService.updateUserData(this.user).subscribe(
      (data) => {
        console.log('User data was updated succsesfully', data);
        this.editMode = false;
        this.errorResponse = null;
      },
      (error) => {
        if (error.status == 0 || error.status == 500){
          this.errorModalService.openErrorModal('Oops! The server is experiencing some difficulties. Please try again later');
        }
        console.error('An error has ocurred updating user data', error);
        this.errorResponse = error.error;
      }
    );
  }

  getPrice(products: Product[], promotionAmount: number): number {
    const total = products.reduce((acc, product) => acc + product.price, 0);
    return total - promotionAmount;
  }

  getPaymentMethodText(method: PaymentType): string {
    switch (method) {
      case PaymentType.Visa:
        return 'Visa';
      case PaymentType.MasterCard:
        return 'MasterCard';
      case PaymentType.Santander:
        return 'Santander';
      case PaymentType.Itau:
        return 'Itau';
      case PaymentType.BBVA:
        return 'BBVA';
      case PaymentType.Paypal:
        return 'Paypal';
      case PaymentType.Paganza:
        return 'Paganza';
      default:
        return 'Unknown Payment Method';
    }
  }

  getPromotionTypeText(method: PromotionType): string {
    switch (method) {
      case PromotionType.twenty:
        return '20% OFF';
      case PromotionType.total:
        return 'Total look';
      case PromotionType.threeXtwo:
        return '3x2';
      case PromotionType.threeXone:
        return '3x1';
      default:
        return 'Unknown Promotion tipe Method';
    }
  }

  goToPurchaseDetails(purchaseId: string) {
    this.router.navigate(['/purchases', purchaseId]);
  }
}
