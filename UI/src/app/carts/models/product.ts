export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  categoryId: number;
  category: Category;
  createdAt: Date;
  updatedAt: Date;
  productSizeStocks: ProductSizeStock[];
  productImages: ProductImage[];
}

export interface Category {
  id: number;
  name: string;
}

export interface ProductSizeStock {
  sizeId: number;
  size: string;
  stock: number;
}

export interface ProductImage {
  id: number;
  url: string;
  productId: number;
}
