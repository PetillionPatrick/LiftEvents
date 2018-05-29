using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVO.LiftSysteem.Api
{
    public class PrioriteitScheduling
    {
        public Queue<object> Prio1(Queue<object> q, string huidigeRichting, int positieLift)
        {
            Queue<object> que = new Queue<object>();
            
            if (q.Count != 0)
            {
                Beweging b = (Beweging)q.Dequeue();
                que.Enqueue(b);
                if (q.Count != 0)
                {
                    for (int i = 0; i < q.Count; i++)
                    {
                        b = ((Beweging)q.ElementAt(i));
                        int controle = 0;

                        if (que.Count != 0)
                        {
                            for (int n = 0; n < que.Count; n++)
                            {
                                Beweging be = new Beweging();
                                be = (Beweging)que.ElementAt(n);
                                if ( be.Verdiep == b.Verdiep)
                                {
                                    controle++;
                                }
                                
                            }
                            if (controle == 0)
                            {
                                que.Enqueue(b);

                            }
                        }
                    }
                }
            }
            que = Prio2(que, huidigeRichting, positieLift);
            return que;
        }


        public Queue<object> Prio2(Queue<object> q, string huidigeRichting, int positieLift)
        {
            Queue<object> que = new Queue<object>();
            Beweging b = new Beweging();
            if (huidigeRichting != null)
            {
                if (q.Count > 1)
                {
                    if (huidigeRichting == "\u23F6")
                    {
                        for (int i = positieLift + 1; i <= 2; i++)
                        {
                            foreach (Beweging be in q)
                            {
                                if (be.Verdiep == i && (be.Richting == 1 || be.Richting == 0))
                                {
                                    que.Enqueue(be);
                                }
                            }
                        }

                        for (int i = positieLift ; i <= 2; i++)
                        {
                            foreach (Beweging be in q)
                            {
                                if (be.Verdiep == i && be.Richting == 2)
                                {
                                    que.Enqueue(be);
                                }
                            }
                        }

                        for (int i = positieLift - 1; i >= -1; i--)
                        {
                            foreach (Beweging be in q)
                            {
                                if (be.Verdiep == i && (be.Richting == 2 || be.Richting == 0))
                                {
                                    que.Enqueue(be);
                                }
                            }
                        }
                    }

                    if (huidigeRichting == "\u23F7")
                    {
                        for (int i = positieLift - 1; i >= -1; i--)
                        {
                            foreach (Beweging be in q)
                            {
                                if (be.Verdiep == i && (be.Richting == 2 || be.Richting == 0))
                                {
                                    que.Enqueue(be);
                                }
                            }
                        }
                        for (int i = positieLift ; i >= -1; i--)
                        {
                            foreach (Beweging be in q)
                            {
                                if (be.Verdiep == i && be.Richting == 1)
                                {
                                    que.Enqueue(be);
                                }
                            }
                        }
                        for (int i = positieLift + 1; i <= 2; i++)
                        {
                            foreach (Beweging be in q)
                            {
                                if (be.Verdiep == i && (be.Richting == 1 || be.Richting == 0))
                                {
                                    que.Enqueue(be);
                                }
                            }
                        }
                    }
                }
                else que.Enqueue((Beweging)q.Dequeue());
            }
            return que;
        }
     }
}
