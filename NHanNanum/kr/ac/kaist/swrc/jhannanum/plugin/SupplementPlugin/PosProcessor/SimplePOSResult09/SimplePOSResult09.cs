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
using TagMapper = kr.ac.kaist.swrc.jhannanum.share.TagMapper;
namespace kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PosProcessor.SimplePOSResult09
{
	
	/// <summary> This plug-in changes the detailed POS tagging results to be simple. The KAIST tag set has
	/// 69 morpheme tags but this plug-in uses only 9 tags: <br>
	/// <br>
	/// N(체언), P(용언), M(수식언), I(독립언), J(관계언), E(어미), X(접사), S(기호), F(외국어)<br>
	/// <br>
	/// It is a POS Processor plug-in which is a supplement plug-in of phase 3 in HanNanum work flow.<br>
	/// <br>
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class SimplePOSResult09 : kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PosProcessor.PosProcessor
	{
		/// <summary>the level of analysis </summary>
		private int TAG_LEVEL = 1;
		
		/// <summary>temporary list for new tags </summary>
		private List< String > tagList = null;
		
		/// <summary>temporary list for morpheme tags </summary>
		private List< String > morphemeList = null;
		
		/// <summary> Constructor.</summary>
		public SimplePOSResult09()
		{
			tagList = new List< String >();
			morphemeList = new List< String >();
		}
		
		/// <summary> It changes the POS tagging result with 69 KAIST tags to the simplified result with 9 tags.</summary>
		/// <param name="st">- the result of morphological analysis where each eojeol has more than analysis result
		/// </param>
		/// <returns> the simplified POS tagging result
		/// </returns>
		public virtual Sentence doProcess(Sentence st)
		{
			System.String prevTag = null;
			bool changed = false;
			
			Eojeol[] eojeolSet = st.Eojeols;
			
			for (int i = 0; i < eojeolSet.Length; i++)
			{
				System.String[] tags = eojeolSet[i].Tags;
				prevTag = "";
				changed = false;
				
				for (int j = 0; j < tags.Length; j++)
				{
					tags[j] = TagMapper.getKaistTagOnLevel(tags[j], TAG_LEVEL);
					
					if (tags[j].Equals(prevTag))
					{
						changed = true;
					}
					prevTag = tags[j];
				}
				
				if (changed)
				{
					tagList.Clear();
					morphemeList.Clear();
					System.String[] morphemes = eojeolSet[i].Morphemes;
					
					for (int j = 0; j < tags.Length - 1; j++)
					{
						if (tags[j].Equals(tags[j + 1]))
						{
							morphemes[j + 1] = morphemes[j] + morphemes[j + 1];
						}
						else
						{
							tagList.Add(tags[j]);
							morphemeList.Add(morphemes[j]);
						}
					}
					tagList.Add(tags[tags.Length - 1]);
					morphemeList.Add(morphemes[morphemes.Length - 1]);
					
					eojeolSet[i] = new Eojeol(morphemeList.ToArray(), tagList.ToArray());
				}
			}
			st.Eojeols = eojeolSet;
			
			return st;
		}
		public virtual void  initialize(System.String baseDir, System.String configFile)
		{
			
		}
		public virtual void  shutdown()
		{
			
		}
	}
}