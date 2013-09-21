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
using PetShop.Data;
using TagSet = kr.ac.kaist.swrc.jhannanum.share.TagSet;
namespace kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.ChartMorphAnalyzer
{
	
	/// <summary> This class is for the impossible connection rules of morphemes. It is used to check
	/// whether morphemes may not appear consecutively.
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class ConnectionNot
	{
		/// <summary>The name of the connection rules. </summary>
		public System.String title = null;
		
		/// <summary>The version of the connection rules. </summary>
		public System.String version = null;
		
		/// <summary>The copyright of the connection rules. </summary>
		public System.String copyright = null;
		
		/// <summary>The author of the connection rules. </summary>
		public System.String author = null;
		
		/// <summary>The date when the connection rules are updated. </summary>
		public System.String date = null;
		
		/// <summary>The people who edited the connection rules. </summary>
		public System.String editor = null;
		
		/// <summary>Start tag. </summary>
		public System.String startTag = null;
		
		/// <summary>Table for the tags that cannot appear consecutively </summary>
		private int[][] notTagTable = null;
		
		/// <summary>Table for the morphemes that cannot appear consecutively </summary>
		private System.String[][] notMorphTable = null;
		
		/// <summary>The number of impossible connection rules </summary>
		private int ruleCount = 0;
		
		
		/// <summary> Constructor.</summary>
		public ConnectionNot()
		{
			title = "";
			version = "";
			copyright = "";
			author = "";
			date = "";
			editor = "";
			startTag = "";
		}
		
		/// <summary> Checks whether the two morphemes may not appear consecutively.</summary>
		/// <returns> true: they may appear consecutively, false: they may not appear consecutively
		/// </returns>
		public virtual bool checkConnection()
		{
			/* It should be updated.s */
			return true;
		}
		
		/// <summary> Cleans the rules loaded and metadata.</summary>
		public virtual void  clear()
		{
			title = "";
			version = "";
			copyright = "";
			author = "";
			date = "";
			editor = "";
			startTag = "";
			ruleCount = 0;
			notTagTable = null;
			notMorphTable = null;
		}
		
		/// <summary> Initializes the object with the specified file for impossible connection rules.</summary>
		/// <param name="filePath">- the file for the impossible connection rules
		/// </param>
		/// <param name="tagSet">- the morpheme tag set used in the rules
		/// </param>
		/// <throws>  IOException </throws>
		public virtual void  init(System.String filePath, TagSet tagSet)
		{
			readFile(filePath, tagSet);
		}
		
		/// <summary> Reads the impossible connection rules from the specified file.</summary>
		/// <param name="filePath">- the file for the impossible connection rules
		/// </param>
		/// <param name="tagSet">- the morpheme tag set used in the rules
		/// </param>
		/// <throws>  IOException </throws>
		private void  readFile(System.String filePath, TagSet tagSet)
		{
			System.IO.StreamReader br = new System.IO.StreamReader(
                new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read), System.Text.Encoding.UTF8);
			System.String line = null;
			
			List< String > ruleList = new List< String >();
			
			title = "";
			version = "";
			copyright = "";
			author = "";
			date = "";
			editor = "";
			startTag = "";
			ruleCount = 0;
			
			while ((line = br.ReadLine()) != null)
			{
                StringTokenizer lineTokenizer = new StringTokenizer(line, "\t");
				if (lineTokenizer.HasMoreTokens == false)
				{
					continue;
				}
				
				System.String lineToken = lineTokenizer.NextToken;
				
				if (lineToken.StartsWith("@"))
				{
					if ("@title".Equals(lineToken))
					{
						title = lineTokenizer.NextToken;
					}
					else if ("@version".Equals(lineToken))
					{
						version = lineTokenizer.NextToken;
					}
					else if ("@copyright".Equals(lineToken))
					{
						copyright = lineTokenizer.NextToken;
					}
					else if ("@author".Equals(lineToken))
					{
						author = lineTokenizer.NextToken;
					}
					else if ("@date".Equals(lineToken))
					{
						date = lineTokenizer.NextToken;
					}
					else if ("@editor".Equals(lineToken))
					{
						editor = lineTokenizer.NextToken;
					}
				}
				else if ("CONNECTION_NOT".Equals(lineToken))
				{
					ruleList.Add(lineTokenizer.NextToken);
				}
			}
			
			ruleCount = ruleList.Count;
			
			notTagTable = new int[ruleCount][];
			for (int i = 0; i < ruleCount; i++)
			{
				notTagTable[i] = new int[2];
			}
			notMorphTable = new System.String[ruleCount][];
			for (int i2 = 0; i2 < ruleCount; i2++)
			{
				notMorphTable[i2] = new System.String[2];
			}

            IEnumerator<string> iter = ruleList.GetEnumerator();
			for (int i = 0; iter.MoveNext(); i++)
			{
				System.String rule = iter.Current;
                StringTokenizer st = new StringTokenizer(rule, " ");
				notMorphTable[i][0] = st.NextToken;
				notTagTable[i][0] = tagSet.getTagID(st.NextToken);
				notMorphTable[i][1] = st.NextToken;
				notTagTable[i][1] = tagSet.getTagID(st.NextToken);
			}
			
			ruleList.Clear();
			br.Close();
		}
	}
}