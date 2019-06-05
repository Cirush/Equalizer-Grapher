using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Globalization;
using EqUtility;
using InteractiveDataDisplay.WPF;
using OxyPlot;
using OxyPlot.Wpf;


namespace GUI_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            logaxis.LabelFormatter = _formatter;
        }

        //Circle point will represent a point for each filter.
        public List<DataPoint> CirclePoint { get; set; }
        //Points are the points of the linearseries which represents the transfer function curve.
        public List<DataPoint> Points { get; set; }
        //We instantiate the Equalizer with 7 bands.
        Equalizer eq = Equalizer.Instance(7);

        /*
         * Method that will move the central frequency of a filter and will make the transfer function curve represent again.  
         */
        private void Knob_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //Get the sender element as a KnobControl
            KnobControl cknob = (KnobControl)sender;

            //The value of the KnobControl set the central frequency of its band
            eq.GetFilter(Knobtoint(cknob.Name)).SetFrequency(cknob.Value);

            //Make an instance of Equalizer drawer and calculate the points
            EqualizerDrawer drawer = EqualizerDrawer.Instance(eq);
            drawer.CalculatePoints();
            List<Point> listPoints = drawer.GetPoints();
            if (this.Points == null)
            {
                this.Points = new List<DataPoint>();
            }

            //If there is not curve we add new points
            //else we change the points
            if (this.Points.Count == 0)
            {
                for (int i = 0; i < listPoints.Count; i++)
                {
                    this.Points.Add(new DataPoint(listPoints[i].X, listPoints[i].Y));
                }
            }
            else
            {
                for (int i = 0; i < this.Points.Count; i++)
                {
                    this.Points[i] = new DataPoint(listPoints[i].X, listPoints[i].Y);
                }
            }

            //Points refering the filter properties
            if (this.CirclePoint == null)
            {
                this.CirclePoint = new List<DataPoint>();
            }
            if (this.CirclePoint.Count == 0)
            {
                for (int band = 0; band < eq.GetNumberOfBands(); band++)
                {
                    this.CirclePoint.Add(new DataPoint(eq.GetFilter(band).GetFrequency(), eq.GetFilter(band).GetBoost()));
                }
            }
            else
            {
                for (int band = 0; band < eq.GetNumberOfBands(); band++)
                {
                    this.CirclePoint[band] = new DataPoint(eq.GetFilter(band).GetFrequency(), eq.GetFilter(band).GetBoost());
                }

            }

            //Redrawing
            if (plotter != null)
            {
                plotter.InvalidatePlot();
            }

        }

        /**
         *  Method that will move the boost of a filter and will make the transfer function curve represent again.
         */
        private void Boost_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //Get the sender slider
            Slider slider = (Slider)sender;

            //Set the boost of the filter
            eq.GetFilter(Boosttoint(slider.Name)).SetBoost(slider.Value);

            //Instantiate the EqualizeDrawer and calculate the points of the transfer function curve.
            EqualizerDrawer drawer = EqualizerDrawer.Instance(eq);
            drawer.CalculatePoints();

            List<Point> listPoints = new List<Point>();
            listPoints = drawer.GetPoints();


            if (this.Points == null)
            {
                this.Points = new List<DataPoint>();
            }

            if (this.Points.Count == 0)
            {
                for (int i = 0; i < listPoints.Count; i++)
                {
                    this.Points.Add(new DataPoint(listPoints[i].X, listPoints[i].Y));

                }
            }
            for (int i = 0; i < this.Points.Count; i++)
            {
                this.Points[i] = new DataPoint(listPoints[i].X, listPoints[i].Y);
            }


            if (this.CirclePoint == null)
                this.CirclePoint = new List<DataPoint>();

            if (this.CirclePoint.Count == 0)
            {
                for (int band = 0; band < eq.GetNumberOfBands(); band++)
                {
                    this.CirclePoint.Add(new DataPoint(eq.GetFilter(band).GetFrequency(), eq.GetFilter(band).GetBoost()));
                }
            }
            else
            {
                for (int band = 0; band < eq.GetNumberOfBands(); band++)
                {
                    this.CirclePoint[band] = new DataPoint(eq.GetFilter(band).GetFrequency(), eq.GetFilter(band).GetBoost());
                }

            }

            //Redraw
            if (plotter != null)
            {
                plotter.InvalidatePlot();
            }
        }

        //Methods that map the knobControl to its filter
        public int Knobtoint(string name)
        {
            if (name == "knob")
            {
                return 0;
            }
            if (name == "knob1")
            {
                return 1;
            }
            if (name == "knob2")
            {
                return 2;
            }
            if (name == "knob3")
            {
                return 3;
            }
            if (name == "knob4")
            {
                return 4;
            }
            if (name == "knob5")
            {
                return 5;
            }
            if (name == "knob6")
            {
                return 6;
            }
            else return -1;
        }

        //Method that map the boost to its filter
        public int Boosttoint(string name)
        {
            if (name == "boost")
            {
                return 0;
            }
            if (name == "boost1")
            {
                return 1;
            }
            if (name == "boost2")
            {
                return 2;
            }
            if (name == "boost3")
            {
                return 3;
            }
            if (name == "boost4")
            {
                return 4;
            }
            if (name == "boost5")
            {
                return 5;
            }
            if (name == "boost6")
            {
                return 6;
            }
            else return -1;
        }

        //Method that map the q to its filter
        public int Qslidertoint(string name)
        {
            if (name == "Qslider")
            {
                return 0;
            }
            if (name == "Qslider1")
            {
                return 1;
            }
            if (name == "Qslider2")
            {
                return 2;
            }
            if (name == "Qslider3")
            {
                return 3;
            }
            if (name == "Qslider4")
            {
                return 4;
            }
            if (name == "Qslider5")
            {
                return 5;
            }
            if (name == "Qslider6")
            {
                return 6;
            }
            else return -1;
        }

        /*
         * Method that will change the Q in a filter and redraw the transfer function
         */
        private void Qslider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = (Slider)sender;


            eq.GetFilter(Qslidertoint(slider.Name)).SetQ(slider.Value);


            EqualizerDrawer drawer = EqualizerDrawer.Instance(eq);
            drawer.CalculatePoints();

            List<Point> listPoints = new List<Point>();
            listPoints = drawer.GetPoints();


            if (this.Points == null)
            {
                this.Points = new List<DataPoint>();
            }

            if (this.Points.Count == 0)
            {
                for (int i = 0; i < listPoints.Count; i++)
                {
                    this.Points.Add(new DataPoint(listPoints[i].X, listPoints[i].Y));

                }
            }
            for (int i = 0; i < this.Points.Count; i++)
            {
                this.Points[i] = new DataPoint(listPoints[i].X, listPoints[i].Y);
            }

            if (this.CirclePoint == null)
                this.CirclePoint = new List<DataPoint>();

            if (this.CirclePoint.Count == 0)
            {
                for (int band = 0; band < eq.GetNumberOfBands(); band++)
                {
                    this.CirclePoint.Add(new DataPoint(eq.GetFilter(band).GetFrequency(), eq.GetFilter(band).GetBoost()));
                }

            }
            else
            {
                for (int band = 0; band < eq.GetNumberOfBands(); band++)
                {
                    this.CirclePoint[band] = new DataPoint(eq.GetFilter(band).GetFrequency(), eq.GetFilter(band).GetBoost());
                }

            }


            if (plotter != null)
            {
                plotter.InvalidatePlot();
            }
        }

        /*
         * Formater of the X axis 
         */
        private static string _formatter(double d)
        {
            if (d == 100)
            {
                return "100";
            }
            if (d == 200)
            {
                return "200";
            }
            if (d == 500)
            {
                return "500";
            }
            if (d == 1000)
            {
                return "1k";
            }
            if (d == 2000)
            {
                return "2k";
            }
            if (d == 5000)
            {
                return "5k";
            }
            if (d == 10000)
            {
                return "10k";
            }
            if (d == 20000)
            {
                return "20k";
            }
            else
            {
                return "";
            }
        }
    }
}
    

   
