using System;
using System.Security.Cryptography;
using System.Text;

namespace CustomLibrary.Helper
{
    public class InvalidHashException : Exception
    {
        public InvalidHashException()
        {
        }

        public InvalidHashException(string message)
            : base(message) { }

        public InvalidHashException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class CannotPerformOperationException : Exception
    {
        public CannotPerformOperationException()
        {
        }

        public CannotPerformOperationException(string message)
            : base(message) { }

        public CannotPerformOperationException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class Security
    {
        // These constants may be changed without breaking existing hashes.
        public const int SALT_BYTES = 24;
        public const int HASH_BYTES = 18;
        public const int PBKDF2_ITERATIONS = 64000;

        // These constants define the encoding and may not be changed.
        public const int HASH_SECTIONS = 2;
        public const int SALT_INDEX = 0;
        public const int PBKDF2_INDEX = 1;

        private static readonly string KeySalt = "M0n5T3rC0D3Sur4B4y@2021";

        public static string Combine(string email, string password)
        {
            //password
            var monster = "M0n5T3r";//Monster.
            var depanEmail = email.Split("@");
            var angkaUnik = "57588";

            //email di substring
            //ditambahin belakang @angka( 57588)
            string result = password + "-" + monster + "." + depanEmail[0] + "@" + angkaUnik;
            return result;
        }

        public static string HashPasswordUsingPBKDF2(string password)
        {
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 32)
            {
                IterationCount = 10000
            };

            byte[] hash = rfc2898DeriveBytes.GetBytes(20);
            byte[] salt = rfc2898DeriveBytes.Salt;

            return Convert.ToBase64String(salt) + "|" + Convert.ToBase64String(hash);
        }

        public static string HashPasswordUsingMD5(string password)
        {
            using var md5 = MD5.Create();
            byte[] passwordBytes = Encoding.ASCII.GetBytes(password);

            byte[] hash = md5.ComputeHash(passwordBytes);

            var stringBuilder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
                stringBuilder.Append(hash[i].ToString("X2"));

            return stringBuilder.ToString();
        }

        public static string EncryptAdditional(string text)
        {
            using var md5 = new MD5CryptoServiceProvider();
            using var tdes = new TripleDESCryptoServiceProvider();

            tdes.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(KeySalt));
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            using var transform = tdes.CreateEncryptor();
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);

            return Convert.ToBase64String(bytes, 0, bytes.Length);
        }

        public static string DecryptAdditional(string cipher)
        {
            using var md5 = new MD5CryptoServiceProvider();
            using var tdes = new TripleDESCryptoServiceProvider();

            tdes.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(KeySalt));
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            using var transform = tdes.CreateDecryptor();
            byte[] cipherBytes = Convert.FromBase64String(cipher);
            byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(bytes);
        }

        //decrypt MD5
        public static string DecryptMd5(string sData)
        {
            try
            {
                var encoder = new UTF8Encoding();
                Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(sData);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);

                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);

                var result = new string(decoded_char);
                var DecryptMD5 = DecryptAdditional(result);

                return DecryptMD5;
            }
            catch
            {
                throw new ArgumentException("Konten Tidak Valid");
            }
        }

        public static bool TryDecryptMd5(string encrypted, out string result)
        {
            try
            {
                var encoder = new UTF8Encoding();
                Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(encrypted);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);

                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);

                var decodedString = new string(decoded_char);
                result = DecryptAdditional(decodedString);

                return true;
            }
            catch
            {
                result = "";
                return false;
            }
        }

        //encrypt MD5
        public static string EncryptMd5(string data)
        {
            try
            {
                var MD5Encrypt = EncryptAdditional(data);
                byte[] encData_byte = new byte[MD5Encrypt.Length];
                encData_byte = Encoding.UTF8.GetBytes(MD5Encrypt);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }
        }

        #region Hash Baru

        public static string CreateHash(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[SALT_BYTES];

            try
            {
                //old code for non core
                //using (RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider()) {
                //  csprng.GetBytes(salt);
                //}

                //new code for core
                using (var csprng = RandomNumberGenerator.Create())
                {
                    csprng.GetBytes(salt);
                }
            }
            catch (CryptographicException ex)
            {
                throw new CannotPerformOperationException(
                    "Random number generator not available.",
                    ex
                );
            }
            catch (ArgumentNullException ex)
            {
                throw new CannotPerformOperationException(
                    "Invalid argument given to random number generator.",
                    ex
                );
            }

            byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTES);

            // format: algorithm:iterations:hashSize:salt:hash
            string parts =
                //PBKDF2_ITERATIONS +
                //":" +
                //hash.Length +
                //":" +
                Convert.ToBase64String(salt) +
                ":" +
                Convert.ToBase64String(hash);
            return parts;
        }

        public static bool VerifyPassword(string password, string goodHash)
        {
            char[] delimiter = { ':' };
            string[] split = goodHash.Split(delimiter);

            if (split.Length != HASH_SECTIONS)
            {
                throw new InvalidHashException(
                    "Fields are missing from the password hash."
                );
            }
            byte[] salt = null;
            try
            {
                salt = Convert.FromBase64String(split[SALT_INDEX]);
            }
            catch (ArgumentNullException ex)
            {
                throw new CannotPerformOperationException(
                    "Invalid argument given to Convert.FromBase64String",
                    ex
                );
            }
            catch (FormatException ex)
            {
                throw new InvalidHashException(
                    "Base64 decoding of salt failed.",
                    ex
                );
            }

            byte[] hash = null;
            try
            {
                hash = Convert.FromBase64String(split[PBKDF2_INDEX]);
            }
            catch (ArgumentNullException ex)
            {
                throw new CannotPerformOperationException(
                    "Invalid argument given to Convert.FromBase64String",
                    ex
                );
            }
            catch (FormatException ex)
            {
                throw new InvalidHashException(
                    "Base64 decoding of pbkdf2 output failed.",
                    ex
                );
            }

            byte[] testHash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTES);
            return SlowEquals(hash, testHash);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt))
            {
                pbkdf2.IterationCount = iterations;
                return pbkdf2.GetBytes(outputBytes);
            }
        }

        #endregion Hash Baru
    }
}