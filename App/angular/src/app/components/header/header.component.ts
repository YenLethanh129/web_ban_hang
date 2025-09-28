import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TokenService } from '../../services/token.service';
import { UserService } from '../../services/user.service';
import { FormsModule } from '@angular/forms';
import { CategoryDTO } from '../../dtos/category.dto';
import { CategoryService } from '../../services/category.service';
import { CacheService } from '../../services/cache.service';
import { SearchComponent } from '../search/search.component';
import { ProductDTO } from '../../dtos/product.dto';
import { Subject, takeUntil, combineLatest } from 'rxjs';
import { UserDTO } from '../../dtos/user.dto';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, CommonModule, FormsModule, SearchComponent],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class HeaderComponent implements OnInit, OnDestroy {
  username: string = '';
  searchTerm: string = '';
  selectedCategory: string = '';
  categories: CategoryDTO[] = [];
  isDropdownOpen: boolean = false;
  isLoggedIn: boolean = false;
  currentUser: UserDTO | null = null;

  private destroy$ = new Subject<void>();

  constructor(
    private tokenService: TokenService,
    private router: Router,
    private userService: UserService,
    private categoryService: CategoryService,
    private cacheService: CacheService
  ) {}

  ngOnInit() {
    this.loadCategories();

    // Subscribe to authentication and user state changes
    combineLatest([this.userService.isAuthenticated$, this.userService.user$])
      .pipe(takeUntil(this.destroy$))
      .subscribe(([isAuthenticated, user]) => {
        this.isLoggedIn = isAuthenticated;
        this.currentUser = user;
        this.username = user?.fullname || '';

        console.log('🔄 Header state updated:', {
          isLoggedIn: this.isLoggedIn,
          username: this.username,
          user: user,
        });
      });

    // Initialize authentication state check
    this.userService.checkAuthenticationStatus().subscribe({
      next: (isAuth) => {
        
      },
      error: (error) => {
        console.error('❌ Initial auth check failed:', error);
      },
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadCategories() {
    this.categoryService.getCategories().subscribe({
      next: (categories) => {
        this.categories = categories;
      },
      error: (error) => {
        console.error('Lỗi khi tải danh sách danh mục:', error);
      },
    });
  }

  private loadUserProfile() {
    // Deprecated - this method is no longer needed as we use reactive streams
    // Left for compatibility but functionality moved to ngOnInit
  }

  logout(): void {
    

    // Đóng dropdown trước
    this.isDropdownOpen = false;

    // Gọi logout service với Observable
    this.userService.logout().subscribe({
      next: (response) => {
        

        // Clear local state
        this.username = '';
        this.currentUser = null;
        this.isLoggedIn = false;

        // Navigate to login
        this.router.navigate(['/login'], {
          queryParams: {
            message: 'Đăng xuất thành công',
          },
        });
      },
      error: (error) => {
        console.error('❌ Header: Logout error:', error);

        // Vẫn clear local state và redirect dù có lỗi
        this.username = '';
        this.currentUser = null;
        this.isLoggedIn = false;

        this.router.navigate(['/login'], {
          queryParams: {
            message: 'Đăng xuất thành công (có lỗi nhỏ)',
          },
        });
      },
    });
  }

  // Handle search events
  onProductSelected(product: ProductDTO): void {
    this.router.navigate(['/detail-product', product.id]);
  }

  onSearchPerformed(query: string): void {
    this.router.navigate(['/search'], { queryParams: { q: query } });
  }

  onSearch(): void {
    // Xử lý tìm kiếm
    console.log('Searching:', {
      term: this.searchTerm,
      category: this.selectedCategory,
    });
  }

  toggleDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  closeDropdown(): void {
    this.isDropdownOpen = false;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: Event): void {
    const target = event.target as HTMLElement;
    const dropdown = target.closest('.dropdown');
    if (!dropdown) {
      this.isDropdownOpen = false;
    }
  }
}
