import { TestBed } from '@angular/core/testing';

import { ProductManageService } from './product-manage.service';

describe('ProductManageService', () => {
  let service: ProductManageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProductManageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
