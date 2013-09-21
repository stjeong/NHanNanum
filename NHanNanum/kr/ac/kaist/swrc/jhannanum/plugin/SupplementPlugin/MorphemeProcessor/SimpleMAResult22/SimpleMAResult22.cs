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
using System.Linq;
using Eojeol = kr.ac.kaist.swrc.jhannanum.comm.Eojeol;
using SetOfSentences = kr.ac.kaist.swrc.jhannanum.comm.SetOfSentences;
using TagMapper = kr.ac.kaist.swrc.jhannanum.share.TagMapper;
namespace kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.MorphemeProcessor.SimpleMAResult22
{
	
	/// <summary> This plug-in changes the detailed morphological analysis results to be simple. The KAIST tag set has
	/// 69 morpheme tags but this plug-in uses 22 tags:<br>
	/// <br>
	/// NC(보통명사), NQ(고유명사), NB(의존명사), NP(대명사), NN(수사)<br>
	/// PV(동사), PA(형용사), PX(보조용언)<br>
	/// MM(관형사), MA(부사)<br>
	/// II(감탄사)<br>
	/// JC(격조사), JX(보조사), JP(서술격조사)<br>
	/// EP(선얼말어미), EC(연결어미), ET(전성어미), EF(종결어미)<br>
	/// XP(접두사), XS(접미사)<br>
	/// S(기호)<br>
	/// F(외국어)<br>
	/// <br>
	/// Note: This plug-in is not compatible with HmmPosTagger.<br>
	/// <br>
	/// It is a morpheme processor plug-in which is a supplement plug-in of phase 2 in HanNanum work flow.<br>
	/// <br>
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class SimpleMAResult22 : kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.MorphemeProcessor.MorphemeProcessor
	{
		/// <summary>the level of analysis </summary>
		private int TAG_LEVEL = 2;
		
		/// <summary>hash map to remove duplicates </summary>
		private Dictionary < String, Eojeol > dupFilterMap = null;
		
		/// <summary>temporary list for new tags </summary>
		private List< String > tagList = null;
		
		/// <summary>temporary list for morpheme tags </summary>
		private List< String > morphemeList = null;
		
		/// <summary> Constructor.</summary>
		public SimpleMAResult22()
		{
			dupFilterMap = new Dictionary < String, Eojeol >();
			tagList = new List< String >();
			morphemeList = new List< String >();
		}
		
		/// <summary> It changes the morphological analysis result with 69 KAIST tags to the simplified result with 22 tags.</summary>
		/// <param name="sos">- the result of morphological analysis where each eojeol has more than analysis result
		/// </param>
		/// <returns> the simplified morphological analysis result
		/// </returns>
		public virtual SetOfSentences doProcess(SetOfSentences sos)
		{
			List< Eojeol [] > eojeolSetArray = sos.getEojeolSetArray();
			List< Eojeol [] > resultSetArray = new List< Eojeol [] >();
			
			int len = eojeolSetArray.Count;
			System.String prevTag = null;
			bool changed = false;
			
			for (int pos = 0; pos < len; pos++)
			{
				Eojeol[] eojeolSet = eojeolSetArray[pos];
				dupFilterMap.Clear();
				
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
					
					System.String key = eojeolSet[i].ToString();
					if (!dupFilterMap.ContainsKey(key))
					{
						dupFilterMap[key] = eojeolSet[i];
					}
				}
				if (eojeolSet.Length != dupFilterMap.Count)
				{
					resultSetArray.Add(dupFilterMap.Values.ToArray());
				}
				else
				{
					resultSetArray.Add(eojeolSet);
				}
			}
			
			sos.setEojeolSetArray(resultSetArray);
			return sos;
		}
		
        public virtual void  initialize(System.String baseDir, System.String configFile)
		{
			
		}

		public virtual void  shutdown()
		{
			
		}
	}
}