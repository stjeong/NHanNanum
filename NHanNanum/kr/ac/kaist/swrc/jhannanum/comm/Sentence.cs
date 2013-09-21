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
	
	/// <summary> This class represents a sentence which is the sequence of eojeols that
	/// are morphologically analyzed. Each eojeol has a plain eojeol, a morpheme list,
	/// and a tag list for a sequence of morphemes.
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// 
	/// </author>
	public class Sentence:CommObject
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Getter of the array of the eojeols that consist of morphemes and their tags.</summary>
		/// <returns> the array of the eojeols
		/// </returns>
		/// <summary> Set the eojeols list with the specified array.</summary>
		/// <param name="eojeols">- new eojeol array for this sentence
		/// </param>
		virtual public Eojeol[] Eojeols
		{
			get
			{
				return eojeols;
			}
			
			set
			{
				this.eojeols = value;
				this.length = value.Length;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Returns the array of the plain eojeols.</summary>
		/// <returns> the plain eojeol array
		/// </returns>
		/// <summary> It sets the plain eojeol list with the specified array.</summary>
		/// <param name="plainEojeols">- the array of the plain eojeols
		/// </param>
		virtual public System.String[] PlainEojeols
		{
			get
			{
				return plainEojeols;
			}
			
			set
			{
				this.plainEojeols = value;
			}
			
		}
		/// <summary> The number of eojeols.</summary>
		public int length = 0;
		
		/// <summary> The array of the plain eojeols.</summary>
		private System.String[] plainEojeols = null;
		
		/// <summary> The array of the eojeols that are morphologically analyzed.</summary>
		private Eojeol[] eojeols = null;
		
		/// <summary> Constructor.</summary>
		/// <param name="documentID">- ID of the document which this sentence belong to
		/// </param>
		/// <param name="sentenceID">- ID of the sentence
		/// </param>
		/// <param name="endOfDocument">- If this flag is true, this sentence is the last one of the document.
		/// </param>
		public Sentence(int documentID, int sentenceID, bool endOfDocument)
		{
			base.DocumentID = documentID;
			base.SentenceID = sentenceID;
			base.EndOfDocument = endOfDocument;
		}
		
		/// <summary> Constructor.</summary>
		/// <param name="documentID">- ID of the document which this sentence belong to
		/// </param>
		/// <param name="sentenceID">- ID of the sentence
		/// </param>
		/// <param name="endOfDocument">- If this flag is true, this sentence is the last one of the document.
		/// </param>
		public Sentence(int documentID, int sentenceID, bool endOfDocument, System.String[] plainEojeols, Eojeol[] eojeols)
		{
			base.DocumentID = documentID;
			base.SentenceID = sentenceID;
			base.EndOfDocument = endOfDocument;
			
			this.eojeols = eojeols;
			this.plainEojeols = plainEojeols;
			
			if (eojeols != null && plainEojeols != null)
			{
				if (plainEojeols.Length <= eojeols.Length)
				{
					length = eojeols.Length;
				}
				else
				{
					length = plainEojeols.Length;
				}
			}
			else
			{
				length = 0;
			}
		}
		
		/// <summary> Returns the eojeol object at the specified index.</summary>
		/// <param name="index">- the index of the eojeol
		/// </param>
		/// <returns> eojeol at the specified index
		/// </returns>
		public virtual Eojeol getEojeol(int index)
		{
			return eojeols[index];
		}
		
		/// <summary> Set an eojeol at the specified index.</summary>
		/// <param name="index">- the index of the eojeol to set up
		/// </param>
		/// <param name="eojeol">- the new eojeol
		/// </param>
		public virtual void  setEojeol(int index, Eojeol eojeol)
		{
			eojeols[index] = eojeol;
		}
		
		/// <summary> Set an eojeol at the specified index with morphemes and their tags.</summary>
		/// <param name="index">- the index of the eojeol to set up
		/// </param>
		/// <param name="morphemes">- the new morpheme list
		/// </param>
		/// <param name="tags">- the new tag list
		/// </param>
		public virtual void  setEojeol(int index, System.String[] morphemes, System.String[] tags)
		{
			Eojeol eojeol = new Eojeol(morphemes, tags);
			eojeols[index] = eojeol;
		}
		
		/// <summary> Returns the string representation of this sentence.
		/// For example,
		/// 나는
		/// 나/npp+는/jxc
		/// 
		/// 학교에서
		/// 학교/ncn+에서/jca
		/// 
		/// 공부를
		/// 공부/ncpa+를/jco
		/// 
		/// 하고
		/// 하/pvg+고/ecc
		/// 
		/// 있다.
		/// 있/paa+다/ef+./sf
		/// 
		/// </summary>
		public override System.String ToString()
		{
			System.String str = "";
			for (int i = 0; i < length; i++)
			{
				str += (plainEojeols[i] + "\n");
				str += ("\t" + eojeols[i].ToString() + "\n\n");
			}
			return str;
		}
	}
}