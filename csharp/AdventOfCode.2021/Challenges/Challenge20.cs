using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(20)]
    public class Challenge20
    {
        private readonly IInputReader inputReader;
        private char[] enhancement;
        private Dictionary<Point2, bool> image;

        public Challenge20(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            string[] lines = await inputReader.ReadLinesAsync(20).ToArrayAsync();
            enhancement = lines[0].ToCharArray();

            lines = lines.Skip(2).ToArray();

            image = lines.SelectMany((line, y) => line.Select((c, x) => (new Point2(x, y), c == '#'))).ToDictionary(kv => kv.Item1, kv => kv.Item2);
        }

        [Part1]
        public string Part1()
        {
            bool outsideLit = false;
            for (int i = 0; i < 2; i++)
            {
                image = Enhance(image, enhancement, outsideLit);
                outsideLit = !outsideLit;
            }

            return image.Count(x => x.Value).ToString();
        }

        [Part2]
        public string Part2()
        {
            bool outsideLit = false;
            for (int i = 0; i < 50; i++)
            {
                image = Enhance(image, enhancement, outsideLit);
                outsideLit = !outsideLit;
            }

            return image.Count(x => x.Value).ToString();
        }

        private static Dictionary<Point2, bool> Enhance(Dictionary<Point2, bool> image, char[] enhancementAlgorithm, bool outsideLit)
        {
            Dictionary<Point2, bool> newImage = new Dictionary<Point2, bool>();
            int left = image.Min(x => x.Key.X);
            int top = image.Min(x => x.Key.Y);
            int right = image.Max(x => x.Key.X);
            int bottom = image.Max(x => x.Key.Y);

            for (int y = top - 1; y <= bottom + 1; y++)
            {
                for (int x = left - 1; x <= right + 1; x++)
                {
                    int index = ToIndex(GetBits(x, y, image, outsideLit));
                    newImage.Add(new Point2(x, y), enhancementAlgorithm[index] == '#');
                }
            }

            return newImage;
        }

        private static IEnumerable<Point2> GetSquare3x3(Point2 center)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    yield return new Point2(center.X + x, center.Y + y);
                }
            }
        }

        private static IEnumerable<char> GetBits(int x, int y, Dictionary<Point2, bool> image, bool outsideLit)
        {
            foreach (var point in GetSquare3x3(new Point2(x, y)))
            {
                if (!image.TryGetValue(point, out bool value))
                {
                    yield return outsideLit ? '1' : '0';
                    continue;
                }

                yield return value ? '1' : '0';
            }
        }

        private static int ToIndex(IEnumerable<char> bits)
        {
            return Convert.ToInt32(new string(bits.ToArray()), 2);
        }
    }
}
