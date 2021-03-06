using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA9, 190.1f, ePacketDirection.ServerToClient, "Player position update v190c")]
	public class StoC_0xA9_PlayerPosition_190c : StoC_0xA9_PlayerPosition_172
	{
		protected byte manaPercent;
		protected byte endurancePercent;
		protected string className;

		#region public access properties

		public byte ManaPercent { get { return manaPercent; } }
		public byte EndurancePercent { get { return endurancePercent; } }
		public string ClassName { get { return className; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			bool isRaided = IsRaided == 1;
			int zSpeed = speed & 0xFFF;
			if ((speed & 0x1000) == 0x1000)
				zSpeed *= -1;
			text.Write("sessionId:0x{0:X4} status:0x{1:X2} speed:{2,-3} {3}:0x{4:X4}(0x{14:X1}) currentZone({5,-3}): ({6,-6} {7,-6} {8,-5}) flyFlags:0x{9:X2} {10}:{11,-5} flags:0x{12:X2} health:{13,3}%",
				sessionId, (status & 0x1FF ^ status) >> 8 ,status & 0x1FF, isRaided ? "mountId" : "heading", isRaided ? heading : heading & 0xFFF, currentZoneId, currentZoneX, currentZoneY, currentZoneZ, (speed & 0x7FF ^ speed) >> 8, (isRaided ? "bSlot " : "SpeedZ") , zSpeed, flag, health & 0x7F, isRaided ? 0 : (heading & 0xFFF ^ heading) >> 13);
			text.Write(" mana:{0,3}% endurance:{1,3}% className:{2}",
				manaPercent, endurancePercent, className);
			if (flagsDescription)
			{
				AddDescription(text);
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			sessionId = ReadShort();
			status = ReadShort();
			currentZoneZ = ReadShort();
			currentZoneX = ReadShort();
			currentZoneY = ReadShort();
			currentZoneId= ReadShort();
			heading = ReadShort();
			speed = ReadShort();
			flag = ReadByte();
			health = ReadByte();
			manaPercent = ReadByte();
			endurancePercent = ReadByte();
			className = ReadString(32);
		}

		/// <summary>
		/// Set all log variables from the packet here
		/// </summary>
		/// <param name="log"></param>
		public override void InitLog(PacketLog log)
		{
			// Reinit only on for 190 version and subversion lower 190.2 (not must be in subversion detection on serverside)
			if (!log.IgnoreVersionChanges && log.Version >= 190 && log.Version < 190.2f)
			{
				if (Length == 54)
				{
					log.Version = 190.2f;
					log.SubversionReinit = true;
//					log.IgnoreVersionChanges = true;
				}
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xA9_PlayerPosition_190c(int capacity) : base(capacity)
		{
		}
	}
}