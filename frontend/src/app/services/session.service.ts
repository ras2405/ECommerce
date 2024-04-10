import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_URL, httpHeaders as headers } from './environment';
import { Session, LogoutResponse } from '../Interfaces/session';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  constructor(private http: HttpClient) { }

  login(email?: string, password?: string): Observable<Session> {
    return this.http.post<Session>(
      `${API_URL}/session`, 
      { email, 
        password,
      }, 
      { headers }
    )
  }

  logout(): Observable<LogoutResponse> {
    const token = this.getLoggedIn().token;
    const headersWithAuthorization = headers.append('Authorization', `Bearer ${token}`);
    return this.http.delete<LogoutResponse>(`${API_URL}/session`, { headers: headersWithAuthorization })
  }

  getLoggedIn(): Session {
    const user = localStorage.getItem('user') ?? '{}';
    return JSON.parse(user);
  }

  isLoggedIn(): boolean {
    return localStorage.getItem('user') != null;
  }

  isAdmin(): boolean {
    const userString = localStorage.getItem('user');
    const user = userString ? JSON.parse(userString) : null;
    return (user != null && user.rol != '1');
  }

  isBuyerOrBoth(): boolean {
    const userString = localStorage.getItem('user');
    const user = userString ? JSON.parse(userString) : null;
    return (user != null && user.rol != '0');
  }
}
