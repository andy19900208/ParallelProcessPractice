using ParallelProcessPractice.Core;
using System;

namespace AndyDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            TaskRunnerBase run = new AndyTaskRunner();
            run.ExecuteTasks(100);
        }
    }
}
