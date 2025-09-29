import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Subject, takeUntil, take } from 'rxjs';
import { ProductDTO } from '../../dtos/product.dto';
import { CartService } from '../../services/cart.service';
import { ProductService } from '../../services/product.service';
import { TokenService } from '../../services/token.service';
import { UserService } from '../../services/user.service';
import { OrderService } from '../../services/order.service';
import { FormsModule } from '@angular/forms';
import { UserDTO } from '../../dtos/user.dto';
import { MomoInfoOrderDTO, OrderRequestDTO } from '../../dtos/order.dto';
import { OrderDetailRequestDTO } from '../../dtos/order.dto';
import { OrderDetailService } from '../../services/order.detail.service';
import { MomoService } from '../../services/momo.service';
import { CreateMomoResponse } from '../../dtos/momo.dto';
import { AddressAutocompleteComponent } from '../shared/address-autocomplete/address-autocomplete.component';
import { AddressPrediction } from '../../dtos/address.dto';
import { UserAddressService } from '../../services/user-address.service';
import { NotificationService } from '../../services/notification.service';
import { AddressService } from '../../services/address.service';
import { ValidateDTO } from '../../dtos/validate.dto';
import { ValidateService } from '../../services/validate.service';

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

  // Validation DTOs
  validateFullNameDTO: ValidateDTO = {
    isValid: false,
    errors: ['Họ và tên không được để trống'],
  };
  validateEmailDTO: ValidateDTO = {
    isValid: true,
    errors: [],
  };
  validatePhoneNumberDTO: ValidateDTO = {
    isValid: false,
    errors: ['Số điện thoại không được để trống'],
  };
  validateShippingAddressDTO: ValidateDTO = {
    isValid: false,
    errors: ['Địa chỉ giao hàng không được để trống'],
  };

  private destroy$ = new Subject<void>();

  orderData = {
    userId: 0,
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

  // The prediction object selected from the autocomplete results.
  selectedAddressPrediction: AddressPrediction | null = null;

  constructor(
    private cartService: CartService,
    private productService: ProductService,
    private userService: UserService,
    private orderService: OrderService,
    private orderDetailService: OrderDetailService,
    private momoService: MomoService,
    private router: Router,
    private userAddressService: UserAddressService,
    private addressService: AddressService,
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

      // Validate after autofill
      this.validateFullName();
      this.validatePhoneNumber();
      this.validateShippingAddress();

      this.notificationService.showSuccess(
        'Đã tự động điền thông tin giao hàng từ hồ sơ của bạn!'
      );
    } else {
      this.notificationService.showWarning(
        'Không tìm thấy thông tin hồ sơ để tự động điền'
      );
    }
  }

  /**
   *
   * VALIDATE METHODS
   *
   */

  validateFullName(event?: Event): void {
    if (event) {
      const input = event.target as HTMLInputElement;
      this.orderData.fullName = input.value;
    }
    this.validateFullNameDTO = ValidateService.validateFullName(
      this.orderData.fullName
    );
  }

  validateEmail(event?: Event): void {
    if (event) {
      const input = event.target as HTMLInputElement;
      this.orderData.email = input.value;
    }
    if (this.orderData.email && this.orderData.email.trim().length > 0) {
      this.validateEmailDTO = ValidateService.validateEmail(
        this.orderData.email
      );
    } else {
      this.validateEmailDTO = { isValid: true, errors: [] };
    }
  }

  validatePhoneNumber(event?: Event): void {
    if (event) {
      const input = event.target as HTMLInputElement;
      this.orderData.phoneNumber = input.value;
    }
    this.validatePhoneNumberDTO = ValidateService.validatePhoneNumber(
      this.orderData.phoneNumber
    );
  }

  validateShippingAddress(): void {
    if (this.orderData.shippingAddress) {
      this.validateShippingAddressDTO = ValidateService.validateAddress(
        this.orderData.shippingAddress
      );
    }
    // Đồng bộ address với shippingAddress
    if (this.orderData.shippingAddress) {
      this.orderData.address = this.orderData.shippingAddress;
    }
  }

  validateForm(): boolean {
    // Trigger all validations
    this.validateFullName();
    this.validateEmail();
    this.validatePhoneNumber();
    this.validateShippingAddress();

    const isValid =
      this.validateFullNameDTO.isValid &&
      this.validateEmailDTO.isValid &&
      this.validatePhoneNumberDTO.isValid &&
      this.validateShippingAddressDTO.isValid;

    if (!isValid) {
      this.notificationService.showWarning(
        '⚠️ Vui lòng kiểm tra lại thông tin đặt hàng!'
      );
    } else {
      this.notificationService.showInfo('📝 Đang xử lý đơn hàng...');
    }

    return isValid;
  }

  // onSubmit moved later to include address radius verification

  private loadUserData() {
    // First try to get current user if already loaded
    const currentUser = this.userService.getCurrentUser();
    if (currentUser) {
      this.setUserData(currentUser);
      return;
    }

    // Subscribe to user changes for real-time updates
    this.userService.user$.pipe(takeUntil(this.destroy$)).subscribe({
      next: (user) => {
        if (user) {
          this.setUserData(user);
        }
      },
      error: (error) => {
        console.error('Error in user subscription:', error);
      },
    });

    // Load from server if not available
    this.userService.getUser().subscribe({
      next: (user) => {
        this.setUserData(user);
      },
      error: (error) => {
        console.error('Lỗi khi tải thông tin người dùng:', error);
        this.notificationService.showError(
          'Không thể tải thông tin người dùng'
        );
        this.router.navigate(['/login']);
      },
    });
  }

  private setUserData(user: UserDTO): void {
    this.user = user;
    this.orderData.fullName = user.fullname;
    this.orderData.phoneNumber = user.phone_number;
    this.orderData.shippingAddress = user.address;
    this.orderData.address = user.address;

    // Validate after setting user data
    this.validateFullName();
    this.validatePhoneNumber();
    this.validateShippingAddress();
  }

  private async loadCartItems() {
    this.isLoading = true;
    // Use getAllEntries so we list each size separately
    const cart = this.cartService.getAllEntries();

    try {
      const items = await Promise.all(
        Array.from(cart.entries()).map(async ([compositeKey, cartItem]) => {
          // compositeKey: "<productId>:<size>"
          const [idStr, size] = String(compositeKey).split(':');
          const productId = Number(idStr);
          const product = await this.productService
            .getProductById(productId)
            .toPromise();
          const resolvedSize = cartItem.size || size || 'S';
          if (!product) {
            return {
              product: undefined as any,
              quantity: cartItem.quantity,
              size: resolvedSize,
            };
          }
          const displayPrice = this.cartService.calculatePriceWithSize(
            product.price,
            resolvedSize
          );
          const productForUi = {
            ...product,
            price: displayPrice,
          } as typeof product;
          return {
            product: productForUi,
            quantity: cartItem.quantity,
            size: resolvedSize,
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

  // Thêm hàm xử lý khi chọn địa chỉ từ autocomplete
  onAddressSelected(address: AddressPrediction): void {
    this.selectedAddressPrediction = address;
    this.orderData.shippingAddress = address.description;

    this.validateShippingAddress();
  }

  onAddressFocus(): void {
    this.showAddressError = true;
  }

  onSubmit() {
    if (!this.validateForm()) {
      return;
    }

    // User must select an address from suggestions (so we have a place_id)
    if (!this.selectedAddressPrediction) {
      this.notificationService.showWarning(
        'Vui lòng chọn địa chỉ giao hàng từ danh sách đề xuất.'
      );
      return;
    }

    this.orderData.totalMoney = this.getTotalPrice();

    this.addressService
      .isWithinDefaultRadius(this.selectedAddressPrediction.place_id)
      .then((isValid) => {
        if (isValid) {
          this.createOrder();
          this.isLoading = true;
        } else {
          this.notificationService.showWarning(
            `Địa chỉ không hỗ trợ giao hàng - Vui lòng chọn địa chỉ gần hơn!`
          );
        }
      });
  }

  createOrder(): void {
    const orderDTO: OrderRequestDTO = {
      // user_id: this.orderData.userId,
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

    this.orderService.createOrder(orderDTO).subscribe({
      next: (response: any) => {
        this.notificationService.showSuccess(
          'Đơn hàng đã được tạo thành công!'
        );

        this.orderId = response.order_id;
        this.momoInfoOrderDTO = {
          order_id: response.order_id ?? response.orderId,
          amount: this.getTotalPrice(),
        };
        this.createOrderDetail(this.orderId);
      },
      error: (error) => {
        this.notificationService.showError(
          'Lỗi khi tạo đơn hàng: ' + error.message
        );
      },
    });
  }

  createOrderDetail(orderId: number): void {
    this.cartItems.forEach((item) => {
      const orderDetailDTO: OrderDetailRequestDTO = {
        order_id: orderId,
        product_id: item.product.id,
        quantity: item.quantity,
        unit_price: item.product.price,
        total_money: item.product.price * item.quantity,
        size: item.size ?? 'S',
      };

      this.orderDetailService.createOrderDetail(orderDetailDTO).subscribe({
        next: (response) => {},
        error: (error) => {
          console.error('Lỗi khi tạo đơn hàng chi tiết:', error);
        },
      });
    });

    if (this.momoInfoOrderDTO) {
      this.momoService.createQR(this.momoInfoOrderDTO).subscribe({
        next: (response: CreateMomoResponse) => {
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
}
