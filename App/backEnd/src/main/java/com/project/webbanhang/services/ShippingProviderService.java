package com.project.webbanhang.services;

import com.project.webbanhang.models.ShippingProvider;
import com.project.webbanhang.repositories.ShippingProviderRepository;
import com.project.webbanhang.services.Interfaces.IShippingProviderService;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.Optional;

@Service
@RequiredArgsConstructor
public class ShippingProviderService implements IShippingProviderService {
    private final ShippingProviderRepository shippingProviderRepository;

    @Override
    public Optional<ShippingProvider> getShippingProviderById(Long id) {
        Optional<ShippingProvider> shippingProvider = shippingProviderRepository.findById(id);
        return shippingProvider;
    }
}
