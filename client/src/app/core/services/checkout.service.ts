import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { DeliveryMethod } from '../../shared/models/deliveryMethods';
import { map, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CheckoutService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  deliveryMethods: DeliveryMethod[] = [];

  getDeliveryMethods() {
    if (this.deliveryMethods.length > 0) of(this.deliveryMethods);

    return this.http
      .get<DeliveryMethod[]>(this.baseUrl + 'payments/delivery-methods')
      .pipe(
        map((methods) => {
          this.deliveryMethods = methods.sort((a, b) => b.price - a.price);

          return methods;
        })
      );
  }
}
