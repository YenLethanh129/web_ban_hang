package com.project.webbanhang.repositories;

import com.project.webbanhang.models.PaymentMethod;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;

public interface PaymentMethodRepository extends JpaRepository<PaymentMethod, Long> {
    Optional<PaymentMethod> findByName(String nameStatus);
}
