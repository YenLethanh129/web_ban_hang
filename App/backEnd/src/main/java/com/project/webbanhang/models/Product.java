package com.project.webbanhang.models;

import jakarta.persistence.*;
import lombok.*;

@Entity
@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
@Builder
@Table(name = "products")
public class Product extends BaseEntity {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name = "price", nullable = false)
    private Float price;

    @ManyToOne
    @JoinColumn(name = "category_id")
    private Category category;

    @Column(name = "is_active", nullable = false)
    private Long isActive;

    @ManyToOne
    @JoinColumn
    private Tax tax;

    @Column(name = "description", nullable = false)
    private String description;

    @Column(name = "name", nullable = false, length = 255)
    private String name;

    @Column(name = "thumbnail", length = 255)
    private String thumbnail;
}
