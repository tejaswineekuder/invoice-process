using invoice_process.DTOs;

namespace invoice_process.Helper
{
    public static class MockResponse
    {
        public static InvoiceClassificationDto CreateMockResponse()
        {
            // This creates a mock response with a random classification and risk level
            var jsonPayload = new InvoiceClassificationDto
            {
                Classification = RandomClassificationSelector(),
                RiskLevel = RandomRiskLevelSelector()
            };
            return jsonPayload;
        }

        public static string RandomClassificationSelector()
        {
            var classifications = new List<string> { "WaterLeakDetection", "RoofingTileReplacement", "FireDamagedWallRepair", "BrokenDoorRepair", "Basement Waterproofing" };
            var random = new Random();
            int index = random.Next(classifications.Count);
            return classifications[index];
        }

        public static string RandomRiskLevelSelector()
        {
            var classifications = new List<string> { "High", "Low" };
            var random = new Random();
            int index = random.Next(classifications.Count);
            return classifications[index];
        }
    }
}
