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
using System.Diagnostics;
using Code = kr.ac.kaist.swrc.jhannanum.share.Code;
namespace kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.ChartMorphAnalyzer
{
	
	/// <summary> This class is for segmentation of morphemes in a given eojeol.</summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class SegmentPosition
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'Position' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		/// <summary> This class marks the position of segmentation.</summary>
		/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, Kaist
		/// </author>
		public class Position
		{
			public Position(SegmentPosition enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(SegmentPosition enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
				morpheme = new int[kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.ChartMorphAnalyzer.SegmentPosition.MAX_MORPHEME_COUNT];
			}
			private SegmentPosition enclosingInstance;
			public SegmentPosition Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			/// <summary>the consonant or vowel of this position </summary>
			internal char key;
			
			/// <summary>the processing state </summary>
			internal int state;
			
			/// <summary>the index of next segment position </summary>
			internal int nextPosition;
			
			/// <summary>the temporary index for system dictionary </summary>
			internal int sIndex;
			
			/// <summary>the temporary index for user dictionary </summary>
			internal int uIndex;
			
			/// <summary>the temporary index for number dictionary </summary>
			internal int nIndex;
			
			/// <summary>the number of morphemes possible at this position </summary>
			internal int morphCount;
			
			/// <summary>the list of morphemes possible at this position </summary>
			//UPGRADE_NOTE: The initialization of  'morpheme' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
			internal int[] morpheme;
		}
		
		/// <summary>the maximum number of segmentation </summary>
		public const int MAX_SEGMENT = 1024;
		
		/// <summary>the maximum number of morphemes possible </summary>
		public const int MAX_MORPHEME_COUNT = 512;
		
		/// <summary>the processing state - dictionary search </summary>
		public const int SP_STATE_N = 0;
		
		/// <summary>the processing state - expansion regarding phoneme change phenomenon </summary>
		public const int SP_STATE_D = 1;
		
		/// <summary>the processing state - recursive processing </summary>
		public const int SP_STATE_R = 2;
		
		/// <summary>the processing state - connection rule </summary>
		public const int SP_STATE_M = 3;
		
		/// <summary>the processing state - failure </summary>
		public const int SP_STATE_F = 4;
		
		/// <summary>the key of the start node for data structure </summary>
		public const char POSITION_START_KEY = (char) (0);
		
		/// <summary>the list of segment positions </summary>
		private Position[] position = null;
		
		/// <summary>the last index of the segment position </summary>
		private int positionEnd = 0;
		
		/// <summary> Constructor.</summary>
		public SegmentPosition()
		{
			position = new Position[MAX_SEGMENT];
			for (int i = 0; i < MAX_SEGMENT; i++)
			{
				position[i] = new Position(this);
			}
		}
		
		/// <summary> Adds new segment position.</summary>
		/// <param name="key">- vowel or consonant of the segment position
		/// </param>
		/// <returns> the index of the segment position in the list
		/// </returns>
		public virtual int addPosition(char key)
		{
			position[positionEnd].key = key;
			position[positionEnd].state = SP_STATE_N;
			position[positionEnd].morphCount = 0;
			position[positionEnd].nextPosition = 0;
			position[positionEnd].sIndex = 0;
			position[positionEnd].uIndex = 0;
			position[positionEnd].nIndex = 0;
			
			return positionEnd++;
		}
		
		/// <summary> Gets the segment position on the specified index.</summary>
		/// <param name="index">- the index of the segment position
		/// </param>
		/// <returns> the segment position on the specified index
		/// </returns>
		public virtual Position getPosition(int index)
		{
			return position[index];
		}
		
		/// <summary> Initializes the data structure for segment positions with given string.</summary>
		/// <param name="str">- the plain string to analyze
		/// </param>
		/// <param name="simti">- SIMple Trie Index
		/// </param>
		public virtual void  init(System.String str, Simti simti)
		{
			int prevIndex = 0;
			int nextIndex = 0;
			
			positionEnd = 0;
			prevIndex = addPosition(POSITION_START_KEY);
			position[prevIndex].state = SP_STATE_M;
			
			System.String rev = "";
			for (int i = str.Length - 1; i >= 0; i--)
			{
				rev += str[i];
			}
			
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				nextIndex = addPosition(c);
				setPositionLink(prevIndex, nextIndex);
				prevIndex = nextIndex;
				
				simti.insert(rev.Substring(0, (str.Length - i) - (0)).ToCharArray(), nextIndex);
			}
			
			/* for marking the end of the eojeol */
			setPositionLink(prevIndex, 0);
		}
		
		/// <summary> Returns the index of the next segment position of the position on the specified index.</summary>
		/// <param name="index">- the index of the segment position
		/// </param>
		/// <returns> the index of the next segment position
		/// </returns>
		public virtual int nextPosition(int index)
		{
			return position[index].nextPosition;
		}
		
		/// <summary> It prints the segment position information to the console.</summary>
		public virtual void  printPosition()
		{
			System.Console.Error.WriteLine("positionEnd: " + positionEnd);
			for (int i = 0; i < positionEnd; i++)
			{
				Trace.WriteLine(
                    string.Format("position[{0}].key={1} nextPosition={2}", i, Code.toCompatibilityJamo(position[i].key), position[i].nextPosition));
			}
		}
		
		/// <summary> It connects two segment positions.</summary>
		/// <param name="prevIndex">- the index of the previous position
		/// </param>
		/// <param name="nextIndex">- the index of the next position
		/// </param>
		/// <returns> the index of the previous position
		/// </returns>
		public virtual int setPositionLink(int prevIndex, int nextIndex)
		{
			position[prevIndex].nextPosition = nextIndex;
			return prevIndex;
		}
	}
}