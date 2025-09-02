import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { StorageService } from './storage.service';

interface CartItem {
  quantity: number;
  size: string;
}

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private readonly CART_STORAGE_KEY = 'shopping_cart';
  private cartMap: Map<number, CartItem> = new Map<number, CartItem>();
  private cartItemCount = new BehaviorSubject<number>(0);

  constructor(private storageService: StorageService) {
    this.loadCartFromStorage();
  }

  private loadCartFromStorage(): void {
    const savedCart = this.storageService.getItem(this.CART_STORAGE_KEY);
    if (savedCart) {
      this.cartMap = new Map(JSON.parse(savedCart));
      this.updateCartCount();
    }
  }

  private saveCartToStorage(): void {
    this.storageService.setItem(
      this.CART_STORAGE_KEY,
      JSON.stringify(Array.from(this.cartMap.entries()))
    );
    this.updateCartCount();
  }

  private updateCartCount(): void {
    const count = Array.from(this.cartMap.values()).reduce(
      (a, b) => a + b.quantity,
      0
    );
    this.cartItemCount.next(count);
  }

  // Thêm sản phẩm vào giỏ hàng với size
  addToCart(productId: number, quantity: number = 1, size: string = 'M'): void {
    const currentItem = this.cartMap.get(productId);
    if (currentItem && currentItem.size === size) {
      // Nếu cùng size, tăng số lượng
      currentItem.quantity += quantity;
      this.cartMap.set(productId, currentItem);
    } else {
      // Nếu khác size hoặc chưa có, tạo mới
      this.cartMap.set(productId, { quantity, size });
    }
    this.saveCartToStorage();
  }

  // Cập nhật số lượng và size sản phẩm
  updateQuantity(productId: number, quantity: number, size: string): void {
    if (quantity > 0) {
      this.cartMap.set(productId, { quantity, size });
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

  // Lấy thông tin item trong giỏ hàng
  getCartItem(productId: number): CartItem | undefined {
    return this.cartMap.get(productId);
  }

  // Lấy số lượng của một sản phẩm
  getQuantity(productId: number): number {
    const item = this.cartMap.get(productId);
    return item ? item.quantity : 0;
  }

  // Lấy size của một sản phẩm
  getSize(productId: number): string {
    const item = this.cartMap.get(productId);
    return item ? item.size : 'M';
  }

  // Lấy toàn bộ giỏ hàng
  getCart(): Map<number, CartItem> {
    return new Map(this.cartMap);
  }

  getCartItemCount(): Observable<number> {
    return this.cartItemCount.asObservable();
  }

  clearCart(): void {
    this.cartMap.clear();
    this.saveCartToStorage();
  }
}
