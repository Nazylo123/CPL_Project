import { Component, OnInit } from '@angular/core';
import { OrderService } from '../Services/order.service'; // Thêm service để gọi API
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-order',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './order.component.html',
  styleUrl: './order.component.css'
})
export class OrderComponent implements OnInit {
  orders: any[] = []; // Mảng chứa danh sách đơn hàng
  //editingOrder: any = null;
  editingOrder: { id: number; status: string } | null = null;

  constructor(private orderService: OrderService, private router: Router) { }

  ngOnInit(): void {
    this.loadOrders();
  }

  // Lấy danh sách đơn hàng từ API
  loadOrders(): void {
    this.orderService.getOrders().subscribe((data) => {
      this.orders = data; // Gán dữ liệu đơn hàng vào mảng
    });
  }

  toggleEdit(order: any): void {
    this.editingOrder = { ...order }; // Tạo một bản sao của đơn hàng
  }
  saveOrderStatus(id: number): void {
    if (this.editingOrder) {
      this.orderService.updateOrder(id, { status: this.editingOrder.status }).subscribe(() => {
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
      this.orderService.deleteOrder(id).subscribe(() => {
        this.loadOrders(); // Tải lại danh sách sau khi xóa
      });
    }
  }
  editOrder(order: any): void {
    this.editingOrder = { id: order.id, status: order.status }; // Lấy cả id và status
  }

}
