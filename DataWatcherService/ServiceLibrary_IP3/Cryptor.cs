using System.IO;
using System.Security.Cryptography;

namespace ServiceLibrary_IP3
{
    public class Cryptor
    {
        private readonly OptionsManager Manager;
        private readonly CryptingOptions Options;
        readonly bool IsEncryptEnable;
        readonly bool IsLoggerEnable;
        readonly byte[] Key;
        private readonly Logger logger;

        public Cryptor()
        {
            Manager = new OptionsManager(true);
            Options = Manager.GetOptions<CryptingOptions>(Options);
            IsEncryptEnable = Options.IsEncryptEnable;
            IsLoggerEnable = Options.IsLoggerEnable;
            Key = Options.Key;
        }
        public FileInfo Encryption(FileInfo fileToEncryption)
        {
            try
            {
                FileInfo currentFile = fileToEncryption;
                if (IsEncryptEnable)
                {
                    if (fileToEncryption.Extension == ".txt")
                    {
                        using (FileStream originalFileStream = fileToEncryption.OpenRead())
                        {
                            using (FileStream cryptedFileStream = File.Create(fileToEncryption.FullName.Replace(".txt", "_AES.txt")))
                            {
                                Aes aes = Aes.Create();
                                aes.Key = Key;
                                byte[] iv = aes.IV;

                                cryptedFileStream.Write(iv, 0, iv.Length);
                                using (CryptoStream encryptionStream = new CryptoStream(cryptedFileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                                {
                                    originalFileStream.CopyTo(encryptionStream);
                                }
                            }
                        }
                        currentFile = new FileInfo(fileToEncryption.FullName.Replace(".txt", "_AES.txt"));
                    }
                }
                return currentFile;
            }
            catch (FileNotFoundException ex)
            {
                if (IsLoggerEnable)
                {
                    //logger.RecordEntry("Encryption error:" + ex.Message);
                }

                return fileToEncryption;
            }
        }

        public FileInfo Decryption(FileInfo fileToDecryption)
        {
            try
            {
                FileInfo currentFile = fileToDecryption;
                if (IsEncryptEnable)
                {
                    if (fileToDecryption.Name.Contains("AES.txt"))
                    {
                        using (FileStream originalFileStream = fileToDecryption.OpenRead())
                        {
                            Aes aes = Aes.Create();
                            byte[] iv = new byte[aes.IV.Length];

                            originalFileStream.Read(iv, 0, iv.Length);
                            using (FileStream decryptedFileStream = File.Create(fileToDecryption.FullName.Replace("_AES.txt", ".txt")))
                            {
                                using (CryptoStream decryptionFileStream = new CryptoStream(originalFileStream, aes.CreateDecryptor(Key, iv), CryptoStreamMode.Read))
                                {
                                    decryptionFileStream.CopyTo(decryptedFileStream);
                                }
                            }
                        }
                        currentFile = new FileInfo(fileToDecryption.FullName.Replace("_AES.txt", ".txt"));
                        fileToDecryption.Delete();
                    }
                }
                return currentFile;
            }
            catch (FileNotFoundException ex)
            {
                if (IsLoggerEnable)
                {
                    //logger.RecordEntry("Decryption error:" + ex.Message);
                }

                return fileToDecryption;
            }
        }
    }
}
