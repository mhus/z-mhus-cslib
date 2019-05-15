using System;
using System.IO;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto.Paddings;

namespace mhuscslib
{
    public class MBouncy
    {

        static readonly Encoding Encoding = Encoding.UTF8;

        public static string EncryptBlowfish(string secret, string key)
        {
            BlowfishEngine engine = new BlowfishEngine();

            PaddedBufferedBlockCipher cipher = new PaddedBufferedBlockCipher(engine);

            KeyParameter keyBytes = new KeyParameter(Encoding.GetBytes(key));

            cipher.Init(true, keyBytes);

            byte[] inB = Encoding.GetBytes(secret);
            byte[] outB = new byte[cipher.GetOutputSize(inB.Length)];
            int len1 = cipher.ProcessBytes(inB, 0, inB.Length, outB, 0);

            cipher.DoFinal(outB, len1);

            return Convert.ToBase64String(outB);
        }

        public static string DecryptBlowfish(string encrypted, string key)
        {
            BlowfishEngine engine = new BlowfishEngine();
            PaddedBufferedBlockCipher cipher = new PaddedBufferedBlockCipher(engine);

            cipher.Init(false, new KeyParameter(Encoding.GetBytes(key)));

            byte[] out1 = Convert.FromBase64String(encrypted);
            byte[] out2 = new byte[cipher.GetOutputSize(out1.Length)];
            int len2 = cipher.ProcessBytes(out1, 0, out1.Length, out2, 0);

            cipher.DoFinal(out2, len2);

            string ret = Encoding.GetString(out2);

            while (ret.Length > 0 && ret.EndsWith("\0", StringComparison.Ordinal))
                ret = ret.Remove(ret.Length - 1);

            return ret;
        }

        public static string EncryptRsa(string clearText, string publicKey)
        {
            var bytesToEncrypt = Encoding.UTF8.GetBytes(clearText);

            var encryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (var txtreader = new StringReader(publicKey))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();

                encryptEngine.Init(true, keyParameter);
            }

            MemoryStream stream = new MemoryStream();
            var blockSize = 117;
            var start = 0;
            while (true)
            {
                var len = bytesToEncrypt.Length - start;
                if (len > blockSize) len = blockSize;
                byte[] buf = encryptEngine.ProcessBlock(bytesToEncrypt, start, len);
                stream.Write(buf, 0, buf.Length);
                start = start + len;
                if (start >= bytesToEncrypt.Length) break;
            }

            var encrypted = Convert.ToBase64String(stream.ToArray());
            return encrypted;

        }

        public static string DecryptRsa(string base64Input, string privateKey)
        {
            var bytesToDecrypt = Convert.FromBase64String(base64Input);
            RsaPrivateCrtKeyParameters key;
            var decryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (var txtreader = new StringReader(privateKey))
            {
                key = (RsaPrivateCrtKeyParameters)new PemReader(txtreader).ReadObject();

                decryptEngine.Init(false, key);
            }

            var blockSize = 128;

            MemoryStream stream = new MemoryStream();
            var start = 0;
            while (true)
            {
                var len = bytesToDecrypt.Length - start;
                if (len > blockSize) len = blockSize;
                byte[] buf = decryptEngine.ProcessBlock(bytesToDecrypt, start, len);
                stream.Write(buf, 0, buf.Length);
                start = start + len;
                if (start >= bytesToDecrypt.Length) break;
            }
            var decrypted = Encoding.UTF8.GetString(stream.ToArray());
            return decrypted;
        }
    }
}
