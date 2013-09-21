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
using SetOfSentences = kr.ac.kaist.swrc.jhannanum.comm.SetOfSentences;
using Plugin = kr.ac.kaist.swrc.jhannanum.plugin.Plugin;
namespace kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.MorphemeProcessor
{
	
	/// <summary> The plug-in interface is for Morpheme Processor, which performs post processing of morphological analysis. </br>
	/// </br>
	/// - Phase: The Second Phase </br>
	/// - Type: Supplement Plug-in </br>
	/// </br>
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public interface MorphemeProcessor:Plugin
	{
		/// <summary> It performs post processing of morphological analysis. The input and output of this method
		/// were a sequence of eojeols, and each eojeol has more than one morphological analysis result.
		/// So combinations of morphological analysis result of each eojeol represent different sentences.
		/// </summary>
		/// <param name="sos">- the result of morphologcial analysis
		/// </param>
		/// <returns> the result of post processing
		/// </returns>
		SetOfSentences doProcess(SetOfSentences sos);
	}
}