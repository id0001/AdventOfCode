using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using AdventOfCode2019.IntCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Challenges
{
    [Challenge(23)]
    public class Challenge23
    {
        private readonly IInputReader inputReader;
        private long[] data;

        public Challenge23(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            data = await inputReader.ReadLineAsync<long>(23, ',').ToArrayAsync();
        }

        [Part1]
        public string Part1()
        {
            var cpus = new Cpu[50];
            var packetQueues = new Queue<long>[50];
            var outputBuffers = new Queue<long>[50];

            long result = -1;
            for (int i = 0; i < 50; i++)
            {
                int id = i;
                outputBuffers[i] = new Queue<long>();
                packetQueues[i] = new Queue<long>();

                cpus[i] = new Cpu();
                cpus[i].SetProgram(data);
                cpus[i].RegisterOutput(v =>
                {
                    outputBuffers[id].Enqueue(v);

                    if (outputBuffers[id].Count >= 3)
                    {
                        int dest = (int)outputBuffers[id].Dequeue();
                        long x = outputBuffers[id].Dequeue();
                        long y = outputBuffers[id].Dequeue();
                        Console.WriteLine($"{id} -> {dest}: ({x},{y})");

                        if (dest == 255)
                        {
                            result = y;
                            foreach (var cpu in cpus)
                                cpu.Halt();

                            return;
                        }

                        packetQueues[dest].Enqueue(x);
                        packetQueues[dest].Enqueue(y);
                    }
                });

                cpus[i].RegisterInput(() =>
                {
                    if (packetQueues[id].Count > 0)
                    {
                        Console.WriteLine($"{id} <-- {packetQueues[id].Peek()}");
                        cpus[id].WriteInput(packetQueues[id].Dequeue());
                    }
                    else
                    {
                        cpus[id].WriteInput(-1);
                    }
                });
            }

            for (int i = 0; i < cpus.Length; i++)
                cpus[i].Start(i);

            while (result == -1)
            {
                for (int i = 0; i < cpus.Length; i++)
                    cpus[i].Next();
            }

            return result.ToString();
        }

        [Part2]
        public string Part2()
        {
            var cpus = new Cpu[50];
            var packetQueues = new Queue<long>[50];
            var outputBuffers = new Queue<long>[50];
            bool[] readIdle = new bool[50];

            long natX = -1;
            long natY = -1;
            long lastY = -1;

            long result = -1;
            for (int i = 0; i < 50; i++)
            {
                int id = i;
                outputBuffers[i] = new Queue<long>();
                packetQueues[i] = new Queue<long>();

                cpus[i] = new Cpu();
                cpus[i].SetProgram(data);
                cpus[i].RegisterOutput(v =>
                {
                    outputBuffers[id].Enqueue(v);

                    if (outputBuffers[id].Count >= 3)
                    {
                        int dest = (int)outputBuffers[id].Dequeue();
                        long x = outputBuffers[id].Dequeue();
                        long y = outputBuffers[id].Dequeue();
                        Console.WriteLine($"{id} -> {dest}: ({x},{y})");

                        if (dest == 255)
                        {
                            natX = x;
                            natY = y;
                        }
                        else
                        {
                            packetQueues[dest].Enqueue(x);
                            packetQueues[dest].Enqueue(y);
                        }
                    }
                });

                cpus[i].RegisterInput(() =>
                {
                    if (packetQueues[id].Count > 0)
                    {
                        Console.WriteLine($"{id} <-- {packetQueues[id].Peek()}");
                        cpus[id].WriteInput(packetQueues[id].Dequeue());
                        readIdle[id] = false;
                    }
                    else
                    {
                        cpus[id].WriteInput(-1);
                        readIdle[id] = true;
                    }
                });
            }

            for (int i = 0; i < cpus.Length; i++)
                cpus[i].Start(i);

            while (result == -1)
            {
                for (int i = 0; i < cpus.Length; i++)
                    cpus[i].Next();

                if (packetQueues.All(b => b.Count == 0) && readIdle.All(i => i) && natY >= 0)
                {
                    if (lastY == natY)
                    {
                        result = natY;
                        break;
                    }

                    packetQueues[0].Enqueue(natX);
                    packetQueues[0].Enqueue(natY);
                    lastY = natY;
                }
            }

            return result.ToString();
        }
    }
}
