import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { DetailProductComponent } from './detail-product/detail-product.component';
import { OrderComponent } from './order/order.component';
import { OrderConfirmComponent } from './order-confirm/order-confirm.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {
    path: 'home',
    component: HomeComponent,
  },
  {
    path: 'register',
    component: RegisterComponent,
    data: { skipLocationChange: false },
  },
  { path: 'detail-product/:id', component: DetailProductComponent },
  { path: 'order', component: OrderComponent },
  { path: 'order-confirm', component: OrderConfirmComponent },
  { path: 'login', component: LoginComponent },
];
