import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Product } from 'src/app/Interfaces/product';
import { ErrorModalService } from 'src/app/services/error-modal.service';
import { PromotionsService } from 'src/app/services/promotions.service';
import { PurchasesService } from 'src/app/services/purchases.service';

@Component({
  selector: 'app-cart-checkout',
  templateUrl: './cart-checkout.component.html',
  styleUrls: ['./cart-checkout.component.css']
})
export class CartCheckoutComponent {
  @Input() cart: Product[] = [];
  discount: number = 0;
  promotionApplied: string = '';
  paymentMethod: number = 0;

  constructor(
    private promotion: PromotionsService,
    private purchase: PurchasesService,
    private router: Router,
    private errorModalService: ErrorModalService
  ) {}

  ngOnInit() {
    this.promotion.calculatePromotions(this.cart).subscribe(
      (response) => {
        this.discount = response.amount;
        this.promotionApplied = this.getPromName(response.type);
        if (response.message !== '') {
          this.errorModalService.openErrorModal(response.message);
        }
      },
      (error: HttpErrorResponse) => {
        if (error.status == 0 || error.status == 500){
          this.errorModalService.openErrorModal('Oops! The server is experiencing some difficulties. Please try again later');
        }
        console.error('Error fetching promotion:', error);
      }
    );
  };

  getSubtotal(): number {
    return this.cart.reduce((total, product) => total + (product.price || 0), 0);
  };

  getTotal(): number { 
    return this.getSubtotal() - this.discount; 
  };

  buy(): void {
    this.purchase.createPurchase(this.cart, this.paymentMethod).subscribe(
      (response) => {
        localStorage.removeItem('cart');
        this.router.navigate(['/purchases', response.id]);
        if (response.message !== '') {
          this.errorModalService.openErrorModal(response.message);
        }
      },
      (error: HttpErrorResponse) => {
        if (error.status == 0 || error.status == 500){
          this.errorModalService.openErrorModal('Oops! The server is experiencing some difficulties. Please try again later');
        }
        console.error('Error creating purchase:', error);
      }
    );
  };

  getPromName(promotionType: number) {
    switch (promotionType) {
        case 0:
            return "20% OFF";
        case 1:
            return "Total look";
        case 2:
            return "3x2";
        case 3:
            return "3x1 Loyalty";
        default:
          return "Invalid promotion";	
    }
  }
}
