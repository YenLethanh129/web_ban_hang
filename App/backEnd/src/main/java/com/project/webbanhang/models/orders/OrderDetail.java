package com.project.webbanhang.models.orders;

import com.project.webbanhang.models.BaseEntity;
import com.project.webbanhang.models.Product;
import jakarta.persistence.*;
import lombok.*;

@Entity
@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
@Builder
@Table(name = "order_details")
public class OrderDetail extends BaseEntity {
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	private Long id;

	@ManyToOne
	@JoinColumn(name = "order_id", nullable = false)
	private Order order;

	@Column(name = "quantity")
	private Long quantity;

	@ManyToOne
	@JoinColumn(name = "product_id", nullable = false)
	private Product product;

	@Column(name = "size")
	private String size;

	@Column(name = "note")
	private String note;

	@Column(name = "total_amount")
	private Long totalAmount;

	@Column(name = "unit_price")
	private Long unitPrice;
}
