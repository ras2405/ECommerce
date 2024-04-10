import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from 'src/app/services/session.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent {
  constructor(private login: LoginService, private router: Router) { }

  isLoggedIn(): boolean {
    return this.login.isLoggedIn();
  }

  isAdmin(): boolean {
    return this.login.isAdmin();
  }

  isBuyerOrBoth(): boolean {
    return this.login.isBuyerOrBoth();
  }

  reload(): void {
    this.router.navigate(['/products']);
  }
}
