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
using Eojeol = kr.ac.kaist.swrc.jhannanum.comm.Eojeol;
using Sentence = kr.ac.kaist.swrc.jhannanum.comm.Sentence;
namespace kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PosProcessor.NounExtractor
{
	
	/// <summary> This plug-in extracts the morphemes recognized as a noun after Part Of Speech tagging was done.
	/// 
	/// It is a POS Processor plug-in which is a supplement plug-in of phase 3 in HanNanum work flow.
	/// 
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class NounExtractor : kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PosProcessor.PosProcessor
	{
		/// <summary>the buffer for noun morphemes </summary>
		private LinkedList < String > nounMorphemes = null;
		
		/// <summary>the buffer for tags of the morphemes </summary>
		private LinkedList < String > nounTags = null;

		public virtual void  initialize(System.String baseDir, System.String configFile)
		{
			nounMorphemes = new LinkedList < String >();
			nounTags = new LinkedList < String >();
		}

		public virtual void  shutdown()
		{
			
		}
		
		/// <summary> It extracts the morphemes which were recognized as noun after POS tagging.</summary>
		/// <param name="st">- the POS tagged sentence
		/// </param>
		/// <returns> the sentence in which only nouns were remained
		/// </returns>
		public virtual Sentence doProcess(Sentence st)
		{
			Eojeol[] eojeols = st.Eojeols;
			
			for (int i = 0; i < eojeols.Length; i++)
			{
				System.String[] morphemes = eojeols[i].Morphemes;
				System.String[] tags = eojeols[i].Tags;
				nounMorphemes.Clear();
                nounTags.Clear();
				
				for (int j = 0; j < tags.Length; j++)
				{
					char c = tags[j][0];
					if (c == 'n')
					{
                        nounMorphemes.AddLast(morphemes[j]);
                        nounTags.AddLast(tags[j]);
					}
					else if (c == 'f')
					{
                        nounMorphemes.AddLast(morphemes[j]);
                        nounTags.AddLast("ncn");
					}
				}

                eojeols[i].Morphemes = nounMorphemes.ToArray();
                eojeols[i].Tags = nounTags.ToArray();
			}
			
			st.Eojeols = eojeols;
			
			return st;
		}
	}
}