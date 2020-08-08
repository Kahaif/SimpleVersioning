using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleVersioning.Extensions
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Gets the string from stream asynchronous.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>A string representation of the stream that has the same encoding as encoding.</returns>
        public static async Task<string> GetStringFromStreamAsync(this Stream stream, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            using var reader = new StreamReader(stream);

            try
            {
                return await reader.ReadToEndAsync();
            }
            catch
            {
                throw;
            }

        }
    }
}
