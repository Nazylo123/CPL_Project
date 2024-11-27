import { response } from 'express';
import { Component, OnInit } from '@angular/core';
import { OrderService } from '../Services/order.service'; // Thêm service để gọi API
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { error } from 'console';
import { Order } from '../Models/order';
@Component({
  selector: 'app-order',
  standalone: true,
  imports: [CommonModule, FormsModule, NgxPaginationModule],
  templateUrl: './order.component.html',
  styleUrl: './order.component.css',
})
export class OrderComponent implements OnInit {
  orders: Order[] = []; // Mảng chứa danh sách đơn hàng
  //editingOrder: any = null;
  editingOrder: { id: number; status: string } | null = null;
  currentPage: number = 1; // Trang hiện tại

  constructor(private orderService: OrderService, private router: Router) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  // Lấy danh sách đơn hàng từ API
  loadOrders(): void {
    this.orderService.getOrders().subscribe(
      (orders) => {
        this.orders = orders; // Cập nhật lại danh sách orders
      },
      (error) => {
        console.log('Error loading orders:', error);
      }
    );
  }

  toggleEdit(order: any): void {
    this.editingOrder = { ...order }; // Tạo một bản sao của đơn hàng
  }
  saveOrderStatus(id: number): void {
    if (this.editingOrder) {
      this.orderService
        .updateOrder(id, { status: this.editingOrder.status })
        .subscribe(() => {
          this.loadOrders(); // Tải lại danh sách đơn hàng sau khi cập nhật
          this.editingOrder = null; // Đóng form chỉnh sửa
        });
    }
  }
  cancelEdit(): void {
    this.editingOrder = null;
  }
  // Xóa đơn hàng
  deleteOrder(id: number): void {
    if (confirm('Are you sure you want to delete this order?')) {
      this.orderService.deleteOrder(id).subscribe(
        (response) => {
          this.loadOrders(); // Tải lại danh sách sau khi xóa
          console.log(response);
        },
        (error) => {
          console.log(error);
        }
      );
    }
  }
  editOrder(order: any): void {
    this.editingOrder = { id: order.id, status: order.status }; // Lấy cả id và status
  }
}
