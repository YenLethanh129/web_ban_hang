package com.project.webbanhang.components;

import java.util.Locale;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.MessageSource;
import org.springframework.stereotype.Component;
import org.springframework.web.servlet.LocaleResolver;

import com.project.webbanhang.utils.WebUtil;

import jakarta.servlet.http.HttpServletRequest;
import lombok.RequiredArgsConstructor;

@RequiredArgsConstructor
@Component
public class LocalizationUtil {
	@Autowired
	private final MessageSource messageSource;
	
	private final LocaleResolver localeResolver;
	
	public String getLocalizedMessage(String messageKey, Object ... params) {
		HttpServletRequest request = WebUtil.getCurrentRequest();
		Locale locale = localeResolver.resolveLocale(request);
		return messageSource.getMessage(messageKey, params, locale);
	}
}
