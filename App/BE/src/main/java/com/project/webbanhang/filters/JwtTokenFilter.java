package com.project.webbanhang.filters;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.data.util.Pair;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.web.authentication.UsernamePasswordAuthenticationFilter;
import org.springframework.security.web.authentication.WebAuthenticationDetails;
import org.springframework.security.web.authentication.WebAuthenticationDetailsSource;
import org.springframework.stereotype.Component;
import org.springframework.web.filter.OncePerRequestFilter;

import com.project.webbanhang.components.JwtTokenUtil;
import com.project.webbanhang.models.User;

import io.jsonwebtoken.JwtException;
import io.micrometer.common.lang.NonNull;
import jakarta.servlet.FilterChain;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import lombok.RequiredArgsConstructor;

@Component
@RequiredArgsConstructor
public class JwtTokenFilter extends OncePerRequestFilter{

	@Value("${api.prefix}")
	private String apiPrefix;
	
	private final UserDetailsService userDetailsService;
	private final JwtTokenUtil jwtTokenUtil;
	
	@Override
	protected void doFilterInternal(
			@NonNull HttpServletRequest request, 
			@NonNull HttpServletResponse response, 
			@NonNull FilterChain filterChain)
			throws ServletException, IOException {
		try {
			// Bỏ qua các endpoint không yêu cầu xác thực
	        if (isByPassToken(request)) {
	            filterChain.doFilter(request, response);
	            return;
	        }

			String authHeader = request.getHeader("Authorization");
			if (authHeader == null || !authHeader.startsWith("Bearer ")) {
			    response.sendError(HttpServletResponse.SC_UNAUTHORIZED, "Unauthorized: Missing or invalid Authorization header");
			    return;
			}
			
			final String token = authHeader.substring(7);
	        String phoneNumber = jwtTokenUtil.extractPhoneNumber(token);
				
			if (phoneNumber != null && SecurityContextHolder.getContext().getAuthentication() == null) {
			User userDetails = (User) userDetailsService.loadUserByUsername(phoneNumber);
				if (jwtTokenUtil.validateToken(token, userDetails)) {
					UsernamePasswordAuthenticationToken authenticationToken = 
							new UsernamePasswordAuthenticationToken(userDetails, null, userDetails.getAuthorities());
					authenticationToken.setDetails(new WebAuthenticationDetailsSource().buildDetails(request));
					SecurityContextHolder.getContext().setAuthentication(authenticationToken);
				}
			}
			
			filterChain.doFilter(request, response);
		} catch (JwtException e) {
		    response.sendError(HttpServletResponse.SC_UNAUTHORIZED, "Unauthorized: Invalid JWT token");
		    return;
		} catch (Exception e) {
		    response.sendError(HttpServletResponse.SC_INTERNAL_SERVER_ERROR, "Internal Server Error");
		    return;
		}
		
	}	
	
	private boolean isByPassToken(@NonNull HttpServletRequest request) {
		final List<Pair<String, String>> byPassTokens = new ArrayList<>();
		
		 // Danh sách các endpoint được bỏ qua xác thực
		byPassTokens.add(Pair.of(String.format("%s/products",apiPrefix), "GET"));
		byPassTokens.add(Pair.of(String.format("%s/categories",apiPrefix), "GET"));
		byPassTokens.add(Pair.of(String.format("%s/users",apiPrefix), "POST"));

		for (Pair<String, String> byPassToken : byPassTokens) {
		    if (request.getServletPath().contains(byPassToken.getFirst()) &&
		    		request.getMethod().equals(byPassToken.getSecond())) {
		        return true;
		    }
		}
		return false;
	}
}
