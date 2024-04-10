import { Component, OnInit, ViewChild} from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { LoginService } from '../../services/session.service';
import * as _ from 'lodash';
import { ErrorModalService } from 'src/app/services/error-modal.service';
import { HttpErrorResponse } from '@angular/common/http';
import { User, UserRol } from 'src/app/Interfaces/user';


@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit{
  users: User[] = [];
  modalRef: NgbModalRef | undefined;
  selectedUser: User = {};
  selectedUserEdit: User = {};
  errorResponse?: HttpErrorResponse["error"];
  UserRol = UserRol;

  constructor(private userService: UserService, private modalService: NgbModal, private router: Router, private login: LoginService, private errorModalService: ErrorModalService) { }
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('editUserModal') editUserModal: any;
  @ViewChild('addUserModal') addUserModal: any;

  ngOnInit(): void {
    this.userService.getUsers().subscribe(
      (data) => {
        this.users = data;
      },
      (error: HttpErrorResponse) => {
        if (error.status == 0 || error.status == 500){
          this.errorModalService.openErrorModal('Oops! The server is experiencing some difficulties. Please try again later');
        }
        console.error('An error has ocurred getting the e-commerce users', error);
      }
    );
  }

  getRolText(rol?: UserRol): string{
    switch(rol){
      case UserRol.Admin:
        return 'Admin';
      case UserRol.Buyer:
        return 'Buyer';
      case UserRol.Both:
        return 'Admin and buyer';
      default:
        return 'Unknown user rol';
    }
  }

  deleteUser(): void {
    this.userService.deleteUser(this.selectedUser);
    if (this.selectedUser.id == this.getCurrentUserId()){
      localStorage.removeItem("user");
      this.router.navigate(['/login']);
    }
    else if (this.modalRef) {
      window.location.reload();
      this.modalRef.close();
    }
  }
  
  getCurrentUserId(): string | null {
    const userSession = this.login.getLoggedIn();
    return userSession !== null ? userSession.id : '';
  }
  
  openAddUserModal(): void {
    this.modalRef = this.modalService.open(this.addUserModal, { centered: true , size: 'lg'});
  }

  openConfirmationModal(user: any): void {
    this.modalRef = this.modalService.open(this.confirmationModal, { centered: true , size: 'lg'});
    this.selectedUser=user;
  }

  openEditUserModal(user: any): void {
    this.selectedUser = user;
    this.selectedUserEdit = _.clone(user);
    this.errorResponse = null;
    this.modalRef = this.modalService.open(this.editUserModal, { centered: true,  size: 'lg' });
  }

  saveChanges(modal: any) {
    this.userService.updateUserData(this.selectedUserEdit).subscribe(
      (data) => {
        console.log('User data was updated succsesfully', data);
        modal.close();
        if (this.selectedUserEdit.id == this.getCurrentUserId() && this.selectedUserEdit.rol == 1){
          this.router.navigate(['/products']);
          localStorage.removeItem('user');

          this.login.login(this.selectedUserEdit.email, this.selectedUserEdit.password).subscribe((data) => {
            const user = JSON.stringify({
              id: data.id,
              rol: data.rol,
              token: data.token	
            });
            localStorage.setItem('user', user)
          });

        } else {
          window.location.reload()
        }
      },
      (error: HttpErrorResponse) => {
        if (error.status == 0 || error.status == 500){
          this.errorModalService.openErrorModal('Oops! The server is experiencing some difficulties. Please try again later');
        }
        console.error('An error has ocurred updating user data', error);
        this.errorResponse = error.error;
        return;
      }
    );
  }
}
