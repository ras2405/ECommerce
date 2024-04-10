import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { API_URL, httpHeaders as defaultHeaders } from './environment';
import { LoginService } from './session.service';
import { DeleteUserResponse, User } from '../Interfaces/user';
import { Purchase } from '../Interfaces/purchase';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient, private login: LoginService) {}

  getUserData(): Observable<User> {
    const headersWithAuthorization = this.appendAuthorizationHeader();
    return this.http.get<User>(`${API_URL}/users/${this.login.getLoggedIn().id}`, { headers: headersWithAuthorization });
  }

  getUser(id : string): Observable<User> {
    const headersWithAuthorization = this.appendAuthorizationHeader();
    return this.http.get<User>(`${API_URL}/users/${id}`, { headers: headersWithAuthorization });
  }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${API_URL}/users`, { headers: defaultHeaders });
  }

  deleteUser(user: User): void {
    const headersWithAuthorization = this.appendAuthorizationHeader();
    this.http.delete<DeleteUserResponse>(`${API_URL}/users/${user.id}`, { headers: headersWithAuthorization }).subscribe();
  }

  getBuyersShoppingHistory(): Observable<Purchase[]> {
    const userId = this.login.getLoggedIn().id;
  
    if (this.login.getLoggedIn().rol !== 0 && userId) {  
      const headersWithAuthorization = this.appendAuthorizationHeader();
      return this.http.get<Purchase[]>(`${API_URL}/purchases?userId=${userId}`, { headers: headersWithAuthorization })
        .pipe(
          catchError(error => {
            console.error('Error getting shopping history:', error);
            return of([]); 
          })
        );
    } else {
      return of([]); 
    }
  }

  updateUserData(user: User): Observable<User> {
    const headersWithAuthorization = this.appendAuthorizationHeader();
    return this.http.put<User>(`${API_URL}/users/${user.id}`, user, { headers: headersWithAuthorization });
  }

  private appendAuthorizationHeader(): HttpHeaders {
    const headersWithAuthorization = defaultHeaders.append('Authorization', `Bearer ${this.login.getLoggedIn().token}`);
    return headersWithAuthorization;
  }

  createUser(user: User): Observable<User> {
    return this.http.post<User>(`${API_URL}/users`, user, { headers: defaultHeaders });
  }
}
