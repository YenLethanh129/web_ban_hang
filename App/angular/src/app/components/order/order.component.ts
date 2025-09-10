import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Subject, takeUntil } from 'rxjs';
import { ProductDTO } from '../../models/product.dto';
import { CartService } from '../../services/cart.service';
import { ProductService } from '../../services/product.service';
import { TokenService } from '../../services/token.service';
import { UserService } from '../../services/user.service';
import { OrderService } from '../../services/order.service';
import { FormsModule } from '@angular/forms';
import { UserDTO } from '../../dtos/user.dto';
import { MomoInfoOrderDTO, OrderDTO } from '../../dtos/order.dto';
import { OrderDetailDTO } from '../../dtos/order.dto';
import { OrderDetailService } from '../../services/order.detail.service';
import { MomoService } from '../../services/momo.service';
import { CreateMomoResponse } from '../../dtos/momo.dto';
import { AddressAutocompleteComponent } from '../shared/address-autocomplete/address-autocomplete.component';
import { AddressPrediction } from '../../dtos/address.dto';
import { UserAddressService } from '../../services/user-address.service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-order',
  standalone: true,
  imports: [
    RouterModule,
    CommonModule,
    FormsModule,
    AddressAutocompleteComponent,
  ],
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss'],
})
export class OrderComponent implements OnInit, OnDestroy {
  cartItems: { product: ProductDTO; quantity: number; size: string }[] = [];
  isLoading: boolean = false;
  user: UserDTO | null = null;
  momoInfoOrderDTO: MomoInfoOrderDTO | null = null;
  orderId: number = 0;
  showAutofillButton: boolean = false;
  showAddressError: boolean = false;

  private destroy$ = new Subject<void>();

  orderData = {
    userId: 0, // This should be set to the actual user ID
    fullName: '',
    email: '',
    phoneNumber: '',
    address: '',
    note: '',
    totalMoney: null as number | null,
    shippingMethod: '',
    shippingAddress: '',
    paymentMethod: '',
    paymentStatus: '',
  };

