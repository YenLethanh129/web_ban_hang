import { Component, OnInit, HostListener } from '@angular/core';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { ProductDTO } from '../../models/product.dto';
import { ProductService } from '../../services/product.service';
import { CommonModule } from '@angular/common';
import { CartService } from '../../services/cart.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-category',
  imports: [
    RouterModule,
    HttpClientModule,
    CommonModule
],
  templateUrl: './category.component.html',
  styleUrl: './category.component.scss',
})
export class CategoryComponent implements OnInit {
  products: ProductDTO[] = [];
  currentPage: number = 1;
  isLoading: boolean = false;
  hasMoreProducts: boolean = true;
  private readonly limit: number = 20;

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private cartService: CartService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      const productId = +params['id'];
      this.loadProducts(productId);
    });
  }

  @HostListener('window:scroll', ['$event'])
  onScroll(): void {
    // if (this.isNearBottom() && !this.isLoading && this.hasMoreProducts) {
    //   this.loadMoreProducts();
    // }
  }

  private loadProducts(id: number): void {
    this.isLoading = true;
    this.productService
      .getProductsByCategoryId(id, this.currentPage, this.limit)
      .subscribe({
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

  addToCart(product: ProductDTO): void {
    this.cartService.addToCart(product.id, 1);
    alert(`Đã thêm 1 ${product.name} vào giỏ hàng`);
  }

  buyNow(product: ProductDTO): void {
    this.cartService.addToCart(product.id, 1);
    this.router.navigate(['/order']);
  }
}
