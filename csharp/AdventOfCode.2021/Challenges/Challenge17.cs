using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;

namespace AdventOfCode2021.Challenges
{
    [Challenge(17)]
    public class Challenge17
    {
        private readonly IInputReader inputReader;
        //private int targetXMin = 128;
        //private int targetXMax = 160;
        //private int targetYMin = -88;
        //private int targetYMax = -142;
        private int targetXMin = 192;
        private int targetXMax = 251;
        private int targetYMin = -59;
        private int targetYMax = -89;

        public Challenge17(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Part1]
        public string Part1()
        {
            int vy = (-targetYMax) - 1;
            return MathEx.TriangularNumber(vy).ToString();
        }

        [Part2]
        public string Part2()
        {
            int left = MathEx.InverseTriangleNumber(targetXMin);
            int right = targetXMax;
            int bottom = targetYMax;
            int top = -targetYMax - 1;

            int count = 0;
            for (int y = bottom; y <= top; y++)
            {
                for (int x = left; x <= right; x++)
                {
                    if (Simulate(x, y, targetXMin, targetXMax, targetYMin, targetYMax))
                        count++;
                }
            }

            return count.ToString();
        }

        private static bool Simulate(int vx, int vy, int xlow, int xhigh, int ylow, int yhigh)
        {
            int x = 0;
            int y = 0;
            while (x < xhigh && y > yhigh)
            {
                x += vx;
                y += vy;
                vx -= Math.Sign(vx);
                vy -= 1;

                if (x >= xlow && x <= xhigh && y <= ylow && y >= yhigh)
                    return true;
            }

            return false;
        }
    }
}
