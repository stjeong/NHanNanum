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
	
	/// <summary> This class represents an Eojeol for internal use. An eojeol consists
	/// of more than one umjeol, and each eojeol is separated with spaces.
	/// Korean is a agglutinative language so lexemes in an eojeol may be inflected. 
	/// 
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class Eojeol
	{
		/// <summary> It returns the morpheme list in the eojeol.</summary>
		/// <returns> morpheme list for this eojeol
		/// </returns>
		/// <summary> Set the morpheme list with a morpheme array.</summary>
		/// <param name="morphemes">- array to set the morphemes
		/// </param>
		virtual public System.String[] Morphemes
		{
			get
			{
				return morphemes;
			}
			
			set
			{
				this.morphemes = value;
				if (tags != null && tags.Length < value.Length)
				{
					length = tags.Length;
				}
				else
				{
					length = value.Length;
				}
			}
			
		}

		/// <summary> It returns the tag list for the morphemes in the eojeol.</summary>
		/// <returns> tags list for morphemes
		/// </returns>
		/// <summary> It sets the tag list for the morphemes of the eojeol.</summary>
		/// <param name="tags">- new tags list for the morpheme list
		/// </param>
		virtual public System.String[] Tags
		{
			get
			{
				return tags;
			}
			
			set
			{
				this.tags = value;
				if (morphemes != null && morphemes.Length < value.Length)
				{
					length = morphemes.Length;
				}
				else
				{
					length = value.Length;
				}
			}
			
		}
		/// <summary> The number of morphemes in this eojeol.</summary>
		public int length = 0;
		
		/// <summary> Morphemes in the eojeol.</summary>
		private System.String[] morphemes = null;
		
		/// <summary> Morpheme tags of each morpheme.</summary>
		private System.String[] tags = null;
		
		/// <summary> Constructor.</summary>
		public Eojeol()
		{
		}
		
		/// <summary> Constructor.</summary>
		/// <param name="morphemes">- array of morphemes
		/// </param>
		/// <param name="tags">- tag array for each morpheme
		/// </param>
		public Eojeol(System.String[] morphemes, System.String[] tags)
		{
			this.morphemes = morphemes;
			this.tags = tags;
			if (morphemes.Length < tags.Length)
			{
				length = morphemes.Length;
			}
			else
			{
				length = tags.Length;
			}
		}
		
		/// <summary> It returns the morpheme on the specific index.</summary>
		/// <param name="index">- index of morpheme
		/// </param>
		/// <returns> the morpheme on the index
		/// </returns>
		public virtual System.String getMorpheme(int index)
		{
			return morphemes[index];
		}
		
		/// <summary> Set a morpheme on the specific position</summary>
		/// <param name="index">- position of the morpheme to change
		/// </param>
		/// <param name="morpheme">- new morpheme for the index
		/// </param>
		/// <returns> index: when the morpheme was set up correctly, otherwise -1
		/// </returns>
		public virtual int setMorpheme(int index, System.String morpheme)
		{
			if (index >= 0 && index < morphemes.Length)
			{
				morphemes[index] = morpheme;
				return index;
			}
			else
			{
				return - 1;
			}
		}
		
		/// <summary> It returns the tag of the morpheme on the given position.</summary>
		/// <param name="index">- the position of the morpheme to get its tag
		/// </param>
		/// <returns> morpheme tag on the given position
		/// </returns>
		public virtual System.String getTag(int index)
		{
			return tags[index];
		}
		
		/// <summary> It changes the tag of the morpheme on the index</summary>
		/// <param name="index">- position of the morpheme to change its tag
		/// </param>
		/// <param name="tag">- new morpheme tag
		/// </param>
		/// <returns> index: the new tag was set up correctly, otherwise -1
		/// </returns>
		public virtual int setTag(int index, System.String tag)
		{
			if (index >= 0 && index < tags.Length)
			{
				tags[index] = tag;
				return index;
			}
			else
			{
				return - 1;
			}
		}
		
		/// <summary> It returns a string that represents the eojeol with morphemes and tags.
		/// For example, 나/npp+는/jxc.
		/// </summary>
		public override System.String ToString()
		{
			System.String str = "";
			for (int i = 0; i < length; i++)
			{
				if (i != 0)
				{
					str += "+";
				}
				str += (morphemes[i] + "/" + tags[i]);
			}
			return str;
		}
	}
}