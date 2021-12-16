using AdventOfCode.Lib;
using AdventOfCode.Lib.Extensions;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(16)]
    public class Challenge16
    {
        private readonly IInputReader inputReader;
        private Packet packet;

        public Challenge16(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            int fromBase = 16;
            int toBase = 2;

            char[] input = (await inputReader.ReadLineAsync(16)
                .ToArrayAsync())
                .SelectMany(c => Convert.ToString(Convert.ToInt32(c.ToString(), fromBase), toBase).PadLeft(4, '0'))
                .ToArray();

            ReadPacket(input, 0, out packet);
        }

        [Part1]
        public string Part1()
        {
            return CountVersion(packet).ToString();
        }

        [Part2]
        public string Part2()
        {
            return ExecutePacket(packet).ToString();
        }

        private long ExecutePacket(Packet packet)
        {
            switch (packet)
            {
                case LiteralValuePacket lv:
                    return lv.Value;
                case OperatorPacket op:
                    return ExecuteOperatorPacket(op);
                default:
                    throw new NotSupportedException();
            }
        }

        private long ExecuteOperatorPacket(OperatorPacket packet)
        {
            return packet.Id switch
            {
                0 => packet.SubPackets.Sum(ExecutePacket),
                1 => packet.SubPackets.Product(ExecutePacket),
                2 => packet.SubPackets.Min(ExecutePacket),
                3 => packet.SubPackets.Max(ExecutePacket),
                5 => ExecutePacket(packet.SubPackets[0]) > ExecutePacket(packet.SubPackets[1]) ? 1L : 0L,
                6 => ExecutePacket(packet.SubPackets[0]) < ExecutePacket(packet.SubPackets[1]) ? 1L : 0L,
                7 => ExecutePacket(packet.SubPackets[0]) == ExecutePacket(packet.SubPackets[1]) ? 1L : 0L,
                _ => throw new NotSupportedException()
            };
        }

        private int CountVersion(Packet packet)
        {
            switch (packet)
            {
                case LiteralValuePacket lv:
                    return lv.Version;
                case OperatorPacket op:
                    return op.Version + op.SubPackets.Sum(CountVersion);
                default:
                    throw new NotSupportedException();
            }
        }

        private static int ReadPacket(char[] data, int index, out Packet packet)
        {
            index = ReadInt(data, index, 3, out int version);
            index = ReadInt(data, index, 3, out int id);

            if (id == 4)
            {
                // Literal value
                index = ReadLiteralValue(data, index, out long literalValue);
                packet = new LiteralValuePacket(version, id, literalValue);
                return index;
            }

            index = ReadInt(data, index, 1, out int lengthTypeId);
            var subPackets = new List<Packet>();

            if (lengthTypeId == 0)
            {
                index = ReadInt(data, index, 15, out int totalSubPacketLength);
                int start = index;
                while (index - start < totalSubPacketLength)
                {
                    index = ReadPacket(data, index, out Packet subPacket);
                    subPackets.Add(subPacket);
                }
            }
            else
            {
                index = ReadInt(data, index, 11, out int totalSubPackets);
                for (int i = 0; i < totalSubPackets; i++)
                {
                    index = ReadPacket(data, index, out Packet subPacket);
                    subPackets.Add(subPacket);
                }
            }

            packet = new OperatorPacket(version, id, subPackets);
            return index;
        }

        private static int ReadInt(char[] data, int index, int count, out int value)
        {
            value = Convert.ToInt32(new string(new ArraySegment<char>(data, index, count)), 2);
            index += count;
            return index;
        }

        private static int ReadLiteralValue(char[] data, int index, out long literalValue)
        {
            var sb = new StringBuilder();
            bool keepReading = true;
            while (keepReading)
            {
                keepReading = data[index] == '1';
                index++;

                sb.Append(new string(new ArraySegment<char>(data, index, 4)));
                index += 4;
            }

            literalValue = Convert.ToInt64(sb.ToString(), 2);
            return index;
        }

        private record Packet(int Version, int Id);

        private record LiteralValuePacket(int Version, int Id, long Value) : Packet(Version, Id);

        private record OperatorPacket(int Version, int Id, List<Packet> SubPackets) : Packet(Version, Id);
    }
}
