import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginService } from './session.service';
import { API_URL, httpHeaders as headers } from './environment';
import { Purchase } from '../Interfaces/purchase';
import { Product } from '../Interfaces/product';

@Injectable({
  providedIn: 'root'
})
export class PurchasesService {

  constructor(private http: HttpClient, private login: LoginService) { }

  getPurchase(id: string) {
    const session = this.login.getLoggedIn();
    const headersWithAuthorization = headers.append('Authorization', `Bearer ${session.token}`);
    return this.http.get<Purchase>(
      `${API_URL}/purchases/${id}`,
      { headers: headersWithAuthorization }
    )
  }

  getPurchases() {
    const session = this.login.getLoggedIn();
    const headersWithAuthorization = headers.append('Authorization', `Bearer ${session.token}`);
    return this.http.get<Purchase[]>(
      `${API_URL}/purchases`,
      { headers: headersWithAuthorization }
    )
  }

  createPurchase(products: Product[], paymentMethod: number) {
    const session = this.login.getLoggedIn();
    const headersWithAuthorization = headers.append('Authorization', `Bearer ${session.token}`);
    return this.http.post<Purchase>(
      `${API_URL}/purchases`,
      { 
        userId: session.id,
        products: this.formatProducts(products),
        paymentMethod
      },
      { headers: headersWithAuthorization }
    )
  }

  formatProducts(products: Product[]) {
    return products.map(product => {
      const newItem: Product = { ...product };

      delete newItem.brand.id;
      delete newItem.category.id;
      newItem.colors.forEach(color => {
        delete color.id;
      });

      return newItem;
    });
  }
}