  constructor(
    private cartService: CartService,
    private productService: ProductService,
    private tokenService: TokenService,
    private userService: UserService,
    private orderService: OrderService,
    private orderDetailService: OrderDetailService,
    private momoService: MomoService,
    private router: Router,
    private userAddressService: UserAddressService,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.loadCartItems().then(() => {
      this.orderData.totalMoney = this.getTotalPrice();
    });
    this.loadUserData();
    this.orderData.shippingMethod = 'STANDARD'; // Default shipping method
    this.orderData.paymentMethod = 'MOMO'; // Default payment method
    this.orderData.paymentStatus = 'PENDING';
    this.isLoading = false;

    // Kiểm tra xem có thể tự động điền địa chỉ không
    this.userAddressService.userAddress$
      .pipe(takeUntil(this.destroy$))
      .subscribe((addressInfo) => {
        this.showAutofillButton = !!addressInfo;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  /**
   * Tự động điền thông tin từ profile user
   */
  autofillFromProfile(): void {
    const addressInfo = this.userAddressService.getCurrentAddress();
    if (addressInfo) {
      this.orderData.fullName = addressInfo.fullname;
      this.orderData.phoneNumber = addressInfo.phoneNumber;
      this.orderData.address = addressInfo.address;
      this.orderData.shippingAddress = addressInfo.address;

      this.notificationService.showSuccess(
        'Đã tự động điền thông tin giao hàng từ hồ sơ của bạn!'
      );
    } else {
      this.notificationService.showWarning(
        'Không tìm thấy thông tin hồ sơ để tự động điền'
      );
    }
  }

  onSubmit() {
    this.isLoading = true;
    this.createOrder();
  }

  private loadUserData() {
    this.userService.getUser().subscribe({
      next: (user) => {
        this.user = user;
        console.log('Thông tin người dùng:', this.user);
        if (this.user) {
          this.orderData.userId = this.user.id;
          this.orderData.fullName = this.user.fullname;
          this.orderData.phoneNumber = this.user.phone_number;
          this.orderData.shippingAddress = this.user.address;
          this.orderData.address = this.user.address;
        }
      },
      error: (error) => {
        console.error('Lỗi khi tải thông tin người dùng:', error);
      },
    });
  }

  private async loadCartItems() {
    this.isLoading = true;
    const cart = this.cartService.getCart();

    try {
      const items = await Promise.all(
        Array.from(cart.entries()).map(async ([productId, cartItem]) => {
          const product = await this.productService
            .getProductById(productId)
            .toPromise();
          return {
            product,
            quantity: cartItem.quantity,
            size: cartItem.size,
          };
        })
      );
      this.cartItems = items.filter(
        (
          item
        ): item is { product: ProductDTO; quantity: number; size: string } =>
          item.product !== undefined
      );
    } catch (error) {
      console.error('Lỗi khi tải thông tin giỏ hàng:', error);
    } finally {
      this.isLoading = false;
    }
  }

  getTotalPrice(): number {
    return this.cartItems.reduce(
      (total, item) => total + item.product.price * item.quantity,
      0
    );
  }

  onAddressSelected(address: AddressPrediction): void {
    this.orderData.shippingAddress = address.description;
  }

  onAddressFocus(): void {
    this.showAddressError = true;
  }

  createOrder(): void {
    const orderDTO: OrderDTO = {
      user_id: this.orderData.userId,
      full_name: this.orderData.fullName,
      email: this.orderData.email,
      phone_number: this.orderData.phoneNumber,
      address: this.orderData.address,
      note: this.orderData.note,
      total_money: this.orderData.totalMoney!,
      shipping_method: this.orderData.shippingMethod,
      shipping_address: this.orderData.shippingAddress,
      payment_method:
        this.orderData.paymentMethod == 'MOMO' ? 'E_WALLET' : 'E_WALLET',
      payment_status: this.orderData.paymentStatus,
    };

    console.log('OrderDTO: ', orderDTO);
    this.orderService.createOrder(orderDTO).subscribe({
      next: (response: any) => {
        console.log('Đơn hàng đã được tạo:', response);
        this.orderId = response.order_id;
        this.momoInfoOrderDTO = {
          order_id: response.order_id ?? response.orderId,
          amount: this.getTotalPrice(),
        };
        this.createOrderDetail(this.orderId);
      },
      error: (error) => {
        console.error('Lỗi khi tạo đơn hàng:', error);
      },
    });
  }

  createOrderDetail(orderId: number): void {
    this.cartItems.forEach((item) => {
      const orderDetailDTO: OrderDetailDTO = {
        order_id: orderId,
        product_id: item.product.id,
        quantity: item.quantity,
        unit_price: item.product.price,
        total_money: item.product.price * item.quantity,
        size: item.size ?? 'L',
      };

      console.log('OrderDetailDTO: ', orderDetailDTO);
      this.orderDetailService.createOrderDetail(orderDetailDTO).subscribe({
        next: (response) => {
          console.log('Đơn hàng chi tiết đã được tạo:', response);
        },
        error: (error) => {
          console.error('Lỗi khi tạo đơn hàng chi tiết:', error);
        },
      });
    });

    this.clearCart();

    if (this.momoInfoOrderDTO) {
      this.momoService.createQR(this.momoInfoOrderDTO).subscribe({
        next: (response: CreateMomoResponse) => {
          console.log('Mã thanh toán MoMo:', response);
          if (response.payUrl) {
            // Chuyển hướng đến URL thanh toán MoMo
            window.location.href = response.payUrl;
          } else {
            console.error('Không tìm thấy payUrl trong response');
          }
        },
        error: (error) => {
          console.error('Lỗi khi lấy mã thanh toán: ', error);
        },
      });
    }

    this.isLoading = false;
  }

  clearCart(): void {
    this.cartService.clearCart();
    this.cartItems = [];
  }
}
