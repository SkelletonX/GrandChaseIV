using System;

namespace GrandChase.Utilities
{
    public sealed class IdLooper
    {
        public int Minimum { get; private set; }
        public int Maximum { get; private set; }
        public int Current { get; private set; }

        public IdLooper(int minimum = 1, int maximum = 100000)
        {
            if (maximum <= minimum)
            {
                throw new ArgumentException("Maximum must be greater than minimum.");
            }

            this.Minimum = minimum;
            this.Maximum = maximum;
            this.Current = minimum;
        }

        public int Next()
        {
            int ret = this.Current;

            if (this.Current == this.Maximum)
            {
                this.Reset();
            }
            else
            {
                this.Current++;
            }

            return ret;
        }

        public void Reset()
        {
            this.Current = this.Minimum;
        }
    }
}
