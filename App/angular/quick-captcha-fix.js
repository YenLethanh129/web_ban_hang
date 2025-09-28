// ====================================
// 🚨 QUICK CAPTCHA FIX TEST
// ====================================



// Test if the math is correct
const question = "4 * 7 = ?";
const correctAnswer = 4 * 7;
const userInput = "28";




console.log(`🔢 Parsed input: ${parseInt(userInput)}`);
console.log(`🎯 Correct? ${parseInt(userInput) === correctAnswer}`);

// Force test the Angular component if accessible
function forceTestComponent() {
  

  try {
    // Try to get Angular component
    const appLogin = document.querySelector("app-login");
    if (appLogin) {
      

      // Try to trigger debug method if it exists
      const ngContext = appLogin.__ngContext__;
      if (ngContext) {
        // Find component instance
        for (let i = 0; i < ngContext.length; i++) {
          const item = ngContext[i];
          if (item && typeof item === "object" && item.debugCaptcha) {
            
            item.debugCaptcha();

            // Force generate new captcha
            if (item.generateNewCaptcha) {
              
              item.generateNewCaptcha();
            }

            break;
          }
        }
      }
    }
  } catch (error) {
    
  }
}

// Manual fix suggestions
function showFixSuggestions() {
  
  console.log("1. Làm mới trang (F5) và thử lại");
  console.log("2. Nhấp vào nút refresh captcha (🔄) để tạo câu hỏi mới");
  
  
  
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
