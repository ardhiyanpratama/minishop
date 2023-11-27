using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#nullable enable
namespace CustomLibrary.Helper
{
    public class FileSignature
    {
        private static readonly Dictionary<string, List<byte[]>> _fileSignatures = new Dictionary<string, List<byte[]>>()
        {
            {
                ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xDB },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xEE },
                }
            },
            {
                ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xDB },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xEE },
                }
            },
            {
                ".png", new List<byte[]>
                {
                    new byte[]{ 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
                }
            },
            {
                ".pdf", new List<byte[]>
                {
                    new byte[]{ 0x25, 0x50, 0x44, 0x46 }
                }
            },
            {
                ".docx", new List<byte[]>
                {
                    new byte[]{ 0x50, 0x4B, 0x03, 0x04 },
                }
            },
            {
                ".xlsx", new List<byte[]>
                {
                    new byte[]{ 0x50, 0x4B, 0x03, 0x04 },
                    new byte[]{ 0x50, 0x4B, 0x03, 0x04, 0x00, 0x06, 0x00 }
                }
            },
            {
                ".xls", new List<byte[]>
                {
                    new byte[]{ 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 }
                }
            },
            {
                ".rar", new List<byte[]>
                {
                    new byte[]{ 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00 },
                    new byte[]{ 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x01, 0x00 }
                }
            }
        };

        public static bool IsSigned(Stream file, string ext, out string headerBytesAsString)
        {
            headerBytesAsString = "";

            try
            {
                if (!_fileSignatures.ContainsKey(ext.ToLower()))
                {
                    return false;
                }

                using var reader = new BinaryReader(file);
                var signatures = _fileSignatures[ext.ToLower()];

                var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));
                headerBytesAsString = Encoding.ASCII.GetString(headerBytes);

                var result = signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
