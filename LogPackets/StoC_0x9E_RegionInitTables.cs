using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x9E, -1, ePacketDirection.ServerToClient, "region init tables")]
	public class StoC_0x9E_RegionInitTables : Packet
	{
		protected RegionEntry[] regions;

		#region public access properties

		public RegionEntry[] Regions { get { return regions; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			for (int i = 0; i < regions.Length; i++)
			{
				RegionEntry region = (RegionEntry)regions[i];
				if (region.regionIndex == 0) continue;
				text.Write("\n\tregionIndex:{0,-3} regionId:{1,-3} name:{2} fromPort:{3} toPort:{4} ip:{5,-15}",
					region.regionIndex, region.region, region.name, region.fromPort, region.toPort, region.ip);
			}

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			regions = new RegionEntry[4] ;

			Position = 0;

			for (int i = 0; i < 4; i++)
			{
				RegionEntry reg = new RegionEntry();

				reg.regionIndex = ReadByte();
				reg.region = ReadByte();
				reg.name = ReadString(20);
				reg.fromPort = ReadString(5);
				reg.toPort = ReadString(5);
				reg.ip = ReadString(20);

				regions[i] = reg;
			}
		}

		public struct RegionEntry
		{
			public int regionIndex;
			public int region;
			public string name;
			public string fromPort;
			public string toPort;
			public string ip;

		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x9E_RegionInitTables(int capacity) : base(capacity)
		{
		}
	}
}