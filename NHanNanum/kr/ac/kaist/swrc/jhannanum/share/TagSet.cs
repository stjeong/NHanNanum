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
namespace kr.ac.kaist.swrc.jhannanum.share
{
	
	/// <summary> Morpheme tag set.</summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class TagSet
	{
		/// <summary> Returns the number of morpheme tags loaded.</summary>
		/// <returns> the number of morpheme tags loaded
		/// </returns>
		virtual public int TagCount
		{
			get
			{
				return tagList.Count;
			}
			
		}
		/// <summary> Sets the tag types.</summary>
		/// <param name="tagSetFlag">- the flag for tag set (TAG_SET_KAIST, ..)
		/// </param>
		virtual public int TagTypes
		{
			set
			{
				if (value == TAG_SET_KAIST)
				{
                    List<int> list = new List<int>();
					int[] values = null;
					
					// verb
					values = tagSetMap["pv"];
					for (int i = 0; i < values.Length; i++)
					{
						list.Add(values[i]);
					}
                    values = tagSetMap["xsm"];
					for (int i = 0; i < values.Length; i++)
					{
						list.Add(values[i]);
					}

					list.Add(tagList.IndexOf("px"));
					tagTypeTable[TAG_TYPE_VERBS] = new int[list.Count];

                    IEnumerator<int> iter = list.GetEnumerator();
					for (int i = 0; iter.MoveNext(); i++)
					{
						tagTypeTable[TAG_TYPE_VERBS][i] = iter.Current;
					}
					list.Clear();
					
					// noun
                    tagTypeTable[TAG_TYPE_NOUNS] = tagSetMap["n"];
					
					// nps
                    tagTypeTable[TAG_TYPE_NPS] = tagSetMap["np"];
					
					// adjs
                    tagTypeTable[TAG_TYPE_ADJS] = tagSetMap["pa"];
					
					// eomies
                    tagTypeTable[TAG_TYPE_EOMIES] = tagSetMap["e"];
					
					// yongs
                    values = tagSetMap["p"];
					for (int i = 0; i < values.Length; i++)
					{
						list.Add(values[i]);
					}
                    values = tagSetMap["xsv"];
					for (int i = 0; i < values.Length; i++)
					{
                        list.Add(values[i]);
					}
                    values = tagSetMap["xsm"];
					for (int i = 0; i < values.Length; i++)
					{
                        list.Add(values[i]);
					}
                    list.Add(tagList.IndexOf("ep"));
                    list.Add(tagList.IndexOf("jp"));
					
					tagTypeTable[TAG_TYPE_YONGS] = new int[list.Count];

					iter = list.GetEnumerator();
					for (int i = 0; iter.MoveNext(); i++)
					{
						tagTypeTable[TAG_TYPE_YONGS][i] = iter.Current;
					}
					list.Clear();
					
					// jp
					tagTypeTable[TAG_TYPE_JP] = new int[1];
					tagTypeTable[TAG_TYPE_JP][0] = tagList.IndexOf("jp");
					
					// nbnp
					tagTypeTable[TAG_TYPE_NBNP] = new int[3];
                    tagTypeTable[TAG_TYPE_NBNP][0] = tagList.IndexOf("nbn");
                    tagTypeTable[TAG_TYPE_NBNP][1] = tagList.IndexOf("npd");
                    tagTypeTable[TAG_TYPE_NBNP][2] = tagList.IndexOf("npp");
					
					// josa
					tagTypeTable[TAG_TYPE_JOSA] = new int[6];
                    tagTypeTable[TAG_TYPE_JOSA][0] = tagList.IndexOf("jxc");
                    tagTypeTable[TAG_TYPE_JOSA][0] = tagList.IndexOf("jco");
                    tagTypeTable[TAG_TYPE_JOSA][0] = tagList.IndexOf("jca");
                    tagTypeTable[TAG_TYPE_JOSA][0] = tagList.IndexOf("jcm");
                    tagTypeTable[TAG_TYPE_JOSA][0] = tagList.IndexOf("jcs");
                    tagTypeTable[TAG_TYPE_JOSA][0] = tagList.IndexOf("jcc");
				}
			}
			
		}
		/// <summary>KAIST tag set </summary>
		public const int TAG_SET_KAIST = 0;
		
		/// <summary>tag type - all </summary>
		public const int TAG_TYPE_ALL = 0;
		
		/// <summary>tag type - verb </summary>
		public const int TAG_TYPE_VERBS = 1;
		
		/// <summary>tag type - noun </summary>
		public const int TAG_TYPE_NOUNS = 2;
		
		/// <summary>tag type - pronoun </summary>
		public const int TAG_TYPE_NPS = 3;
		
		/// <summary>tag type - adjective </summary>
		public const int TAG_TYPE_ADJS = 4;
		
		/// <summary>tag type - bound noun </summary>
		public const int TAG_TYPE_NBNP = 5;
		
		/// <summary>tag type - josa(particle) </summary>
		public const int TAG_TYPE_JOSA = 6;
		
		/// <summary>tag type - yongeon(verb, adjective) </summary>
		public const int TAG_TYPE_YONGS = 7;
		
		/// <summary>tag type - eomi(ending) </summary>
		public const int TAG_TYPE_EOMIES = 8;
		
		/// <summary>tag type - predicative particle </summary>
		public const int TAG_TYPE_JP = 9;
		
		/// <summary>the number of tag types </summary>
		public const int TAG_TYPE_COUNT = 10;
		
		/// <summary>phoneme type - all </summary>
		public const int PHONEME_TYPE_ALL = 0;
		
		/// <summary>the name of tag set </summary>
		public System.String title = null;
		
		/// <summary>the version of tag set </summary>
		public System.String version = null;
		
		/// <summary>the copyright of tag set </summary>
		public System.String copyright = null;
		
		/// <summary>the author of tag set </summary>
		public System.String author = null;
		
		/// <summary>the last update date of tag set </summary>
		public System.String date = null;
		
		/// <summary>the editor of the tag set </summary>
		public System.String editor = null;
		
		/// <summary>the morpheme tag list </summary>
		private List< String > tagList = null;
		
		/// <summary>the irregular rule list </summary>
		private List< String > irregularList = null;
		
		/// <summary>the hash map for the group of tags </summary>
		private Dictionary < String, int [] > tagSetMap = null;
		
		/// <summary>the table for tag types </summary>
		private int[][] tagTypeTable = null;
		
		/// <summary>the list of index tags </summary>
		public int[] indexTags = null;
		
		/// <summary>the list of unknown tags </summary>
		public int[] unkTags = null;
		
		/// <summary>the start tag </summary>
		public int iwgTag = 0;
		
		/// <summary>the unknown tag </summary>
		public int unkTag = 0;
		
		/// <summary>the number tag </summary>
		public int numTag = 0;
		
		/// <summary>'ㅂ' irregular </summary>
		public int IRR_TYPE_B;
		
		/// <summary>'ㅅ' irregular </summary>
		public int IRR_TYPE_S;
		
		/// <summary>'ㄷ' irregular </summary>
		public int IRR_TYPE_D;
		
		/// <summary>'ㅎ' irregular </summary>
		public int IRR_TYPE_H;
		
		/// <summary>'르' irregular </summary>
		public int IRR_TYPE_REU;
		
		/// <summary>'러' irregular </summary>
		public int IRR_TYPE_REO;
		
		/// <summary> Constructor.</summary>
		public TagSet()
		{
			title = "";
			version = "";
			copyright = "";
			author = "";
			date = "";
			editor = "";
			
			tagList = new List< String >();
			
			irregularList = new List< String >();
			
			tagSetMap = new Dictionary< String, int [] >();
			tagTypeTable = new int[TAG_TYPE_COUNT][];
		}
		
		/// <summary> Checks the phoneme type</summary>
		/// <param name="phonemeType">- phoneme type
		/// </param>
		/// <param name="phoneme">- phoneme
		/// </param>
		/// <returns> true: the phoneme belongs to the specified type, otherwise false
		/// </returns>
		public virtual bool checkPhonemeType(int phonemeType, int phoneme)
		{
			if (phonemeType == PHONEME_TYPE_ALL)
			{
				return true;
			}
			return phonemeType == phoneme;
		}
		
		/// <summary> Checks morpheme tag type</summary>
		/// <param name="tagType">- tag type
		/// </param>
		/// <param name="tag">- morpheme tag
		/// </param>
		/// <returns> true: the morpheme tag belongs to the specified type
		/// </returns>
		public virtual bool checkTagType(int tagType, int tag)
		{
			if (tagType == TAG_TYPE_ALL)
			{
				return true;
			}
			for (int i = 0; i < tagTypeTable[tagType].Length; i++)
			{
				if (tagTypeTable[tagType][i] == tag)
				{
					return true;
				}
			}
			return false;
		}
		
		/// <summary> Cleans the data loaded.</summary>
		public virtual void  clear()
		{
			title = "";
			version = "";
			copyright = "";
			author = "";
			date = "";
			editor = "";
			tagList.Clear();
			irregularList.Clear();
			tagSetMap.Clear();
		}
		
		/// <summary> Returns the ID of the specified irregular rule.</summary>
		/// <param name="irregular	-">irregular rule
		/// </param>
		/// <returns> the ID of the irregular rule
		/// </returns>
		public virtual int getIrregularID(System.String irregular)
		{
			return irregularList.IndexOf(irregular);
		}
		
		/// <summary> Returns the name of the irregular rule for the specified ID.</summary>
		/// <param name="irregularID">- the ID of the irregular rule
		/// </param>
		/// <returns> the name of the irregular rule
		/// </returns>
		public virtual System.String getIrregularName(int irregularID)
		{
			return irregularList[irregularID];
		}
		
		/// <summary> Returns the ID of the morpheme tag.</summary>
		/// <param name="tag">- morpheme tag
		/// </param>
		/// <returns> the tag ID, -1 if it doesn't contain the tag
		/// </returns>
		public virtual int getTagID(System.String tag)
		{
			return tagList.IndexOf(tag);
		}
		
		/// <summary> Returns the tag name for the specified ID.</summary>
		/// <param name="tagID	-">the morpheme tag ID
		/// </param>
		/// <returns> the tag name
		/// </returns>
		public virtual System.String getTagName(int tagID)
		{
            return tagList[tagID];
		}
		
		/// <summary> Returns the morpheme tags in the specified tag group.</summary>
		/// <param name="tagSetName">- the name of the tag group
		/// </param>
		/// <returns> the list of tag IDs
		/// </returns>
		public virtual int[] getTags(System.String tagSetName)
		{
            if (tagSetMap.ContainsKey(tagSetName) == false)
            {
                return null;
            }

            return tagSetMap[tagSetName];
		}
		
		/// <summary> Reads the tag set file, and initializes the object.</summary>
		/// <param name="filePath">- the file for morpheme tag set
		/// </param>
		/// <throws>  IOException </throws>
		public virtual void  init(System.String filePath, int tagSetFlag)
		{
			System.IO.StreamReader br = new System.IO.StreamReader(
                new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read),
                System.Text.Encoding.UTF8);
			System.String line = null;
			
			title = "";
			version = "";
			copyright = "";
			author = "";
			date = "";
			editor = "";
			tagList.Clear();
            irregularList.Clear();
            tagSetMap.Clear();
			
            List<int> tempTagNumbers = new List<int>();
			
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
				else if ("TAG".Equals(lineToken))
				{
					tagList.Add(lineTokenizer.NextToken);
				}
				else if ("TSET".Equals(lineToken))
				{
					System.String tagSetName = lineTokenizer.NextToken;
                    StringTokenizer tagTokenizer = new StringTokenizer(lineTokenizer.NextToken, " ");
					
					while (tagTokenizer.HasMoreTokens)
					{
						System.String tagToken = tagTokenizer.NextToken;
						int tagNumber = tagList.IndexOf(tagToken);
						
						if (tagNumber != - 1)
						{
                            tempTagNumbers.Add(tagNumber);
						}
						else
						{
                            int[] values = tagSetMap[tagToken];
							if (values != null)
							{
								for (int i = 0; i < values.Length; i++)
								{
                                    tempTagNumbers.Add(values[i]);
								}
							}
						}
					}
					int[] tagNumbers = new int[tempTagNumbers.Count];

                    IEnumerator<int> iter = tempTagNumbers.GetEnumerator();
					for (int i = 0; iter.MoveNext(); i++)
					{
						tagNumbers[i] = iter.Current;
					}
                    tagSetMap[tagSetName] = tagNumbers;
					tempTagNumbers.Clear();
				}
				else if ("IRR".Equals(lineToken))
				{
                    irregularList.Add(lineTokenizer.NextToken);
				}
			}
			br.Close();
			
			TagTypes = tagSetFlag;
            indexTags = tagSetMap["index"];
			unkTags = tagSetMap["unkset"];
			iwgTag = tagList.IndexOf("iwg");
            unkTag = tagList.IndexOf("unk");
            numTag = tagList.IndexOf("nnc");
			
			IRR_TYPE_B = getIrregularID("irrb");
			IRR_TYPE_S = getIrregularID("irrs");
			IRR_TYPE_D = getIrregularID("irrd");
			IRR_TYPE_H = getIrregularID("irrh");
			IRR_TYPE_REU = getIrregularID("irrlu");
			IRR_TYPE_REO = getIrregularID("irrle");
		}
	}
}