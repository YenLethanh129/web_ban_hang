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
import { LoginGuard } from './guards/login.guard';
import { CartComponent } from './components/cart/cart.component';
import { CategoryComponent } from './components/category/category.component';
import { InfoOrderComponent } from './components/info-order/info-order.component';
import { SearchResultsComponent } from './components/search-results/search-results.component';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';

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
    canActivate: [LoginGuard],
  },
  { path: 'detail-product/:id', component: DetailProductComponent },
  { path: 'order', component: OrderComponent, canActivate: [AuthGuard] },
  // { path: 'order', component: OrderComponent},
  { path: 'order-confirm/:id', component: OrderConfirmComponent },
  { path: 'login', component: LoginComponent, canActivate: [LoginGuard] },
  {
    path: 'forgot-password',
    component: ForgotPasswordComponent,
    canActivate: [LoginGuard],
  },
  {
    path: 'verify-otp',
    component: VerifyOtpComponent,
    canActivate: [LoginGuard],
  },
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
  { path: '**', redirectTo: 'home' },
];
