using System;

namespace mhuscslib
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

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

        }
    }
}
