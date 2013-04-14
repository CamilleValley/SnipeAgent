using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace UltimateSniper_SchedulerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!Program.IsProcessOpen(Process.GetCurrentProcess().ProcessName))
            {
                UltimateSniper_SchedulerEngine.Scheduler_Engine scheduler = new UltimateSniper_SchedulerEngine.Scheduler_Engine();

                scheduler.BidOptimizerEnabled = false;
                scheduler.SnipeEnabled = true;

                scheduler.StartScheduler();

                string str;

                Console.WriteLine("Enter some characters.");
                str = Console.ReadLine(); 
            }
        }

        public static bool IsProcessOpen(string name)
        {
            bool found2 = false;
            //here we're going to get a list of all running processes on
            //the computer
            foreach (Process clsProcess in Process.GetProcesses())
            {
                //now we're going to see if any of the running processes
                //match the currently running processes. Be sure to not
                //add the .exe to the name you provide, i.e: NOTEPAD,
                //not NOTEPAD.EXE or false is always returned even if
                //notepad is running.
                //Remember, if you have the process running more than once, 
                //say IE open 4 times the loop thr way it is now will close all 4,
                //if you want it to just close the first one it finds
                //then add a return; after the Kill
                if (clsProcess.ProcessName.Contains(name))
                {
                    //if the process is found to be running then we
                    //return a true
                    if (found2)
                        return true;
                    else
                        found2 = true;
                }
            }
            //otherwise we return a false
            return false;
        }
    }
}
