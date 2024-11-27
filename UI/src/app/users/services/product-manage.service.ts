import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ProductManage {
  id: number;
  name: string;
  description: string;
  price: number;
  categoryId: number;
  categoryName: string;
  createdAt: string;
  updatedAt: string;
  sizeId: number[];
  sizeName: string[];
  quantity: number[];
  url: string;
}

@Injectable({
  providedIn: 'root',
})
export class ProductManageService {
  private apiUrl = 'https://localhost:7102/api/Product';

  constructor(private http: HttpClient) {}

  getProducts(): Observable<ProductManage[]> {
    return this.http.get<ProductManage[]>(this.apiUrl);
  }

  getProductById(id: number): Observable<ProductManage> {
    return this.http.get<ProductManage>(`${this.apiUrl}/${id}`);
  }
  addProductWithImage(formData: FormData): Observable<any> {
    return this.http.post(`${this.apiUrl}/upload`, formData);
  }
  addProduct(product: ProductManage, categoryId: number): Observable<any> {
    const url = `${this.apiUrl}/${categoryId}`;
    return this.http.post<any>(url, product);
  }

  updateProduct(id: number, product: ProductManage): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, product);
  }

  deleteProduct(id: number): Observable<string> {
    return this.http.delete(`${this.apiUrl}/${id}`, { responseType: 'text' });
  }
}
