using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogWriters
{
	/// <summary>
	/// Writes logs in own format to allow mixed packet versions.
	/// </summary>
	[LogWriter("PacketLogConverter v1", "*.plc", Priority=5000)]
	public class PacketLogConverterV1LogWriter : ILogWriter
	{
		/// <summary>
		/// Writes the log.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="stream">The stream.</param>
		/// <param name="callback">The callback for UI updates.</param>
		public void WriteLog(IExecutionContext context, Stream stream, ProgressCallback callback)
		{
			using (BinaryWriter s = new BinaryWriter(stream, Encoding.ASCII))
			{
				s.Write("[PacketLogConverter v1]");

				foreach (PacketLog log in context.LogManager.Logs)
				{
					for (int i = 0; i < log.Count; i++)
					{
						if (callback != null && (i & 0xFFF) == 0) // update progress every 4096th packet
							callback(i + 1, log.Count);

						Packet packet = log[i];
						if (context.FilterManager.IsPacketIgnored(packet))
							continue;

						byte[] buf = packet.GetBuffer();
						s.Write((ushort) buf.Length);
						s.Write(packet.GetType().FullName);
						s.Write((ushort) packet.Code);
						s.Write((byte) packet.Direction);
						s.Write((byte) packet.Protocol);
						s.Write(packet.Time.Ticks);
						s.Write(buf);
					}
				}
			}
		}
	}
}
