import { inject, Injectable } from '@angular/core';
import { CartService } from './cart.service';
import { forkJoin, of } from 'rxjs';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class InitService {
  private cartServices = inject(CartService);
  private accountService = inject(AccountService);

  Init() {
    const cartId = localStorage.getItem('cart_id');
    const cart$ = cartId ? this.cartServices.getCart(cartId) : of(null)

    return forkJoin({
      cart: cart$,
      user: this.accountService.getUserInfo()
    });
  }

}
