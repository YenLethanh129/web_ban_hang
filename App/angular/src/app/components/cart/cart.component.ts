import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { CartService } from '../../services/cart.service';
import { ProductService } from '../../services/product.service';
import { ProductDTO } from '../../dtos/product.dto';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss',
})
export class CartComponent implements OnInit {
  cartItems: { product: ProductDTO; quantity: number; size: string }[] = [];
  isLoading: boolean = true;
  feeUpSize: number = 5000;

  constructor(
    private cartService: CartService,
    private productService: ProductService,
    private notificationService: NotificationService,
    private router: Router
  ) {}

  // Kiểm tra tổng số lượng sản phẩm, nếu >10 sẽ chặn chuyển trang và thông báo
  confirmOrder(): void {
    const total = this.cartItems.reduce(
      (acc, it) => acc + (it.quantity || 0),
      0
    );
    if (total > 10) {
      this.notificationService.showWarning('Không được đặt quá 10 sản phẩm');
      return;
    }
    // Đủ điều kiện, chuyển sang trang đặt hàng
    this.router.navigate(['/order']);
  }

  ngOnInit(): void {
    this.loadCartItems();
  }

  private async loadCartItems() {
    this.isLoading = true;
    const cart = this.cartService.getAllEntries();

    try {
      const items = await Promise.all(
        Array.from(cart.entries()).map(async ([compositeKey, cartItem]) => {
          // compositeKey is in format "<productId>:<size>"
          const [idStr, size] = String(compositeKey).split(':');
          const productId = Number(idStr);
          const product = await this.productService
            .getProductById(productId)
            .toPromise();
          const resolvedSize = cartItem.size || size || 'S';
          if (!product) {
            return {
              product: undefined as any,
              quantity: cartItem.quantity,
              size: resolvedSize,
            };
          }
          // Do not mutate the product price; calculate adjusted price for display
          const displayPrice = this.cartService.calculatePriceWithSize(
            product.price,
            resolvedSize
          );
          // Create a shallow copy of product for UI so we can show per-size price without side effects
          const productForUi = {
            ...product,
            price: displayPrice,
          } as typeof product;
          return {
            product: productForUi,
            quantity: cartItem.quantity,
            size: resolvedSize,
          };
        })
      );
      this.cartItems = items.filter(
        (
          item
        ): item is { product: ProductDTO; quantity: number; size: string } =>
          item.product !== undefined
      );
    } catch (error) {
      console.error('Lỗi khi tải thông tin giỏ hàng:', error);
      this.notificationService.showError('Không thể tải thông tin giỏ hàng');
    } finally {
      this.isLoading = false;
    }
  }

  // Price-per-size is now calculated by CartService.calculatePriceWithSize

  updateQuantity(productId: number, newQuantity: number, size?: string): void {
    if (newQuantity > 0) {
      const resolvedSize =
        size ||
        this.cartItems.find((i) => i.product.id === productId)?.size ||
        'S';
      // Call the size-aware update in the CartService
      this.cartService.updateQuantity(productId, newQuantity, resolvedSize);
      this.notificationService.showSuccess('🔄 Đã cập nhật số lượng sản phẩm!');
    } else {
      // If size provided, remove only that size. Otherwise remove all sizes for the product.
      if (size) this.removeItemBySize(productId, size);
      else this.removeItem(productId);
    }
    this.loadCartItems();
  }

  removeItem(productId: number): void {
    const item = this.cartItems.find((item) => item.product.id === productId);
    this.cartService.removeFromCart(productId);
    this.notificationService.showSuccess(
      `🗑️ Đã xóa "${item?.product.name || 'sản phẩm'}" khỏi giỏ hàng!`
    );
    this.loadCartItems();
  }

  // Remove a specific size entry
  removeItemBySize(productId: number, size: string): void {
    const item = this.cartItems.find(
      (it) => it.product.id === productId && it.size === size
    );
    this.cartService.removeFromCartBySize(productId, size);
    this.notificationService.showSuccess(
      `🗑️ Đã xóa "${
        item?.product.name || 'sản phẩm'
      }" (size ${size}) khỏi giỏ hàng!`
    );
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
    this.notificationService.showSuccess(
      '🧹 Đã xóa tất cả sản phẩm khỏi giỏ hàng!'
    );
  }
}
