using AdventOfCode2019.IntCode.Core;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2019.Challenges
{
    [Challenge(23)]
    public class Challenge23
    {
        private readonly IInputReader _inputReader;

        public Challenge23(IInputReader inputReader)
        {
            _inputReader = inputReader;
        }

        [Part1]
        public async Task<string> Part1Async()
        {
            var program = await _inputReader.ReadLineAsync<long>(23, ',').ToArrayAsync();

            var cpus = new Cpu[50];
            var packetQueues = new Queue<long>[50];
            var outputBuffers = new Queue<long>[50];

            long result = -1;
            for (var i = 0; i < 50; i++)
            {
                var id = i;
                outputBuffers[i] = new Queue<long>();
                packetQueues[i] = new Queue<long>();

                cpus[i] = new Cpu();
                cpus[i].SetProgram(program);
                cpus[i].RegisterOutput(v =>
                {
                    outputBuffers[id].Enqueue(v);

                    if (outputBuffers[id].Count < 3) return;
                    
                    var dest = (int)outputBuffers[id].Dequeue();
                    var x = outputBuffers[id].Dequeue();
                    var y = outputBuffers[id].Dequeue();

                    if (dest == 255)
                    {
                        result = y;
                        foreach (var cpu in cpus)
                            cpu.Halt();

                        return;
                    }

                    packetQueues[dest].Enqueue(x);
                    packetQueues[dest].Enqueue(y);
                });

                cpus[i].RegisterInput(() =>
                {
                    if (packetQueues[id].Count > 0)
                    {
                        cpus[id].WriteInput(packetQueues[id].Dequeue());
                    }
                    else
                    {
                        cpus[id].WriteInput(-1);
                    }
                });
            }

            for (var i = 0; i < cpus.Length; i++)
                cpus[i].Start(i);

            while (result == -1)
            {
                foreach (var cpu in cpus)
                    cpu.Next();
            }

            return result.ToString();
        }

        [Part2]
        public async Task<string> Part2Async()
        {
            var program = await _inputReader.ReadLineAsync<long>(23, ',').ToArrayAsync();

            var cpus = new Cpu[50];
            var packetQueues = new Queue<long>[50];
            var outputBuffers = new Queue<long>[50];
            var readIdle = new bool[50];

            long natX = -1;
            long natY = -1;
            long lastY = -1;

            for (var i = 0; i < 50; i++)
            {
                var id = i;
                outputBuffers[i] = new Queue<long>();
                packetQueues[i] = new Queue<long>();

                cpus[i] = new Cpu();
                cpus[i].SetProgram(program);
                cpus[i].RegisterOutput(v =>
                {
                    outputBuffers[id].Enqueue(v);

                    if (outputBuffers[id].Count < 3) return;
                    
                    var dest = (int)outputBuffers[id].Dequeue();
                    var x = outputBuffers[id].Dequeue();
                    var y = outputBuffers[id].Dequeue();

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
                });

                cpus[i].RegisterInput(() =>
                {
                    if (packetQueues[id].Count > 0)
                    {
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

            for (var i = 0; i < cpus.Length; i++)
                cpus[i].Start(i);

            while (true)
            {
                foreach (var t in cpus)
                    t.Next();

                if (packetQueues.Any(b => b.Count != 0) || !readIdle.All(i => i) || natY < 0) continue;
                
                if (lastY == natY)
                {
                    return natY.ToString();
                }

                packetQueues[0].Enqueue(natX);
                packetQueues[0].Enqueue(natY);
                lastY = natY;
            }
        }
    }
}
