package com.project.webbanhang.services;

import com.project.webbanhang.models.Customer;
import com.project.webbanhang.repositories.CustomerRepository;
import com.project.webbanhang.services.Interfaces.ICustomerService;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.Optional;

@Service
@RequiredArgsConstructor
public class CustomerService implements ICustomerService {
    private final CustomerRepository customerRepository;

    @Override
    public Customer createCustomer(Customer customer) throws Exception {
        Optional<Customer> existsByUserId = customerRepository.findByUserId(customer.getId());
        if (existsByUserId.isEmpty()) {
            customerRepository.save(customer);
        }

        return customer;
    }
}
