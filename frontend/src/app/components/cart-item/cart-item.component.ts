import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from 'src/app/Interfaces/product';

@Component({
  selector: 'app-cart-item',
  templateUrl: './cart-item.component.html',
  styleUrls: ['./cart-item.component.css']
})
export class CartItemComponent {
  @Input() product?: Product;
  @Output() deleteClicked = new EventEmitter<void>();

  onDeleteClick() {
    this.deleteClicked.emit();
  }
}
