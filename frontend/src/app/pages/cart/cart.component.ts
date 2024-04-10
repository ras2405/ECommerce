import { Component } from '@angular/core';
import { Product } from 'src/app/Interfaces/product';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent {
  cart: Product[] = JSON.parse(localStorage.getItem('cart') || '[]');

  onItemDelete(index: number) {
    this.cart.splice(index, 1);
    localStorage.setItem('cart', JSON.stringify(this.cart));
    window.location.reload();
  }
}
