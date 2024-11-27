import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.css',
})
export class FooterComponent implements OnInit {
  isAdmin: boolean = false;
  constructor(private authService: AuthService) {}
  ngOnInit(): void {
    // Kiểm tra xem người dùng có phải là admin không
    this.isAdmin = this.authService.isAdmin();
  }
}
