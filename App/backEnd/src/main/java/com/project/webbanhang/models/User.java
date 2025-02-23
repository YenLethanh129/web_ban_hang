package com.project.webbanhang.models;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Date;
import java.util.List;

import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.authority.SimpleGrantedAuthority;

import jakarta.persistence.*;
import lombok.*;

@Entity
@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
@Table(name = "users")
@Builder
public class User extends BaseEntity implements UserDetails{
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
    
    @Column(name = "is_active")
    private boolean isActive;
    
    @Column(name = "date_of_birth")
    private Date dateOfBirth;
    
    @Column(name = "facebook_account_id")
    private Long facebookAccountId;
    
    @Column(name = "google_account_id")
    private Long googleAccountId;
    
    @ManyToOne
    @JoinColumn(name = "role_id")
    private Role role;

	@Override
	public Collection<? extends GrantedAuthority> getAuthorities() {
		List<SimpleGrantedAuthority> authorityList = new ArrayList<>();
		authorityList.add(new SimpleGrantedAuthority("ROLE_"+getRole().getName().toUpperCase()));
		//authorityList.add(new SimpleGrantedAuthority("ROLE_USER"));
		return authorityList;
	}

	@Override
	public String getUsername() {
		return phoneNumber;
	}
}
