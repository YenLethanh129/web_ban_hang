package com.project.webbanhang.models;

import java.util.Date;

import jakarta.persistence.*;
import lombok.*;

@Entity
@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
@Table(name = "users")
@Builder
public class User extends BaseEntity{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name = "fullname", length = 100)
    private String fullName;
    
    @Column(name = "phone_number", length = 20, nullable = false)
    private String phoneNumber;
    
    @Column(name = "address", length = 200)
    private String address;
    
    @Column(name = "password", length = 200, nullable = false)
    private String password;
    
    private boolean active;
    
    @Column(name = "date_of_birth")
    private Date dateOfBirth;
    
    @Column(name = "facebook_account_id")
    private Long facebookAccountId;
    
    @Column(name = "google_account_id")
    private Long googleAccountId;
    
    @ManyToOne
    @JoinColumn(name = "role_id")
    private Role role;
}
