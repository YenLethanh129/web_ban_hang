import { Component, OnInit, HostListener } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { ProductDTO } from '../../models/product.dto';
import { ProductService } from '../../services/product.service';
import { CommonModule } from '@angular/common';
import { CartService } from '../../services/cart.service';
import { Router } from '@angular/router';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterModule, HttpClientModule, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  products: ProductDTO[] = [];
  currentPage: number = 1;
  isLoading: boolean = false;
  hasMoreProducts: boolean = true;
  private readonly limit: number = 20;

  constructor(
    private productService: ProductService,
    private cartService: CartService,
    private router: Router,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  @HostListener('window:scroll', ['$event'])
  onScroll(): void {
    // if (this.isNearBottom() && !this.isLoading && this.hasMoreProducts) {
    //   this.loadMoreProducts();
    // }
  }

  private isNearBottom(): boolean {
    const threshold = 100;
    const position = window.scrollY + window.innerHeight;
    const height = document.documentElement.scrollHeight;
    return position > height - threshold;
  }

  private loadProducts(): void {
    this.isLoading = true;
    this.productService.getProducts(this.currentPage, this.limit).subscribe({
      next: (response) => {
        this.products = response.products;
        this.hasMoreProducts = response.products.length === this.limit;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Lỗi khi lấy danh sách sản phẩm:', error);
        this.notificationService.showHttpError(
          error,
          'Không thể tải danh sách sản phẩm'
        );
        this.isLoading = false;
      },
    });
  }

  private loadMoreProducts(): void {
    if (this.isLoading) return;

    this.isLoading = true;
    this.currentPage++;

    this.productService.getProducts(this.currentPage, this.limit).subscribe({
      next: (response) => {
        this.products = [...this.products, ...response.products];
        this.hasMoreProducts = response.products.length === this.limit;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Lỗi khi tải thêm sản phẩm:', error);
        this.notificationService.showHttpError(
          error,
          'Không thể tải thêm sản phẩm'
        );
        this.currentPage--; // Rollback page number on error
        this.isLoading = false;
      },
    });
  }

  addToCart(product: ProductDTO): void {
    this.cartService.addToCart(product.id, 1);
    this.notificationService.showSuccess(
      `🛒 Đã thêm "${product.name}" vào giỏ hàng!`
    );
  }

  buyNow(product: ProductDTO): void {
    this.cartService.addToCart(product.id, 1);
    this.notificationService.showSuccess(
      `🛍️ Đã thêm "${product.name}" vào giỏ hàng!`
    );
    this.router.navigate(['/order']);
  }
}
