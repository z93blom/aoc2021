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
        yield return PartOne(input.Lines().First());
        foreach (var line in input.Lines())
        {
            yield return PartTwo(line);
        }
    }

    static object PartOne(string input)
    {
        var bits = ToBits(input);

        var packetReader = new PacketReader(bits);

        var packet = packetReader.ReadPacket();

        var visitor = new VersionSumVisitor();
        packet.Visit(visitor);

        return visitor.Sum;
    }

    private static bool[] ToBits(string input)
    {
        return input.SelectMany(c =>
        {
            var bits = c switch
            {
                '0' => new[] { false, false, false, false },
                '1' => new[] { false, false, false, true },
                '2' => new[] { false, false, true, false },
                '3' => new[] { false, false, true, true },
                '4' => new[] { false, true, false, false },
                '5' => new[] { false, true, false, true },
                '6' => new[] { false, true, true, false },
                '7' => new[] { false, true, true, true },
                '8' => new[] { true, false, false, false },
                '9' => new[] { true, false, false, true },
                'A' => new[] { true, false, true, false },
                'B' => new[] { true, false, true, true },
                'C' => new[] { true, true, false, false },
                'D' => new[] { true, true, false, true },
                'E' => new[] { true, true, true, false },
                'F' => new[] { true, true, true, true },
                _ => throw new NotImplementedException()
            };
            return bits;
        }).ToArray();
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
            unknownOperatorPacket.VisitChildren(this);
        }

        public void Visit(KnownOperatorPacket knownOperatorPacket)
        {
            Sum += knownOperatorPacket.Header.Version;
            knownOperatorPacket.VisitChildren(this);
        }
    }

    static object PartTwo(string input)
    {
        var bits = ToBits(input);

        var packetReader = new PacketReader(bits);

        var packet = packetReader.ReadPacket();
        var value = packet.CalculateValue();
        return value;
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
            var subPackets = ReadSubPackets();
            switch (header.PacketType)
            {
                case 0:
                    // Sum packet.
                    return new KnownOperatorPacket(header, subPackets, p => p.SubPackets.Sum(c => c.CalculateValue()));

                case 1:
                    // Product packet.
                    return new KnownOperatorPacket(header, subPackets, p => p.SubPackets.Aggregate(1L, (v, c) => v * c.CalculateValue()));

                case 2:
                    // Minimum packet.
                    return new KnownOperatorPacket(header, subPackets, p => p.SubPackets.Min(c => c.CalculateValue()));

                case 3:
                    // Maximum packet.
                    return new KnownOperatorPacket(header, subPackets, p => p.SubPackets.Max(c => c.CalculateValue()));

                case 5:
                    // Greater than packet.
                    return new KnownOperatorPacket(header, subPackets, p => p.SubPackets[0].CalculateValue() > p.SubPackets[1].CalculateValue() ? 1 : 0);

                case 6:
                    // Less than packet.
                    return new KnownOperatorPacket(header, subPackets, p => p.SubPackets[0].CalculateValue() < p.SubPackets[1].CalculateValue() ? 1 : 0);

                case 7:
                    // Equal to packet.
                    return new KnownOperatorPacket(header, subPackets, p => p.SubPackets[0].CalculateValue() == p.SubPackets[1].CalculateValue() ? 1 : 0);

                default:
                    return new UnknownOperatorPacket(header, subPackets);
            }
        }

        private Packet[] ReadSubPackets()
        {
            Packet[] children;
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

                    children = list.ToArray();
                    break;
                case 1:
                    var numberOfSubPackets = ReadInt(11);
                    children = new Packet[numberOfSubPackets];
                    for (var i = 0; i < numberOfSubPackets; i++)
                    {
                        children[i] = ReadSinglePacket();
                    }

                    break;
                default:
                    throw new ArgumentException($"Unknown operator type id: {lengthTypeId}");
            }

            return children;
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
        }

        public override long CalculateValue()
        {
            throw new InvalidOperationException(
                $"Trying to calculate the value of an unknown operator ('{Header.PacketType}') is not supported. ");
        }
    }

    public class KnownOperatorPacket : OperatorPacket
    {
        private readonly Func<Packet, long> _valueFunction;

        public KnownOperatorPacket(Header header, Packet[] subPackets, Func<Packet, long> valueFunction)
            : base(header, subPackets)
        {
            _valueFunction = valueFunction;
        }

        public override void Visit(IPacketVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override long CalculateValue()
        {
            return _valueFunction(this);
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
        public long Value { get; }

        public LiteralPacket(Header header, long value)
            : base(header, Array.Empty<Packet>())
        {
            Value = value;
        }

        public override void Visit(IPacketVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override long CalculateValue()
        {
            return Value;
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

        public void VisitChildren(IPacketVisitor visitor)
        {
            foreach (var subPacket in SubPackets)
            {
                subPacket.Visit(visitor);
            }
        }
        public abstract long CalculateValue();
    }

    public interface IPacketVisitor
    {
        void Visit(LiteralPacket literalPacket);
        void Visit(UnknownOperatorPacket unknownOperatorPacket);

        void Visit(KnownOperatorPacket knownOperatorPacket);
    }

}