import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestcartComponent } from './testcart.component';

describe('TestcartComponent', () => {
  let component: TestcartComponent;
  let fixture: ComponentFixture<TestcartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TestcartComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TestcartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
