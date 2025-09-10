package com.project.webbanhang.dtos.momo;

import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.NotNull;
import lombok.Data;

@Data
/*
* Nhận dữ liệu từ frontend khi tạo yêu cầu thanh toán.
* Có validation để tránh lỗi request thiếu tiền hoặc tiền nhỏ hơn mức tối thiểu.
* */
public class CreatePaymentInputDTO {
    @NotNull @Min(1000)
    private Long amount;
    private String orderId;
    private String orderInfo;
}
