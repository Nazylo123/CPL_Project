import { FormsModule } from '@angular/forms';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  standalone: true,
  imports: [FormsModule, CommonModule],
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  onLogin() {
    console.log('Email:', this.email);
    console.log('Password:', this.password);

    this.authService
      .login({ email: this.email, password: this.password })
      .subscribe({
        next: (response: any) => {
          localStorage.setItem('token', response.token);
          localStorage.setItem('roles', JSON.stringify(response.roles));
          this.router.navigate(['/dashboard']);
        },
        error: (err) => {
          this.errorMessage = 'Invalid email or password';
        },
      });
  }
}
