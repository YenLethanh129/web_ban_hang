package com.project.webbanhang.services.Interfaces;

import com.project.webbanhang.models.ShippingProvider;

import java.util.Optional;

public interface IShippingProviderService {
    Optional<ShippingProvider> getShippingProviderById(Long id);
}
