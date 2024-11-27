import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private apiUrl = 'https://localhost:7102/api/Order'; // Địa chỉ API backend
  private deleteUrl = 'https://localhost:7102/api/Order';
  constructor(private http: HttpClient) {}

  // Lấy danh sách đơn hàng
  getOrders(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}`);
  }
  updateOrder(id: number, data: any): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, data);
  }

  // Xóa đơn hàng
  deleteOrder(id: number): Observable<void> {
    return this.http.delete<void>(`${this.deleteUrl}/${id}`);
  }
}
