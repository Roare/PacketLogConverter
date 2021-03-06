using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xF5, -1, ePacketDirection.ServerToClient, "Siege weapon interact")]
	public class StoC_0xF5_SiegeWeaponInteract: Packet, IObjectIdPacket
	{
		protected byte siegeMenu;
		protected byte canMove;
		protected ushort unk2;
		protected byte timer; // in sec/10
		protected byte ammoCount;
		protected byte action;
		protected byte currentAmmoIndex;
		protected ushort effect;
		protected ushort unk6;
		protected ushort unk7;
		protected ushort oid;
		protected string name;
		protected Item[] m_items;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { oid }; }
		}

		#region public access properties

		public byte SiegeMenu { get { return siegeMenu; } }
		public byte CanMove { get { return canMove ; } }
		public ushort Unk2 { get { return unk2; } }
		public byte Timer { get { return timer; } }
		public byte AmmoCount { get { return ammoCount; } }
		public byte Action { get { return action; } }
		public byte CurrentAmmoIndex { get { return currentAmmoIndex; } }
		public ushort Effect { get { return effect; } }
		public ushort Unk6 { get { return unk6; } }
		public ushort Unk7 { get { return unk7; } }
		public string Name { get { return name; } }
		public Item[] Items { get { return m_items; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			string actionType;
			switch (action)
			{
				case 0:
					actionType = "opening";
					if (unk2 == 1)
						actionType = "closing";
					break;
				case 1:
					actionType = "aiming ";
					break;
				case 2:
					actionType = "arming ";
					break;
				case 3:
					actionType = "loading";
					break;
				case 5:
					actionType = "helping";
					break;
				default:
					actionType = "unknown";
					break;
			}
			text.Write("menuButtons:0x{0:X2} canMove:{1} unk2:0x{2:X4} timer:{3,-3} externalAmmoCount:{4} action:{5}({6}) currentAmmoIndex:{7,-2} effect:0x{8:X4} unk6:0x{9:X4} unk7:0x{10:X4} oid:0x{11:X4} name:\"{12}\"",
				siegeMenu, canMove, unk2, timer , ammoCount, action, actionType, currentAmmoIndex, effect, unk6, unk7, oid, name);

			if (flagsDescription && effect != 0)
			{
				text.Write(" spellId:({0})", (StoC_0xE3_SiegeWeaponAnimation.eSiegeWeaponEffect)effect);
			}
			for (int i = 0; i < ammoCount; i++)
			{
				Item item = (Item)Items[i];
				text.Write("\n\tindex:{0,-2} level:{1,-2} value1:0x{2:X2} value2:0x{3:X2} unk1:0x{4:X2} objectType:0x{5:X2} unk2:0x{6:X2} count:{7,-2} condition:{8,-3} durability:{9,-3} quality:{10,-3} bonus:{11,-2} model:0x{12:X4} extension:{13} effect:0x{14:X4} color:0x{15:X4} name:\"{16}\"",
	                 item.index, item.level, item.value1, item.value2, item.unk1, item.objectType, item.unk2, item.count, item.condition, item.durability, item.quality, item.bonus, item.model, item.extension, item.unk3, item.unk4, item.name);
			}

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			siegeMenu = ReadByte();        // 0x00
			canMove = ReadByte();          // 0x01
			unk2 = ReadShort();            // 0x02
			timer = ReadByte();            // 0x04
			ammoCount = ReadByte();        // 0x05
			action = ReadByte();           // 0x06
			currentAmmoIndex = ReadByte(); // 0x07
			effect = ReadShort();          // 0x08
			unk6 = ReadShort();            // 0x0A
			unk7 = ReadShort();            // 0x0C
			oid = ReadShort();             // 0x0E
			m_items = new Item[ammoCount];
			for (int i = 0; i < ammoCount; i++)
			{
				Item item = new Item();

				item.index = ReadByte();      // 0x10
				item.level = ReadByte();      // 0x11
				item.value1 = ReadByte();     // 0x12
				item.value2 = ReadByte();     // 0x13
				item.unk1 = ReadByte();       // 0x14
				item.objectType = ReadByte(); // 0x15
				item.unk2 = ReadByte();       // 0x16
				item.count = ReadByte();      // 0x17
				item.condition = ReadByte();  // 0x18
				item.durability = ReadByte(); // 0x19
				item.quality = ReadByte();    // 0x1A
				item.bonus = ReadByte();      // 0x1B
				item.model = ReadShort();     // 0x1C
				item.extension = ReadByte();  // 0x1E
				item.unk3 = ReadShort();      // 0x1F
				item.unk4 = ReadShort();      // 0x21
				item.name = ReadPascalString();//0x23

				m_items[i] = item;
			}

			name = ReadPascalString();
		}

		public struct Item
		{
			public byte index;
			public byte level;
			public byte value1;
			public byte value2;
			public byte unk1;
			public byte objectType;
			public byte unk2;
			public byte count;
			public byte condition;
			public byte durability;
			public byte quality;
			public byte bonus;
			public ushort model;
			public byte extension;
			public ushort unk3;
			public ushort unk4;
			public string name;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xF5_SiegeWeaponInteract(int capacity) : base(capacity)
		{
		}
	}
}
