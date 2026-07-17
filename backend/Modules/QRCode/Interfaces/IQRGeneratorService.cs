namespace backend.Modules.QRCode.Interfaces;

public interface IQRGeneratorService
{
    byte[] GeneratePng(
        string content,
        string foregroundColorHex,
        string backgroundColorHex,
        int pixelsPerModule,
        string errorCorrectionLevel);

    string GenerateSvg(
        string content,
        string foregroundColorHex,
        string backgroundColorHex,
        int pixelsPerModule,
        string errorCorrectionLevel);
}
