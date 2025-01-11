package com.project.webbanhang.models;

import jakarta.persistence.*;
import lombok.*;

@Entity
@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
@Table(name = "social_accounts")

public class SocialAccount {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    
    @Column(name = "provider", length = 20, nullable = false)
    private String provider;
    
    @Column(name = "provider_id", length = 100, nullable = false)
    private Long providerId;
    
    @Column(name = "email", length = 150)
    private String email;
    
    @Column(name = "name", length = 100)
    private String name;
    
    @ManyToOne
    @JoinColumn(name = "user_id")
    private User userId;
}
