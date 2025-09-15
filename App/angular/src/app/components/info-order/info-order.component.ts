import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { OrderService } from '../../services/order.service';
import { UserService } from '../../services/user.service';
import { NewOrderResponseDTO } from '../../dtos/order.dto';

@Component({
  selector: 'app-info-order',
  imports: [RouterModule, CommonModule],
  templateUrl: './info-order.component.html',
  styleUrl: './info-order.component.scss',
})
export class InfoOrderComponent implements OnInit {
  userId: number | null = null;
  orders: NewOrderResponseDTO[] = [];
  isLoading: boolean = false;

  orderStatus: { [key: string]: string } = {
    PENDING: 'Chờ xác nhận',
    CONFIRMED: 'Đã xác nhận',
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
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.loadUserOrders();
  }

  private loadUserOrders(): void {
    this.isLoading = true;

    // Kiểm tra authentication với UserService thay vì TokenService
    if (!this.userService.isAuthenticated()) {
      console.error('User not authenticated, redirecting to login');
      this.router.navigate(['/login']);
      this.isLoading = false;
      return;
    }

    const currentUser = this.userService.getCurrentUser();
    if (!currentUser) {
      console.error('No user info found, redirecting to login');
      this.router.navigate(['/login']);
      this.isLoading = false;
      return;
    }

    console.log('Loading user orders for:', currentUser.fullname);

    this.orderService.getUserOrders().subscribe({
      next: (orders) => {
        // Sắp xếp theo ngày tạo giảm dần (mới nhất trước)
        this.orders = orders.sort(
          (a, b) =>
            new Date(b.created_at).getTime() - new Date(a.created_at).getTime()
        );
        console.log('Orders loaded successfully:', this.orders);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading orders:', error);

        // Xử lý các loại lỗi cụ thể
        if (error.status === 401) {
          console.error('Token expired or invalid');
          this.userService.logout().subscribe({
            next: () => {
              this.router.navigate(['/login'], {
                queryParams: {
                  message: 'Phiên đăng nhập đã hết hạn, vui lòng đăng nhập lại',
                },
              });
            },
            error: (logoutError) => {
              console.error('Logout error:', logoutError);
              // Vẫn redirect dù logout có lỗi
              this.router.navigate(['/login'], {
                queryParams: {
                  message: 'Phiên đăng nhập đã hết hạn, vui lòng đăng nhập lại',
                },
              });
            },
          });
        } else if (error.status === 0) {
          console.error('Network error or server is down');
        } else {
          console.error('API error:', error.status, error.message);
        }

        this.isLoading = false;
      },
    });
  }

  // Utility methods for status and payment
  getStatusClass(status: string): string {
    const statusMap: { [key: string]: string } = {
      PENDING: 'status-pending',
      CONFIRMED: 'status-confirmed',
      PROCESSING: 'status-processing',
      SHIPPING: 'status-shipping',
      DELIVERED: 'status-delivered',
      CANCELLED: 'status-cancelled',
    };
    return statusMap[status] || 'status-default';
  }

  getStatusText(status: string): string {
    return this.orderStatus[status] || status;
  }

  getPaymentMethodClass(method: string): string {
    const methodMap: { [key: string]: string } = {
      CASH: 'payment-cash',
      E_WALLET: 'payment-momo',
      BANKING: 'payment-banking',
    };
    return methodMap[method] || 'payment-default';
  }

  getPaymentMethodIcon(method: string): string {
    const iconMap: { [key: string]: string } = {
      CASH: 'fas fa-money-bill-wave',
      E_WALLET: 'fab fa-paypal',
      BANKING: 'fas fa-credit-card',
    };
    return iconMap[method] || 'fas fa-wallet';
  }

  getPaymentMethodText(method: string): string {
    const methodMap: { [key: string]: string } = {
      CASH: 'Thanh toán khi nhận hàng',
      E_WALLET: 'Ví điện tử',
      BANKING: 'Chuyển khoản ngân hàng',
    };
    return methodMap[method] || method;
  }

  getPaymentStatusClass(status: string): string {
    const statusMap: { [key: string]: string } = {
      PAID: 'payment-paid',
      PENDING: 'payment-pending',
      FAILED: 'payment-failed',
    };
    return statusMap[status] || 'payment-default';
  }

  getPaymentStatusText(status: string): string {
    return this.orderPaymentStatus[status] || status;
  }

  canCancelOrder(status: string): boolean {
    return ['PENDING', 'CONFIRMED'].includes(status);
  }

  canReorder(status: string): boolean {
    return ['DELIVERED', 'CANCELLED'].includes(status);
  }

  trackByOrderUuid(index: number, order: NewOrderResponseDTO): string {
    return order.order_uuid;
  }

  // New methods for actions
  viewOrderDetails(order: NewOrderResponseDTO): void {
    console.log('Viewing order details:', order);
    // Navigate to order details page with order_uuid
    this.router.navigate(['/order-details', order.order_uuid]);
  }

  editOrder(order: NewOrderResponseDTO): void {
    console.log('Editing order:', order);
    // Navigate to edit order page
    this.router.navigate(['/edit-order', order.order_uuid]);
  }

  cancelOrder(orderUuid: string): void {
    if (confirm('Bạn có chắc chắn muốn hủy đơn hàng này?')) {
      // Implement cancel order logic
      console.log('Cancel order:', orderUuid);
    }
  }

  reorder(orderUuid: string): void {
    // Implement reorder logic
    console.log('Reorder:', orderUuid);
    this.router.navigate(['/order-details', orderUuid], {
      queryParams: { reorder: true },
    });
  }
}
