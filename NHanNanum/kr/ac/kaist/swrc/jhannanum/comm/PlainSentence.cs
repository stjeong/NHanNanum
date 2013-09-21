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
namespace kr.ac.kaist.swrc.jhannanum.comm
{
	
	/// <summary> This class contains a plain sentence which is not analyzed yet.
	/// It is used by the HanNanum work flow and its plug-ins to communicate
	/// each other.
	/// 
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class PlainSentence:CommObject
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Getter of the plain sentence.</summary>
		/// <returns> the plain sentence
		/// </returns>
		/// <summary> Setter of the plain sentence.</summary>
		/// <param name="sentence">- the plain sentence
		/// </param>
		virtual public System.String Sentence
		{
			get
			{
				return sentence;
			}
			
			set
			{
				this.sentence = value;
			}
			
		}
		/// <summary> The plain sentence.</summary>
		private System.String sentence = null;
		
		/// <summary> Constructor.</summary>
		/// <param name="documentID">- ID of the document which this sentence belong to
		/// </param>
		/// <param name="sentenceID">- ID of this sentence
		/// </param>
		/// <param name="endOfDocument">- If this flag is true, the sentence is the last one of the document.
		/// </param>
		public PlainSentence(int documentID, int sentenceID, bool endOfDocument)
		{
			base.DocumentID = documentID;
			base.SentenceID = sentenceID;
			base.EndOfDocument = endOfDocument;
		}
		
		/// <summary> Constructor.</summary>
		/// <param name="documentID">- ID of the document which this sentence belong to
		/// </param>
		/// <param name="sentenceID">- ID of this sentence
		/// </param>
		/// <param name="endOfDocument">- If this flag is true, the sentence is the last one of the document.
		/// </param>
		/// <param name="sentence">- the plain sentence
		/// </param>
		public PlainSentence(int documentID, int sentenceID, bool endOfDocument, System.String sentence)
		{
			base.DocumentID = documentID;
			base.SentenceID = sentenceID;
			base.EndOfDocument = endOfDocument;
			this.sentence = sentence;
		}
		
		/// <summary> It returns the plain string.</summary>
		/// <returns> the plain string or null when it is not set set up
		/// </returns>
		public override System.String ToString()
		{
			if (sentence == null)
			{
				return "";
			}
			return sentence;
		}
	}
}