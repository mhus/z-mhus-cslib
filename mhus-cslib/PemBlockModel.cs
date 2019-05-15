using System;
using System.Text;

namespace mhuscslib
{
    public class PemBlockModel : PemBlock
    {
        private string rest;

        public PemBlockModel()
        {
        }

        public PemBlockModel(string name)
        {
            this.name = name;
        }

        public PemBlockModel(string name, string block)
        {
            this.name = name;
            this.block = block;
        }

        public PemBlockModel(PemBlock clone)
        {
            this.name = clone.Name;
            this.block = clone.Block;
        }

        public PemBlockModel parse(string block)
        {

            int p = block.IndexOf("-----BEGIN ", StringComparison.Ordinal);
            if (p < 0) throw new ParseException("start of block not found");

            block = block.Substring(p + 11);
            // get name
            p = block.IndexOf("-----", StringComparison.Ordinal);
            if (p < 0) throw new ParseException("end of header not found");
            String n = block.Substring(0, p);
            if (n.Contains("\n") || n.Contains("\r"))
                throw new ParseException("name contains line break " + n);
            Name = n.Trim();
            block = block.Substring(p + 5);

            // find end
            String endMark = "-----END " + Name + "-----";
            p = block.IndexOf(endMark, StringComparison.Ordinal);
            if (p < 0)
                throw new ParseException("end of block not found " + Name);

            rest = block.Substring(p + endMark.Length).Trim();
            block = block.Substring(0, p).Trim(); // remove line break

            // read lines
            bool param = true;
            string blockOrg = "";
            string lastKey = null;
            while (true)
            {
                string line = block;
                p = block.IndexOf('\n');
                if (p >= 0)
                {
                    line = block.Substring(0, p);
                    block = block.Substring(p + 1);
                }
                if (param) {
                    string l = line.Trim();
                    if (l.Length == 0)
                    {
                        param = false;
                    }
                    else
                    if (line.StartsWith(" ", StringComparison.Ordinal) && lastKey != null)
                    {
                        SetString(lastKey, GetString(lastKey, "") + line.Substring(1));
                    }
                    else
                    {
                        int pp = line.IndexOf(':');
                        if (pp < 0)
                        {
                    //  throw new ParseException("Parameter key not identified",line);
                        // start of the block
                            param = false;
                            blockOrg = line;
                        }
                        else
                        {
                            lastKey = line.Substring(0, pp).Trim();
                            string value = line.Substring(pp + 1).Trim();
                            SetString(lastKey, value);
                        }
                    }
                } else
                {
                    //              if (line.length() == 0)
                    //                  break; // end of block
                    blockOrg = blockOrg + line;
                }

                if (p < 0) break; // end of block
            }

            // decode unicode
            this.block = MString.DecodeUnicode(blockOrg);


            return this;
        } 

        public void SetString(string key, string value)
        {
            parameters.Add(key, value);
        }

        public string GetString(string key, string def)
        {
            string ret = parameters[key];
            if (ret == null) return def;
            return ret;
        }

        public override string ToString()
        {
            StringBuilder o = new StringBuilder();
            o.Append("-----BEGIN ").Append(Name).Append("-----\n");
            foreach (string key in parameters.Keys)
            {
                string val = parameters[key];
                o.Append(key).Append(": ");
                int len = key.Length + 2;
                //TODO use BLOCK_WIDTH
                o.Append(val).Append("\n");
            }

            o.Append("\n");
            o.Append(GetEncodedBlock());
            o.Append("\n\n");
            o.Append("-----END ").Append(Name).Append("-----\n");
            return o.ToString();
        }

        public string GetEncodedBlock()
        {
            string b = MString.EncodeUnicode(block, true);
            StringBuilder o = new StringBuilder();
            while (b.Length > BLOCK_WIDTH)
            {
                o.Append(b.Substring(0, BLOCK_WIDTH)).Append("\n");
                b = b.Substring(BLOCK_WIDTH);
            }
            o.Append(b);
            return o.ToString();
        }

        public string Rest { get => rest; }
    }
}
