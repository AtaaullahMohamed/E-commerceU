import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { ShopComponent } from './features/shop/shop.component';
import { ProductDetalisComponent } from './features/shop/product-detalis/product-detalis.component';
import { TestErrorComponent } from './features/test-error/test-error.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';
import { ServerErrorComponent } from './shared/components/server-error/server-error.component';
import { CartComponent } from './fearures/cart/cart.component';
import { CheckoutComponent } from './features/checkout/checkout.component';
import { LoginComponent } from './features/account/login/login.component';
import { RegisterComponent } from './features/account/register/register.component';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'shop', component: ShopComponent },
    { path: 'shop/:id', component: ProductDetalisComponent },
    { path: 'cart', component: CartComponent },
    { path: 'checkout', component: CheckoutComponent },
    { path: 'account/login', component: LoginComponent },
    { path: 'account/register', component: RegisterComponent },
    { path: 'test-error', component: TestErrorComponent },
    { path: 'not-found', component: NotFoundComponent },
    { path: 'server-error', component: ServerErrorComponent },
    { path: '**', redirectTo: '', pathMatch: 'full' }

];
