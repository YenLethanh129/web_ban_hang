import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { OrderConfirmResponseDTO } from '../../dtos/order.dto';
import { OrderService } from '../../services/order.service';
import { OrderDetailService } from '../../services/order.detail.service';
import { CommonModule, Location } from '@angular/common';
import { MomoIpnRequestDTO } from '../../dtos/momo.dto';
import { MomoService } from '../../services/momo.service';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-order-confirm',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './order-confirm.component.html',
  styleUrls: ['./order-confirm.component.scss'],
})
export class OrderConfirmComponent implements OnInit {
  orderResponse: any;
  listOrderDetails: any[] = [];
  isLoading: boolean = false; // ← Thêm property này

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private orderService: OrderService,
    private momoService: MomoService,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    this.isLoading = true; // ← Bắt đầu loading
    this.route.params.subscribe((params) => {
      const orderId = +params['id'];
      this.cleanUrl();
      this.getOrderDetails(orderId);
      this.ipnHandler(orderId);
    });
    this.cartService.clearCart(); // ← Xóa giỏ hàng sau khi đặt hàng thành công
  }

  private cleanUrl(): void {
    this.location.replaceState(`/order-confirm`);
  }

  getOrderDetails(orderId: number): void {
    this.orderService.getOrderConfirm(orderId).subscribe({
      next: (response: OrderConfirmResponseDTO) => {
        this.listOrderDetails = response.order_details;
        this.orderResponse = response;
        this.isLoading = false; // ← Kết thúc loading
      },
      error: (error) => {
        console.error('Error fetching order details:', error);
        this.isLoading = false; // ← Kết thúc loading khi có lỗi
      },
    });
  }

  ipnHandler(orderId: number): void {
    this.isLoading = true;
    const momoIpnRequestDTO: MomoIpnRequestDTO = {
      orderId: orderId.toString(),
      orderType: '',
      amount: 0,
      partnerCode: '',
      extraData: '',
      signature: '',
      transId: 0,
      responseTime: 0,
      resultCode: 0,
      message: '',
      payType: '',
      requestId: '',
      orderInfo: '',
    };
    this.momoService.ipnHandler(momoIpnRequestDTO).subscribe({
      next: (response: any) => {
        this.isLoading = false;
      },
      error: (error: any) => {
        console.error('Error in IPN Handler:', error);
        this.isLoading = false;
      },
    });
  }
}
