using System;
using System.IO;
using System.Reflection;

namespace Sverto.General.Extensions
{
    public static class StreamExtensions
    {

        // https://www.daniweb.com/programming/software-development/threads/35078/streamreader-and-position
        /// <summary>
        /// Gets the current read position of the StreamReader.
        /// </summary>
        /// <param name="streamReader">The StreamReader object to get the position for.</param>
        /// <returns>Current read position in the StreamReader.</returns>
        public static int GetPosition(this StreamReader streamReader)
        {
            // Based on code shared on www.daniweb.com by user mfm24(Matt).
            int charpos = Convert.ToInt32(streamReader.GetType().InvokeMember("charPos", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, streamReader, null));
            int charlen = Convert.ToInt32(streamReader.GetType().InvokeMember("charLen", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, streamReader, null));
            return Convert.ToInt32(streamReader.BaseStream.Position) - charlen + charpos;
        }

        /// <summary>
        /// Sets the current read position of the StreamReader.
        /// </summary>
        /// <param name="streamReader">The StreamReader object to get the position for.</param>
        /// <param name="position">The position to move to in the file, starting from the beginning.</param>
        public static void SetPosition(this StreamReader streamReader, long position)
        {
            streamReader.BaseStream.Seek(position, SeekOrigin.Begin);
            streamReader.DiscardBufferedData();
        }

    }
}
