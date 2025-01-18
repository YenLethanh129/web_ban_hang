import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { DetailProductComponent } from './components/detail-product/detail-product.component';
import { OrderComponent } from './components/order/order.component';
import { OrderConfirmComponent } from './components/order-confirm/order-confirm.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthGuard } from './guards/auth.guard';
import { GuestGuard } from './guards/guest.guard';

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
    canActivate: [GuestGuard],
  },
  { path: 'detail-product/:id', component: DetailProductComponent },
  { path: 'order', component: OrderComponent, canActivate: [AuthGuard] },
  { path: 'order-confirm', component: OrderConfirmComponent },
  { path: 'login', component: LoginComponent, canActivate: [GuestGuard] },
];
