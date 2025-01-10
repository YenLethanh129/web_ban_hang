package com.project.webbanhang.components;

import java.beans.Encoder;
import java.security.SecureRandom;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;
import java.util.function.Function;

import javax.crypto.SecretKey;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Component;

import com.project.webbanhang.models.User;

import io.jsonwebtoken.Claims;
import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.Jwts.SIG;
import io.jsonwebtoken.io.Decoders;
import io.jsonwebtoken.io.Encoders;
import io.jsonwebtoken.security.Keys;
import lombok.RequiredArgsConstructor;

@Component
@RequiredArgsConstructor
public class JwtTokenUtil {
	@Value("${jwt.expiration}")
	private int expiration;
	
	@Value("${jwt.secretKey}")
	private String secretKey;
	
	public String generateToken(User user) {
		Map<String, Object> claims = new HashMap<>();
		claims.put("phoneNumber", user.getPhoneNumber());
		try {
			String token = Jwts.builder()
					.claims(claims)
					.subject(user.getPhoneNumber())
					.issuedAt(new Date(System.currentTimeMillis()))
					.expiration(new Date(System.currentTimeMillis() + expiration * 1000L))
					.signWith(getSignInKey(), Jwts.SIG.HS256)
					.compact();
			return token;
		} catch (Exception e) {
			throw new RuntimeException("Cannot create jwt token, error: " + e.getMessage());

		}
	}
	
	public Claims extractAllClaims(String token) {
		return Jwts.parser()
				.verifyWith(getSignInKey())
				.build()
				.parseSignedClaims(token)
				.getPayload();
	}
	
	private SecretKey getSignInKey() {
		byte[] bytes = Decoders.BASE64.decode(secretKey);
		return Keys.hmacShaKeyFor(bytes);
	}
	
	private <T> T extractClaim(String token, Function<Claims, T> claimsResolver) {
		final Claims claims = this.extractAllClaims(token);
		return claimsResolver.apply(claims);
	}
	
	public boolean isTokenExpired(String token) {
		Date expirationDate = this.extractClaim(token, Claims::getExpiration);
		return expirationDate.before(new Date());
	}
}
