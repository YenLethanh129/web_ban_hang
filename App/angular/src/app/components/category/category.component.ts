import { Component, OnInit, HostListener } from '@angular/core';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { ProductDTO } from '../../models/product.dto';
import { ProductService } from '../../services/product.service';
import { CommonModule } from '@angular/common';
import { CartService } from '../../services/cart.service';
import { Router } from '@angular/router';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-category',
  imports: [RouterModule, HttpClientModule, CommonModule],
  templateUrl: './category.component.html',
  styleUrl: './category.component.scss',
})
export class CategoryComponent implements OnInit {
  products: ProductDTO[] = [];
  categoryName: string = '';
  categoryId: number = 0;
  currentPage: number = 1;
  isLoading: boolean = false;
  hasMoreProducts: boolean = true;
  private readonly limit: number = 20;

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private cartService: CartService,
    private router: Router,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      const categoryId = +params['id'];
      this.categoryId = categoryId;
      this.loadProducts(categoryId);
      this.loadCategoryName(categoryId);
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

  private loadCategoryName(id: number): void {
    // You can add a service call here to get category name
    // For now, let's set a default name based on ID
    const categoryNames: { [key: number]: string } = {
      1: 'Cà Phê',
      2: 'Trà',
      3: 'Bánh & Đồ Ăn Nhẹ',
      4: 'Thức Uống Đặc Biệt',
    };
    this.categoryName = categoryNames[id] || 'Danh Mục Sản Phẩm';
  }

  addToCart(product: ProductDTO): void {
    this.cartService.addToCart(product.id, 1);
    this.notificationService.showSuccess(
      `Đã thêm 1 ${product.name} vào giỏ hàng`
    );
  }

  buyNow(product: ProductDTO): void {
    this.cartService.addToCart(product.id, 1);
    this.router.navigate(['/order']);
  }
}
