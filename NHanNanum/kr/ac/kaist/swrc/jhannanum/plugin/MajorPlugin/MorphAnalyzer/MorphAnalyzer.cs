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
using PlainSentence = kr.ac.kaist.swrc.jhannanum.comm.PlainSentence;
using SetOfSentences = kr.ac.kaist.swrc.jhannanum.comm.SetOfSentences;
using Plugin = kr.ac.kaist.swrc.jhannanum.plugin.Plugin;
namespace kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer
{
	
	/// <summary> The plug-in interface is for morphological analysis
	/// 
	/// - Phase: The Second Phase
	/// - Type: Major Plug-in
	/// 
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public interface MorphAnalyzer:Plugin
	{
		/// <summary> It performs morphological analysis on the specified plain sentence, and returns the all analysis result where
		/// each plain eojeol has more than one morphologically analyzed eojeol.
		/// </summary>
		/// <param name="ps">- the plain sentence to be morphologically analyzed
		/// </param>
		/// <returns> - the set of eojeols where each eojeol has at least one morphological analysis result
		/// </returns>
		SetOfSentences morphAnalyze(PlainSentence ps);
	}
}