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
namespace kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.ChartMorphAnalyzer
{
	
	/// <summary> Number dictionary for recognizing number expressions using automata.
	/// 
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class NumberDic
	{
		/// <summary>number automata </summary>
		private sbyte[][] num_automata = new sbyte[][]{new sbyte[]{0, 0, 0, 0, 0, 0, 0}, new sbyte[]{0, 9, 9, 0, 0, 2, 0}, new sbyte[]{1, 0, 0, 11, 5, 3, 0}, new sbyte[]{1, 0, 0, 11, 5, 4, 0}, new sbyte[]{1, 0, 0, 11, 5, 10, 0}, new sbyte[]{0, 0, 0, 0, 0, 6, 0}, new sbyte[]{0, 0, 0, 0, 0, 7, 0}, new sbyte[]{0, 0, 0, 0, 0, 8, 0}, new sbyte[]{1, 0, 0, 0, 5, 0, 0}, new sbyte[]{0, 0, 0, 0, 0, 10, 0}, new sbyte[]{1, 0, 0, 11, 0, 10, 0}, new sbyte[]{1, 0, 0, 0, 0, 12, 0}, new sbyte[]{1, 0, 0, 0, 0, 12, 0}};
		
		/// <summary> Returns whether the input was recognized as a number.</summary>
		/// <param name="idx">- current state to check
		/// </param>
		/// <returns> true: the input sequence was recognized as a number, false: not a number 
		/// </returns>
		public virtual bool isNum(int idx)
		{
			if (num_automata[idx][0] == 1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		/// <summary> It searches the number dictionary with the specified character and the current state of the automata.</summary>
		/// <param name="c">- the next character for searching
		/// </param>
		/// <param name="nidx">- the current state of the automata
		/// </param>
		/// <returns> next state
		/// </returns>
		public virtual int node_look(int c, int nidx)
		{
			int inp;
			switch (c)
			{
				
				case '+':  inp = 1; break;
				
				case '-':  inp = 2; break;
				
				case '.':  inp = 3; break;
				
				case ',':  inp = 4; break;
				
				default: 
					if (char.IsDigit((char)c))
						inp = 5;
					else
						inp = 6;
					break;
				
			}
			return num_automata[nidx][inp];
		}
	}
}