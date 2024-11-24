import { Component, OnInit } from '@angular/core';
import {
  PaginatedResponse,
  ProductViewModel,
} from '../models/product.viewmodel';
import { ProductsService } from '../services/products.service';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { response } from 'express';
import { error } from 'console';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.css',
})
export class ShopComponent implements OnInit {
  products: ProductViewModel[] = [];
  totalCount: number = 0;
  pageNumber: number = 1;
  pageSize: number = 6;
  totalPages: number = 0;
  pages: number[] = []; // Các trang hiển thị

  minPrice: number = 0; // Giá trị tối thiểu
  maxPrice: number = 5000000; // Giá trị tối đa
  searchTerm: string = '';
  sizeIds: number[] = [];

  categories: string[] = []; // Danh sách thể loại
  selectedCategory: string = ''; // Thể loại người dùng chọn
  constructor(
    private productService: ProductsService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.searchTerm = params['search'];
      this.getAllCategory();
      this.loadProducts();
    });
  }

  onProductClick(product: any): void {
    // Điều hướng đến trang chi tiết và truyền dữ liệu qua state
    this.router.navigate(['/shop-single'], { state: { product } });
  }

  loadProducts(): void {
    this.productService
      .getProductsByMun(
        this.pageNumber,
        this.pageSize,
        this.minPrice, // Giá trị giá tối thiểu
        this.maxPrice, // Giá trị giá tối đa
        this.searchTerm, // Từ khóa tìm kiếm
        this.sizeIds, // Danh sách sizeIds để lọc
        this.selectedCategory
      )
      .subscribe(
        (response: PaginatedResponse<ProductViewModel>) => {
          this.products = response.data; // Dữ liệu sản phẩm
          this.totalCount = response.totalCount; // Tổng số sản phẩm
          this.pageNumber = response.pageNumber; // Trang hiện tại
          this.pageSize = response.pageSize; // Số sản phẩm mỗi trang
          this.totalPages = response.totalPages; // Tổng số trang
          this.updatePagination(); // Cập nhật trạng thái phân trang
          console.log('products:', this.products);
        },
        (error) => {
          console.error('Error loading products', error);
        }
      );
  }

  // Cập nhật danh sách các trang hiển thị
  updatePagination(): void {
    const maxPagesToShow = 5;
    const halfRange = Math.floor(maxPagesToShow / 2);
    let startPage = Math.max(this.pageNumber - halfRange, 1);
    let endPage = Math.min(startPage + maxPagesToShow - 1, this.totalPages);

    if (endPage - startPage < maxPagesToShow - 1) {
      startPage = Math.max(endPage - maxPagesToShow + 1, 1);
    }

    this.pages = Array.from(
      { length: endPage - startPage + 1 },
      (_, i) => startPage + i
    );
  }

  // Hàm xử lý khi chuyển trang
  onPageChange(newPage: number): void {
    if (newPage >= 1 && newPage <= this.totalPages) {
      this.pageNumber = newPage;
      this.loadProducts();
    }
  }

  // Chuyển đến trang đầu tiên
  goToFirstPage(): void {
    if (this.pageNumber !== 1) {
      this.pageNumber = 1;
      this.loadProducts();
    }
  }

  // Chuyển đến trang cuối cùng
  goToLastPage(): void {
    if (this.pageNumber !== this.totalPages) {
      this.pageNumber = this.totalPages;
      this.loadProducts();
    }
  }

  // Cập nhật giá trị slider và gọi API
  onPriceChange(): void {
    if (this.minPrice > this.maxPrice) {
      [this.minPrice, this.maxPrice] = [this.maxPrice, this.minPrice]; // Hoán đổi nếu min > max
    }
    this.pageNumber = 1; // Reset lại trang
    this.loadProducts();
  }

  // Phương thức lấy danh sách thể loại
  getAllCategory(): void {
    this.productService.getAllCategory().subscribe(
      (response) => {
        this.categories = response; // Gán dữ liệu trả về vào biến categories
      },
      (error) => {
        console.error('Error loading categories', error); // Log lỗi nếu có
      }
    );
  }

  onCategorySelect(categorie: string): void {
    // Lọc sản phẩm theo thể loại được chọn
    console.log('Selected Category:', categorie);
    this.selectedCategory = categorie;
    this.loadProducts(); // Gọi lại API để lọc sản phẩm theo thể loại
  }
}
