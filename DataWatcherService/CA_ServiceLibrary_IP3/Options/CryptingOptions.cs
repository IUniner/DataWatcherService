namespace ServiceLibrary_IP3
{
    public class CryptingOptions : Options //"IsCompressEnable": true,    "compressionLevel": 1
    {
        //"IsEncryptEnable": true - Json
        // "IsEncryptEnable": true - Xml
        public CryptingOptions() { }
        public CryptingOptions(bool isEncryptEnable, string sourceDirectory, string targetDirectory, bool IsLoggerEnable, byte[] key = null)
                              : base(sourceDirectory, targetDirectory, IsLoggerEnable)
        {
            IsEncryptEnable = isEncryptEnable;
            if (!(key is null))
            {
                Key = key;
            }
        }

        public bool IsEncryptEnable { get; set; }
        public byte[] Key { get; set; } = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
    }
}
