using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Collections.Generic;
   
namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class Compresor
    {
        private const int buffer_size = 100;

        public static int ReadAllBytesFromStream(Stream stream, byte[] buffer)
        {
            // Use this method is used to read all bytes from a stream.
            int offset = 0;
            int totalCount = 0;
            while (true)
            {
                int bytesRead = stream.Read(buffer, offset, buffer_size);
                if (bytesRead == 0)
                {
                    break;
                }
                offset += bytesRead;
                totalCount += bytesRead;
            }
            return totalCount;
        }

        public static bool CompareData(byte[] buf1, int len1, byte[] buf2, int len2)
        {
            // Use this method to compare data from two different buffers.
            if (len1 != len2)
            {
                //Console.WriteLine("Number of bytes in two buffer are different {0}:{1}", len1, len2);
                return false;
            }

            for (int i = 0; i < len1; i++)
            {
                if (buf1[i] != buf2[i])
                {
                    //Console.WriteLine("byte {0} is different {1}|{2}", i, buf1[i], buf2[i]);
                    return false;
                }
            }
            //Console.WriteLine("All bytes compare.");
            return true;
        }

        public static byte[] Comprimir(byte[] datosEntrada)
        {
            MemoryStream ms = new MemoryStream();
            // Use the newly created memory stream for the compressed data.
            try
            {
                GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);      //  Console.WriteLine("Compresion");
                compressedzipStream.Write(datosEntrada, 0, datosEntrada.Length);
                // Close the stream.
                compressedzipStream.Close();      
                // Console.WriteLine("Tamaño original: {0}, Tamaño comprimido: {1}", buffer.Length, ms.Length);

                return ms.ToArray();
            }
            catch (Exception ex)
            {
                string verQhagoConEsto = ex.Message;
                return null;
            }

        }

        public static string  Descomprimir(FileStream fs)
        {
            byte[] datosSalida = new byte[0];
            try
            {
                GZipStream decompressedZipStream = new GZipStream(fs, CompressionMode.Decompress);
                List<byte> listdatosSalida = new List<byte>(); 
                ASCIIEncoding oASCIIEncoding = new ASCIIEncoding();
                string base64String = "";

                for (int read = decompressedZipStream.ReadByte(); read != -1; read = decompressedZipStream.ReadByte())
                {
                    listdatosSalida.Add((byte)read);
                    base64String = oASCIIEncoding.GetString(listdatosSalida.ToArray());
                }  

                //datosSalida = new byte[10000];
                //int nroOfBytes = decompressedZipStream.Read(datosSalida, 0, datosSalida.Length);                             
                //base64String = oASCIIEncoding.GetString(datosSalida);
                
                // Close the stream.
                decompressedZipStream.Close();
                return base64String;
            }
            catch (Exception ex)
            {
                string verQhagoConEsto = ex.Message;
                return null;
            }
        }

        public static bool SetFileContent(string FileName, byte[] byteContent, FileMode fm, int length)
        {
            if (length <= -1) length = byteContent.Length;
            BinaryWriter sr = null;
            try
            {
                sr = new BinaryWriter(File.Open(FileName, fm), System.Text.Encoding.GetEncoding("Windows-1251"));
                sr.Write(byteContent, 0, length);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }

        public static void GZipCompress(string filename, byte[] data)
        {
            try
            {
                byte[] comprimido = Comprimir(data);
               SetFileContent(filename, comprimido, FileMode.OpenOrCreate, comprimido.Length);
            }
            catch (InvalidDataException)
            {
                //Console.WriteLine("Error: The file being read contains invalid data.");
            }
            catch (FileNotFoundException)
            {
                //Console.WriteLine("Error:The file specified was not found.");
            }
            catch (ArgumentException)
            {
                //Console.WriteLine("Error: path is a zero-length string, contains only white space, or contains one or more invalid characters");
            }
            catch (PathTooLongException)
            {
                //Console.WriteLine("Error: The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.");
            }
            catch (DirectoryNotFoundException)
            {
                //Console.WriteLine("Error: The specified path is invalid, such as being on an unmapped drive.");
            }
            catch (IOException)
            {
                //Console.WriteLine("Error: An I/O error occurred while opening the file.");
            }
            catch (UnauthorizedAccessException)
            {
                //Console.WriteLine("Error: path specified a file that is read-only, the path is a directory, or caller does not have the required permissions.");
            }
            catch (IndexOutOfRangeException)
            {
                //Console.WriteLine("Error: You must provide parameters for MyGZIP.");
            }
           
        }

        public static byte[] GZipDescompress(string filename)
        {
            FileStream infile;
            byte[] decompressedBuffer = null;
            try
            {
                // Open the file as a FileStream object.
                infile = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] buffer = new byte[infile.Length];
                // Read the file to ensure it is readable.
                int count = infile.Read(buffer, 0, buffer.Length);
                if (count != buffer.Length)
                {
                    infile.Close();  
                    //Console.WriteLine("Test Failed: Unable to read data from file");
                    return null;
                }
                infile.Close();

                MemoryStream ms = new MemoryStream();
                // Reset the memory stream position to begin decompression.
                ms.Position = 0;
                GZipStream zipStream = new GZipStream(ms, CompressionMode.Decompress);

                decompressedBuffer = new byte[buffer.Length + buffer_size];
                // Use the ReadAllBytesFromStream to read the stream.
                int totalCount = ReadAllBytesFromStream(zipStream, decompressedBuffer);

                if (!CompareData(buffer, buffer.Length, decompressedBuffer, totalCount))
                {
                    //throw new Exception("Error. The two buffers did not compare.");
                    return null;
                }
                zipStream.Close();
                return decompressedBuffer;
            } // end try
            catch (InvalidDataException)
            {
                return null; // Console.WriteLine("Error: The file being read contains invalid data.");
            }
            catch (FileNotFoundException)
            {
                return null;// Console.WriteLine("Error:The file specified was not found.");
            }
            catch (ArgumentException)
            {
                return null;// Console.WriteLine("Error: path is a zero-length string, contains only white space, or contains one or more invalid characters");
            }
            catch (PathTooLongException)
            {
                return null;//Console.WriteLine("Error: The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.");
            }
            catch (DirectoryNotFoundException)
            {
                return null;//Console.WriteLine("Error: The specified path is invalid, such as being on an unmapped drive.");
            }
            catch (IOException)
            {
                return null;//Console.WriteLine("Error: An I/O error occurred while opening the file.");
            }
            catch (UnauthorizedAccessException)
            {
                return null; // Console.WriteLine("Error: path specified a file that is read-only, the path is a directory, or caller does not have the required permissions.");
            }
            catch (IndexOutOfRangeException)
            {
                return null;//Console.WriteLine("Error: You must provide parameters for MyGZIP.");
            }
         
        }        
    }
}

