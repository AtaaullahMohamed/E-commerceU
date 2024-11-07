import { inject, Injectable } from '@angular/core';
import { CartService } from './cart.service';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InitService {
  private cartServices = inject(CartService);

  Init() {
    const cartId = localStorage.getItem('cart_id');
    const cart$ = cartId ? this.cartServices.getCart(cartId) : of(null)

    return cart$;
  }

}
