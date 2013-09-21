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
	
	/// <summary> CommObject is used by a work flow and plug-ins that are expected to communicate
	/// with each other. This object contains the meta data of input documents and
	/// sentences. The input and output data types of HanNanum plug-ins such as PlainSentence,
	/// Sentence, and SetOfSentences inherit this class.
	/// 
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class CommObject
	{
		/// <summary> It returns true when it is the last element of the document.
		/// 
		/// </summary>
		/// <returns> true - when it is the end of document, otherwise false
		/// </returns>
		/// <summary> Set the end of document flag of this object.</summary>
		/// <param name="endOfDocument">- true: when it is the end of document, otherwise false
		/// </param>
		virtual public bool EndOfDocument
		{
			get
			{
				return endOfDocument;
			}
			
			set
			{
				this.endOfDocument = value;
			}
			
		}
		/// <summary> It returns the ID of the document which this object is belong to.</summary>
		/// <returns> documentID
		/// </returns>
		/// <summary> Set the document ID for this object.</summary>
		/// <param name="documentID">
		/// </param>
		virtual public int DocumentID
		{
			get
			{
				return documentID;
			}
			
			set
			{
				this.documentID = value;
			}
			
		}
		/// <summary> Get the sentence ID for this object.</summary>
		/// <returns> sentenceID
		/// </returns>
		/// <summary> Set the sentence ID for this object.</summary>
		/// <param name="sentenceID">
		/// </param>
		virtual public int SentenceID
		{
			get
			{
				return sentenceID;
			}
			
			set
			{
				this.sentenceID = value;
			}
			
		}
		/// <summary> ID of the document which this object belong to.</summary>
		private int documentID = 0;
		
		/// <summary> ID of the sentence which this object belong to.</summary>
		private int sentenceID = 0;
		
		/// <summary> Flag for notifying the end of document.</summary>
		private bool endOfDocument = false;
	}
}