import {
  Component,
  OnInit,
  OnDestroy,
  Input,
  Output,
  EventEmitter,
  ViewChild,
  ElementRef,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { SearchService, SearchResult } from '../../services/search.service';
import { ProductDTO } from '../../models/product.dto';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  template: `
    <div class="search-container" [class.focused]="isFocused">
      <!-- Search Input -->
      <div class="search-input-container">
        <input
          #searchInput
          type="text"
          class="search-input"
          placeholder="Tìm kiếm sản phẩm..."
          [(ngModel)]="searchQuery"
          (input)="onSearchInput($event)"
          (focus)="onFocus()"
          (blur)="onBlur()"
          (keydown.enter)="onEnterPress()"
          (keydown.arrowdown)="navigateResults(1)"
          (keydown.arrowup)="navigateResults(-1)"
          (keydown.escape)="clearSearch()"
        />

        <!-- Search Icon -->
        <button
          class="search-btn"
          (click)="performSearch()"
          [disabled]="!searchQuery.trim()"
        >
          <svg class="search-icon" fill="currentColor" viewBox="0 0 20 20">
            <path
              fill-rule="evenodd"
              d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z"
              clip-rule="evenodd"
            ></path>
          </svg>
        </button>

        <!-- Clear Button -->
        <button
          *ngIf="searchQuery"
          class="clear-btn"
          (click)="clearSearch()"
          type="button"
        >
          <svg fill="currentColor" viewBox="0 0 20 20">
            <path
              fill-rule="evenodd"
              d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z"
              clip-rule="evenodd"
            ></path>
          </svg>
        </button>
      </div>

      <!-- Search Dropdown -->
      <div
        *ngIf="showDropdown && (isFocused || hasResults)"
        class="search-dropdown"
        [class.visible]="showDropdown"
      >
        <!-- Quick Results (while typing) -->
        <div
          *ngIf="quickResults.length > 0 && !searchResult"
          class="search-section"
        >
          <div class="section-title">Gợi ý</div>
          <div
            *ngFor="let product of quickResults; let i = index"
            class="search-item"
            [class.highlighted]="i === selectedIndex"
            (click)="selectProduct(product)"
            (mouseenter)="selectedIndex = i"
          >
            <img
              [src]="product.thumbnail"
              [alt]="product.name"
              class="product-thumbnail"
            />
            <div class="product-info">
              <div
                class="product-name"
                [innerHTML]="highlightMatch(product.name, searchQuery)"
              ></div>
              <div class="product-price">{{ formatPrice(product.price) }}đ</div>
            </div>
          </div>
        </div>

        <!-- Search Results -->
        <div
          *ngIf="searchResult && searchResult.results.length > 0"
          class="search-section"
        >
          <div class="section-title">
            Kết quả tìm kiếm
            <span class="results-count"
              >({{ searchResult.totalResults }} sản phẩm)</span
            >
            <span class="search-time" *ngIf="searchResult.searchTime">
              - {{ searchResult.searchTime.toFixed(0) }}ms
            </span>
          </div>

          <div
            *ngFor="
              let product of searchResult.results.slice(0, 8);
              let i = index
            "
            class="search-item"
            [class.highlighted]="i === selectedIndex"
            (click)="selectProduct(product)"
            (mouseenter)="selectedIndex = i"
          >
            <img
              [src]="product.thumbnail"
              [alt]="product.name"
              class="product-thumbnail"
            />
            <div class="product-info">
              <div
                class="product-name"
                [innerHTML]="highlightMatch(product.name, searchQuery)"
              ></div>
              <div
                class="product-description"
                [innerHTML]="highlightMatch(product.description, searchQuery)"
              ></div>
              <div class="product-price">{{ formatPrice(product.price) }}đ</div>
            </div>
          </div>

          <div
            *ngIf="searchResult.totalResults > 8"
            class="view-all-btn"
            (click)="viewAllResults()"
          >
            Xem tất cả {{ searchResult.totalResults }} kết quả
          </div>
        </div>

        <!-- No Results -->
        <div
          *ngIf="searchResult && searchResult.results.length === 0"
          class="no-results"
        >
          <div class="no-results-icon">🔍</div>
          <div class="no-results-text">Không tìm thấy sản phẩm nào</div>
          <div class="no-results-subtext">Thử tìm kiếm với từ khóa khác</div>
        </div>

        <!-- Recent Searches -->
        <div
          *ngIf="!searchQuery && recentSearches.length > 0"
          class="search-section"
        >
          <div class="section-title">Tìm kiếm gần đây</div>
          <div
            *ngFor="let search of recentSearches; let i = index"
            class="recent-search-item"
            (click)="useRecentSearch(search)"
          >
            <svg class="recent-icon" fill="currentColor" viewBox="0 0 20 20">
              <path
                fill-rule="evenodd"
                d="M10 18a8 8 0 100-16 8 8 0 000 16zm1-12a1 1 0 10-2 0v4a1 1 0 00.293.707l2.828 2.829a1 1 0 101.415-1.415L11 9.586V6z"
                clip-rule="evenodd"
              ></path>
            </svg>
            {{ search }}
          </div>
        </div>

        <!-- Popular Searches -->
        <div
          *ngIf="!searchQuery && popularSearches.length > 0"
          class="search-section"
        >
          <div class="section-title">Tìm kiếm phổ biến</div>
          <div class="popular-searches">
            <span
              *ngFor="let search of popularSearches"
              class="popular-search-tag"
              (click)="useRecentSearch(search)"
            >
              {{ search }}
            </span>
          </div>
        </div>
      </div>
    </div>

    <!-- Overlay -->
    <div
      *ngIf="showDropdown && isFocused"
      class="search-overlay"
      (click)="hideDropdown()"
    ></div>
  `,
  styleUrls: ['./search.component.scss'],
})
export class SearchComponent implements OnInit, OnDestroy {
  @ViewChild('searchInput') searchInput!: ElementRef;
  @Input() placeholder: string = 'Tìm kiếm sản phẩm...';
  @Input() showInModal: boolean = false;
  @Output() productSelected = new EventEmitter<ProductDTO>();
  @Output() searchPerformed = new EventEmitter<string>();

