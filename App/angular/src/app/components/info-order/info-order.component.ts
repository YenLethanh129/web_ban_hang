import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { OrderService } from '../../services/order.service';
import { UserService } from '../../services/user.service';
import { TokenService } from '../../services/token.service';
import { OrderResponseDTO } from '../../dtos/order.dto';

@Component({
  selector: 'app-info-order',
  imports: [RouterModule, CommonModule],
  templateUrl: './info-order.component.html',
  styleUrl: './info-order.component.scss',
})
export class InfoOrderComponent implements OnInit {
  userId: number | null = null;
  orders: OrderResponseDTO[] = [];
  isLoading: boolean = false;

  orderStatus: { [key: string]: string } = {
    PENDING: 'Chờ xác nhận',
    PROCESSING: 'Đang xử lý',
    SHIPPING: 'Đang vận chuyển',
    DELIVERED: 'Đã giao hàng',
    CANCELLED: 'Đã hủy',
  };

  orderPaymentStatus: { [key: string]: string } = {
    PENDING: 'Chưa thanh toán',
    PAID: 'Đã thanh toán',
    FAILED: 'Thanh toán thất bại',
  };

  constructor(
    private router: Router,
    private orderService: OrderService,
    private userService: UserService,
    private tokenService: TokenService
  ) {}

  ngOnInit(): void {
    this.loadUserProfile();
  }

  private loadOrderInfo(userId: number): void {
    this.orderService.getOrdersByUserId(userId).subscribe({
      next: (orderInfo) => {
        // Sắp xếp theo ID giảm dần (mới nhất trước)
        this.orders = orderInfo.sort((a: any, b: any) => b.id - a.id);
        console.log('Order Info:', this.orders);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading order info:', error);
        this.isLoading = false;
      },
    });
  }

  private loadUserProfile() {
    this.isLoading = true;
    this.userService.getUser().subscribe({
      next: (user) => {
        this.userId = user.id;
        this.loadOrderInfo(user.id);
      },
      error: (error) => {
        console.error('Lỗi khi tải thông tin user:', error);
        this.tokenService.removeToken();
        this.isLoading = false;
      },
    });
  }

  // Utility methods for status and payment
  getStatusClass(status: string): string {
    const statusMap: { [key: string]: string } = {
      pending: 'status-pending',
      confirmed: 'status-confirmed',
      processing: 'status-processing',
      shipping: 'status-shipping',
      delivered: 'status-delivered',
      cancelled: 'status-cancelled',
    };
    return statusMap[status] || 'status-default';
  }

  getStatusText(status: string): string {
    const statusMap: { [key: string]: string } = {
      pending: 'Chờ xác nhận',
      confirmed: 'Đã xác nhận',
      processing: 'Đang xử lý',
      shipping: 'Đang vận chuyển',
      delivered: 'Đã giao hàng',
      cancelled: 'Đã hủy',
    };
    return statusMap[status] || status;
  }

  getPaymentMethodClass(method: string): string {
    const methodMap: { [key: string]: string } = {
      cash: 'payment-cash',
      momo: 'payment-momo',
      banking: 'payment-banking',
    };
    return methodMap[method] || 'payment-default';
  }

  getPaymentMethodIcon(method: string): string {
    const iconMap: { [key: string]: string } = {
      cash: 'fas fa-money-bill-wave',
      momo: 'fab fa-paypal',
      banking: 'fas fa-credit-card',
    };
    return iconMap[method] || 'fas fa-wallet';
  }

  getPaymentMethodText(method: string): string {
    const methodMap: { [key: string]: string } = {
      cash: 'Thanh toán khi nhận hàng',
      momo: 'Ví MoMo',
      banking: 'Chuyển khoản ngân hàng',
    };
    return methodMap[method] || method;
  }

  getPaymentStatusClass(status: string): string {
    const statusMap: { [key: string]: string } = {
      paid: 'payment-paid',
      unpaid: 'payment-unpaid',
      refunded: 'payment-refunded',
    };
    return statusMap[status] || 'payment-default';
  }

  getPaymentStatusText(status: string): string {
    const statusMap: { [key: string]: string } = {
      paid: 'Đã thanh toán',
      unpaid: 'Chưa thanh toán',
      refunded: 'Đã hoàn tiền',
    };
    return statusMap[status] || status;
  }

  canCancelOrder(status: string): boolean {
    return ['pending', 'confirmed'].includes(status);
  }

  canReorder(status: string): boolean {
    return ['delivered', 'cancelled'].includes(status);
  }

  trackByOrderId(index: number, order: any): number {
    return order.id;
  }

  cancelOrder(orderId: number): void {
    if (confirm('Bạn có chắc chắn muốn hủy đơn hàng này?')) {
      // Implement cancel order logic
      console.log('Cancel order:', orderId);
    }
  }

  reorder(orderId: number): void {
    // Implement reorder logic
    console.log('Reorder:', orderId);
    this.router.navigate(['/order-details', orderId], {
      queryParams: { reorder: true },
    });
  }
}
