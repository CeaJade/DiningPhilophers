using System;
using System.Collections.Generic;
using System.Threading;

namespace DiningPhilophers
{
    class Program
    {
        static readonly Random rdm = new Random();
        static bool[] forks = new bool[5] {false, false, false, false, false };
        static List<int>[] forkToPhilo = new List<int>[5];

        static void Main(string[] args)
        {

            Thread[] philosiphers = new Thread[5];
            while (true)
            {
            // create the amount of philosophers and assigning their fork to them.
                for (int i = 0; i < philosiphers.Length; i++)
                {
                    philosiphers[i] = new Thread(CheckFork);
                    philosiphers[i].Name = "P" + i.ToString();
                    forkToPhilo[i] = new List<int>() {i, (i+1)%5};
                    philosiphers[i].Start(forkToPhilo[i]);
                }
            }
        }

        static void CheckFork(object index)
        {
            List<int> i = index as List<int>;
            
            Monitor.Enter(forks);
            if (forks[i[0]] == true || forks[i[1]] == true)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} is waiting...");
                Monitor.Wait(forks);
            }
            forks[i[0]] = true;
            forks[i[1]] = true;
            Monitor.Pulse(forks);
            Console.WriteLine($"{Thread.CurrentThread.Name} is eating.");
            Thread.Sleep(rdm.Next(1000, 3000));
            Monitor.Exit(forks);

            forks[i[0]] = false;
            forks[i[1]] = false;
            Console.WriteLine($"{Thread.CurrentThread.Name} is thinking..");
            Thread.Sleep(rdm.Next(2000, 4000));
        }
    }
}