  searchQuery: string = '';
  searchResult: SearchResult | null = null;
  quickResults: ProductDTO[] = [];
  recentSearches: string[] = [];
  popularSearches: string[] = [];

  isFocused: boolean = false;
  showDropdown: boolean = false;
  selectedIndex: number = -1;

  private destroy$ = new Subject<void>();
  private searchTimeout: any;

  constructor(private searchService: SearchService) {}

  ngOnInit(): void {
    this.loadInitialData();
    this.subscribeToSearchResults();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    if (this.searchTimeout) {
      clearTimeout(this.searchTimeout);
    }
  }

  private loadInitialData(): void {
    this.recentSearches = this.searchService.getRecentSearches();
    this.popularSearches = this.searchService.getPopularSearches();
  }

  private subscribeToSearchResults(): void {
    this.searchService
      .getSearchResults()
      .pipe(takeUntil(this.destroy$))
      .subscribe((result) => {
        this.searchResult = result;
        this.selectedIndex = -1;
      });
  }

  onSearchInput(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.searchQuery = target.value;

    // Clear previous timeout
    if (this.searchTimeout) {
      clearTimeout(this.searchTimeout);
    }

    // Get quick results immediately for autocomplete
    if (this.searchQuery.trim().length >= 2) {
      this.quickResults = this.searchService.quickSearch(this.searchQuery, 5);
      this.showDropdown = true;

      // Perform full search after delay
      this.searchTimeout = setTimeout(() => {
        this.searchService.search(this.searchQuery);
      }, 300);
    } else {
      this.quickResults = [];
      this.searchResult = null;
      this.showDropdown = this.isFocused;
    }
  }

  onFocus(): void {
    this.isFocused = true;
    this.showDropdown = true;
  }

  onBlur(): void {
    // Delay hiding to allow clicks on dropdown items
    setTimeout(() => {
      this.isFocused = false;
      if (!this.hasResults) {
        this.showDropdown = false;
      }
    }, 200);
  }

  onEnterPress(): void {
    if (this.selectedIndex >= 0) {
      // Select highlighted item
      const results =
        this.quickResults.length > 0
          ? this.quickResults
          : this.searchResult?.results || [];
      if (results[this.selectedIndex]) {
        this.selectProduct(results[this.selectedIndex]);
      }
    } else if (this.searchQuery.trim()) {
      // Perform search
      this.performSearch();
    }
  }

  navigateResults(direction: number): void {
    const results =
      this.quickResults.length > 0
        ? this.quickResults
        : this.searchResult?.results || [];
    if (results.length === 0) return;

    this.selectedIndex += direction;

    if (this.selectedIndex < 0) {
      this.selectedIndex = results.length - 1;
    } else if (this.selectedIndex >= results.length) {
      this.selectedIndex = 0;
    }
  }

  performSearch(): void {
    if (!this.searchQuery.trim()) return;

    this.searchService.search(this.searchQuery);
    this.searchPerformed.emit(this.searchQuery);
    this.hideDropdown();
  }

  selectProduct(product: ProductDTO): void {
    this.productSelected.emit(product);
    this.hideDropdown();
    this.searchInput.nativeElement.blur();
  }

  useRecentSearch(search: string): void {
    this.searchQuery = search;
    this.searchService.search(search);
    this.searchPerformed.emit(search);
  }

  clearSearch(): void {
    this.searchQuery = '';
    this.searchResult = null;
    this.quickResults = [];
    this.selectedIndex = -1;
    this.searchService.clearSearchResults();
    this.searchInput.nativeElement.focus();
  }

  hideDropdown(): void {
    this.showDropdown = false;
    this.isFocused = false;
  }

  viewAllResults(): void {
    if (this.searchResult) {
      this.searchPerformed.emit(this.searchResult.query);
      this.hideDropdown();
    }
  }

  get hasResults(): boolean {
    return (
      this.quickResults.length > 0 ||
      (this.searchResult?.results.length || 0) > 0
    );
  }

  highlightMatch(text: string, query: string): string {
    if (!query || !text) return text;

    const regex = new RegExp(`(${query})`, 'gi');
    return text.replace(regex, '<mark>$1</mark>');
  }

  formatPrice(price: number): string {
    return price.toLocaleString('vi-VN');
  }
}
