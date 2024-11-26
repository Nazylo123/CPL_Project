import { Component, OnInit } from '@angular/core';
import { UserService } from '../user.service';
import { User } from '../Model/user.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgxPaginationModule } from 'ngx-pagination';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [FormsModule, CommonModule, NgxPaginationModule],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css',
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  errorMessage: string = '';

  currentPage: number = 1; // Trang hiện tại
  constructor(private userService: UserService) {}
  ngOnInit(): void {
    this.loadUsers();
  }
  loadUsers(): void {
    this.userService.getAllUsers().subscribe({
      next: (data) => {
        this.users = data;
      },
      error: (err) => {
        this.errorMessage = 'Lỗi khi tải danh sách người dùng';
        console.error(err);
      },
    });
  }
}
