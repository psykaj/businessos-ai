using System.Security.Cryptography;
using System.Text;
using backend.Modules.Workflow.Interfaces;
using Microsoft.Extensions.Configuration;

namespace backend.Modules.Workflow.Services;

public class EncryptionService : IEncryptionService
{
    private readonly byte[] _key;

    public EncryptionService(IConfiguration configuration)
    {
        var secret = configuration["EncryptionSettings:SecretKey"] ?? "BusinessOS_AI_Workflow_Encryption_Secret_Key_32bytes!";
        _key = SHA256.HashData(Encoding.UTF8.GetBytes(secret));
    }

    public string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText)) return plainText;

        using var aes = Aes.Create();
        aes.Key = _key;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        ms.Write(aes.IV, 0, aes.IV.Length);

        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText)) return cipherText;

        try
        {
            var fullCipher = Convert.FromBase64String(cipherText);
            using var aes = Aes.Create();
            aes.Key = _key;

            var iv = new byte[aes.BlockSize / 8];
            var cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            using var decryptor = aes.CreateDecryptor(aes.Key, iv);
            using var ms = new MemoryStream(cipher);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
        catch
        {
            // If decryption fails, return as-is (or empty) to avoid breaking unencrypted legacy records
            return cipherText;
        }
    }

    public string MaskSensitiveData(string text, int visibleChars = 4)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        if (text.Length <= visibleChars) return new string('*', text.Length);

        var visible = text.Substring(text.Length - visibleChars);
        return new string('*', text.Length - visibleChars) + visible;
    }
}
