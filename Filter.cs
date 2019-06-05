using System;
using System.Diagnostics;

namespace EqUtility
{
    public class Filter
    {
        //-----------------------------------------------------
        //Properties
        //-----------------------------------------------------
        private double gain;
        private double boost;
        private double frequency;
        private String type = "peak";
        private double q = 0.707;
        //-----------------------------------------------------
        //Constructors
        //-----------------------------------------------------

        public Filter()
        {
            this.gain = 0;
            this.boost = 0;
            this.frequency = 0;
            Debug.WriteLine("Filtro Construido");
        }

        public Filter(int _gain, int _boost, int _frequency, int _q)
        {
            this.gain = _gain;
            this.boost = _boost;
            this.frequency = _frequency;
        }

        public Filter(int _gain, int _boost, int _frequency, int _q, String _type)
        {
            this.gain = _gain;
            this.boost = _boost;
            this.frequency = _frequency;
            this.type = _type;
        }

        //-----------------------------------------------------
        //Setters and getters
        //-----------------------------------------------------

        public void SetGain(double _gain)
        {
            this.gain = _gain;
        }

        public double GetGain()
        {
            return this.gain;
        }

        public void SetBoost(double _boost)
        {
            this.boost = _boost;
        }

        public double GetBoost()
        {
            return this.boost;
        }

        public double GetFrequency()
        {
            return this.frequency;
        }

        public void SetFrequency(double _frequency)
        {
            this.frequency = _frequency;
        }

        public String GetShape()
        {
            return this.type;
        }

        public void SetShape(String _type)
        {
            this.type = _type;
        }

        public double GetQ()
        {
            return this.q;
        }

        public void SetQ(double _q)
        {
            this.q = _q;
        }

    }
}
