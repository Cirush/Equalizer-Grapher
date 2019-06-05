using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace GUI_WPF
{
    /*
     * Custom UserControl that will change its value exponentialy. 
     */ 


    public partial class KnobControl : UserControl
    {
        // Using a DependencyProperty backing store for Angle.
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(KnobControl), new UIPropertyMetadata(0.0));
        
        // Using a DependencyProperty backing store for Value
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(KnobControl), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnValueChanged), new CoerceValueCallback(CoerceValue)));
        
        // Using a DependencyProperty backing store for Maximum
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(KnobControl), new UIPropertyMetadata(0.0));

        // Using a DependencyProperty backing store for Minimum
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(KnobControl), new UIPropertyMetadata(0.0));

        // Using a RoutedEvent wich is triggered when Value is changed
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(KnobControl));

        /*----------------------------------------
         * Constructor
         -----------------------------------------*/ 
        public KnobControl()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += new MouseButtonEventHandler(OnMouseLeftButtonDown);
            this.MouseUp += new MouseButtonEventHandler(OnMouseUp);
            this.MouseMove += new MouseEventHandler(OnMouseMove);

        }

        /*-----------------------------------------
         * Properties, getters and setters
         -----------------------------------------*/
        private const double MinValue = 0;
        private Point InitialPosition;

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }


        /*---------------------------------------------
         * Events
         ----------------------------------------------*/
        
        //Method called when user click left button
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitialPosition = Mouse.GetPosition(this);
            Mouse.Capture(this);

        }

        //Method called when user end the click
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }

        //Method called when the mouse is moving when is clicked
        //It change the angle of the KnobControl and change its value
        private void OnMouseMove(object sender, MouseEventArgs e)
        {            
            if (Mouse.Captured == this)
            {  
                double moveSpeed = 500;
                // Get the current mouse position relative to the volume control
                Point currentLocation = Mouse.GetPosition(this);

                // Calculate an angle
                this.Angle = 360 / moveSpeed * (currentLocation.Y-InitialPosition.Y) + this.Angle;
                InitialPosition = currentLocation;

                if (this.Angle > 360)
                {
                    this.Angle = 360;
                }
                if(this.Angle < 0)
                {
                    this.Angle = 0;
                }

                this.Value = AngletoValue(this.Angle);
            }

        }

        //Method that maps by exponential interpolation the angle to the value.
        private double AngletoValue(double angle)
        {
            //Exponential interpolation y0 = minfreq, y1 = maxfreq, x0 = minangle, x1 = maxangle, k = growthfactor
            double x0 = 0;
            double x1= 360;
            double y0 = 20;
            double k = 2;
            double y1 = 20000;
            double a = (Math.Pow(x0, k) * y1 - y0 * Math.Pow(x1, k)) / (y1 - y0);
            double b = (Math.Pow(x0,k)-a)/y0;

  
            double _value = 0;
            _value = Math.Floor((Math.Pow(angle,k)-a)/b);
            return _value;
        }

        /*
         * Methods that make the ValueChange event in this control
         */
        protected static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            KnobControl control = (KnobControl)obj;
            RoutedPropertyChangedEventArgs<double> e = new RoutedPropertyChangedEventArgs<double>((double)args.OldValue, (double)args.NewValue, ValueChangedEvent);
            control.OnValueChanged(e);
        }

        public event RoutedPropertyChangedEventHandler<double> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        private static object CoerceValue(DependencyObject element, object value)
        {
            double newValue = (double)value;
            KnobControl control = (KnobControl)element;
            return newValue;
        }

        protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<double> args)
        {
            RaiseEvent(args);
        }
    }


}
