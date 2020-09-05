using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Runtime.InteropServices;

namespace Payments.Model
{
    static class Converters
    {
        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static byte[] GetBytes(SecureString sstr)
        {
            // pointer to hold unmanaged reference to SecureString instance
            IntPtr bstr = IntPtr.Zero;
            byte[] ssBytes;
            try
            {
                // marshall SecureString into byte array
                ssBytes = new byte[sstr.Length * 2];
                bstr = Marshal.SecureStringToBSTR(sstr);
                Marshal.Copy(bstr, ssBytes, 0, ssBytes.Length);
            }
            finally
            {
                // Make sure that the clear text data is zeroed out
                Marshal.ZeroFreeBSTR(bstr);
            }
            return ssBytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static T[] Concat<T>(T[] x, T[] y)
        {
            var z = new T[x.Length + y.Length];
            x.CopyTo(z, 0);
            y.CopyTo(z, x.Length);
            return z;
        }
    }
}
