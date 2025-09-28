import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.scss',
})
export class PaginationComponent implements OnChanges {
  @Input() currentPage: number = 1;
  @Input() totalPages: number = 1;
  @Input() totalItems: number = 0;
  @Input() itemsPerPage: number = 20;
  @Input() showFirstLast: boolean = true;
  @Input() showPrevNext: boolean = true;
  @Input() maxVisiblePages: number = 5;
  @Input() isLoading: boolean = false;

  @Output() pageChange = new EventEmitter<number>();

  visiblePages: number[] = [];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['currentPage'] || changes['totalPages']) {
      this.calculateVisiblePages();
    }
  }

  private calculateVisiblePages(): void {
    this.visiblePages = [];

    if (this.totalPages <= this.maxVisiblePages) {
      // Hiển thị tất cả trang nếu số trang ít
      for (let i = 1; i <= this.totalPages; i++) {
        this.visiblePages.push(i);
      }
    } else {
      // Tính toán trang hiển thị xung quanh trang hiện tại
      const half = Math.floor(this.maxVisiblePages / 2);
      let start = Math.max(1, this.currentPage - half);
      let end = Math.min(this.totalPages, start + this.maxVisiblePages - 1);

      // Điều chỉnh nếu cần
      if (end - start < this.maxVisiblePages - 1) {
        start = Math.max(1, end - this.maxVisiblePages + 1);
      }

      for (let i = start; i <= end; i++) {
        this.visiblePages.push(i);
      }
    }
  }

  onPageClick(page: number): void {
    if (
      page !== this.currentPage &&
      page >= 1 &&
      page <= this.totalPages &&
      !this.isLoading
    ) {
      this.pageChange.emit(page);
    }
  }

  onPreviousClick(): void {
    if (this.currentPage > 1 && !this.isLoading) {
      this.pageChange.emit(this.currentPage - 1);
    }
  }

  onNextClick(): void {
    if (this.currentPage < this.totalPages && !this.isLoading) {
      this.pageChange.emit(this.currentPage + 1);
    }
  }

  onFirstClick(): void {
    if (this.currentPage !== 1 && !this.isLoading) {
      this.pageChange.emit(1);
    }
  }

  onLastClick(): void {
    if (this.currentPage !== this.totalPages && !this.isLoading) {
      this.pageChange.emit(this.totalPages);
    }
  }

  get startItem(): number {
    return Math.min(
      (this.currentPage - 1) * this.itemsPerPage + 1,
      this.totalItems
    );
  }

  get endItem(): number {
    return Math.min(this.currentPage * this.itemsPerPage, this.totalItems);
  }

  get hasItems(): boolean {
    return this.totalItems > 0;
  }

  get hasPrevious(): boolean {
    return this.currentPage > 1;
  }

  get hasNext(): boolean {
    return this.currentPage < this.totalPages;
  }
}
