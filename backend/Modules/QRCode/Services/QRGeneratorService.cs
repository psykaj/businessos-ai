using backend.Modules.QRCode.Interfaces;
using QRCoder;

namespace backend.Modules.QRCode.Services;

public class QRGeneratorService : IQRGeneratorService
{
    public byte[] GeneratePng(
        string content,
        string foregroundColorHex,
        string backgroundColorHex,
        int pixelsPerModule,
        string errorCorrectionLevel)
    {
        var level = GetEccLevel(errorCorrectionLevel);

        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(content, level);
        
        using var qrCode = new PngByteQRCode(qrCodeData);
        // Note: PngByteQRCode doesn't support custom colors directly in some older versions, 
        // but since 1.4.3+ we can pass them in GetGraphic.
        // QRCoder uses darkColor / lightColor as byte arrays for RGB, let's use the simplest override.
        // Actually, PngByteQRCode has: byte[] GetGraphic(int pixelsPerModule, byte[] darkColorRgba, byte[] lightColorRgba)
        
        var darkColor = HexToRgba(foregroundColorHex);
        var lightColor = HexToRgba(backgroundColorHex);

        return qrCode.GetGraphic(pixelsPerModule, darkColor, lightColor);
    }

    public string GenerateSvg(
        string content,
        string foregroundColorHex,
        string backgroundColorHex,
        int pixelsPerModule,
        string errorCorrectionLevel)
    {
        var level = GetEccLevel(errorCorrectionLevel);

        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(content, level);
        
        using var qrCode = new SvgQRCode(qrCodeData);
        // SvgQRCode supports string hex colors natively
        return qrCode.GetGraphic(pixelsPerModule, foregroundColorHex, backgroundColorHex);
    }

    private QRCodeGenerator.ECCLevel GetEccLevel(string levelStr)
    {
        return levelStr.ToUpper() switch
        {
            "L" => QRCodeGenerator.ECCLevel.L,
            "M" => QRCodeGenerator.ECCLevel.M,
            "Q" => QRCodeGenerator.ECCLevel.Q,
            "H" => QRCodeGenerator.ECCLevel.H,
            _ => QRCodeGenerator.ECCLevel.M
        };
    }

    private byte[] HexToRgba(string hex)
    {
        if (hex.StartsWith("#"))
            hex = hex.Substring(1);

        if (hex.Length == 6)
        {
            return new byte[]
            {
                Convert.ToByte(hex.Substring(0, 2), 16),
                Convert.ToByte(hex.Substring(2, 2), 16),
                Convert.ToByte(hex.Substring(4, 2), 16),
                255 // Alpha
            };
        }
        else if (hex.Length == 8)
        {
            return new byte[]
            {
                Convert.ToByte(hex.Substring(0, 2), 16),
                Convert.ToByte(hex.Substring(2, 2), 16),
                Convert.ToByte(hex.Substring(4, 2), 16),
                Convert.ToByte(hex.Substring(6, 2), 16)
            };
        }

        // Fallback to black/white
        return new byte[] { 0, 0, 0, 255 };
    }
}
