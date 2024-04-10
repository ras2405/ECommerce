import { HttpHeaders } from "@angular/common/http";

export const httpHeaders = new HttpHeaders({
    'Content-Type': 'application/json',
    'Access-Control-Allow-Origin': 'http://localhost:4200'
});

export const API_URL = 'https://localhost:7019/api';
