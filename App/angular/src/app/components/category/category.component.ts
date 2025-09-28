import { Component, OnInit, HostListener } from '@angular/core';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { ProductDTO } from '../../dtos/product.dto';
import { ProductService } from '../../services/product.service';
import { CommonModule } from '@angular/common';
import { CartService } from '../../services/cart.service';
import { Router } from '@angular/router';
import { NotificationService } from '../../services/notification.service';
import { CategoryService } from '../../services/category.service';
import { PaginationComponent } from '../shared/pagination/pagination.component';

@Component({
  selector: 'app-category',
  imports: [RouterModule, HttpClientModule, CommonModule, PaginationComponent],
  templateUrl: './category.component.html',
  styleUrl: './category.component.scss',
})
export class CategoryComponent implements OnInit {
  products: ProductDTO[] = [];
  categoryName: string = '';
  categoryId: number = 0;
  currentPage: number = 1;
  totalPages: number = 1;
  totalItems: number = 0;
  isLoading: boolean = false;
  hasMoreProducts: boolean = true;
  private readonly limit: number = 20;

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private categoryService: CategoryService,
    private cartService: CartService,
    private router: Router,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      const categoryId = +params['id'];
      this.categoryId = categoryId;
      this.currentPage = 1; // Reset page when category changes
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
          this.totalPages = response.totalPage || 1;
          this.totalItems = this.totalPages * this.limit; // Estimate total items
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

  private loadCategoryName(id: number): void {
    const categoryNames: { [key: number]: string } = {};
    this.categoryService.getCategories().subscribe({
      next: (categories) => {
        categories.forEach((category) => {
          categoryNames[category.id] = category.name;
        });
        this.categoryName = categoryNames[id] || 'Danh Mục Sản Phẩm';
      },
      error: (error) => {
        console.error('Lỗi khi lấy tên danh mục:', error);
        this.categoryName = 'Danh Mục Sản Phẩm';
      },
    });
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

  onPageChange(page: number): void {
    if (page === this.currentPage || this.isLoading) return;

    this.currentPage = page;

    // Scroll to top of page
    window.scrollTo({ top: 0, behavior: 'smooth' });

    // Load products for the new page
    this.loadProducts(this.categoryId);
  }

  get itemsPerPage(): number {
    return this.limit;
  }
}
