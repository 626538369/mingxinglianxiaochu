using System;
using System.Collections.Generic;


namespace Curves
{
    class BezierCurve
    {
        private float[] FactorialLookup;

        public BezierCurve()
        {
            CreateFactorialTable();
        }

        // just check if n is appropriate, then return the result
        private float factorial(int n)
        {
            if (n < 0) { throw new Exception("n is less than 0"); }
            if (n > 32) { throw new Exception("n is greater than 32"); }

            return FactorialLookup[n]; /* returns the value n! as a SUMORealing point number */
        }

        // create lookup table for fast factorial calculation
        private void CreateFactorialTable()
        {
            // fill untill n=32. The rest is too high to represent
            float[] a = new float[33]; 
            a[0] = 1.0f;
            a[1] = 1.0f;
            a[2] = 2.0f;
            a[3] = 6.0f;
            a[4] = 24.0f;
            a[5] = 120.0f;
            a[6] = 720.0f;
            a[7] = 5040.0f;
            a[8] = 40320.0f;
            a[9] = 362880.0f;
            a[10] = 3628800.0f;
            a[11] = 39916800.0f;
            a[12] = 479001600.0f;
            a[13] = 6227020800.0f;
            a[14] = 87178291200.0f;
            a[15] = 1307674368000.0f;
            a[16] = 20922789888000.0f;
            a[17] = 355687428096000.0f;
            a[18] = 6402373705728000.0f;
            a[19] = 121645100408832000.0f;
            a[20] = 2432902008176640000.0f;
            a[21] = 51090942171709440000.0f;
            a[22] = 1124000727777607680000.0f;
            a[23] = 25852016738884976640000.0f;
            a[24] = 620448401733239439360000.0f;
            a[25] = 15511210043330985984000000.0f;
            a[26] = 403291461126605635584000000.0f;
            a[27] = 10888869450418352160768000000.0f;
            a[28] = 304888344611713860501504000000.0f;
            a[29] = 8841761993739701954543616000000.0f;
            a[30] = 265252859812191058636308480000000.0f;
            a[31] = 8222838654177922817725562880000000.0f;
            a[32] = 263130836933693530167218012160000000.0f;
            FactorialLookup = a;
        }

        private float Ni(int n, int i)
        {
            float ni;
            float a1 = factorial(n);
            float a2 = factorial(i);
            float a3 = factorial(n - i);
            ni =  a1/ (a2 * a3);
            return ni;
        }

        // Calculate Bernstein basis
        private float Bernstein(int n, int i, float t)
        {
            float basis;
            float ti; /* t^i */
            float tni; /* (1 - t)^i */

            /* Prevent problems with pow */

            if (t == 0.0f && i == 0) 
                ti = 1.0f; 
            else 
                ti = (float)Math.Pow((double)t, (double)i);

            if (n == i && t == 1.0f) 
                tni = 1.0f; 
            else 
                tni = (float)Math.Pow((1 - (double)t), (double)(n - i));

            //Bernstein basis
            basis = Ni(n, i) * ti * tni; 
            return basis;
        }

        public void Bezier2D(float[] b, int cpts, float[] p)
        {
            int npts = (b.Length) / 2;
            int icount, jcount;
            float step, t;

            // Calculate points on curve

            icount = 0;
            t = 0;
            step = (float)1.0 / (cpts - 1);

            for (int i1 = 0; i1 != cpts; i1++)
            { 
                if ((1.0f - t) < 5e-6) 
                    t = 1.0f;

                jcount = 0;
                p[icount] = 0.0f;
                p[icount + 1] = 0.0f;
                for (int i = 0; i != npts; i++)
                {
                    float basis = Bernstein(npts - 1, i, t);
                    p[icount] += basis * b[jcount];
                    p[icount + 1] += basis * b[jcount + 1];
                    jcount = jcount +2;
                }

                icount += 2;
                t += step;
            }
        }
    }
}
