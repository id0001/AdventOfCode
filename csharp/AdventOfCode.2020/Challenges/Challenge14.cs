using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
    [Challenge(14)]
    public class Challenge14
    {
        private readonly IInputReader inputReader;

        public Challenge14(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Part1]
        public async Task<string> Part1Async()
        {
            var memory = new Dictionary<ulong, ulong>();

            string mask = null;

            await foreach (var line in inputReader.ReadLinesAsync(14))
            {
                if (line.StartsWith("mask = "))
                {
                    mask = line.Substring("mask = ".Length);
                }
                else
                {
                    var match = Regex.Match(line, @"^mem\[(\d+)\] = (\d+)$");
                    ulong addr = ulong.Parse(match.Groups[1].Value);
                    ulong value = ulong.Parse(match.Groups[2].Value);

                    if (!memory.ContainsKey(addr))
                        memory.Add(addr, 0);

                    memory[addr] = ApplyMask(value, mask);
                }
            }

            return memory.Sum(kv => (long)kv.Value).ToString();
        }

        [Part2]
        public async Task<string> Part2Async()
        {
            var memory = new Dictionary<ulong, ulong>();

            string mask = null;

            await foreach (var line in inputReader.ReadLinesAsync(14))
            {
                if (line.StartsWith("mask = "))
                {
                    mask = line.Substring("mask = ".Length);
                }
                else
                {
                    var match = Regex.Match(line, @"^mem\[(\d+)\] = (\d+)$");
                    ulong addr = ulong.Parse(match.Groups[1].Value);
                    ulong value = ulong.Parse(match.Groups[2].Value);

                    foreach (var realAddr in EnumerateAddresses(addr, mask))
                    {
                        if (!memory.ContainsKey(realAddr))
                            memory.Add(realAddr, 0);

                        memory[realAddr] = value;
                    }
                }
            }

            return memory.Sum(kv => (long)kv.Value).ToString();
        }

        private ulong ApplyMask(ulong value, string mask)
        {
            for (int i = 0; i < mask.Length; i++)
            {
                switch (mask[i])
                {
                    case '0':
                        value = value & MaskFor(i);
                        break;
                    case '1':
                        value = (value & MaskFor(i)) + (1ul << (35 - i));
                        break;
                }
            }

            return value;
        }

        private IEnumerable<ulong> EnumerateAddresses(ulong value, string mask)
        {
            var floatingIndices = new List<int>();
            for (int i = 0; i < mask.Length; i++)
            {
                switch (mask[i])
                {
                    case '1':
                        value = (value & MaskFor(i)) + (1ul << (35 - i));
                        break;
                    case 'X':
                        value = value & MaskFor(i);
                        floatingIndices.Add(i);
                        break;
                }
            }

            int count = (int)Math.Pow(2, floatingIndices.Count);
            for (int i = 0; i < count; i++)
            {
                var bits = new BitArray(new int[] { i });
                yield return AddBitVal(bits, floatingIndices, value, 0);
            }
        }

        private ulong AddBitVal(BitArray bits, IList<int> indices, ulong value, int i)
        {
            if (i >= bits.Count)
                return value;

            return AddBitVal(bits, indices, value + (bits[i] ? 1ul << (35 - indices[i]) : 0ul), i + 1);
        }

        private void PrintBin(ulong v)
        {
            Console.WriteLine(Convert.ToString((long)v, 2).PadLeft(64, '0') + $"({v})");
        }

        private ulong MaskFor(int i)
        {
            ulong realMask = 0x0000_000F_FFFF_FFFF;
            return realMask - (1ul << (35 - i));
        }
    }
}
