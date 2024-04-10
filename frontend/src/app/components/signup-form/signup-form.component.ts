import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from 'src/app/services/session.service';
import { UserService } from 'src/app/services/user.service';
import { ErrorModalService } from 'src/app/services/error-modal.service';
import { HttpErrorResponse } from '@angular/common/http';
import { User } from 'src/app/Interfaces/user';

@Component({
  selector: 'app-signup-form',
  templateUrl: './signup-form.component.html',
  styleUrls: ['./signup-form.component.css']
})
export class SignupFormComponent {
  responseError?: HttpErrorResponse["error"];
  user: User = { rol: 0 }

  constructor(
    private loginService: LoginService,
    private userService: UserService,
    private router: Router,
    private errorModalService: ErrorModalService
  ) { }

  onSubmit() {
    this.userService.createUser(this.user).subscribe(
      (data) => {
        this.loginService.login(this.user.email, this.user.password).subscribe(
          (data) => {
            if (localStorage.getItem('user') == null) {
              const user = JSON.stringify({
                id: data.id,
                rol: data.rol,
                token: data.token	
              });
              localStorage.setItem('user', user)
              this.router.navigate(['/products']);
            } else {
              window.location.reload();
            }
          },
          (error: HttpErrorResponse) => {
            if (error.status == 0 || error.status == 500){
              this.errorModalService.openErrorModal('Oops! The server is experiencing some difficulties. Please try again later');
            }
            this.responseError = error.error;
          }
        );
      },
      (error: HttpErrorResponse) => {
        if (error.status == 0 || error.status == 500){
          this.errorModalService.openErrorModal('Oops! The server is experiencing some difficulties. Please try again later');
        }
        this.responseError = error.error;
      }
    )
  }

  roleText(role?: number): string {
    switch (role) {
      case 0:
        return 'Admin';
      case 1:
        return 'Buyer';
      case 2:
        return 'Admin & Buyer';
      default:
        return 'Select Role';
    }
  }
}
