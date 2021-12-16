using System.Buffers.Binary;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;
using AdventOfCode.Utilities;

namespace AdventOfCode.Y2021.Day16;

class Solution : ISolver
{
    public int Year => 2021;

    public int Day => 16;

    public string GetName() => "Packet Decoder";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    static object PartOne(string input)
    {
        var bytes = Convert.FromHexString(input);
        var bits = bytes.SelectMany(b => new[]
        {
            (b & 128) == 128,
            (b & 64) == 64,
            (b & 32) == 32,
            (b & 16) == 16,
            (b & 8) == 8,
            (b & 4) == 4,
            (b & 2) == 2,
            (b & 1) == 1,
        }).ToArray();

        var packetReader = new PacketReader(bits);

        var packet = packetReader.ReadPacket();

        var visitor = new VersionSumVisitor();
        packet.Visit(visitor);

        return visitor.Sum;
    }

    private class VersionSumVisitor : IPacketVisitor
    {
        public long Sum { get; private set; }
        public void Visit(LiteralPacket literalPacket)
        {
            Sum += literalPacket.Header.Version;
        }

        public void Visit(UnknownOperatorPacket unknownOperatorPacket)
        {
            Sum += unknownOperatorPacket.Header.Version;
        }
    }

    static object PartTwo(string input)
    {
        return 0;
    }

    public ref struct PacketReader
    {
        private readonly ReadOnlySpan<bool> _bits;
        private int _index;

        public PacketReader(ReadOnlySpan<bool> bits)
        {
            _bits = bits;
            _index = 0;
        }

        public bool IsAtEnd => _index >= _bits.Length;

        public Packet ReadPacket()
        {
            if (IsAtEnd)
            {
                throw new InvalidOperationException("Reading a new package after the end has been reached already.");
            }

            var header = ReadHeader();
            var packet = header.PacketType switch
            {
                4 => ReadLiteralPacket(header),
                _ => ReadOperatorPacket(header)
            };

            return packet;
        }

        private Packet ReadSinglePacket()
        {
            var header = ReadHeader();
            var packet = header.PacketType switch
            {
                4 => ReadLiteralPacket(header),
                _ => ReadOperatorPacket(header)
            };

            return packet;
        }

        public long ReadLiteral()
        {
            var value = 0L;
            var isLastGroup = false;
            while (!isLastGroup)
            {
                isLastGroup = !Read(1)[0];
                foreach (var b in Read(4))
                {
                    value = (value << 1) + (b ? 1 : 0);
                }
            }

            return value;
        }

        private Packet ReadLiteralPacket(Header header)
        {
            var value = ReadLiteral();
            return new LiteralPacket(header, value);
        }

        public OperatorPacket ReadOperatorPacket(Header header)
        {
            var lengthTypeId = ReadInt(1);
            switch (lengthTypeId)
            {
                case 0:
                    var totalLengthOfSubPackets = ReadInt(15);
                    var subPacketBits = Read(totalLengthOfSubPackets);
                    var subPacketReader = new PacketReader(subPacketBits);
                    var list = new List<Packet>();
                    while (!subPacketReader.IsAtEnd)
                    {
                        list.Add(subPacketReader.ReadPacket());
                    }
                    return new UnknownOperatorPacket(header, list.ToArray());
                case 1:
                    var numberOfSubPackets = ReadInt(11);
                    var subPackets = new Packet[numberOfSubPackets];
                    for (var i = 0; i < numberOfSubPackets; i++)
                    {
                        subPackets[i] = ReadSinglePacket();
                    }
                    return new UnknownOperatorPacket(header, subPackets);
                default:
                    throw new ArgumentException($"Unknown operator type id: {lengthTypeId}");
            }
        }

        private Header ReadHeader()
        {
            var version = ReadInt(3);
            var packetType = ReadInt(3);
            return new Header(version, packetType);
        }

        public int ReadInt(int count)
        {
            var value = 0;
            foreach (var b in Read(count))
            {
                value = (value << 1) + (b ? 1 : 0);
            }

            return value;
        }

        private ReadOnlySpan<bool> Read(int count)
        {
            var span = _bits.Slice(_index, count);
            _index += count;
            return span;
        }
    }

    public class UnknownOperatorPacket : OperatorPacket
    {
        public UnknownOperatorPacket(Header header, Packet[] subPackets)
            : base(header, subPackets)
        {
        }

        public override void Visit(IPacketVisitor visitor)
        {
            visitor.Visit(this);
            VisitChildren(visitor);
        }
    }

    public abstract class OperatorPacket : Packet
    {
        protected OperatorPacket(Header header, Packet[] subPackets)
            : base(header, subPackets)
        {
        }
    }

    public class LiteralPacket : Packet
    {
        private long Value { get; }

        public LiteralPacket(Header header, long value)
            : base(header, Array.Empty<Packet>())
        {
            Value = value;
        }

        public override void Visit(IPacketVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public struct Header
    {
        public int Version { get; }
        public int PacketType { get; }

        public Header(int version, int packetType)
        {
            Version = version;
            PacketType = packetType;
        }
    }

    public abstract class Packet
    {
        public Header Header { get; }

        public Packet[] SubPackets { get; }

        protected Packet(Header header, Packet[] subPackets)
        {
            Header = header;
            SubPackets = subPackets;
        }

        public abstract void Visit(IPacketVisitor visitor);

        protected void VisitChildren(IPacketVisitor visitor)
        {
            foreach (var subPacket in SubPackets)
            {
                subPacket.Visit(visitor);
            }
        }
    }

    public interface IPacketVisitor
    {
        void Visit(LiteralPacket literalPacket);
        void Visit(UnknownOperatorPacket unknownOperatorPacket);
    }

}