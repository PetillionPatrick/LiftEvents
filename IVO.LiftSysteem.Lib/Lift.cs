using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace IVO.LiftSysteem.Api

    // unicode caracter string s = "&#3456;"; moet "\u3456"; zijn
{
    public class Lift
    {
        private int hoogsteverdieping;  //houdt de hoogste verdieping bij
        private int laagsteverdieping;  //houdt de laagste verdieping bij


        private Thread thread;

        PrioriteitScheduling prioriteit = new PrioriteitScheduling();

        public int DoelVerdieping { get; set; }
        public Queue<object> q = new Queue<object>();
        //private Queue<object> clone = new Queue<object>();


        public event PositieLiftEventHandler Positie;
        

        public Lift(int hoogsteverdieping, int laagsteverdieping)
        {
            this.hoogsteverdieping = hoogsteverdieping;
            this.laagsteverdieping = laagsteverdieping;
        }

        /// <summary>
        /// Voorbeeldfunctie waarmee aan het Liftsysteem kenbaar wordt gemaakt dat er een
        /// nieuw doel moet toegevoegd worden aan de wachtrij. Wordt aangeroepen door de binnenbediening.
        /// </summary>
        /// <param name="doelVerdieping"></param>
        public void VerdiepingAanvragen(int doelverdieping) 
        {
            Beweging b = new Beweging();
            b.Verdiep = doelverdieping;                     
            q.Enqueue(b);
        }

        /// <summary>
        /// Voorbeeldfunctie waarmee aan het Liftsysteem kenbaar wordt gemaakt dat er bepaalde richting
        /// wordt aangevraagd op een verdieping. Wordt aangeroepen door de buitenbediening.
        /// </summary>
        /// </summary>
        /// <param name="aanvragendeVerdieping"></param>
        /// <param name="richting"></param>
        public void RichtingAanvragen(int aanvragendeVerdieping, LiftDirection richting) 
        {
            Beweging b = new Beweging();
            b.Verdiep = aanvragendeVerdieping;
            b.Richting = (int)richting;
            q.Enqueue(b);
        }

        public void BeginLift()
        {
            thread = new Thread(new ThreadStart(PositieBepalen));
            thread.IsBackground = true;
            thread.Start();
        }

        public void PositieBepalen()
        {
            var arg = new PositieLiftEventArgs();
            int positie = 1;
            DoelVerdieping = 1;
            Beweging b = new Beweging();
            b.Verdiep = 1;
            q.Enqueue(b);

            while (true)
            {
                if (q.Count != 0)
                {
                    Beweging v = (Beweging) q.Peek();
                    this.DoelVerdieping = v.Verdiep;
                    {
                        while (DoelVerdieping != positie)
                        {
                            Positie(this, arg);

                            if (DoelVerdieping > positie)
                            {
                                arg.Deuren = " De deuren sluiten";
                                arg.Positie = positie;
                                Positie(this, arg);
                                Thread.Sleep(4000);
                                arg.Richting = "\u23F6";

                                Positie(this, arg);
                                while (DoelVerdieping > positie)
                                {
                                    q = prioriteit.Prio1(q, arg.Richting, arg.Positie);
                                    DoelVerdieping = DoelverdiepHerzoeken(DoelVerdieping, q);
                                    arg.Deuren = "";
                                    Positie(this, arg);
                                    Thread.Sleep(4000);
                                    positie += 1;

                                    arg.Positie = positie;

                                    Positie(this, arg);
                                }
                            }
                            else if (DoelVerdieping < positie)
                            {
                                arg.Deuren = " De deuren sluiten";
                                arg.Positie = positie;
                                Positie(this, arg);
                                Thread.Sleep(4000);
                                arg.Richting = "\u23F7";
                               
                                Positie(this, arg);
                                while (DoelVerdieping < positie)
                                {
                                    q = prioriteit.Prio1(q, arg.Richting, arg.Positie);
                                    DoelVerdieping = DoelverdiepHerzoeken(DoelVerdieping, q);
                                    arg.Deuren = "";
                                    Positie(this, arg);
                                    Thread.Sleep(4000);
                                    positie -= 1;

                                    arg.Positie = positie;

                                    Positie(this, arg);
                                }
                            }
                        }
                         if (DoelVerdieping == positie)
                        {
                            arg.Positie = positie;
                            arg.Richting = "";
                            
                            Positie(this, arg);
                        }
                    }
                }

                if (q.Count != 0)
                {
                    Beweging v = (Beweging)q.Dequeue();
                    this.DoelVerdieping = v.Verdiep;
                }

                arg.Deuren = "Stop";
                Positie(this, arg);
                Thread.Sleep(1000);
            }
            
        }

        int DoelverdiepHerzoeken(int doelverdiep, Queue<object> q)
        {
            Beweging b = new Beweging();
            b = (Beweging)q.Peek();

            doelverdiep = b.Verdiep;
            return doelverdiep;
        }
    }
}
