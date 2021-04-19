using AdventOfCode.Lib;

namespace AdventOfCode2020.Challenges
{
    [Challenge(25)]
    public class Challenge25
    {
        [Part1]
        public string Part1()
        {
            long pub1 = 13135480;
            long pub2 = 8821721;

            long loopSize1 = FindLoopSize(7, pub1);

            long key1 = Transform(pub2, loopSize1);

            return key1.ToString();
        }

        private long FindLoopSize(long subjectNumber, long target)
        {
            long value = 1;

            int i = 0;
            while (value != target)
            {
                value = value * subjectNumber;
                value %= 20201227;
                i++;
            }

            return i;
        }

        private long Transform(long subjectNumber, long loopSize)
        {
            long value = 1;
            for(int i = 0; i < loopSize; i++)
            {
                value = value * subjectNumber;
                value %= 20201227;
            }

            return value;
        }
    }
}
