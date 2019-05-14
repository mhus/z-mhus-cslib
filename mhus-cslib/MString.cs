using System;
using System.Text;

namespace mhuscslib
{
    public class MString
    {

        public static bool IsUnicode(String _in, char _joker, bool encodeCr)
        {
            for (int i = 0; i < _in.Length; i++)
            {
                char c = _in[i];
                if ((c < 32 || c > 127 || c == _joker) && (encodeCr || (c != '\n' && c != '\r')))
                    return true;
            }
            return false;
        }

        public static String EncodeUnicode(String _in, bool encodeCr)
        {

            if (_in == null)
                return "";

            if (!IsUnicode(_in, '\\', encodeCr))
                return _in;

            StringBuilder o = new StringBuilder();
            for (int i = 0; i < _in.Length; i++)
            {
                char c = _in[i];
                if ((c < 32 || c > 127) && (encodeCr || (c != '\n' && c != '\r')))
                {
                o.Append("\\u" + MCast.ToHex2LowerString(c / 256)
                        + MCast.ToHex2LowerString(c % 256));
                }
                else if (c == '\\')
                {
                    o.Append("\\\\");
                }
                else
                    o.Append(c);

            }
            return o.ToString();
        }

        public static string DecodeUnicode(string _in)
        {
            int mode = 0;
            char[] buffer = new char[4];

            if (_in == null)
                return "";

            StringBuilder o = new StringBuilder();
            for (int i = 0; i < _in.Length; i++)
            {
                char c = _in[i];
                switch (mode)
                {
                    case 0:
                        if (c == '\\')
                            mode = 1;
                        else
                    o.Append(c);
                        break;
                    case 1:
                        if (c == 'u')
                            mode = 2;
                        else if (c == 'n')
                        {
                    o.Append('\n');
                            mode = 0;
                        }
                        else if (c == 'r')
                        {
                    o.Append('\r');
                            mode = 0;
                        }
                        else if (c == 't')
                        {
                    o.Append('\t');
                            mode = 0;
                        }
                        else
                    o.Append('\\' + c);
                        break;
                    case 2:
                        buffer[0] = c;
                        mode = 3;
                        break;
                    case 3:
                        buffer[1] = c;
                        mode = 4;
                        break;
                    case 4:
                        buffer[2] = c;
                        mode = 5;
                        break;
                    case 5:
                        buffer[3] = c;
                        o.Append((char)MCast.ToIntFromHex(new string(buffer)) );
                        mode = 0;
                        break;

                }
            }
            return o.ToString();
        }
    }
}
