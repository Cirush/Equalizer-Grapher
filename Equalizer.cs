using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace EqUtility
{
    class Equalizer
    {
        //----------------------------------------
        // Properties
        //----------------------------------------
        private int numberOfBands;
        private Filter[] filters;
        private static Equalizer instance = null;


        //----------------------------------------
        // Constructors
        //----------------------------------------        

        private Equalizer(int _nBands)
        {
            
            this.numberOfBands = _nBands;
            filters = new Filter[_nBands];
            for(int i = 0; i<_nBands; i++)
            {
                filters[i] = new Filter();
            }
            //Debug.WriteLine("Equalizador construido");

        }

        private Equalizer()
        {

            this.numberOfBands = 1;
            filters = new Filter[1];
            for (int i = 0; i < 1; i++)
            {
                filters[i] = new Filter();
            }
            //Debug.WriteLine("Equalizador construido");

        }

        //----------------------------------------
        // Singleton
        //----------------------------------------             

        public static Equalizer Instance(int _nBands)
        {
            if(instance == null)
            {
                instance = new Equalizer(_nBands);

            } 
            return instance;
        }

        public static Equalizer Instance()
        {
            if (instance == null)
            {
                instance = new Equalizer();

            }
            return instance;
        }

        //----------------------------------------
        // Setters and getters
        //----------------------------------------

        public int GetNumberOfBands()
        {
            return this.numberOfBands;
        }

        public void SetNumberOfBands(int _nBands)
        {
            if(this.numberOfBands != this.GetNumberOfBands())
            {
                instance = new Equalizer(_nBands);
            }
        }

        public Filter GetFilter(int n)
        {
            if( n <= this.numberOfBands && n >= 0)
            {
                return filters[n];
            }
            else
            {
                Debug.WriteLine("The index is out of range");
                return null;
            }
        }

        public void SetFilter(int n, Filter filter)
        {
            if(n <= this.numberOfBands && numberOfBands > 0)
            {
                this.filters[n] = filter;
            }
            else
            {
                Debug.WriteLine("The index is out of range");
            }
        }

        public void AddFilter()
        {
            Filter newFilter = new Filter();

            filters[filters.Length] = newFilter;
            this.numberOfBands++;
        }

        public void AddFilter(Filter filter)
        {

            filters[filters.Length] = filter;
            this.numberOfBands++;
        }

        public void RemoveFilter()
        {
            Filter[] _filters = new Filter[this.filters.Length - 1];
            
            for(int i = 0; i < _filters.Length; i++)
            {
                _filters[i] = this.filters[i];
            }
            this.numberOfBands = _filters.Length;
            this.filters = _filters;
        }

    }
}
