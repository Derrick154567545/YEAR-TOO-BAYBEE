
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace hrose
{
    internal class Program
    {
        static int tracklength = 100;

        static void Main(string[] args)
        {
            List<hrosne> stables = new List<hrosne>();
            stables.Add(new hrosne("Drk", "roided", "DARKYELLOW","Derrick"));
            stables.Add(new hrosne("Mnz", "threelegged", "BLUE", " Menzel"));
            stables.Add(new hrosne("Ira", "roided", "RED", "    Ira"));
            stables.Add(new hrosne("Rmn", "roided", "CYAN", "  Ramon"));
            stables.Add(new hrosne("Cld", "roided", "GREEN", " Claude"));
            stables.Add(new hrosne("Brd", "roided", "magenta", "   Brid"));
            stables.Add(new hrosne("Sph", "roided", "darkcyan", " Joseph"));
            stables.Add(new hrosne("Dnzl", "roided", "darkmagenta", "Denzel"));
            stables.Add(new hrosne("Ian", "roided", "yellow", "    Ian"));
            stables.Add(new hrosne("Jls", "roided", "darkred", " Julius"));

            string program = "go";
            while(program == "go")
            {
                List<string> history = new List<string>();
                history.Add("RACE STARTS");

                int win = 0;    

                DTrack(stables);

                Console.ReadKey();

                while (win == 0)
                {
                    Console.Clear();
                    string leading = wholead(stables);
                    enoughtimehaspassed(stables);
                    if (leadingchangecheck(stables, leading) != "")
                        history.Add(leadingchangecheck(stables, leading));
                    string specialqualityupdate = specialqualitiescheck(stables);
                    if (specialqualityupdate != "")
                        history.Add(specialqualityupdate);
                    win = wincheck(stables);
                    DTrack(stables);
                    for (int i = 0; i < history.Count; i++)
                        Console.WriteLine(history[i]);
                    //Console.ReadKey();
                    Thread.Sleep(50);
                }
                Console.WriteLine(winmessage(stables, win));
                Console.ReadKey();
                Console.Clear();
                for(int x = 0; x < stables.Count; x ++)
                    stables[x].specialqualityupdate("reset");
            }
        }

        static void DTrack(List<hrosne> stables)
        {
            for (int i = 0; i < stables.Count; i++)
            {
                string output = "";
                string[] hoong = stables[i].getname();
                colordecider(hoong[2]);
                output = output + hoong[0] + " : ";
                for (int j = 0; j < tracklength + 2; j++)
                {
                    if (j == stables[i].getpos())
                        output = output + hoong[4];
                    else if (j == tracklength + 1)
                        output = output + "||";
                    else
                        output = output + " ";
                }
                Console.WriteLine(output);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        static void enoughtimehaspassed(List<hrosne> stables)
        {
            for (int i = 0; i < stables.Count; i++)
            {
                stables[i].horsedo();
            }
        }

        static int wincheck(List<hrosne> stables)
        {
            for (int i = 0; i < stables.Count; i++)
            {
                string[] hoong = stables[i].getname();
                if (stables[i].getpos() > tracklength)
                    return 1;
            }
            if (tiecheck(stables) == "Nobody Wins")
                return 2;
            return 0;
        }

        static string winmessage(List<hrosne> stables, int won)
        {
            string op = "";
            if (won == 1)
            {
                List<string> winners = new List<string>();
                for (int i = 0; i < stables.Count; i++)
                {
                    string[] hoong = stables[i].getname();
                    if (stables[i].getpos() > tracklength)
                        winners.Add(hoong[0]);
                }
                if(winners.Count == 1)
                    op = op + winners[0] + " REACHES THE FINISH LINE";
                if(winners.Count > 1)
                {
                    for(int x = 0; x < winners.Count; x++)
                    {
                        op = op + winners[x];
                        op = op + " AND ";
                    }
                    op = op + " CROSSED THE FINISH LINE AT THE SAME TIME!!!";
                }
            }
            if (won == 2)
                return "Nobody wins";
            return op;
        }

        static void colordecider(string color)
        {
            color = color.ToUpper();
            if (color == "GREEN")
                Console.ForegroundColor = ConsoleColor.Green;
            if (color == "RED")
                Console.ForegroundColor = ConsoleColor.Red;
            if (color == "BLUE")
                Console.ForegroundColor = ConsoleColor.Blue;
            if (color == "CYAN")
                Console.ForegroundColor = ConsoleColor.Cyan;
            if (color == "YELLOW")
                Console.ForegroundColor = ConsoleColor.Yellow;
            if (color == "DARKYELLOW")
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            if(color == "DARKBLUE")
                Console.ForegroundColor = ConsoleColor.DarkBlue;
            if (color == "DARKCYAN")
                Console.ForegroundColor = ConsoleColor.DarkCyan;
            if (color == "DARKGREEN")
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            if (color == "DARKRED")
                Console.ForegroundColor = ConsoleColor.DarkRed;
            if (color == "DARKMAGENTA")
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            if (color == "GRAY")
                Console.ForegroundColor = ConsoleColor.Gray;
            if (color == "DARKGRAY")
                Console.ForegroundColor = ConsoleColor.DarkGray;
            if (color == "MAGENTA")
                Console.ForegroundColor = ConsoleColor.Magenta;
            if (color == "WHITE")
                Console.ForegroundColor = ConsoleColor.White;
        }

        static string wholead (List<hrosne> stables)
        {
            string leading = "";
            List<int> currentstate = new List<int> { };
            for (int i = 0; i < stables.Count; i++)
            {
                int j = stables[i].getpos();
                currentstate.Add(j);
                
            }
            int max = -999;
            for (int i = 0; i < currentstate.Count; i++)
            {
                string[] hoong = stables[i].getname();
                if (currentstate[i] > max)
                {
                    max = currentstate[i];
                    leading = hoong[0];
                }
            }
            int dupes = 0;
            for (int i = 0; i < currentstate.Count; i++)
            {
                if (max == currentstate[i])
                    dupes = dupes + 1;
            }
            if (dupes > 1)
                leading = "";
            return leading;
        }

        static string leadingchangecheck(List<hrosne> stables, string leading)
        {
            string newleader = wholead(stables);
            string output = "";
            if (newleader == "" && leading != "")
                return (leading + " LOSES THE LEAD!!!!");
            if (newleader != leading)
                output = (newleader + " TAKES THE LEAD!!");
            return output;
        }

        static string specialqualitiescheck(List<hrosne> stables)
        {
            string output = "";
            for (int i = 0; i < stables.Count; i++)
            {
                string[] hoong = stables[i].getname();
                if (hoong[1] == "roided" && hoong[3] == "DiedFromSteroidUse")
                {
                    output = hoong[0] + " has fallen over from steroid use";
                    stables[i].specialqualityupdate("rip");
                }
                if (hoong[1] == "threelegged" && hoong[3] == "Pitiful")
                {
                    output = hoong[0] + " has fallen over, flailing its legs impotently, everyone watches uncomfortably";
                    stables[i].specialqualityupdate("Begging");
                }
                if (hoong[1] == "threelegged" && hoong[3] == "PutOutOfItsMisery")
                {
                    output = hoong[0] + " has been put out of it's misery by the referee";
                    stables[i].specialqualityupdate("rip");
                }
                if (hoong[1] == "Diego Brando" && hoong[3] == "ZA WARUDO")
                {
                    output = hoong[0] + " has stopped time for 10 seconds";
                    stables[i].specialqualityupdate("Walking through frozen time");
                    for (int c = 0; c < stables.Count; c++)
                    {
                        if(c != i)
                        {
                            string[] getstats = stables[c].getname();
                            if (getstats[3] != "RIP")
                                stables[c].specialqualityupdate("Frozen in time");
                        }
                    }
                }
                if (hoong[1] == "Diego Brando" && hoong[3] == "Time is no longer frozen")
                {
                    output = hoong[0] + "'s time stop has ended... the flow of time returns to normal...";
                    stables[i].specialqualityupdate("normalize");
                    for (int c = 0; c < stables.Count; c++)
                    {
                        if (c != i)
                        {
                            string[] getstats = stables[c].getname();
                            if (getstats[3] != "RIP")
                                stables[c].specialqualityupdate("normalize");
                        }
                    }
                }
            }
            return output;
        }

        static string tiecheck(List<hrosne> stables)
        {
            string output = "";
            int dead = 0;
            for (int i = 0; i < stables.Count; i++)
            {
                string[] hoong = stables[i].getname();
                if (hoong[3] == "RIP")
                    dead = dead + 1;
            }
            if (dead == stables.Count)
                output = "Nobody Wins";
            return output;
        }
    }
}
