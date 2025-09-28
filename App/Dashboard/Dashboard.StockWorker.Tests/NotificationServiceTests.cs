using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.StockWorker.Models;
using Dashboard.StockWorker.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Dashboard.StockWorker.Tests;

public class NotificationServiceTests
{
    [Fact]
    public async Task SendLowStockEmail_WritesPickupFile_WithExpectedContent()
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), "Dashboard.StockWorker.Tests", Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);

        var inMemorySettings = new System.Collections.Generic.Dictionary<string, string?>
        {
            { "Email:UsePickupDirectory", "true" },
            { "Email:PickupDirectory", tempDir },
            { "Email:AlertsFromAddress", "from@example.com" },
            { "Email:AlertsToAddress", "to@example.com" }
        };

        var config = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
        var svc = new NotificationService(config, new NullLogger<NotificationService>());

        var alert = new StockAlert
        {
            IngredientId = 1,
            IngredientName = "Test Ingredient",
            BranchId = 2,
            BranchName = "Main Branch",
            CurrentStock = 3,
            SafetyStock = 10
        };

        // Act
        await svc.SendLowStockEmailAsync(alert);

        // Assert
        var files = Directory.GetFiles(tempDir, "*.html").OrderByDescending(f => f).ToList();
        Assert.True(files.Count > 0, "No pickup files were written");

        var content = await File.ReadAllTextAsync(files.First());
        Assert.Contains("Test Ingredient", content);
        Assert.Contains("Main Branch", content);
        Assert.Contains("Current stock", content);

        // Cleanup
        try { Directory.Delete(tempDir, true); } catch { }
    }
}
