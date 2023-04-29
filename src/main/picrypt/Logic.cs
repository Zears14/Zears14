using System;
using System.IO;
using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {
        string inputFile = @"C:\input.jpg";
        string encryptedFile = @"C:\encrypted.bin";
        string decryptedFile = @"C:\decrypted.jpg";
        string password = "MySecretPassword";

        // Encrypt the file
        EncryptFile(inputFile, encryptedFile, password);

        // Decrypt the file
        DecryptFile(encryptedFile, decryptedFile, password);
    }

    static void EncryptFile(string inputFile, string outputFile, string password)
    {
        using (FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
        using (FileStream fsOutput = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);

            var key = new Rfc2898DeriveBytes(password, salt, 10000);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);

            fsOutput.Write(salt, 0, salt.Length);

            using (CryptoStream cs = new CryptoStream(fsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                byte[] buffer = new byte[8192];
                int read;
                while ((read = fsInput.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cs.Write(buffer, 0, read);
                }
            }
        }
    }

    static void DecryptFile(string inputFile, string outputFile, string password)
    {
        using (FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
        using (FileStream fsOutput = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] salt = new byte[16];
            fsInput.Read(salt, 0, salt.Length);

            var key = new Rfc2898DeriveBytes(password, salt, 10000);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);

            using (CryptoStream cs = new CryptoStream(fsOutput, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                byte[] buffer = new byte[8192];
                int read;
                while ((read = fsInput.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cs.Write(buffer, 0, read);
                }
            }
        }
    }
}

