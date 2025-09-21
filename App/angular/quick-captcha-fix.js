// ====================================
// 🚨 QUICK CAPTCHA FIX TEST
// ====================================

console.log("🚨 Quick captcha fix test...");

// Test if the math is correct
const question = "4 * 7 = ?";
const correctAnswer = 4 * 7;
const userInput = "28";

console.log(`❓ Question: ${question}`);
console.log(`✅ Correct answer: ${correctAnswer}`);
console.log(`👤 User input: "${userInput}"`);
console.log(`🔢 Parsed input: ${parseInt(userInput)}`);
console.log(`🎯 Correct? ${parseInt(userInput) === correctAnswer}`);

// Force test the Angular component if accessible
function forceTestComponent() {
  console.log("\n🔧 Attempting to force test component...");

  try {
    // Try to get Angular component
    const appLogin = document.querySelector("app-login");
    if (appLogin) {
      console.log("✅ Found app-login element");

      // Try to trigger debug method if it exists
      const ngContext = appLogin.__ngContext__;
      if (ngContext) {
        // Find component instance
        for (let i = 0; i < ngContext.length; i++) {
          const item = ngContext[i];
          if (item && typeof item === "object" && item.debugCaptcha) {
            console.log("✅ Found component with debugCaptcha method");
            item.debugCaptcha();

            // Force generate new captcha
            if (item.generateNewCaptcha) {
              console.log("🔄 Generating new captcha...");
              item.generateNewCaptcha();
            }

            break;
          }
        }
      }
    }
  } catch (error) {
    console.log("❌ Could not access component:", error.message);
  }
}

// Manual fix suggestions
function showFixSuggestions() {
  console.log("\n💡 MANUAL FIX SUGGESTIONS:");
  console.log("1. Làm mới trang (F5) và thử lại");
  console.log("2. Nhấp vào nút refresh captcha (🔄) để tạo câu hỏi mới");
  console.log("3. Đảm bảo nhập chính xác: 4 * 7 = 28");
  console.log("4. Kiểm tra console.log khi submit form");
  console.log("5. Thử đăng nhập sai 3 lần để trigger captcha mới");
}

// Auto run
forceTestComponent();
showFixSuggestions();

// Make available globally
window.quickCaptchaTest = {
  force: forceTestComponent,
  suggestions: showFixSuggestions,
};

console.log("\n🔧 Use window.quickCaptchaTest.force() to test component");
