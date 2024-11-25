export interface ProductViewModel {
  id: number;
  name: string;
  description: string;
  price: number;
  categoryId: number;
  categoryName: string;
  createdAt: string; // ISO string format
  updatedAt: string; // ISO string format
  sizeId: number[];
  sizeName: string[];
  quantity: number[];
  url: string; // Có thể null nếu không có hình ảnh
}

export interface PaginatedResponse<T> {
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  data: T[];
}
