import { Component, OnInit, NgModule } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ProductDTO } from '../../models/product.dto';
import { CartService } from '../../services/cart.service';
import { ProductService } from '../../services/product.service';
import { TokenService } from '../../services/token.service';
import { UserService } from '../../services/user.service';
import { FormsModule } from '@angular/forms';
import { UserDTO } from '../../dtos/user.dto';

@Component({
  selector: 'app-order',
  standalone: true,
  imports: [RouterModule, CommonModule, FormsModule],
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss'],
})
export class OrderComponent implements OnInit {
  cartItems: { product: ProductDTO; quantity: number }[] = [];
  isLoading: boolean = false;
  user: UserDTO | null = null;
  orderData = {
    userId: 0, // This should be set to the actual user ID
    fullName: '',
    email: '',
    phoneNumber: '',
    address: '',
    note: '',
    totalMoney: null as number | null,
    shippingMethod: '',
    shippingAddress: '',
    paymentMethod: 'Credit Card',
  };

  constructor(
    private cartService: CartService,
    private productService: ProductService,
    private tokenService: TokenService,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    if (!this.tokenService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }
    this.loadCartItems();
    this.loadUserData();
    this.orderData.shippingMethod = 'standard'; // Default shipping method
    this.orderData.paymentMethod = 'bank-transfer'; // Default payment method
  }

  private loadUserData() {
    this.userService.getUser().subscribe({
      next: (user) => {
        this.user = user;
        console.log('Thông tin người dùng:', this.user);
        if (this.user) {
          this.orderData.userId = this.user.id;
          this.orderData.fullName = this.user.fullname;
          this.orderData.phoneNumber = this.user.phone_number;
          this.orderData.shippingAddress = this.user.address;
          this.orderData.address = this.user.address;
        }
      },
      error: (error) => {
        console.error('Lỗi khi tải thông tin người dùng:', error);
      },
    });
  }

  private async loadCartItems() {
    this.isLoading = true;
    const cart = this.cartService.getCart();

    try {
      const items = await Promise.all(
        Array.from(cart.entries()).map(async ([productId, quantity]) => {
          const product = await this.productService
            .getProductById(productId)
            .toPromise();
          return { product, quantity };
        })
      );
      this.cartItems = items.filter(
        (item): item is { product: ProductDTO; quantity: number } =>
          item.product !== undefined
      );
    } catch (error) {
      console.error('Lỗi khi tải thông tin giỏ hàng:', error);
    } finally {
      this.isLoading = false;
    }
  }

  getTotalPrice(): number {
    return this.cartItems.reduce(
      (total, item) => total + item.product.price * item.quantity,
      0
    );
  }

  createOrder(): void {}
}
