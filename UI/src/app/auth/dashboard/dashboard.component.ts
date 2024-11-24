import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { CommonModule } from '@angular/common';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  role: string = '';
  token: string | null = '';

  constructor(private authService: AuthService) {}

  ngOnInit() {
    this.token = this.authService.getToken();
    console.log('Token:', this.token);

    if (this.token) {
      const decoded: any = jwtDecode(this.token);
      console.log('Decoded Token:', decoded);
      this.role = decoded.role || 'No role found';
    }
  }

  logout() {
    this.authService.logout();
  }
}
