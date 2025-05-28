import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CartService } from '../../services/cart.service'
import { ProductService } from '../../services/product.service';
import { ProductDTO } from '../../models/product.dto';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss'
})
export class CartComponent implements OnInit {
  cartItems: { product: ProductDTO; quantity: number }[] = [];
  isLoading: boolean = true;

  constructor(
    private cartService: CartService,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
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
      this.cartItems = items.filter((item): item is { product: ProductDTO; quantity: number } => 
        item.product !== undefined
      );
    } catch (error) {
      console.error('Lỗi khi tải thông tin giỏ hàng:', error);
    } finally {
      this.isLoading = false;
    }
  }

  updateQuantity(productId: number, newQuantity: number): void {
    if (newQuantity > 0) {
      this.cartService.updateQuantity(productId, newQuantity);
    } else {
      this.removeItem(productId);
    }
    this.loadCartItems();
  }

  removeItem(productId: number): void {
    this.cartService.removeFromCart(productId);
    this.loadCartItems();
  }

  getTotalPrice(): number {
    return this.cartItems.reduce(
      (total, item) => total + item.product.price * item.quantity,
      0
    );
  }

  clearCart(): void {
    this.cartService.clearCart();
    this.cartItems = [];
  }
} 