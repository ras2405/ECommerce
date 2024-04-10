import { Component, Input } from '@angular/core';
import { Product } from 'src/app/Interfaces/product';

@Component({
  selector: 'app-purchase-item',
  templateUrl: './purchase-item.component.html',
  styleUrls: ['./purchase-item.component.css']
})
export class PurchaseItemComponent {
  @Input() product?: Product;
}
