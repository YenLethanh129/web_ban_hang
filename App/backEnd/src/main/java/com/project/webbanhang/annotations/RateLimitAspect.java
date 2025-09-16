package com.project.webbanhang.annotations;

import lombok.RequiredArgsConstructor;
import org.aspectj.lang.ProceedingJoinPoint;
import org.aspectj.lang.annotation.Around;
import org.aspectj.lang.annotation.Aspect;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.stereotype.Component;
import org.springframework.web.context.request.RequestAttributes;
import org.springframework.web.context.request.RequestContextHolder;

import java.time.Duration;

@Aspect
@Component
@RequiredArgsConstructor
public class RateLimitAspect {

    private final StringRedisTemplate redisTemplate; // hoặc caffeine cache

    @Around("@annotation(rateLimited)")
    public Object limit(ProceedingJoinPoint pjp, RateLimited rateLimited) throws Throwable {
        String key = buildKey(pjp); // vd: user/IP + method
        int max = rateLimited.maxAttempts();
        Duration window = parseDuration(rateLimited.window());

        String redisKey = "ratelimit:" + key;
        Long count = redisTemplate.opsForValue().increment(redisKey);
        if (count == 1) {
            redisTemplate.expire(redisKey, window);
        }
        if (count > max) {
            throw new Exception("Quá số lần cho phép");
        }
        return pjp.proceed();
    }

    private String buildKey(ProceedingJoinPoint pjp) {
        // Lấy IP từ RequestContextHolder hoặc username từ SecurityContext
        String ip = RequestContextHolder.currentRequestAttributes()
                .getAttribute("REMOTE_ADDR", RequestAttributes.SCOPE_REQUEST).toString();
        return ip + ":" + pjp.getSignature().toShortString();
    }

    private Duration parseDuration(String window) {
        if (window.endsWith("h")) return Duration.ofHours(Long.parseLong(window.replace("h","")));
        if (window.endsWith("m")) return Duration.ofMinutes(Long.parseLong(window.replace("m","")));
        return Duration.ofSeconds(Long.parseLong(window.replace("s","")));
    }
}

