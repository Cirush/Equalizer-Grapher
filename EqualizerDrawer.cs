using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace EqUtility
{
    class EqualizerDrawer
    {
        //----------------------------------------
        // Properties
        //----------------------------------------    
        private List<Point> points;
        public Equalizer equalizer;
        private static EqualizerDrawer instance = null;
        //----------------------------------------
        // Constructors
        //----------------------------------------    
        public EqualizerDrawer(Equalizer _equalizer)
        {
            this.equalizer = _equalizer;
            this.points = new List<Point>();
        }

        //----------------------------------------
        // Singleton
        //----------------------------------------    

        public static EqualizerDrawer Instance(Equalizer _equalizer)
        {
            if (instance == null)
            {
                instance = new EqualizerDrawer(_equalizer);

            }
            return instance;
        }

        //----------------------------------------
        // Setters and getters
        //----------------------------------------    

        public void SetPoints(List<Point> _points)
        {
            this.points = _points;
        }

        public List<Point> GetPoints()
        {
            return this.points;
        }

        public void SetEqualizer(Equalizer _equalizer)
        {
            this.equalizer = _equalizer;
        }

        public Equalizer getEqualizer()
        {
            return this.equalizer;
        }

        //----------------------------------------
        // Methods
        //----------------------------------------    

        /*
         * Method that calculate the points of the transfer function of a Equalizer made of filters.
         * The transfer function is the same as Sigma Studio's one. 
         * In this case the transfer function is not normalized at f = 1hz
         * H(wj) = Sqrt((w0^2-x^2)^2+(a*w0/q)^2)*x^2/(w0^2-x^2)^2+(w0/(a*q))^2*x^2))
         */
        public void CalculatePoints()
        {
            //Variables
            double w0 = 0;
            double a = 0;
            double q = 0;
            double dbBoost = 0; 

            //We dont paint 20k points. Instead of that we interpolate exponentialy the points in order to have more
            //points in the lower frequency range than in the higher. Due to the representation is in logarithmic axis.
            int x0 = 20;
            int xlast = 20000;
            int jump = 10;
            double k = 10;
            double c = (Math.Pow(x0,k)*xlast-x0*Math.Pow(xlast,k))/(xlast-x0);
            double b = (Math.Pow(x0, k) - c) / x0;

            this.points = new List<Point>();
            if(this.equalizer == null || this.equalizer.GetNumberOfBands() < 1)
            {
                Debug.WriteLine("Cannot calculate points because there is not equalizer or filters in it");
                return;
            }

            
            //For each band, calculate the points gived by the transfer function.              
            for(int i = x0; i<xlast; i=i+jump)
            {
                double x = (Math.Pow(i,k)+c)/b;
                double y = 0;
                for(int j = 0; j < equalizer.GetNumberOfBands(); j++)
                {
                    dbBoost = this.equalizer.GetFilter(j).GetBoost();
                    a = dbBoost;
                    a = Math.Pow(10, a / 20);
                    w0 = this.equalizer.GetFilter(j).GetFrequency();
                    q = this.equalizer.GetFilter(j).GetQ();
                    double _y = ((w0 * w0 - x * x) * (w0 * w0 - x * x) + (a * w0 / q) * (a * w0 / q) * x * x) / ((w0 * w0 - x * x) * (w0 * w0 - x * x) + (w0 / (a * q) * w0 / (a * q) * x * x));
                    _y = Math.Sqrt(_y);
                    if(y == 0)
                    {
                        y = 10*Math.Log10(_y);
                    }
                    else
                    {
                        y = y+10*Math.Log10(_y);
                    }

                 }

                Point point = new Point(x, y);
                this.points.Add(point);
            }
        }
    }
}
