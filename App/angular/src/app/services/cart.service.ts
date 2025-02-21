import { Injectable, PLATFORM_ID, Inject } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private readonly CART_STORAGE_KEY = 'shopping_cart';
  private cartMap: Map<number, number> = new Map<number, number>();
  private cartItemCount = new BehaviorSubject<number>(0);
  private isBrowser: boolean;

  constructor(@Inject(PLATFORM_ID) platformId: Object) {
    this.isBrowser = isPlatformBrowser(platformId);
    if (this.isBrowser) {
      this.loadCartFromStorage();
    }
  }

  // Lấy cart từ localStorage khi khởi tạo service
  private loadCartFromStorage(): void {
    if (this.isBrowser) {
      const savedCart = localStorage.getItem(this.CART_STORAGE_KEY);
      if (savedCart) {
        this.cartMap = new Map(JSON.parse(savedCart));
        this.updateCartCount();
      }
    }
  }

  // Lưu cart vào localStorage
  private saveCartToStorage(): void {
    if (this.isBrowser) {
      localStorage.setItem(
        this.CART_STORAGE_KEY,
        JSON.stringify(Array.from(this.cartMap.entries()))
      );
      this.updateCartCount();
    }
  }

  // Cập nhật số lượng item trong cart
  private updateCartCount(): void {
    const count = Array.from(this.cartMap.values()).reduce((a, b) => a + b, 0);
    this.cartItemCount.next(count);
  }

  // Thêm sản phẩm vào giỏ hàng
  addToCart(productId: number, quantity: number = 1): void {
    const currentQuantity = this.cartMap.get(productId) || 0;
    this.cartMap.set(productId, currentQuantity + quantity);
    this.saveCartToStorage();
  }

  // Cập nhật số lượng sản phẩm
  updateQuantity(productId: number, quantity: number): void {
    if (quantity > 0) {
      this.cartMap.set(productId, quantity);
    } else {
      this.cartMap.delete(productId);
    }
    this.saveCartToStorage();
  }

  // Xóa sản phẩm khỏi giỏ hàng
  removeFromCart(productId: number): void {
    this.cartMap.delete(productId);
    this.saveCartToStorage();
  }

  // Lấy số lượng của một sản phẩm
  getQuantity(productId: number): number {
    return this.cartMap.get(productId) || 0;
  }

  // Lấy toàn bộ giỏ hàng
  getCart(): Map<number, number> {
    return new Map(this.cartMap);
  }

  // Observable để theo dõi số lượng item trong giỏ hàng
  getCartItemCount(): Observable<number> {
    return this.cartItemCount.asObservable();
  }

  // Xóa toàn bộ giỏ hàng
  clearCart(): void {
    this.cartMap.clear();
    this.saveCartToStorage();
  }
}
