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
namespace kr.ac.kaist.swrc.jhannanum.comm
{
	
	/// <summary> This class represents the set of sentences that were results of the morphological analysis
	/// about a input sentence. Each eojeol has more than one morphological analysis result which consists of
	/// a morpheme list and their tags. So a morphologically analyzed sentence is a sequence of
	/// analysis result of each eojeol. For example, <br>
	/// 
	/// <table>
	/// <tr><td>나는</td>						<td>학교에</td>					<td>간다.</td></tr>
	/// <tr><td>-------------------------</td><td>-------------------------</td><td>-------------------------</td></tr>
	/// <tr><td>나/ncn+는/jxc</td>			<td>학교/ncn+에/jca</td>			<td>갈/pvg+ㄴ다/ef+./sf</td></tr>
	/// <tr><td>나/npp+는/jxc</td>			<td></td>						<td>가/pvg+ㄴ다/ef+./sf</td></tr>
	/// <tr><td>나/pvg+는/etm</td>			<td></td>						<td>가/px+ㄴ다/ef+./sf</td></tr>
	/// <tr><td>나/px+는/etm</td>				<td></td>						<td></td></tr>
	/// <tr><td>나/pvg+아/ecs+는/jxc</td>		<td></td>						<td></td></tr>
	/// <tr><td>나/pvg+아/ef+는/etm</td>		<td></td>						<td></td></tr>
	/// <tr><td>나/px+아/ecs+는/jxc</td>		<td></td>						<td></td></tr>
	/// <tr><td>나/px+아/ef+는/etm</td>		<td></td>						<td></td></tr>
	/// <tr><td>날/pvg+는/etm</td>			<td></td>						<td></td></tr>
	/// </table>
	/// <br>
	/// In this example, there are 9 x 1 x 3 = 27 morphologically analyzed sentences.<br>
	/// 
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class SetOfSentences:CommObject
	{
        //private void  InitBlock()
        //{
        //    base.DocumentID = documentID;
        //    base.SentenceID = sentenceID;
        //    base.EndOfDocument = endOfDocument;
			
        //    if (eojeolSetArray != null)
        //    {
        //        length = eojeolSetArray.size();
        //    }
        //    this.plainEojeolArray = plainEojeolArray;
        //    this.eojeolSetArray = eojeolSetArray;
        //    return plainEojeolArray;
        //    this.plainEojeolArray = plainEojeolArray;
        //    return eojeolSetArray;
        //    this.eojeolSetArray = eojeolSetArray;
        //}
		/// <summary> The number of eojeols.</summary>
		public int length = 0;
		
		private List< Eojeol [] > eojeolSetArray = null;
		private List< String > plainEojeolArray = null;
		
		/// <summary> Constructor.</summary>
		/// <param name="documentID">- ID of the document which this sentence is belong to
		/// </param>
		/// <param name="sentenceID">- ID of the sentence
		/// </param>
		/// <param name="endOfDocument">- If this flag is true, the sentence is the last one of the document.
		/// </param>
		public SetOfSentences(int documentID, int sentenceID, bool endOfDocument)
		{
		//	InitBlock();
			base.DocumentID = documentID;
			base.SentenceID = sentenceID;
			base.EndOfDocument = endOfDocument;
			
			eojeolSetArray = new List< Eojeol [] >();
			plainEojeolArray = new List< String >();
		}
		
		/// <summary> Constructor.</summary>
		/// <param name="documentID">- ID of the document which this sentence is belong to
		/// </param>
		/// <param name="sentenceID">- ID of the sentence
		/// </param>
		/// <param name="endOfDocument">- If this flag is true, the sentence is the last one of the document.
		/// </param>
		/// <param name="plainEojeolArray">- the array of the plain eojeols
		/// </param>
		/// <param name="eojeolSetArray">- the array of the eojeol lists
		/// </param>
		public SetOfSentences(int documentID, int sentenceID, bool endOfDocument, List< String > plainEojeolArray, List< Eojeol [] > eojeolSetArray)
        {
    			base.DocumentID = documentID;
			base.SentenceID = sentenceID;
			base.EndOfDocument = endOfDocument;

            		if (eojeolSetArray != null) {
			length = eojeolSetArray.Count;
		}
			
			this.plainEojeolArray = plainEojeolArray;
			this.eojeolSetArray = eojeolSetArray;
        }
		
		/// <summary> Returns the array of the plain eojeol.</summary>
		/// <returns> the array of the plain eojeol
		/// </returns>
		public List< String > getPlainEojeolArray()
        {
            return plainEojeolArray;
        }
		
		/// <summary> Sets the array of the plain eojeols.</summary>
		/// <param name="plainEojeolArray">
		/// </param>
		public void setPlainEojeolArray(List< String > plainEojeolArray)
        {
            this.plainEojeolArray = plainEojeolArray;
        }
		
		/// <summary> Adds an plain eojeol to the end of the sentence.</summary>
		/// <param name="eojeol">- plain eojeol
		/// </param>
		/// <returns> true: when it is added correctly, otherwise false
		/// </returns>
		public virtual bool addPlainEojeol(System.String eojeol)
		{
            plainEojeolArray.Add(eojeol);
            return true;
		}
		
		/// <summary> Adds eojeols that are morphologically analyzed about one eojeol
		/// to the end of the sentence.
		/// </summary>
		/// <param name="eojeols">- morphologically analyzed eojeol list about one eojeol
		/// </param>
		/// <returns> true: when it is added correctly, otherwise false
		/// </returns>
		public virtual bool addEojeolSet(Eojeol[] eojeols)
		{
            eojeolSetArray.Add(eojeols);
            return true;
		}
		
		/// <summary> Returns the array of the morphologically analyzed eojeol list.</summary>
		/// <returns> the array of the morphologically analyzed eojeol list
		/// </returns>
		public List< Eojeol [] > getEojeolSetArray()
        {
            return eojeolSetArray;
        }
		
		/// <summary> Sets the array of the morphologically analyzed eojeol list. </summary>
		/// <param name="eojeolSetArray">- the array of the morphologically analyzed eojeol list
		/// </param>
		public void setEojeolSetArray(List< Eojeol [] > eojeolSetArray)
        {
            this.eojeolSetArray = eojeolSetArray;
        }
		
		/// <summary> Returns the string representation of the morphologically analyzed sentences.
		/// For example,
		/// 
		/// 나는
		/// 나/ncn+는/jxc
		/// 나/npp+는/jxc
		/// 나/pvg+는/etm
		/// 나/px+는/etm
		/// 나/pvg+아/ecs+는/jxc
		/// 나/pvg+아/ef+는/etm
		/// 나/px+아/ecs+는/jxc
		/// 나/px+아/ef+는/etm
		/// 날/pvg+는/etm
		/// 
		/// 학교에
		/// 학교/ncn+에/jca
		/// 
		/// 간다.
		/// 갈/pvg+ㄴ다/ef+./sf
		/// 가/pvg+ㄴ다/ef+./sf
		/// 가/px+ㄴ다/ef+./sf
		/// 
		/// </summary>
		public override System.String ToString()
		{
			System.String str = "";
			for (int i = 0; i < length; i++)
			{
				str += (plainEojeolArray[i] + "\n");
                Eojeol[] eojeolArray = eojeolSetArray[i];
				for (int j = 0; j < eojeolArray.Length; j++)
				{
					str += ("\t" + eojeolArray[j] + "\n");
				}
				str += "\n";
			}
			return str;
		}
	}
}