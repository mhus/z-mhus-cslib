using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

namespace mhuscslib
{
    public class MBouncy
    {
        public string EncryptRsa(string clearText, string publicKey)
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

        public string DecryptRsa(string base64Input, string privateKey)
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
