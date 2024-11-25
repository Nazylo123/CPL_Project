import { Component, NgModule } from '@angular/core';
import { AuthService } from '../auth.service';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  registerForm: FormGroup;
  message: string = '';
  constructor(private fb: FormBuilder, private authService: AuthService) {
    // Khởi tạo form
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }
  get email() {
    return this.registerForm.get('email');
  }
  get password() {
    return this.registerForm.get('password');
  }
  onRegister(): void {
    if (this.registerForm.valid) {
      const registerData = this.registerForm.value;
      this.authService.register(registerData).subscribe({
        next: (res: any) => {
          this.message =
            res.message ||
            'Registration successful! Please check your email to confirm.';
          this.registerForm.reset();
        },
        error: (err) => {
          this.message = err.error || 'Registration failed. Please try again.';
        },
      });
    }
  }
}
