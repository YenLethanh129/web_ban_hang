import { Component, OnInit } from '@angular/core';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../services/product.service';
import { ProductDTO } from '../../models/product.dto';
import { CartService } from '../../services/cart.service'
import { Router } from '@angular/router';

@Component({
  selector: 'app-detail-product',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './detail-product.component.html',
  styleUrl: './detail-product.component.scss',
})
export class DetailProductComponent implements OnInit {
  product?: ProductDTO;
  isLoading: boolean = true;
  quantity: number = 1;
  selectedColor: string | null = null;
  colors = [
    { name: 'Đỏ', code: '#FF0000' },
    { name: 'Xanh lá', code: '#00FF00' },
    { name: 'Vàng', code: '#FFFF00' },
    { name: 'Cam', code: '#FFA500' },
  ];
  activeImageIndex: number = 0;
  currentImage: string = '';

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private cartService: CartService,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params) => {
      const productId = +params['id'];
      this.loadProduct(productId);
    });
  }

  private loadProduct(id: number): void {
    this.isLoading = true;
    this.productService.getProductById(id).subscribe({
      next: (product) => {
        this.product = product;
        this.currentImage = product.thumbnail;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Lỗi khi tải thông tin sản phẩm:', error);
        this.isLoading = false;
      },
    });
  }

  decreaseQuantity(): void {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  increaseQuantity(): void {
    this.quantity++;
  }

  getColorName(colorCode: string): string {
    return this.colors.find((c) => c.code === colorCode)?.name || '';
  }

  setActiveImage(index: number): void {
    this.activeImageIndex = index;
  }

  setCurrentImage(image: string): void {
    this.currentImage = image;
  }

  addToCart(): void {
    if (this.product) {
      this.cartService.addToCart(this.product.id, this.quantity);
      alert(`Đã thêm ${this.quantity} ${this.product.name} vào giỏ hàng`);
    }
  }

  buyNow(): void {
    if (this.product) {
      this.cartService.addToCart(this.product.id, this.quantity);
      this.router.navigate(['/order']);
    }
  }
}
