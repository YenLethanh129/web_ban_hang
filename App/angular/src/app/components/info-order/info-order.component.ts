import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { OrderService } from '../../services/order.service';
import { UserService } from '../../services/user.service';
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

  // Modal properties
  showOrderModal: boolean = false;
  selectedOrder: OrderResponseDTO | null = null;

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

    

    this.orderService.getUserOrders().subscribe({
      next: (orders) => {
        // Sắp xếp theo ngày tạo giảm dần (mới nhất trước)
        this.orders = orders.sort(
          (a, b) =>
            new Date(b.created_at).getTime() - new Date(a.created_at).getTime()
        );
        
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

  trackByOrderUuid(index: number, order: OrderResponseDTO): string {
    return order.order_uuid;
  }

  // New methods for actions
  viewOrderDetails(order: OrderResponseDTO): void {
    
    this.selectedOrder = order;
    this.showOrderModal = true;
  }

  closeOrderModal(): void {
    this.showOrderModal = false;
    this.selectedOrder = null;
  }

  editOrder(order: OrderResponseDTO): void {
    
    // Navigate to edit order page
    this.router.navigate(['/edit-order', order.order_uuid]);
  }

  // Function mới để xử lý khi user click button hủy đơn hàng
  handleCancelOrder(order: OrderResponseDTO): void {
    

    // Hiển thị confirmation dialog
    const confirmMessage = `Bạn có chắc chắn muốn hủy đơn hàng ${order.order_uuid.substring(
      0,
      5
    )}...?\n\nHành động này không thể hoàn tác.`;

    if (confirm(confirmMessage)) {
      this.processCancelOrder(order);
    }
  }

  // Function xử lý logic hủy đơn hàng
  private processCancelOrder(order: OrderResponseDTO): void {
    

    this.orderService.cancelOrder(order.order_id).subscribe({
      next: (response) => {
        
        this.updateOrderStatusAfterCancel(order.order_uuid);
        this.showSuccessMessage('Đơn hàng đã được hủy thành công!');

        if (
          this.showOrderModal &&
          this.selectedOrder?.order_uuid === order.order_uuid
        ) {
          this.closeOrderModal();
        }
      },
      error: (error) => {
        console.error('Error cancelling order:', error);
        this.showErrorMessage(
          'Có lỗi xảy ra khi hủy đơn hàng. Vui lòng thử lại sau!'
        );
      },
    });
  }

  private updateOrderStatusAfterCancel(orderUuid: string): void {
    const orderIndex = this.orders.findIndex((o) => o.order_uuid === orderUuid);
    if (orderIndex !== -1) {
      this.orders[orderIndex].status = 'CANCELLED';

      if (this.selectedOrder && this.selectedOrder.order_uuid === orderUuid) {
        this.selectedOrder.status = 'CANCELLED';
      }
    }
  }

  // Function hiển thị thông báo thành công
  private showSuccessMessage(message: string): void {
    alert(message);
    // TODO: Có thể thay thế bằng notification service sau này
  }

  // Function hiển thị thông báo lỗi
  private showErrorMessage(message: string): void {
    alert(message);
    // TODO: Có thể thay thế bằng notification service sau này
  }

  // Giữ lại function cũ để tương thích (deprecated)
  cancelOrder(order: OrderResponseDTO): void {
    console.warn('cancelOrder is deprecated, use handleCancelOrder instead');
    this.handleCancelOrder(order);
  }

  cancelOrderFromModal(): void {
    if (this.selectedOrder) {
      this.handleCancelOrder(this.selectedOrder);
    }
  }

  reorder(orderUuid: string): void {
    // Implement reorder logic
    
    this.router.navigate(['/order-details', orderUuid], {
      queryParams: { reorder: true },
    });
  }
}
