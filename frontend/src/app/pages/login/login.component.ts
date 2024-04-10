import { Component, ViewChild } from '@angular/core';
import { LoginService } from 'src/app/services/session.service';
import { NgbNav } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email?: string;
  password?: string;
  activeTab: number = 1;

  @ViewChild('nav', { static: true }) nav!: NgbNav;

  constructor(private loginService: LoginService) { }

  switchToTab(tabNumber: number) {
    this.activeTab = tabNumber;
    this.nav.select(tabNumber);
  }
}
