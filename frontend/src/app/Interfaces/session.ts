export interface Session {
    id: string;
    rol: number;
    token: string;
}

export interface LogoutResponse {
    message: string;
}