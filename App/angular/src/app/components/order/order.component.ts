import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ProductDTO } from '../../models/product.dto';
import { CartService } from '../../services/cart.service';
import { ProductService } from '../../services/product.service';
import { TokenService } from '../../services/token.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-order',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './order.component.html',
  styleUrl: './order.component.scss',
})
export class OrderComponent implements OnInit {
  cartItems: { product: ProductDTO; quantity: number }[] = [];
  isLoading: boolean = false;
  orderData = {
    user_id: null, // This should be set to the actual user ID
    full_name: '',
    email: '',
    phone_number: '',
    address: '',
    note: '',
    total_money: null as number | null,
    shipping_method: '',
    shipping_address: '',
    payment_method: 'Credit Card',
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
