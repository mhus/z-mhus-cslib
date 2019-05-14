using System.Collections.Generic;
using System.Text;

namespace mhuscslib
{
    public class PemBlock
    { 

        public const string BLOCK_CIPHER = "CIPHER";
        public const string METHOD = "Method";
        public const string BLOCK_SIGN = "SIGNATURE";
        public const string BLOCK_PRIV = "PRIVATE KEY";
        public const string BLOCK_PUB = "PUBLIC KEY";
        public const string LENGTH = "Length";
        public const string FORMAT = "Format";
        public const string IDENT = "Ident";
        public const string STRING_ENCODING = "Encoding";
        public const string PRIV_ID = "PrivateKey"; // private key for asymmetric algorithms
        public const string PUB_ID = "PublicKey"; // public key for asymmetric algorithms
        public const string KEY_ID = "Key"; // for symmetric algorithms
        public const string SYMMETRIC = "Symmetric"; // set a hint if the algorithm is symmetric
        public const string DESCRIPTION = "Description";
        public const string CREATED = "Created";
        public const string ENCRYPTED = "Encrypted";
        public const string ENC_BLOWFISH = "blowfish";
        public const string BLOCK_HASH = "HASH";
        public const string EMBEDDED = "Embedded"; // declare embedded blocks in encrypted content, set to true
        public const string BLOCK_CONTENT = "CONTENT";

        protected const int BLOCK_WIDTH = 50;

        protected string name;
        protected string block;
        protected Dictionary<string,string> parameters = new Dictionary<string,string>();

        public string Name { get => name; set => name = value; }
        public string Block { get => block; set => block = value; }

    }


}