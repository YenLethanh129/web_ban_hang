export const WebEnvironment = {
  production: true,
  apiUrl: 'https://your-production-api.com/api/v1',
  momoUrl: 'https://your-production-api.com/api/momo',

  // Security Configuration - Production Settings
  security: {
    // Session settings - More restrictive for production
    sessionTimeout: 15 * 60 * 1000, // 15 minutes (shorter for production)
    sessionWarningTime: 2 * 60 * 1000, // 2 minutes warning
    maxSessionDuration: 4 * 60 * 60 * 1000, // 4 hours max (shorter)
    sessionCheckInterval: 30 * 1000, // Check every 30 seconds

    // Authentication settings - Stricter for production
    tokenExpiryBuffer: 2 * 60 * 1000, // 2 minutes buffer
    maxLoginAttempts: 3, // Fewer attempts allowed
    lockoutDuration: 30 * 60 * 1000, // 30 minutes lockout (longer)

    // Password policy - Stronger requirements
    passwordPolicy: {
      minLength: 16, // Longer minimum
      requireUppercase: true,
      requireLowercase: true,
      requireNumbers: true,
      requireSpecialChars: true,
      minCharacterGroups: 4, // All groups required
      preventCommonPasswords: true,
      preventPersonalInfo: true,
      maxPasswordAge: 60 * 24 * 60 * 60 * 1000, // 60 days (shorter)
    },

    // Captcha settings - More aggressive
    captcha: {
      enableAfterFailedAttempts: 2, // Show earlier
      expiryTime: 3 * 60 * 1000, // 3 minutes (shorter)
      type: 'image', // More secure image captcha
    },

    // HTTP security - Production settings
    http: {
      timeout: 15000, // 15 seconds (shorter)
      retryAttempts: 1, // Fewer retries
      retryDelay: 2000, // 2 seconds
      enableCSRFProtection: true,
      enableRequestEncryption: true, // Enable for production
    },

    // Content Security Policy - Strict
    csp: {
      enableStrictMode: true,
      allowedDomains: ['your-production-api.com'], // Only production domains
      blockMixedContent: true,
    },

    // Rate limiting - Stricter limits
    rateLimit: {
      maxRequestsPerMinute: 60, // Lower limit
      maxLoginAttemptsPerHour: 5, // Fewer login attempts
      blockDuration: 5 * 60 * 1000, // 5 minutes block
    },
  },

  // Feature flags - Production ready features
  features: {
    enableTwoFactorAuth: true, // Enable for production
    enableBiometric: true, // Enable for production
    enableSocialLogin: true, // If needed
    enableRememberMe: false, // Disable for security
    enableSessionSharing: false, // Keep disabled
    enableOfflineMode: false, // Keep disabled
  },

  // Monitoring and logging - Production settings
  monitoring: {
    enableSecurityLogging: true,
    enablePerformanceTracking: true,
    enableErrorReporting: true,
    logLevel: 'warn', // Less verbose for production
  },
};
