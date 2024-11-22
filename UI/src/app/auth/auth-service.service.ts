import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthServiceService {
  constructor(private http: HttpClient) {}
  private apiUrl = 'http://localhost:7102/api';
  login(email: string, password: string): Observable<any> {
    const payload = { email, password };
    return this.http.post(`${this.apiUrl}/login`, payload);
  }

  saveToken(token: string): void {
    localStorage.setItem('authToken', token); // Lưu token vào Local Storage
  }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  logout(): void {
    localStorage.removeItem('authToken'); // Xóa token khi đăng xuất
  }
}
