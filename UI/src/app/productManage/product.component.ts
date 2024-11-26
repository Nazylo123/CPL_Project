import { Component, OnInit } from '@angular/core';
import { ProductManageService } from '../users/services/product-manage.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProductManage } from '../users/services/product-manage.service';
@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
})
export class ProductManageComponent implements OnInit {
  products: ProductManage[] = [];
  displayedProducts: ProductManage[] = [];
  productForm!: FormGroup;
  isEdit = false;
  currentProductId: number = 0;

  currentPage: number = 1;
  totalPages: number = 1;
  pageSize: number = 4;
  constructor(private productManageService: ProductManageService, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();
    this.loadProducts();
  }

  initializeForm(): void {
    this.productForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      description: ['', [Validators.required, Validators.maxLength(200)]],
      price: [0, [Validators.required, Validators.min(1)]],
      categoryId: [0, [Validators.required]],
      sizeId: ['', [Validators.required]],
      sizeName: ['', [Validators.required]],
      quantity: ['', [Validators.required]],
      url: ['', [Validators.required, Validators.pattern('https?://.+')]],
    });
  }

  loadProducts(): void {
    this.productManageService.getProducts().subscribe((data) => {
      this.products = data;
      this.totalPages = Math.ceil(this.products.length / this.pageSize);
      this.updateDisplayedProducts();
    });
  }

  updateDisplayedProducts(): void {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.displayedProducts = this.products.slice(startIndex, endIndex);
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updateDisplayedProducts();
    }
  }

  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updateDisplayedProducts();
    }
  }

  checkFormValidity(): boolean {
    const { sizeId, quantity, url } = this.productForm.value;
    if (!sizeId || !quantity || !url) {
      alert('Please fill all required fields correctly.');
      return false;
    }
    return true;
  }

  addProduct(): void {
    if (!this.checkFormValidity()) return;

    const transformedProduct = this.transformFormValues(this.productForm.value);
    const categoryId = this.productForm.value.categoryId;

    this.productManageService.addProduct(transformedProduct, categoryId).subscribe({
      next: () => {
        alert('Product added successfully!');
        this.loadProducts();
        this.resetForm();
      },
      error: (err) => {
        console.error('Add failed', err);
        const errorMessage = err.error?.message || 'An unexpected error occurred';
        alert(`Failed to add product: ${errorMessage}`);
      },
    });
  }


  editProduct(): void {
    if (!this.checkFormValidity()) return;

    const transformedProduct = this.transformFormValues(this.productForm.value);

    this.productManageService.updateProduct(this.currentProductId, transformedProduct).subscribe({
      next: () => {
        this.loadProducts();
        this.resetForm();
      },
      error: (err) => {
        console.error('Update failed', err);
        alert('Failed to update product. Check the console for details.');
      },
    });
  }

  prepareEditProduct(product: ProductManage): void {
    this.isEdit = true;
    this.currentProductId = product.id;

    const formValue = {
      ...product,
      sizeId: product.sizeId.join(', '),
      sizeName: product.sizeName.join(', '),
      quantity: product.quantity.join(', '),
    };

    this.productForm.patchValue(formValue);
  }

  deleteProduct(id: number): void {
    if (confirm('Are you sure you want to delete this product?')) {
      this.productManageService.deleteProduct(id).subscribe({
        next: (response) => {
          console.log(response);
          this.loadProducts();
          alert(response);
        },
        error: (err) => {
          console.error('Delete failed:', err);
          alert('Failed to delete product. Check the console for details.');
        },
      });
    }
  }

  resetForm(): void {
    this.isEdit = false;
    this.currentProductId = 0;
    this.productForm.reset({
      name: '',
      description: '',
      price: 0,
      categoryId: 0,
      sizeId: '',
      sizeName: '',
      quantity: '',
      url: '',
    });
  }

  transformFormValues(formValues: any): ProductManage {
    const sizeQuantities = formValues.sizeId.split(',').map((id: string, index: number) => {
      return {
        SizeId: parseInt(id.trim(), 10),
        Quantity: parseInt(formValues.quantity.split(',')[index].trim(), 10)
      };
    });

    return {
      ...formValues,
      SizeQuantities: sizeQuantities,
      ImageUrls: formValues.url.split(',').map((url: string) => url.trim()),
    };
  }
}
