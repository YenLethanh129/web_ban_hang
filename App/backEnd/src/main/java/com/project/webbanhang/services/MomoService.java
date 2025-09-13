package com.project.webbanhang.services;

import com.project.webbanhang.api.MomoApi;
import com.project.webbanhang.dtos.momo.CreateMomoRequestDTO;
import com.project.webbanhang.dtos.momo.MomoInfoOrderDTO;
import com.project.webbanhang.dtos.momo.MomoIpnRequestDTO;
import com.project.webbanhang.models.orders.Order;
import com.project.webbanhang.models.orders.OrderPayment;
import com.project.webbanhang.models.orders.OrderStatus;
import com.project.webbanhang.models.PaymentStatus;
import com.project.webbanhang.repositories.OrderPaymentRepository;
import com.project.webbanhang.repositories.OrderRepository;
import com.project.webbanhang.response.CreateMomoResponse;
import com.project.webbanhang.response.OrderResponse;
import com.project.webbanhang.services.Interfaces.IMomoService;
import com.project.webbanhang.services.orders.OrderService;
import com.project.webbanhang.utils.HmacUtil;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;

import java.util.Optional;
import java.util.UUID;

@Service
@RequiredArgsConstructor
@Slf4j
public class MomoService implements IMomoService {
    @Value(value = "${momo.partnerCode}")
    private String PARTNER_CODE;
    @Value(value = "${momo.accessKey}")
    private String ACCESS_KEY;
    @Value(value = "${momo.secretKey}")
    private String SECRET_KEY;
    // Tra ve trang web ket qua thanh cong
    @Value(value = "${momo.redirectUrl}")
    private String REDIRECT_URL;
    // Sever to Sever - Truyen thong tin
    @Value(value = "${momo.ipnUrl}")
    private String IPN_URL;
    @Value(value = "${momo.requestType}")
    private String REQUEST_TYPE;

    private final MomoApi momoApi;
    private final OrderRepository orderRepository;
    private final OrderPaymentRepository orderPaymentRepository;
    private final OrderService orderService;

    @Override
    public CreateMomoResponse createQR(MomoInfoOrderDTO momoInfoOrderDTO){

        // Lay thong tin don thanh toan
        String orderId = momoInfoOrderDTO.getOrderId().toString();

        String redirectUrl = REDIRECT_URL + "/" + momoInfoOrderDTO.getOrderId();
        Long amount = momoInfoOrderDTO.getAmount();
        String orderInfo = "Thanh toan don hang: " + orderId;
        String requestId = UUID.randomUUID().toString();
        String extraData = "Khong co voucher nao duoc tim thay!";

        String rawData =
                "accessKey=" + ACCESS_KEY +
                        "&amount=" + amount +
                        "&extraData=" + extraData +
                        "&ipnUrl=" + IPN_URL +
                        "&orderId=" + orderId +
                        "&orderInfo=" + orderInfo +
                        "&partnerCode=" + PARTNER_CODE +
                        "&redirectUrl=" + redirectUrl +
                        "&requestId=" + requestId +
                        "&requestType=" + REQUEST_TYPE;
        String signature;
        try {
            signature = HmacUtil.hmacSHA256(rawData, SECRET_KEY);
        } catch (Exception e) {
            throw new RuntimeException("Co loi khi tao chu ki dien tu: ", e);
        }

        CreateMomoRequestDTO requestDTO = CreateMomoRequestDTO.builder()
                .partnerCode(PARTNER_CODE)
                .requestId(requestId)
                .requestType(REQUEST_TYPE)
                .ipnUrl(IPN_URL)
                .redirectUrl(redirectUrl)
                .orderId(orderId)
                .orderInfo(orderInfo)
                .lang("vi")
                .amount(amount)
                .extraData(extraData)
                .signature(signature)
                .build();

        return momoApi.createMomoQR(requestDTO);
    }

    @Override
    public OrderResponse ipnHandler(MomoIpnRequestDTO momoIpnRequestDTO) {
        Optional<Order> existingOrder = orderRepository.findById(Long.parseLong(momoIpnRequestDTO.getOrderId()));
        Optional<OrderPayment> existingOrderPayment = orderPaymentRepository.findByOrderId(Long.parseLong(momoIpnRequestDTO.getOrderId()));

        Order order = new Order();
        if (existingOrder.isPresent() && existingOrderPayment.isPresent()) {
            PaymentStatus paymentStatus = PaymentStatus.builder()
                    .id(2L)
                    .name(PaymentStatus.PAID)
                    .build();

            OrderStatus orderStatus = OrderStatus.builder()
                    .id(2L)
                    .name(OrderStatus.PROCESSING)
                    .build();

            existingOrder.get().setStatus(orderStatus);
            existingOrderPayment.get().setPaymentStatus(paymentStatus);

            orderPaymentRepository.save(existingOrderPayment.get());
            order = orderRepository.save(existingOrder.get());
        }
        return orderService.mapOrderToOrderResponse(order);
    }
}
