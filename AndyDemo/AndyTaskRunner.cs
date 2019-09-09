using ParallelProcessPractice.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AndyDemo
{
    public class MyTaskPlus
    {
        private MyTask _myTask = null;
        public MyTaskPlus(MyTask myTask)
        {
            _myTask = myTask;
        }
        public bool doing = false;
        public void DoStepN(int step)
        {
            _myTask.DoStepN(step);
            doing = false;
        }

        public int ID
        {
            get
            {
                return _myTask.ID;
            }
        }

        public int CurrentStep
        {
            get
            {
                return _myTask.CurrentStep;
            }
        }
    }

    public class AndyTaskRunner : TaskRunnerBase
    {
        private List<MyTaskPlus> newTasks = new List<MyTaskPlus>();

        public override void Run(IEnumerable<MyTask> tasks)
        {
            newTasks = tasks.Select(s => new MyTaskPlus(s)).ToList();

            List<Task> threads = new List<Task>();

            threads.Add(Task.Run(() => Process(1)));
            threads.Add(Task.Run(() => Process(1)));
            threads.Add(Task.Run(() => Process(1)));
            threads.Add(Task.Run(() => Process(1)));
            threads.Add(Task.Run(() => Process(1)));

            threads.Add(Task.Run(() => Process(2)));
            threads.Add(Task.Run(() => Process(2)));
            threads.Add(Task.Run(() => Process(2)));

            threads.Add(Task.Run(() => Process(3)));
            threads.Add(Task.Run(() => Process(3)));
            threads.Add(Task.Run(() => Process(3)));

            Task.WaitAll(threads.ToArray());
        }

        private void Process(int step)
        {
            while (newTasks.Any(a => a.CurrentStep < step || a.doing))
            {
                Task T = null;
                lock (newTasks)
                {
                    if (newTasks.Any(f => f.CurrentStep == step - 1 && !f.doing))
                    {
                        var firstTask = newTasks.First(f => f.CurrentStep == step - 1 && !f.doing);
                        firstTask.doing = true;
                        T = Task.Run(() => firstTask.DoStepN(step));
                    }
                    else
                    {
                        Thread.Sleep(1);
                    }
                }
                if (T != null)
                {
                    T.Wait();
                }
            }
        }
    }
}
