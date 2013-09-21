/*  Copyright 2010, 2011 Semantic Web Research Center, KAIST

This file is part of JHanNanum.

JHanNanum is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

JHanNanum is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with JHanNanum.  If not, see <http://www.gnu.org/licenses/>   */
using System;
using System.Collections.Generic;
namespace kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.PosTagger.HmmPosTagger
{
	
	/// <summary> This class is for statistic data which is important to the Hidden Markov Model.</summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class ProbabilityDBM
	{
		/// <summary>hash table </summary>
		private Dictionary < String, double [] > table = null;
		
		/// <summary> Constructor.</summary>
		/// <param name="fileName">- the name of the file which has statistic data
		/// </param>
		/// <throws>  IOException </throws>
		public ProbabilityDBM(System.String fileName)
		{
            table = new Dictionary<String, double[]>();
			init(fileName);
		}
		
		/// <summary> Cleans the hash table.</summary>
		public virtual void  clear()
		{
			table.Clear();
		}
		
		/// <summary> Gets the probability data to which specified key mapped.</summary>
		/// <param name="key">- the key of probability data
		/// </param>
		/// <returns> the probability data to which specified key mapped
		/// </returns>
		public virtual double[] get_Renamed(System.String key)
		{
            double [] value;
            table.TryGetValue(key, out value);
            return value;
		}
		
		/// <summary> It loads the probability data from the specified file.</summary>
		/// <param name="fileName">- the path of the file which has the probability data
		/// </param>
		/// <throws>  IOException </throws>
		private void  init(System.String fileName)
		{
            System.IO.StreamReader br = new System.IO.StreamReader(fileName, System.Text.Encoding.UTF8);
			System.String line = null;
			System.String[] tokens = null;
			double[] numbers = null;
			
			while ((line = br.ReadLine()) != null)
			{
                tokens = line.Split(" ");
				
				numbers = new double[tokens.Length - 1];
				
				for (int i = 0; i < tokens.Length - 1; i++)
				{
					numbers[i] = System.Double.Parse(tokens[i + 1]);
				}
				
				if (tokens == null || tokens[0] == null || numbers == null)
				{
					System.Console.Out.WriteLine("hi");
				}
				
				table[tokens[0]] = numbers;
			}
			br.Close();
		}
	}
}