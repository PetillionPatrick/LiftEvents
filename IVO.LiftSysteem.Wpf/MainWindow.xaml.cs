using IVO.LiftSysteem.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IVO.LiftSysteem.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Lift lift;

        string i;

        public MainWindow()
        {
            InitializeComponent();

            //lift initializeren met max en min verdieping
            lift = new Lift(2, -1);

           
            lift.Positie += new PositieLiftEventHandler(Lift_Positie);
            
        }
       
        private void btnBuitenBediening_Click(object sender, RoutedEventArgs e)
        {
            string[] parameters = ((Button)e.OriginalSource).CommandParameter.ToString().Split('#');
            int verdieping = int.Parse(parameters[0]);
            LiftDirection direction = parameters[1] == "UP" ? LiftDirection.Up : LiftDirection.Down;

            //voorbeeld aanroep:
            lift.RichtingAanvragen(verdieping, direction);
        }

        private void btnBinnenBediening_Click(object sender, RoutedEventArgs e)
        {
            int gevraagdeverdieping = int.Parse(((Button)e.OriginalSource).Content.ToString());
           
            //voorbeeld aanroep:
            this.lift.VerdiepingAanvragen(gevraagdeverdieping);
        }

        private void Lift_Positie(object sender, PositieLiftEventArgs args)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.InvokeAsync(() => { Lift_Positie(sender, args); });
            }
            else
            {                                    
                    lblStatus.Content = args.Positie + args.Richting + " " + args.Deuren; 
                    tbVerdiep.Text = Convert.ToString(args.Positie) + args.Richting;
                if (lift.q.Count != 0)
                {
                    i = "";
                    foreach (Beweging b in lift.q)
                    {
                        string richting = "";

                        if (b.Richting == 1)
                        {
                            richting = "\u23F6";
                        }
                        else if (b.Richting == 2)
                        {
                            richting = "\u23F7";
                        }
                        i += b.Verdiep + " " + richting + " ; ";
                    }
                    lblWachtrij.Content = i;
                }
                else lblWachtrij.Content = "";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int verdiep = Convert.ToInt32(tbVerdiep.Text);
            this.lift.BeginLift();
        }
    }
}
