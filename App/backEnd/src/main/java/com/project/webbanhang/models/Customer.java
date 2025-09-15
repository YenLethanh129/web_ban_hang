package com.project.webbanhang.models;

import jakarta.persistence.*;
import lombok.*;
import org.hibernate.annotations.ManyToAny;

@Entity
@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
@Builder
@Table(name = "customers")
public class Customer extends BaseEntity{
//    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Id
    private Long id;

    @ManyToOne
    @JoinColumn(name = "user_id")
    private User user;

    @Column(name = "fullname")
    private String fullName;

    @Column(name = "phone_number")
    private String phoneNumber;

    private String email;

    private String address;
}
