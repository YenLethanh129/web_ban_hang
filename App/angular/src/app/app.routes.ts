import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { DetailProductComponent } from './components/detail-product/detail-product.component';
import { OrderComponent } from './components/order/order.component';
import { OrderConfirmComponent } from './components/order-confirm/order-confirm.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { VerifyOtpComponent } from './components/verify-otp/verify-otp.component';
import { AuthGuard } from './guards/auth.guard';
import { GuestGuard } from './guards/guest.guard';
import { CartComponent } from './components/cart/cart.component';
import { CategoryComponent } from './components/category/category.component';
import { InfoOrderComponent } from './components/info-order/info-order.component';
import { SearchResultsComponent } from './components/search-results/search-results.component';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { ContractInfoComponent } from './components/contract-info/contract-info.component';

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
  {
    path: 'order-confirm/:id',
    component: OrderConfirmComponent,
    canActivate: [AuthGuard],
  },
  { path: 'login', component: LoginComponent, canActivate: [GuestGuard] },
  {
    path: 'forgot-password',
    component: ForgotPasswordComponent,
    canActivate: [GuestGuard],
  },
  {
    path: 'verify-otp',
    component: VerifyOtpComponent,
    canActivate: [GuestGuard],
  },
  { path: 'product/:id', component: DetailProductComponent },
  { path: 'cart', component: CartComponent, canActivate: [AuthGuard] },
  { path: 'category/:id', component: CategoryComponent },
  { path: 'search', component: SearchResultsComponent },
  {
    path: 'info-order',
    component: InfoOrderComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'user-profile',
    component: UserProfileComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'profile',
    redirectTo: 'user-profile',
    pathMatch: 'full',
  },
  {
    path: 'change-password',
    component: ChangePasswordComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'contract-info',
    component: ContractInfoComponent,
  },
  { path: '**', redirectTo: 'home' },
];
