using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
//using SevenZip;

namespace ForgeBot
{
    public static class Zipper
    {
        //public static void SaveAndCompress(this Bitmap image,string filename, ImageFormat imageFormat )
        //{
        //    if (image == null)
        //        return;
        //    using (var ms = new MemoryStream())
        //    {
        //        image.Save(ms,imageFormat);
        //        ms.Position = 0;
        //        //var compressed = new byte[ms.Length];
        //        //ms.Read(compressed, 0, compressed.Length);

        //        using(var fs=new FileStream(filename+".7zip",FileMode.Create,FileAccess.Write))
        //        {
        //            Compress(ms,fs);
        //        }
        //    }
        //}
        
        /// <summary>
        /// 7Zip compress
        /// </summary>
        /// <param name="inStream"></param>
        /// <param name="outStream"></param>
        public static void Compress(Stream inStream,Stream outStream)
        {

            //Int32 posStateBits = 2;
            //Int32 litContextBits = 3; // for normal files
            //// UInt32 litContextBits = 0; // for 32-bit data
            //Int32 litPosBits = 0;
            //// UInt32 litPosBits = 2; // for 32-bit data
            //Int32 algorithm = 2;
            //Int32 numFastBytes = 128;
            //Int32 dictionary = 1 << 23;
            //string mf = "bt4";
            //bool eos = false;

            //CoderPropID[] propIDs = 
            //    {
            //        CoderPropID.DictionarySize,
            //        CoderPropID.PosStateBits,
            //        CoderPropID.LitContextBits,
            //        CoderPropID.LitPosBits,
            //        CoderPropID.Algorithm,
            //        CoderPropID.NumFastBytes,
            //        CoderPropID.MatchFinder,
            //        CoderPropID.EndMarker
            //    };
            //object[] properties = 
            //    {
            //        (Int32)(dictionary),
            //        (Int32)(posStateBits),
            //        (Int32)(litContextBits),
            //        (Int32)(litPosBits),
            //        (Int32)(algorithm),
            //        (Int32)(numFastBytes),
            //        mf,
            //        eos
            //    };

            //var encoder = new SevenZip.Compression.LZMA.Encoder();
            //encoder.SetCoderProperties(propIDs, properties);
            //encoder.WriteCoderProperties(outStream);
            //Int64 fileSize= inStream.Length;
            //for (int i = 0; i < 8; i++)
            //    outStream.WriteByte((Byte)(fileSize >> (8 * i)));
            
            //encoder.Code(inStream, outStream, -1, -1, null);

            //using (var ms = new MemoryStream())
            //{
            //    using (var zip = new GZipStream(ms, CompressionMode.Compress, true))
            //    {
            //        zip.Write(buffer, 0, buffer.Length);
            //    }

            //    ms.Position = 0;

            //    var compressed = new byte[ms.Length];
            //    ms.Read(compressed, 0, compressed.Length);

            //    var gzBuffer = new byte[compressed.Length + 4];
            //    System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            //    System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            //    return gzBuffer;
            //}
        }

        

        /// <summary>
        /// Decompresses a document
        /// </summary>
        /// <param name="compressedBuffer">compressed buffer</param>
        /// <returns></returns>
        public static void Decompress(Stream inStream,Stream outStream)
        {
            //byte[] properties = new byte[5];
            //if (inStream.Read(properties, 0, 5) != 5)
            //    throw (new Exception("input .lzma is too short"));
            //var decoder = new SevenZip.Compression.LZMA.Decoder();
            //decoder.SetDecoderProperties(properties);
            
            //long outSize = 0;
            //for (int i = 0; i < 8; i++)
            //{
            //    int v = inStream.ReadByte();
            //    if (v < 0)
            //        throw (new Exception("Can't Read 1"));
            //    outSize |= ((long)(byte)v) << (8 * i);
            //}
            //long compressedSize = inStream.Length - inStream.Position;
            //decoder.Code(inStream, outStream, compressedSize, outSize, null);
            
            //using (var ms = new MemoryStream())
            //{
            //    byte[] gzBuffer = compressedBuffer;
            //    int msgLength = BitConverter.ToInt32(gzBuffer, 0);
            //    ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

            //    var buffer = new byte[msgLength];

            //    ms.Position = 0;
            //    using (var zip = new GZipStream(ms, CompressionMode.Decompress))
            //    {
            //        zip.Read(buffer, 0, buffer.Length);
            //    }

            //    return buffer;
            //}
        }

        /// <summary>
        /// Reads stream into byte[]
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] ReadFully(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                var buffer = new byte[16 * 1024];
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
