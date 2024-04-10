export enum UserRol{
    Admin = 0,
    Buyer = 1,
    Both= 2
  }

export interface User {
    id?: string;
    email?: string;
    address?: string;
    password?: string;
    rol?: UserRol;
}

export interface DeleteUserResponse {
    message: string;
}