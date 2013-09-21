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
using SetOfSentences = kr.ac.kaist.swrc.jhannanum.comm.SetOfSentences;
namespace kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.MorphemeProcessor.UnknownMorphProcessor
{
	
	/// <summary> This plug-in is for morphemes tagged with 'unk'. These morphemes can not be found in the morpheme dictionaries
	/// so their POS tag was temporarily mapped with 'unknown'. The most of morphemes not registered in the dictionaries
	/// can be expected to be noun with highly probability. So this plug-in maps the 'unk' tag to 'ncn' and 'nqq'.
	/// 
	/// It is a morpheme processor plug-in which is a supplement plug-in of phase 2 in HanNanum work flow.
	/// 
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class UnknownProcessor : kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.MorphemeProcessor.MorphemeProcessor
	{
		public virtual SetOfSentences doProcess(SetOfSentences sos)
		{
			List< Eojeol [] > eojeolSetArray = sos.getEojeolSetArray();
			
			LinkedList < Eojeol > eojeolArray = new LinkedList < Eojeol >();
			
			for (int i = 0; i < eojeolSetArray.Count; i++)
			{
				Eojeol[] eojeolSet = eojeolSetArray[i];
				
				eojeolArray.Clear();
				for (int j = 0; j < eojeolSet.Length; j++)
				{
					eojeolArray.AddLast(eojeolSet[j]);
				}
				
				int unkCount = 0;
				for (int j = 0; j < eojeolArray.Count; j++)
				{
					Eojeol eojeol = eojeolArray.Get_Renamed(j);
					System.String[] tags = eojeol.Tags;
					System.String[] morphemes = eojeol.Morphemes;
					
					for (int k = 0; k < tags.Length; k++)
					{
						if (tags[k].Equals("unk"))
						{
							tags[k] = "nqq";
							
							Eojeol newEojeol = new Eojeol(morphemes.Clone() as string[], tags.Clone() as string[]);
                            eojeolArray.AddLast(newEojeol);
							
							tags[k] = "ncn";
							unkCount++;
						}
					}
				}
				
				if (unkCount > 0)
				{
                    eojeolSetArray[i] = eojeolArray.ToArray(eojeolSet);
				}
			}
			
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