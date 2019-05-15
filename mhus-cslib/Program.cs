using System;

namespace mhuscslib
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            testPemModel();
            //testBlowfish();

        }

        public static void testPemModel()
        {
            string text = "-----BEGIN PUBLIC KEY-----\n" +
            	"Format: X.509\n" +
            	"Ident: 87d1f379-ff74-4c5b-9a50-ff4f64708166\n" +
            	"PrivateKey: 908cdd9b-a874-4d96-9d63-2d2889121069\n" +
            	"Length: 1024\n" +
            	"Method: RSA-BC-01\n" +
            	"\n" +
            	"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQClnXj+W3ImS9\n" +
            	"z8s419XVw/AXfXU4OeMTfW+lyN81DPzeSpeMuZXDfDHJRD/i1k\n" +
            	"vddPWpaLtBKAfBB0/DCCtWgITrdVbncLT3aseihTL82oJ3Oh9q\n" +
            	"cp07AoJwxE79Czrk7AAR/0S8yG/ZLadFS0TDwSwPPtOODQOIPy\n" +
            	"XUh3aHg/IQIDAQAB\n" +
            	"\n" +
            	"-----END PUBLIC KEY-----\n" +
            	"Rest";

            PemBlock pem = new PemBlockModel().parse(text);

            Console.WriteLine(pem);
            Console.WriteLine(((PemBlockModel)pem).Rest);

            assertEquals(text, pem.ToString() + ((PemBlockModel)pem).Rest);
        }

        public static void testBlowfish()
        {
            string key = "hellohellohello";
            string text = "Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum " +
                "Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum " +
                "Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum.";
            string encrypted = MBouncy.EncryptBlowfish(text, key);

            Console.WriteLine("Encrypted");
            Console.WriteLine(encrypted);

            string decrypted = MBouncy.DecryptBlowfish(encrypted, key);

            Console.WriteLine("Decrypted");
            Console.WriteLine(decrypted);

            assertEquals(text, decrypted);
        }

        public static void assertEquals(string expected, string test)
        {
            if (expected == null && test == null) return;
            if (expected == null || !expected.Equals(test))
                throw new AssertationException();
        }
    }
}
