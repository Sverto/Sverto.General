using static Sverto.General.Logging.Logging;
using Sverto.General.Logging;
using System;
using System.Net;
using System.Net.Sockets;

namespace Sverto.General.Net
{
    /// <summary>
    /// Send a Wake On LAN packet to the destination host
    /// The MAC-Address can be in the format 001122334455 or 00:11:22:33:44:55 (in fact any character can be between the bytes).
    /// </summary>
    public class WakeOnLan
    {
        /// <summary>
        /// Send a Wake On Lan packet
        /// </summary>
        /// <param name="mac">The MAC-Address of the client to wake</param>
        /// <param name="broadcastAddress">The broadcast address of the local network</param>
        /// <param name="port">The port the packet will be send to</param>
        /// <returns>True if succeeded, False if not</returns>
        /// <remarks></remarks>
        public static bool SendPacket(string mac, IPAddress broadcastAddress, int port = 9)
        {
            Byte[] packet = null;
            // ERROR: Not supported in C#: ReDimStatement

            IPEndPoint localBCast = new IPEndPoint(broadcastAddress, port);
            UdpClient udpClt = new UdpClient();
            bool result = false;
            //
            result = GeneratePacket(mac, ref packet);
            if (result == false)
            {
                return result;
            }
            //
            try
            {
                udpClt.Send(packet, 102, localBCast);
                udpClt.Close();
            }
            catch
            {
                Log(nameof(SendPacket), LogLevel.Debug, "Failed sending WOL packet.");
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Generate a Wake On LAN packet
        /// </summary>
        /// <param name="mac">The MAC-Address of the client to wake</param>
        /// <param name="packet">Contains the generated packet</param>
        /// <returns>True if succeeded, False if not</returns>
        /// <remarks></remarks>
        private static bool GeneratePacket(string mac, ref byte[] packet)
        {
            // Get bytelength of MAC
            int byteLength = 0;
            switch (mac.Length)
            {
                case 12:
                    byteLength = 2;
                    break;
                case 17:
                    byteLength = 3;
                    break;
                default:
                    Log(nameof(GeneratePacket), LogLevel.Error, "Error while generating packet: The MAC-Address length is incorrect.");
                    return false;
            }
            // Convert MAC to bytes
            int i = 0;
            int j = 0;
            Byte[] block = null;
            // ERROR: Not supported in C#: ReDimStatement

            try
            {
                for (i = 0; i <= 5; i++)
                {
                    block[i] = Convert.ToByte(mac.Substring(byteLength * i, 2), 16);
                }
            }
            catch
            {
                Log(nameof(GeneratePacket), LogLevel.Error, "Error while generating packet: Invallid MAC-Address specified.");
                return false;
            }
            //
            for (i = 0; i <= 5; i++)
            {
                packet[i] = 0xff;
            }
            for (i = 6; i <= 97; i += 6)
            {
                for (j = 0; j <= 5; j++)
                {
                    packet[i + j] = block[j];
                }
            }
            return true;
        }
    }
}
