import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = 'https://localhost:7102/api/Auth';

  constructor(private http: HttpClient, private router: Router) {}

  register(data: { email: string; password: string }): Observable<any> {
    return this.http.post(`${this.baseUrl}/register`, data);
  }
  login(data: { email: string; password: string }) {
    return this.http.post(`${this.baseUrl}/login`, data);
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('email');
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    const token = localStorage.getItem('token');
    return !!token;
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getEmail(): string | null {
    return localStorage.getItem('email');
  }

  checkLoginStatus(): boolean {
    return this.isLoggedIn();
  }
  setEmail(email: string): void {
    localStorage.setItem('email', email);
  }
}
