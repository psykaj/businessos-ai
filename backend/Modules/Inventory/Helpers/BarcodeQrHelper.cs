using System.Text.Json;

namespace backend.Modules.Inventory.Helpers;

public static class BarcodeQrHelper
{
    public static string GenerateProductQrCode(Guid organizationId, Guid productId, string sku)
    {
        var payload = new
        {
            OrgId = organizationId,
            ProdId = productId,
            SKU = sku,
            Timestamp = DateTime.UtcNow
        };
        
        return $"BIZOS:PROD:{Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload)))}";
    }

    public static bool TryParseQrCode(string qrCode, out Guid productId, out string sku)
    {
        productId = Guid.Empty;
        sku = string.Empty;

        if (string.IsNullOrWhiteSpace(qrCode) || !qrCode.StartsWith("BIZOS:PROD:"))
            return false;

        try
        {
            var base64 = qrCode.Replace("BIZOS:PROD:", "");
            var json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64));
            using var doc = JsonDocument.Parse(json);
            
            if (doc.RootElement.TryGetProperty("ProdId", out var prodIdProp) &&
                doc.RootElement.TryGetProperty("SKU", out var skuProp))
            {
                if (Guid.TryParse(prodIdProp.GetString(), out productId))
                {
                    sku = skuProp.GetString() ?? string.Empty;
                    return true;
                }
            }
        }
        catch
        {
            // Ignore format errors
        }

        return false;
    }
}
