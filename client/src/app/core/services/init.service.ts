import { inject, Injectable } from '@angular/core';
import { CartService } from './cart.service';
import { forkJoin, of, tap } from 'rxjs';
import { AccountService } from './account.service';
import { SignalrService } from './signalr.service';

@Injectable({
  providedIn: 'root'
})
export class InitService {
  private cartServices = inject(CartService);
  private accountService = inject(AccountService);
  private signalrService = inject(SignalrService)

  Init() {
    const cartId = localStorage.getItem('cart_id');
    const cart$ = cartId ? this.cartServices.getCart(cartId) : of(null)

    return forkJoin({
      cart: cart$,
      user: this.accountService.getUserInfo().pipe(
        tap(user => {
          if (user) this.signalrService.createHubConnection();
        })
      )
    });
  }

}
