using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace PopStudio.Plugin
{
    internal static class RijndaelHelper
    {
        public static byte[] Encrypt(byte[] plainTextBytes, byte[] keyBytes, byte[] ivStringBytes, IBlockCipherPadding padding)
        {
            var engine = new RijndaelEngine(ivStringBytes.Length << 3);
            var blockCipher = new CbcBlockCipher(engine);
            var cipher = new PaddedBufferedBlockCipher(blockCipher, padding);
            var keyParam = new KeyParameter(keyBytes);
            var keyParamWithIV = new ParametersWithIV(keyParam, ivStringBytes, 0, ivStringBytes.Length);
            cipher.Init(true, keyParamWithIV);
            var comparisonBytes = new byte[cipher.GetOutputSize(plainTextBytes.Length)];
            var length = cipher.ProcessBytes(plainTextBytes, comparisonBytes, 0);
            cipher.DoFinal(comparisonBytes, length);
            return comparisonBytes;
        }

        public static byte[] Decrypt(byte[] cipherTextBytes, byte[] keyBytes, byte[] ivStringBytes, IBlockCipherPadding padding)
        {
            var engine = new RijndaelEngine(ivStringBytes.Length << 3);
            var blockCipher = new CbcBlockCipher(engine);
            var cipher = new PaddedBufferedBlockCipher(blockCipher, padding);
            var keyParam = new KeyParameter(keyBytes);
            var keyParamWithIV = new ParametersWithIV(keyParam, ivStringBytes, 0, ivStringBytes.Length);
            cipher.Init(false, keyParamWithIV);
            var comparisonBytes = new byte[cipher.GetOutputSize(cipherTextBytes.Length)];
            var length = cipher.ProcessBytes(cipherTextBytes, comparisonBytes, 0);
            cipher.DoFinal(comparisonBytes, length);
            //var nullIndex = comparisonBytes.Length - 1;
            //while (comparisonBytes[nullIndex] == 0)
            //{
            //    nullIndex--;
            //}
            //nullIndex++;
            //byte[] ans = new byte[nullIndex];
            //Array.Copy(comparisonBytes, 0, ans, 0, nullIndex);
            //return ans;
            return comparisonBytes;
        }
    }
}