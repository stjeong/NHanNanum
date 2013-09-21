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
namespace kr.ac.kaist.swrc.jhannanum.share
{
	
	/// <summary> It changes the analysis level of the KAIST morpheme tag.
	/// 
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class TagMapper
	{
		/// <summary> Changes the analysis level of the KAIST morpheme tag.</summary>
		/// <param name="tag">- morpheme tag
		/// </param>
		/// <param name="level">- the analysis level
		/// </param>
		/// <returns> the morpheme tag on the analysis level
		/// </returns>
		public static System.String getKaistTagOnLevel(System.String tag, int level)
		{
			if (tag == null || level > 4 || level < 1)
			{
				return null;
			}
			
			int tagLen = tag.Length;
			if (tagLen > level)
			{
				return tag.Substring(0, (level) - (0)).ToUpper();
			}
			else
			{
				return tag.ToUpper();
			}
		}
	}
}