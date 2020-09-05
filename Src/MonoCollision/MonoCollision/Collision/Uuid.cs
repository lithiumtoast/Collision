using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MonoCollision
{
    public static class Uuid
    {
        private static readonly HashAlgorithm HashAlgorithm = SHA1.Create();
        
        public static Guid CreateV5(Guid @namespace, string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            // convert the name to a sequence of octets (as defined by the standard or conventions of its namespace) (step 3)
            // ASSUME: UTF-8 encoding is always appropriate
            var nameBytes = Encoding.UTF8.GetBytes(name);

            // convert the namespace UUID to network order (step 3)
            var namespaceBytes = @namespace.ToByteArray();
            SwapByteOrder(namespaceBytes);

            return CreateV5(namespaceBytes, nameBytes);
        }

        private static Guid CreateV5(IEnumerable<byte> @namespace, IEnumerable<byte> name)
        {
            // compute the hash of the namespace ID concatenated with the name (step 4)
            var data = @namespace.Concat(name).ToArray();
            var hash = HashAlgorithm.ComputeHash(data);

            // most bytes from the hash are copied straight to the bytes of the new GUID (steps 5-7, 9, 11-12)
            var uuid = new byte[16];
            Array.Copy(hash, 0, uuid, 0, 16);

            // set the four most significant bits (bits 12 through 15) of the time_hi_and_version field to the appropriate 4-bit version number from Section 4.1.3 (step 8)
            uuid[6] = (byte)((uuid[6] & 0x0F) | (5 << 4));

            // set the two most significant bits (bits 6 and 7) of the clock_seq_hi_and_reserved to zero and one, respectively (step 10)
            uuid[8] = (byte)((uuid[8] & 0x3F) | 0x80);

            // convert the resulting UUID to local byte order (step 13)
            SwapByteOrder(uuid);
            return new Guid(uuid);
        }

        // Converts a GUID (expressed as a byte array) to/from network order (MSB-first).
        private static void SwapByteOrder(byte[] guid)
        {
            SwapBytes(guid, 0, 3);
            SwapBytes(guid, 1, 2);
            SwapBytes(guid, 4, 5);
            SwapBytes(guid, 6, 7);
        }

        private static void SwapBytes(byte[] guid, int left, int right)
        {
            var temp = guid[left];
            guid[left] = guid[right];
            guid[right] = temp;
        }
    }
}