using System.Collections;
using System.Text;

namespace Crypto6.Stenography;

public static class Lsb
{
    public static byte[] HideTextInBytes(byte[] bytes, string str)
    {
        var bitArrayStr = new BitArray(Encoding.UTF8.GetBytes(str));
            
        for (int i = 0, j = 0; i < bitArrayStr.Length + 8; i += 2, j++)
        {
            var bits = new BitArray(new[] { bytes[j] })
            {
                [6] = i < bitArrayStr.Length && bitArrayStr[i],
                [7] = i < bitArrayStr.Length && bitArrayStr[i + 1]
            };
                
            bits.CopyTo(bytes, j);
        }
            
        return bytes;
    }

    public static string GetHiddenTextFromBytes(byte[] bytes)
    {
        var memoryStream = new MemoryStream();
        var binaryWriter = new BinaryWriter(memoryStream);
        var bits = new BitArray(bytes);
        var byteFromBits = new BitArray(8);
            
        for(int i = 6, j = 0; i < bits.Length; i += 8)
        {
            byteFromBits[j++] = bits[i];
            byteFromBits[j++] = bits[i + 1];
                
            if (j != 8) continue;
                
            if (IsEndOfMessage(byteFromBits))
                break;
                
            binaryWriter.Write(BitArrayToByte(byteFromBits));
            j = 0;
        }
            
        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }

    private static bool IsEndOfMessage(IEnumerable bitArray) => 
        bitArray.Cast<bool>().All(bit => !bit);

    private static byte[] BitArrayToByte(ICollection bits)
    {
        var byteFromBits = new byte[1];
            
        bits.CopyTo(byteFromBits, 0);
            
        return byteFromBits;
    }
}