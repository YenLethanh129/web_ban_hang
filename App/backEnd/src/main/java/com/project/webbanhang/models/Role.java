package com.project.webbanhang.models;

import jakarta.persistence.*;
import lombok.*;

/**
 * 1 ADMIN
 * 2 MANAGER
 * 3 EMPLOYEE
 * 4 CUSTOMER
 * 5 GUEST
 */
@Entity
@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
@Builder
@Table(name = "roles")
public class Role extends BaseEntity{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    
    @Column(name = "name", length = 100)
    private String name;
    
    public static String ADMIN = "ADMIN";
    
    public static String CUSTOMER = "CUSTOMER";
}
