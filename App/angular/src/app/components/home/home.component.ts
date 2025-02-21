import { Component, OnInit, HostListener } from '@angular/core';
import { HeaderComponent } from '../header/header.component';
import { FooterComponent } from '../footer/footer.component';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { ProductDTO } from '../../models/product.dto';
import { ProductService } from '../../services/product.service';
import { CommonModule } from '@angular/common';
import { CartService } from '../../services/cart.service'
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    HeaderComponent,
    FooterComponent,
    RouterModule,
    HttpClientModule,
    CommonModule,
  ],
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
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  @HostListener('window:scroll', ['$event'])
  onScroll(): void {
    if (this.isNearBottom() && !this.isLoading && this.hasMoreProducts) {
      this.loadMoreProducts();
    }
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
        this.currentPage--; // Rollback page number on error
        this.isLoading = false;
      },
    });
  }

  addToCart(product: ProductDTO): void {
    this.cartService.addToCart(product.id, 1);
    alert(`Đã thêm 1 ${product.name} vào giỏ hàng`);
  }

  buyNow(product: ProductDTO): void {
    this.cartService.addToCart(product.id, 1);
    this.router.navigate(['/order']);
  }
}
