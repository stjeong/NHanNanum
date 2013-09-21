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
	
	/// <summary> This class is for the connection rules of morphemes. It is used to check whether the morphemes
	/// can appear consecutively.
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class Connection
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
		
		/// <summary>The connectoin table which has the connection information ofo morphemes </summary>
		private bool[][] connectionTable = null;
		
		/// <summary> Constructor.</summary>
		public Connection()
		{
			title = "";
			version = "";
			copyright = "";
			author = "";
			date = "";
			editor = "";
			startTag = "";
			connectionTable = null;
		}
		
		/// <summary> Checks whether two morpheme tags can appear consecutively.</summary>
		/// <param name="tagSet">- morpheme tag set
		/// </param>
		/// <param name="tag1">- the first morpheme tag to check
		/// </param>
		/// <param name="tag2">- the second morpheme tag to check
		/// </param>
		/// <param name="len1">- the length of the first morpheme
		/// </param>
		/// <param name="len2">- the length of the second morpheme
		/// </param>
		/// <param name="typeOfTag2">- the tag type of the second morpheme tag
		/// </param>
		/// <returns> true: the two consecutive morpheme tags can appear, false: they cannot appear
		/// </returns>
		public virtual bool checkConnection(TagSet tagSet, int tag1, int tag2, int len1, int len2, int typeOfTag2)
		{
			System.String tag1Name = tagSet.getTagName(tag1);
			System.String tag2Name = tagSet.getTagName(tag2);
			
			if ((tag1Name.StartsWith("nc") || tag1Name[0] == 'f') && tag2Name[0] == 'n')
			{
				if (tag2Name.StartsWith("nq"))
				{
					return false;
				}
				else if (len1 < 4 || len2 < 2)
				{
					return false;
				}
			}
			
			//		System.err.println(tag1Name + "\t" + tag2Name + ": " + connectionTable[tag1][tag2] + " " + tagSet.checkTagType(nextTagType, tag2));
			return connectionTable[tag1][tag2] && tagSet.checkTagType(typeOfTag2, tag2);
		}
		
		/// <summary> Cleans the connection rules and metadata.</summary>
		public virtual void  clear()
		{
			title = "";
			version = "";
			copyright = "";
			author = "";
			date = "";
			editor = "";
			startTag = "";
			connectionTable = null;
		}
		
		/// <summary> Initialize the connection rules from the rule data file.</summary>
		/// <param name="filePath">- the path for the connection rule data file
		/// </param>
		/// <param name="tagCount">- the number of the total tags
		/// </param>
		/// <param name="tagSet">- the tag set which is used in the connection rules
		/// </param>
		/// <throws>  IOException </throws>
		public virtual void  init(System.String filePath, int tagCount, TagSet tagSet)
		{
			readFile(filePath, tagCount, tagSet);
		}
		
		/// <summary> Reads the connection rule data file, and initialize the object.</summary>
		/// <param name="filePath">- the path for the connection rule file
		/// </param>
		/// <param name="tagCount">- the number of total tags in the tag set
		/// </param>
		/// <param name="tagSet">- the tag set which is used in the connection rules
		/// </param>
		/// <throws>  IOException </throws>
		private void  readFile(System.String filePath, int tagCount, TagSet tagSet)
		{
			System.IO.StreamReader br = new System.IO.StreamReader(
                new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read),
                System.Text.Encoding.UTF8);
			System.String line = null;
            HashSet<int> tagSetA = new HashSet<int>();
            HashSet<int> tagSetB = new HashSet<int>();
			
			title = "";
			version = "";
			copyright = "";
			author = "";
			date = "";
			editor = "";
			startTag = "";
			connectionTable = new bool[tagCount][];
			for (int i = 0; i < tagCount; i++)
			{
				connectionTable[i] = new bool[tagCount];
			}
			
			for (int i = 0; i < tagCount; i++)
			{
				for (int j = 0; j < tagCount; j++)
				{
					connectionTable[i][j] = false;
				}
			}
			
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
				else if ("CONNECTION".Equals(lineToken))
				{
					lineToken = lineTokenizer.NextToken;
                    System.String[] tagLists = lineToken.Split("\\*", 2);

                    StringTokenizer tagTokenizer = new StringTokenizer(tagLists[0], ",()");
					while (tagTokenizer.HasMoreTokens)
					{
						System.String tagToken = tagTokenizer.NextToken;

                        StringTokenizer tok = new StringTokenizer(tagToken, "-");
						while (tok.HasMoreTokens)
						{
							System.String t = tok.NextToken;
							int[] fullTagIDSet = tagSet.getTags(t);
							
							if (fullTagIDSet != null)
							{
								for (int i = 0; i < fullTagIDSet.Length; i++)
								{
									tagSetA.Add(fullTagIDSet[i]);
								}
							}
							else
							{
                                tagSetA.Add(tagSet.getTagID(t));
							}
							while (tok.HasMoreTokens)
							{
								tagSetA.Remove(tagSet.getTagID(tok.NextToken));
							}
						}
					}

                    tagTokenizer = new StringTokenizer(tagLists[1], ",()");
					while (tagTokenizer.HasMoreTokens)
					{
						System.String tagToken = tagTokenizer.NextToken;

                        StringTokenizer tok = new StringTokenizer(tagToken, "-");
						while (tok.HasMoreTokens)
						{
							System.String t = tok.NextToken;
							int[] fullTagIDSet = tagSet.getTags(t);
							
							if (fullTagIDSet != null)
							{
								for (int i = 0; i < fullTagIDSet.Length; i++)
								{
									tagSetB.Add(fullTagIDSet[i]);
								}
							}
							else
							{
                                tagSetB.Add(tagSet.getTagID(t));
							}
							while (tok.HasMoreTokens)
							{
								tagSetB.Remove(tagSet.getTagID(tok.NextToken));
							}
						}
					}

                    IEnumerator<int> iterA = tagSetA.GetEnumerator();
					while (iterA.MoveNext())
					{
						int leftSide = iterA.Current;
                        IEnumerator<int> iterB = tagSetB.GetEnumerator();

                        while (iterB.MoveNext())
						{
                            connectionTable[leftSide][iterB.Current] = true;
						}
					}

                    tagSetA.Clear();
                    tagSetB.Clear();
				}
				else if ("START_TAG".Equals(lineToken))
				{
					startTag = lineTokenizer.NextToken;
				}
			}
			br.Close();
		}
	}
}