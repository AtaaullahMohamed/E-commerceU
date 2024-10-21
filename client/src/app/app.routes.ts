import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { ShopComponent } from './features/shop/shop.component';
import { ProductDetalisComponent } from './features/shop/product-detalis/product-detalis.component';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'shop', component: ShopComponent },
    { path: 'shop/:id', component: ProductDetalisComponent },
    { path: '**', redirectTo: '', pathMatch: 'full' }

];
