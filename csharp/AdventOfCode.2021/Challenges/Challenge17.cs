using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Challenges
{
    [Challenge(17)]
    public class Challenge17
    {
        private readonly IInputReader inputReader;
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
            // Only calculate first half of the arc
            int vy = (-targetYMax) - 1;
            return Enumerable.Range(0, vy).Select(x => vy - x).Sum().ToString();
        }

        private int[] FindAllXVelocities(int lowerBound, int upperBound)
        {
            List<int> velocities = new List<int>();
            for (int ivx = upperBound; ivx > 0; ivx--)
            {
                int x = 0;
                int vx = ivx;
                while (x < upperBound && vx != 0)
                {
                    x += vx;
                    if (x >= lowerBound && x <= upperBound)
                    {
                        velocities.Add(ivx);
                        break;
                    }

                    vx -= Math.Sign(vx);
                }
            }

            return velocities.ToArray();
        }

        [Part2]
        public string Part2()
        {
            var xVelocities = FindAllXVelocities(targetXMin, targetXMax);
            var yVelocities = Enumerable.Range(targetYMax, (-targetYMax)-targetYMax+1).ToArray();

            int count = 0;
            foreach (var vy in yVelocities)
            {
                foreach (var vx in xVelocities)
                {
                    if (Simulate(vx, vy, targetXMin, targetXMax, targetYMin, targetYMax))
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
