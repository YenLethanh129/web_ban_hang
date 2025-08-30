package com.project.webbanhang.services;

import com.project.webbanhang.models.Order;
import com.project.webbanhang.models.OrderPayment;
import com.project.webbanhang.models.PaymentMethod;
import com.project.webbanhang.models.PaymentStatus;
import com.project.webbanhang.repositories.OrderPaymentRepository;
import com.project.webbanhang.repositories.PaymentMethodRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class OrderPaymentService implements IOrderPaymentService{
    private final PaymentMethodRepository paymentMethodRepository;
    private final OrderPaymentRepository orderPaymentRepository;

    @Override
    public void createOrderPayment(Order order, String paymentMethod) throws Exception {
        Optional<PaymentMethod> existsPaymentMethod = paymentMethodRepository.findByName(paymentMethod);
        if (existsPaymentMethod.isEmpty()) {
            throw new Exception("Can't not found Payment Method");
        }

        PaymentStatus paymentStatus = PaymentStatus.builder()
                .id(1L)
                .name(PaymentStatus.PENDING)
                .build();

        OrderPayment orderPayment = OrderPayment.builder()
                .order(order)
                .paymentMethod(existsPaymentMethod.get())
                .paymentStatus(paymentStatus)
                .amount(order.getTotalMoney())
                .paymentDate(LocalDateTime.now())
                .transactionId(null)
                .notes(order.getNotes())
                .build();

        orderPaymentRepository.save(orderPayment);
    }
}
