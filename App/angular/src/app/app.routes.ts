import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component'; 
import { DetailProductComponent } from './components/detail-product/detail-product.component';
import { OrderComponent } from './components/order/order.component';
import { OrderConfirmComponent } from './components/order-confirm/order-confirm.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthGuard } from './guards/auth.guard';
import { GuestGuard } from './guards/guest.guard';
import { CartComponent } from './components/cart/cart.component';
import { CategoryComponent } from './components/category/category.component';
import { InfoOrderComponent } from './info-order/info-order.component';
import { NotificationTestComponent } from './components/notification-test/notification-test.component';
import { SearchResultsComponent } from './components/search-results/search-results.component';

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
  // { path: 'order', component: OrderComponent},
  { path: 'order-confirm/:id', component: OrderConfirmComponent },
  { path: 'login', component: LoginComponent, canActivate: [GuestGuard] },
  { path: 'product/:id', component: DetailProductComponent },
  { path: 'cart', component: CartComponent },
  { path: 'category/:id', component: CategoryComponent },
  { path: 'search', component: SearchResultsComponent },
  {
    path: 'info-order',
    component: InfoOrderComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'notification-test',
    component: NotificationTestComponent,
  },
  { path: '**', redirectTo: 'home' },
];
