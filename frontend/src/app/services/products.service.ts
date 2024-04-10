import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_URL, httpHeaders as defaultHeaders } from './environment';
import { LoginService } from './session.service';
import { Product } from '../Interfaces/product';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  constructor(private http: HttpClient, private login: LoginService) { }
  returnedProducts?: Product[];
  params : any = {};

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(`${API_URL}/products`, { headers: defaultHeaders });
  }

  getFilteredProducts(
    category?: string,
    brand?: string, 
    name?: string, min?:
    number, max?: number, 
    promo: boolean | null = null
  ): Observable<Product[]> {
    if (category !== null && category !== undefined) {
      this.params.category = category;
    }

    if (brand !== null && brand !== undefined) {
      this.params.brand = brand;
    }

    if (name !== null && name !== undefined) {
      this.params.text = name;
    }

    if (min !== null && min !== undefined) {
      this.params.min = min;
    }

    if (max !== null && max !== undefined) {
      this.params.max = max;
    }

    if (promo !== null && promo !== undefined) {
      this.params.promo = promo;
    }

    const searchParams = new URLSearchParams(this.params).toString();
    const url = `${API_URL}/products?${searchParams}`;

    return this.http.get<Product[]>(url, { headers: defaultHeaders });
  }

  getSpecificProduct(id: string): Observable<Product> {
    return this.http.get<any>(`${API_URL}/products/${id}`, { headers: defaultHeaders });
  }

  addProduct(product: Product): Observable<Product> {
    const headersWithAuthorization = this.appendAuthorizationHeader();
    return this.http.post<any>(`${API_URL}/products`, product, { headers: headersWithAuthorization });
  }

  updateProduct(product: Product): Observable<Product> {
    const headersWithAuthorization = this.appendAuthorizationHeader();
    return this.http.put<Product>(`${API_URL}/products/${product.id}`, product, { headers: headersWithAuthorization });
  }

  deleteProduct(productId: string): Observable<Product> {
    const headersWithAuthorization = this.appendAuthorizationHeader();
    return this.http.delete<Product>(`${API_URL}/products/${productId}`, { headers: headersWithAuthorization })
  }

  private appendAuthorizationHeader(): HttpHeaders {
    const headersWithAuthorization = defaultHeaders.append('Authorization', `Bearer ${this.login.getLoggedIn().token}`);
    return headersWithAuthorization;
  }
}