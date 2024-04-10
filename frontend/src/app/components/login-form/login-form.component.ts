import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from 'src/app/services/session.service';
import { ErrorModalService } from 'src/app/services/error-modal.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css']
})
export class LoginFormComponent {
  email?: string;
  password?: string;
  responseError?: HttpErrorResponse["error"];

  constructor(private loginService: LoginService, private router: Router, private errorModalService: ErrorModalService) { }

  onSubmit() {
    this.loginService.login(this.email, this.password).subscribe(
      (data) => {
        const user = JSON.stringify({
          id: data.id,
          rol: data.rol,
          token: data.token	
        });
        localStorage.setItem('user', user);
        this.router.navigate(['/products']);
      },
      (error: HttpErrorResponse ) => {
        if (error.status == 0 || error.status == 500){
          this.errorModalService.openErrorModal('Oops! The server is experiencing some difficulties. Please try again later');
        }
        this.responseError = error.error;
      }
    );
  }
}
