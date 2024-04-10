import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { API_URL, httpHeaders as headers } from './environment';
import { LoginService } from './session.service';
import { Promotion } from '../Interfaces/promotion';
import { Product } from '../Interfaces/product';

@Injectable({
  providedIn: 'root'
})
export class PromotionsService {

  constructor(private http: HttpClient, private login: LoginService) { }

  calculatePromotions(products: Product[]) {
    const token = this.login.getLoggedIn().token;
    const headersWithAuthorization = headers.append('Authorization', `Bearer ${token}`);
    return this.http.post<Promotion>(
      `${API_URL}/promotions`, 
      products, 
      {headers: headersWithAuthorization}
    )
  }
}
