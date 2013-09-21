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
using Plugin = kr.ac.kaist.swrc.jhannanum.plugin.Plugin;
namespace kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor
{
	
	/// <summary> The plug-in interface is for Plain Text Processor, which performs pre-processing of morphological analysis. </br>
	/// </br>
	/// - Phase: The First Phase </br>
	/// - Type: Supplement Plug-in </br>
	/// </br>
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public interface PlainTextProcessor:Plugin
	{
		/// <summary> It performs pre-processing of the plain text before the input text were delivered to
		/// the morphological analyzer.
		/// </summary>
		/// <param name="ps">- the plain text
		/// </param>
		/// <returns> the result plain sentence after processing
		/// </returns>
		PlainSentence doProcess(PlainSentence ps);
		
		/// <summary> It checks if there are some remaining text. If it returns true, the HanNanum work flow
		/// will not give more data to this plug-in by passing null for doProcess(). It's because 
		/// from the next phase the processing unit should be just one sentence. This mechanism allows
		/// the plug-in not to manage am input buffer.
		/// </summary>
		/// <returns> true: there are some remaining data, false: all given text were processed
		/// </returns>
		bool hasRemainingData();
		
		/// <summary> It returns the text which has been stored in the internal buffer. This method is called
		/// by HanNanum work flow only if hasRemainingData() returns true.
		/// </summary>
		/// <returns> the data in the internal buffer, if the internal buffer is empty, null is returned
		/// </returns>
		PlainSentence flush();
	}
}