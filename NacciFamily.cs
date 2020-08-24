using System;
using System.Numerics;
using static System.Math;

namespace Nacci
{
	using static System.Math;
	public class Constants
	{
		public static readonly double sr2 = Sqrt(2);
		public static readonly double sr5 = Sqrt(5);
		public static readonly double sr33 = Sqrt(33);
		public static readonly double phi = (1 + sr5) / 2;
		public static readonly double triAP = Pow(19 + 3 * sr33, 1/3);
		public static readonly double triAN = Pow(19 - 3 * sr33, 1/3);
		public static readonly double triN = 1/3 * (1 + triAN + triAP);
	}

	public static class Sequences
	{
		public static double NthRoot(double a, int n)
			=> Pow(a, 1.0 / n);

		public class Multi
		{
			public Fib fib = new Fib();
			public Pad pad = new Pad();
			public Jac jac = new Jac();
			public Pel pel = new Pel();
			public Tri tri = new Tri();
			public Tet tet = new Tet();

			public Multi() { }

			public BigInteger Next(string type)
				=> type switch
				{
						"fib" => fib.Next(),
						"pad" => pad.Next(),
						"jac" => jac.Next(),
						"pel" => pel.Next(),
						"tri" => tri.Next(),
						"tet" => tet.Next(),
						_ => 0
				};
		}

		// Root of all sequence classes to reduce redundancy
		public class Init
		{
			public int SequencePos { get; set; }
			public Init(int pos = 0) => SequencePos = pos;
		}

		public class Fib : Init
		{   
			public BigInteger Next()
				=> (BigInteger)Round(Pow(Constants.phi, SequencePos++) / Constants.sr5);
			// round( phi^x / sqrt(5) )
		}

		public class Pad : Init
		{
			private static int N_Max = 8192;
			private static long[] pdvn = new long[N_Max];

			public BigInteger Next()
			{
				int n = SequencePos++;
				switch(n)
				{
					case 0:
					case 3:
						return 1;
					case 1:
					case 2:
					case 4:
						return 0;
				}
				n -= 5;

				int pPrevPrev = 1, pPrev = 1, pCurr = 1, pNext = 1; 

				for (int i=3; i <= n; i++) 
				{ 
					pNext = pPrevPrev + pPrev;
					pPrevPrev = pPrev;
					pPrev = pCurr;
					pCurr = pNext;
				} 

				return pNext;
			}
		}

		public class Jac : Init
		{
			public BigInteger Next()
				=> (BigInteger)(Pow(2, SequencePos) - Pow(-1, SequencePos++)) / 3;
			// ( 2^n - (-1)^n ) / 3
		}

		public class Pel : Init
		{
			public BigInteger Next()
				=> (BigInteger)Round( (Pow(1 + Constants.sr2, SequencePos)
									   - Pow(1 - Constants.sr2, SequencePos++))
									 / (2 * Constants.sr2) );
			// round( ((1 + sqrt(2))^n
			//        -(1 - sqrt(2))^n)
			//         / 2)
		}

		// NYI: Fast calculations
		public class Tri : Init
		{
			/* Not working
                  public BigInteger Next()
                  {
            					// https://mathworld.wolfram.com/TribonacciNumber.html
                      double ap = Constants.triAP;
                      double an = Constants.triAN;
                      double  b = Constants.triB;

            					double numer1 = 1/3 * ap + 1/3 * an + 1/3;
            					double numer = Math.Pow(numer1, SequencePos++) * b;

            					double denom = Math.Pow(b, 2) + 4 - 2 * b;
                      return (BigInteger) Math.Round(3 * (numer / denom));
                  }*/

			public BigInteger Next(int n = -1)
			{
				if(n == -1) n = SequencePos++;

				switch(n)
				{
					case 0:
					case 1:
						return 0;
					case 2:
						return 1;
					default:
						return Next(n-1) + Next(n-2) + Next(n-3);
				}
			}
		}

		public class Tet : Init
		{
			public BigInteger Next()
			{
				int n = SequencePos++ - 2;
				if(n <= -1)
					return 0;

				int[] dp = new int[n + 5];
				dp[0] = 0;
				dp[1] = dp[2] = 1;
				dp[3] = 2;

				for (int i = 4; i <= n; i++) 
					dp[i] = dp[i - 1] + dp[i - 2] + 
					dp[i - 3] + dp[i - 4]; 

				return dp[n];
			}
		}
	}
}