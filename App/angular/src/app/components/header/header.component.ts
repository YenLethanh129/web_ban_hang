import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TokenService } from '../../services/token.service';
import { UserService } from '../../services/user.service';
import { FormsModule } from '@angular/forms';
import { CategoryDTO } from '../../models/category.dto';
import { CategoryService } from '../../services/category.service';
import { CacheService } from '../../services/cache.service';
import { SearchComponent } from '../search/search.component';
import { ProductDTO } from '../../models/product.dto';
import { Subject, takeUntil } from 'rxjs';

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

    // Subscribe to user changes from cache
    this.userService
      .getUserObservable()
      .pipe(takeUntil(this.destroy$))
      .subscribe((user) => {
        this.username = user?.fullname || '';
      });

    // Load user if logged in
    if (this.isLoggedIn) {
      this.loadUserProfile();
    }
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
    // Check cache first
    const cachedUser = this.cacheService.getUser();
    if (cachedUser) {
      this.username = cachedUser.fullname;
      return;
    }

    // Load from server if not in cache
    this.userService.getUser().subscribe({
      next: (user) => {
        this.userService.setCurrentUser(user);
        this.username = user.fullname;
      },
      error: (error) => {
        console.error('Lỗi khi tải thông tin user:', error);
        this.tokenService.removeToken();
      },
    });
  }

  get isLoggedIn(): boolean {
    return this.tokenService.isLoggedIn();
  }

  logout(): void {
    this.userService.logout(); // Use the new logout method from UserService
    this.username = '';
    this.isDropdownOpen = false;
    this.router.navigate(['/login']);
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
