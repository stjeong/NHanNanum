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
using PetShop.Data;
using PlainSentence = kr.ac.kaist.swrc.jhannanum.comm.PlainSentence;
namespace kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.InformalSentenceFilter
{
	
	/// <summary> This plug-in filters informal sentences in which an eojeol is quite long and some characters were
	/// repeated many times. These informal patterns occur poor performance of morphological analysis
	/// so this plug-in should be used in HanNanum work flow which will analyze documents with informal sentences.
	/// 
	/// It is a Plain Text Processor plug-in which is a supplement plug-in of phase 1 in HanNanum work flow.
	/// 
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class InformalSentenceFilter : kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.PlainTextProcessor
	{
		/// <summary>the maximum number of repetition of a character allowed </summary>
		private const int REPEAT_CHAR_ALLOWED = 5;
		
		/// <summary> It recognizes informal sentences in which an eojeol is quite long and some characters were
		/// repeated many times. To prevent decrease of analysis performance because of those unimportant
		/// irregular pattern, it inserts some blanks in those eojeols to seperate them.
		/// </summary>
		public virtual PlainSentence doProcess(PlainSentence ps)
		{
			System.String word = null;
			System.Text.StringBuilder buf = new System.Text.StringBuilder();
            StringTokenizer st = new StringTokenizer(ps.Sentence, " \t");
			
			while (st.HasMoreTokens)
			{
				word = st.NextToken;
				
				/* repeated character */
				if (word.Length > REPEAT_CHAR_ALLOWED)
				{
					char[] wordArray = word.ToCharArray();
					int repeatCnt = 0;
					char checkChar = wordArray[0];
					
					buf.Append(checkChar);
					
					for (int i = 1; i < wordArray.Length; i++)
					{
						if (checkChar == wordArray[i])
						{
							if (repeatCnt == REPEAT_CHAR_ALLOWED - 1)
							{
								buf.Append(' ');
								buf.Append(wordArray[i]);
								repeatCnt = 0;
							}
							else
							{
								buf.Append(wordArray[i]);
								repeatCnt++;
							}
						}
						else
						{
							if (checkChar == '.')
							{
								buf.Append(' ');
							}
							buf.Append(wordArray[i]);
							checkChar = wordArray[i];
							repeatCnt = 0;
						}
					}
				}
				else
				{
					buf.Append(word);
				}
				buf.Append(' ');
			}
			ps.Sentence = buf.ToString();
			return ps;
		}
		public virtual void  initialize(System.String baseDir, System.String configFile)
		{
			
		}
		public virtual PlainSentence flush()
		{
			return null;
		}
		public virtual void  shutdown()
		{
			
		}
		public virtual bool hasRemainingData()
		{
			return false;
		}
	}
}