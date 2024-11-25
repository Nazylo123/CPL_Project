import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Base_Url } from '../../app.config';
import {
  PaginatedResponse,
  ProductViewModel,
} from '../models/product.viewmodel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProductsService {
  constructor(private http: HttpClient) {}

  // getProductsByMun(pageNumber: number = 1, pageSize: number = 6): Observable<PaginatedResponse<ProductViewModel>> {
  //   const params = new HttpParams()
  //     .set('pageNumber', pageNumber.toString())
  //     .set('pageSize', pageSize.toString());

  //   return this.http.get<PaginatedResponse<ProductViewModel>>(`${Base_Url}Product/get-by-mun`, { params });
  // }
  getProductsByMun(
    pageNumber: number = 1,
    pageSize: number = 6,
    minPrice?: number,
    maxPrice?: number,
    searchTerm?: string,
    sizeIds?: number[],
    selectedCategory?: string
  ): Observable<PaginatedResponse<ProductViewModel>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (minPrice !== undefined) {
      params = params.set('minPrice', minPrice.toString());
    }
    if (maxPrice !== undefined) {
      params = params.set('maxPrice', maxPrice.toString());
    }
    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }
    if (sizeIds && sizeIds.length > 0) {
      sizeIds.forEach((sizeId) => {
        params = params.append('sizeIds', sizeId.toString());
      });
    }
    if (selectedCategory) {
      params = params.set('categoryName', selectedCategory);
    }
    return this.http.get<PaginatedResponse<ProductViewModel>>(
      `${Base_Url}Product/get-by-mun`,
      { params }
    );
  }
  getSearchSuggestions(searchTerm: string): Observable<string[]> {
    return this.http.get<string[]>(
      `${Base_Url}Product/search-suggestions?searchTerm=${searchTerm}`
    );
  }

  getAllCategory(): Observable<string[]> {
    return this.http.get<string[]>(`${Base_Url}Product/get-categories`);
  }
}
