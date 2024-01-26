using System.Text;

namespace CliToolkit.Tools;

public class Encoder
{
    public const int HeaderLength = 10; // Does not include 4-byte message length.

    public static byte[] EncodeMessage(RconMessage msg)
    {
        List<byte> bytes = new List<byte>();

        bytes.AddRange(BitConverter.GetBytes(msg.Length));
        bytes.AddRange(BitConverter.GetBytes(msg.ID));
        bytes.AddRange(BitConverter.GetBytes((int) msg.Type));
        bytes.AddRange(Encoding.ASCII.GetBytes(msg.Body));
        bytes.AddRange(new byte[] {0, 0});

        return bytes.ToArray();
    }

    public static RconMessage DecodeMessage(byte[] bytes)
    {
        int len  = BitConverter.ToInt32(bytes, 0);
        int id   = BitConverter.ToInt32(bytes, 4);
        int type = BitConverter.ToInt32(bytes, 8);

        int bodyLen = bytes.Length - (HeaderLength + 4);
        if(bodyLen > 0)
        {
            byte[] bodyBytes = new byte[bodyLen];
            Array.Copy(bytes, 12, bodyBytes, 0, bodyLen);
            Array.Resize(ref bodyBytes, bodyLen);
            return new RconMessage(len, id, (RconMessageType) type, Encoding.ASCII.GetString(bodyBytes));
        }
        else
        {
            return new RconMessage(len, id, (RconMessageType) type, "");
        }
    }
}