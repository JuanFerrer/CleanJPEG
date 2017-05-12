using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Diagnostics;

namespace CleanJPEG
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = args[0];
            string destFile = args.Length == 1? file : args[1];
            byte[] bytes = new byte[1];
            if (File.Exists(file))
            {
                bytes = GetHexFromFile(file);
            }
            int discardBeforeIndex = 0;
            for (int i = 0; i < bytes.Length; ++i)
            {
                #if DEBUG
                Console.Write(bytes[i].ToString("X2") + " ");
                #endif
                if (bytes[i] == 0xFF && bytes[i + 1] == 0xD8)
                {
                    # if DEBUG
                    Console.WriteLine("Signature found at " + i);
                    #endif
                    discardBeforeIndex = i;
                    break;
                }
            }
            byte[] newBytes = new byte[bytes.Length - discardBeforeIndex];
            Array.Copy(bytes, discardBeforeIndex, newBytes, 0, bytes.Length - discardBeforeIndex);
            ByteArrayToFile(destFile, newBytes);
        }

        static byte[] GetHexFromFile(string file)
        {
            BinaryReader reader = new BinaryReader(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None));
            reader.BaseStream.Position = 0x0;     // The offset you are reading the data from
            byte[] data = ReadAllBytes(reader);
            reader.Close();
            return data;
        }


        static byte[] ReadAllBytes(BinaryReader reader)
        {
            const int bufferSize = 4096;
            using (var ms = new MemoryStream())
            {
                byte[] buffer = new byte[bufferSize];
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);
                return ms.ToArray();
            }
        }

        static void ByteArrayToFile(string fileName, byte[] byteArray)
        {
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                fs.Write(byteArray, 0, byteArray.Length);
            }
        }
    }
}
