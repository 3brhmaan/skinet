import { inject, Injectable } from '@angular/core';
import { AccountService } from './account.service';
import { forkJoin, of } from 'rxjs';
import { CartService } from './cart.service';

@Injectable({
  providedIn: 'root',
})
export class InitService {
  private accountService = inject(AccountService);
  private cartService = inject(CartService);

  init() {
    const cartId = localStorage.getItem('cart_id');
    const cart$ = cartId ? this.cartService.getCart(cartId) : of(null);

    return forkJoin({
      user: this.accountService.getUserInfo(),
      cart: cart$,
    });
  }
}
