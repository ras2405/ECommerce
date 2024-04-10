import { Component, Input } from '@angular/core';
import { PaymentType, Purchase } from 'src/app/Interfaces/purchase';

@Component({
  selector: 'app-purchase-summary',
  templateUrl: './purchase-summary.component.html',
  styleUrls: ['./purchase-summary.component.css']
})
export class PurchaseSummaryComponent {
  @Input() purchase?: Purchase;

  hasPaganzaPromotion(): boolean {
    return this.purchase?.paymentMethod === 6;
  }

  getSubtotal(): number {
    return this.purchase?.products.reduce((total: any, product: any) => total + (product.price || 0), 0);
  };

  getPromName(promotionType?: number) {
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

  getPaymentName(paymentType?: PaymentType) {
    switch (paymentType) {
      case 0:
        return "Visa credit card";
      case 1:
        return "MasterCard credit card";
      case 2:
        return "Santander bank debit";
      case 3:
        return "Itau bank debit";
      case 4:
        return "BBVA bank debit";
      case 5:
        return "Paypal";
      case 6:
        return "Paganza";
      default:
        return "";
    }
  }
}
