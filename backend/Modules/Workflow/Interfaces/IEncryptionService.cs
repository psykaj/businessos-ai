namespace backend.Modules.Workflow.Interfaces;

public interface IEncryptionService
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
    string MaskSensitiveData(string text, int visibleChars = 4);
}
