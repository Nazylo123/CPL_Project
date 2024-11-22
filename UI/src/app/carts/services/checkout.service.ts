import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { MomoRequest } from '../models/momo.request';
import { MomoCreatePaymentResponse } from '../models/momo-create-payment-response';
import { Base_Url } from '../../app.config';

@Injectable({
  providedIn: 'root',
})
export class CheckoutService {
  constructor(private http: HttpClient) {}

  createMomoPayment(model: MomoRequest): Observable<MomoCreatePaymentResponse> {
    return this.http.post<MomoCreatePaymentResponse>(
      `${Base_Url}Momo/create-payment`,
      model
    );
  }

  getResultmomo(queryString: string): Observable<MomoRequest> {
    // Dùng template literal để truyền đúng giá trị cho URL
    return this.http.get<MomoRequest>(`${Base_Url}Momo/payment-result?${queryString}`);
  }
}