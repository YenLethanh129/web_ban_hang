package com.project.webbanhang.models;

import java.time.LocalDateTime;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Entity
@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
@Table(name = "tokens")

public class Token {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    
    @Column(name = "token", length = 255)
    private String token;
    
    @Column(name = "token_type", length = 50)
    private String tokenType;
    
    @Column(name = "expiration_date")
    private LocalDateTime expirationDate;
    
    private boolean revoked;
    private boolean expired;
    
    @ManyToOne
    @JoinColumn(name = "user_id")
    private User userId;
}
