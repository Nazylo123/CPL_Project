import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { CommonModule } from '@angular/common';
import { jwtDecode } from 'jwt-decode';
import { log } from 'node:console';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  role1: string = '';
  token: string | null = '';
  email: string | null = '';
  constructor(private authService: AuthService) {}

  ngOnInit() {
    this.token = this.authService.getToken();
    this.email = this.authService.getEmail();
    console.log('Token:', this.token);
    console.log('Email: ', this.email);
    if (this.token) {
      const decoded: any = jwtDecode(this.token);
      console.log('Decoded Token:', decoded);
      this.role1 =
        decoded[
          'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
        ] || 'No role found';
      console.log('Role:', this.role1);

      // this.role = decoded.role || 'No role found';
    }
  }

  logout() {
    this.authService.logout();
  }
}
