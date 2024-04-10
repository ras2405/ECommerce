import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ErrorModalService } from 'src/app/services/error-modal.service';
import { PurchasesService } from 'src/app/services/purchases.service';
import { UserService } from 'src/app/services/user.service';
import { map } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { PaymentType, Purchase } from 'src/app/Interfaces/purchase';
import { Product } from 'src/app/Interfaces/product';
import { PromotionType } from 'src/app/Interfaces/promotion';

@Component({
  selector: 'app-purchases',
  templateUrl: './purchases.component.html',
  styleUrls: ['./purchases.component.css']
})
export class PurchasesComponent {
  purchasesList?: Purchase[] = [];
  
  constructor( public purchases: PurchasesService, private router: Router, public userService: UserService, private errorModalService: ErrorModalService) { }

  ngOnInit() {
    this.purchases.getPurchases().subscribe(
      (purchases) => {
        this.purchasesList = purchases
      },
      (error: HttpErrorResponse) => {
        console.error('Error fetching purchases:', error);
      }
    )
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

  getUserEmail(id: string): Observable<string | undefined> {
    return this.userService.getUser(id).pipe(
      map(data => data.email)
    );
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

  getPrice(products: Product[], promotionAmount: number): number {
    const total = products.reduce((acc, product) => acc + product.price, 0);
    return total - promotionAmount;
  }

  goToPurchaseDetails(purchaseId: string) {
    this.router.navigate(['/purchases', purchaseId]);
  }
}
