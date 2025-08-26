import { Component, OnInit } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TokenService } from '../../services/token.service';
import { UserService } from '../../services/user.service';
import { FormsModule } from '@angular/forms';
import { CategoryDTO } from '../../models/category.dto';
import { CategoryService } from '../../services/category.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, CommonModule, FormsModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class HeaderComponent implements OnInit {
  username: string = '';
  searchTerm: string = '';
  selectedCategory: string = '';
  categories: CategoryDTO[] = [];

  constructor(
    private tokenService: TokenService,
    private router: Router,
    private userService: UserService,
    private categoryService: CategoryService
  ) {}

  ngOnInit() {
    this.loadCategories();
    if (this.isLoggedIn) {
      this.loadUserProfile();
    }
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
    this.userService.getUser().subscribe({
      next: (user) => {
        //this.userService.setUser(user);
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
    this.tokenService.removeToken();
    this.username = '';
    this.router.navigate(['/login']);
  }

  onSearch(): void {
    // Xử lý tìm kiếm
    console.log('Searching:', {
      term: this.searchTerm,
      category: this.selectedCategory,
    });
  }
}
