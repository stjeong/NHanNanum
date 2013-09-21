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
namespace kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.PosTagger.HmmPosTagger
{
	
	/// <summary> This class is to generate the eojeol tag which represents the features of morphemes in an eojeol.</summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class PhraseTag
	{
		/// <summary> Generates the eojeol tag regarding the sequence of morpheme tags in an eojeol</summary>
		/// <param name="tags">- the sequence of morpheme tags in an eojeol
		/// </param>
		/// <returns> the eojeol tag
		/// </returns>
		public static System.String getPhraseTag(System.String[] tags)
		{
			char[] res = new char[]{'.', '.'};
			int end = tags.Length - 1;
			
			if (tags.Length < 4)
			{
				System.String[] tmp = new System.String[]{"", "", "", ""};
				
				/* 초기화 */
				for (int i = 0; i < tags.Length; i++)
				{
					tmp[i] = tags[i];
				}
				tags = tmp;
			}
			
			if (tags.Length <= 0 || tags[0].Length == 0)
			{
				return new String(res);
			}
			
			// checks the tags in order
			switch (tags[0][0])
			{
				
				case 'm': 
					if (tags[0].StartsWith("ma"))
					{
						if (tags[1].StartsWith("p"))
						{
							res[0] = 'P';
						}
						else if (tags[1].StartsWith("x"))
						{
							res[0] = 'P';
						}
						else if (tags[1].StartsWith("jcp"))
						{
							res[0] = 'P';
						}
						else
						{
							res[0] = 'A';
						}
					}
                    else if (tags[0].Matches("m^a.*"))
					{
						if (tags[end].StartsWith("j"))
						{
							res[0] = 'N';
						}
						else if (tags[1].StartsWith("n"))
						{
							res[0] = 'N';
						}
						else if (tags[1].StartsWith("p"))
						{
							res[0] = 'P';
						}
						else
						{
							res[0] = 'M';
						}
					}
					break;
				
				
				case 'e': 
					if (tags[0].StartsWith("ecc") || tags[0].StartsWith("ecs"))
					{
						res[0] = 'C';
					}
					break;
				
				
				case 'f': 
					res[0] = 'N';
					break;
				
				
				case 'i': 
					if (tags[1].StartsWith("j"))
					{
						res[0] = 'N';
					}
					else
					{
						res[0] = 'I';
					}
					break;
				
				
				case 'n':
                    if (tags[1].Matches("x.(v|m).*"))
					{
                        if (tags[2].Matches("..n.*") || tags[3].Matches("..n.*"))
						{
							res[0] = 'N';
						}
						else
						{
							res[0] = 'P';
						}
					}
                    else if (tags[1].Matches("x.n.*"))
					{
						res[0] = 'N';
					}
					else if (tags[1].StartsWith("p"))
					{
                        if (tags[2].Matches("..n.*") || tags[3].Matches("..n.*"))
						{
							res[0] = 'N';
						}
						else
						{
							res[0] = 'P';
						}
					}
					else
					{
						res[0] = 'N';
					}
					break;
				
				
				case 'p': 
					if (tags[1].StartsWith("xsa"))
					{
						res[0] = 'A';
					}
					else if (tags[1].StartsWith("etn") || tags[2].StartsWith("n"))
					{
						res[0] = 'N';
					}
					else
					{
						res[0] = 'P';
					}
					break;
				
				
				case 's': 
					if (tags[1].StartsWith("su") || tags[2].StartsWith("j"))
					{
						res[0] = 'N';
					}
					else if (tags[2].StartsWith("n") || tags[end].StartsWith("j"))
					{
						res[0] = 'N';
					}
					else
					{
						res[0] = 'S';
					}
					
					if (tags[0].StartsWith("sf") || tags[1].StartsWith("s"))
					{
						res[1] = 'F';
					}
					break;
				
				
				case 'x': 
					if (tags[0].StartsWith("xsn") || tags[0].StartsWith("xp"))
					{
						res[0] = 'N';
					}
					break;
				}
			
			// checks the last tag
			System.String lastTag = tags[end];
			switch (lastTag[0])
			{
				
				case 'e': 
					if (lastTag.StartsWith("ecc") || lastTag.StartsWith("ecs") || lastTag.StartsWith("ecx"))
					{
						res[1] = 'C';
					}
					else if (lastTag.StartsWith("ef"))
					{
						res[1] = 'F';
					}
					else if (lastTag.StartsWith("etm"))
					{
						res[1] = 'M';
					}
					else if (lastTag.StartsWith("etn"))
					{
						res[1] = 'N';
					}
					break;
				
				
				case 'j': 
					if (lastTag.StartsWith("jcv"))
					{
						res[0] = 'I';
					}
					else if (lastTag.StartsWith("jx"))
					{
						if (res[0] == 'A')
						{
							res[1] = 'J';
						}
						else
						{
							res[1] = 'X';
						}
					}
					else if (lastTag.StartsWith("jcj"))
					{
						if (res[0] == 'A')
						{
							res[1] = 'J';
						}
						else
						{
							res[1] = 'Y';
						}
					}
					else if (lastTag.StartsWith("jca"))
					{
						res[1] = 'A';
					}
					else if (lastTag.StartsWith("jcm"))
					{
						if (res[0] == 'A')
						{
							res[1] = 'J';
						}
						else
						{
							res[1] = 'M';
						}
					}
					else if (lastTag.StartsWith("jc"))
					{
						res[1] = 'J';
					}
					break;
				
				
				case 'm':
                    if (lastTag.Matches("m^a.*"))
					{
						res[1] = 'M';
					}
					else if (lastTag.StartsWith("mag"))
					{
						res[1] = 'A';
					}
					break;
				
				
				case 'n': 
					if (lastTag.StartsWith("n"))
					{
						res[0] = 'N';
					}
					break;
				
				
				case 'x': 
					if (lastTag.StartsWith("xsa"))
					{
						res[1] = 'A';
					}
					break;
				}
			
			// post processing
			if (res[0] == res[1])
			{
				res[1] = '.';
			}
			else if (res[0] == '.')
			{
				res[0] = res[1];
				res[1] = '.';
			}
			
			if (res[0] == 'A')
			{
				if (res[1] == 'M')
				{
					res[0] = 'N';
				}
			}
			else if (res[0] == 'M')
			{
				if (res[1] == 'A')
				{
					res[0] = 'A';
				}
				else if (res[1] == 'F')
				{
					res[0] = 'N';
				}
				else if (res[1] == 'C')
				{
					res[0] = 'N';
				}
			}
			else if (res[0] == 'I')
			{
				if (res[1] == 'M' || res[1] == 'J')
				{
					res[0] = 'N';
				}
				else if (res[1] == 'C')
				{
					res[0] = 'P';
				}
				else if (res[1] == 'F')
				{
					res[0] = 'N';
				}
			}
			
			if (res[0] == res[1])
			{
				res[1] = '.';
			}
			
			return new String(res);
		}
	}
}