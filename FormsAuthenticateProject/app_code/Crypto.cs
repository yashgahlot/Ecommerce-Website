using System;
using System.Security.Cryptography;
using System.Text;

public static class Crypto
{
    public static byte[] CreateSalt(int size = 16)
    {
        var salt = new byte[size];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    public static byte[] Sha256(string input, byte[] salt)
    {
        byte[] inputBytes = Encoding.Unicode.GetBytes(input ?? "");
        byte[] combined = new byte[inputBytes.Length + salt.Length];

        Buffer.BlockCopy(inputBytes, 0, combined, 0, inputBytes.Length);
        Buffer.BlockCopy(salt, 0, combined, inputBytes.Length, salt.Length);

        using (var sha = SHA256.Create())
        {
            return sha.ComputeHash(combined);
        }
    }


    public static bool AreEqual(byte[] a, byte[] b)
    {
        if (a == null || b == null || a.Length != b.Length) return false;
        for (int i = 0; i < a.Length; i++)
            if (a[i] != b[i]) return false;
        return true;
    }
}
