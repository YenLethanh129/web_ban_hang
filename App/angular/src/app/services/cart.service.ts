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
  // key format: `${productId}:${size}` to allow multiple sizes per product
  private cartMap: Map<string, CartItem> = new Map<string, CartItem>();
  private cartItemCount = new BehaviorSubject<number>(0);
  private feeUpSize = 5000;

  constructor(private storageService: StorageService) {
    this.loadCartFromStorage();
  }

  private loadCartFromStorage(): void {
    const savedCart = this.storageService.getItem(this.CART_STORAGE_KEY);
    if (savedCart) {
      try {
        const entries: any[] = JSON.parse(savedCart);
        // entries may be in old format where key was a number
        const normalized = entries.map(([k, v]) => {
          if (typeof k === 'number') {
            const size = v && v.size ? v.size : 'S';
            return [`${k}:${size}`, v];
          }
          return [String(k), v];
        });
        this.cartMap = new Map(normalized as any);
      } catch (e) {
        try {
          this.cartMap = new Map(JSON.parse(savedCart));
        } catch (err) {
          this.cartMap = new Map<string, CartItem>();
        }
      }
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
  addToCart(productId: number, quantity: number = 1, size: string = 'S'): void {
    const key = `${productId}:${size}`;
    const currentItem = this.cartMap.get(key);
    if (currentItem) {
      // Nếu cùng productId và cùng size, tăng số lượng
      currentItem.quantity += quantity;
      this.cartMap.set(key, currentItem);
    } else {
      // Nếu khác size hoặc chưa có, tạo mới entry (không ghi đè các size khác)
      this.cartMap.set(key, { quantity, size });
    }
    this.saveCartToStorage();
  }

  // Calculate adjusted price for a given base price and size.
  // This is a pure helper so UI code can call it without relying on component state.
  calculatePriceWithSize(basePrice: number, size: string): number {
    switch (size) {
      case 'M':
        return basePrice + this.feeUpSize;
      case 'L':
        return basePrice + this.feeUpSize * 2;
      default:
        return basePrice; // 'S' and unknown sizes use base price
    }
  }

  // Cập nhật số lượng và size sản phẩm
  updateQuantity(productId: number, quantity: number, size: string): void {
    const key = `${productId}:${size}`;
    if (quantity > 0) {
      this.cartMap.set(key, { quantity, size });
    } else {
      this.cartMap.delete(key);
    }
    this.saveCartToStorage();
  }

  // Xóa sản phẩm khỏi giỏ hàng
  removeFromCart(productId: number): void {
    // backward-compatible: remove all sizes for this productId
    const prefix = `${productId}:`;
    for (const key of Array.from(this.cartMap.keys())) {
      if (key.startsWith(prefix)) this.cartMap.delete(key);
    }
    this.saveCartToStorage();
  }

  // Remove a specific size entry for a product
  removeFromCartBySize(productId: number, size: string): void {
    const key = `${productId}:${size}`;
    this.cartMap.delete(key);
    this.saveCartToStorage();
  }

  // Lấy thông tin item trong giỏ hàng
  getCartItem(productId: number): CartItem | undefined {
    // return first found item for compatibility
    const prefix = `${productId}:`;
    for (const [key, item] of this.cartMap.entries()) {
      if (key.startsWith(prefix)) return item;
    }
    return undefined;
  }

  // Lấy số lượng của một sản phẩm
  getQuantity(productId: number): number {
    // sum across sizes
    const prefix = `${productId}:`;
    let total = 0;
    for (const [key, item] of this.cartMap.entries()) {
      if (key.startsWith(prefix)) total += item.quantity;
    }
    return total;
  }

  // Lấy size của một sản phẩm
  getSize(productId: number): string {
    // if multiple sizes exist, return the first one (backward compatibility)
    const prefix = `${productId}:`;
    for (const [key, item] of this.cartMap.entries()) {
      if (key.startsWith(prefix)) return item.size;
    }
    return 'S';
  }

  // Lấy toàn bộ giỏ hàng
  getCart(): Map<number, CartItem> {
    // Return a Map keyed by productId -> first found size item (backward-compatible)
    const result = new Map<number, CartItem>();
    for (const [key, item] of this.cartMap.entries()) {
      const [idStr] = key.split(':');
      const id = Number(idStr);
      if (!result.has(id)) result.set(id, item);
    }
    return result;
  }

  // New: return all raw entries keyed by "productId:size" so UI can list each size separately
  getAllEntries(): Map<string, CartItem> {
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
