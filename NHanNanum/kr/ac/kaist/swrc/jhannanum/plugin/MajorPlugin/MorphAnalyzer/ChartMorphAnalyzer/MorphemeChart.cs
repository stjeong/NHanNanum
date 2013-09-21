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
using Eojeol = kr.ac.kaist.swrc.jhannanum.comm.Eojeol;
using Position = kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.ChartMorphAnalyzer.SegmentPosition.Position;
using INFO = kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.ChartMorphAnalyzer.Trie.INFO;
using TNODE = kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.ChartMorphAnalyzer.Trie.TNODE;
using Code = kr.ac.kaist.swrc.jhannanum.share.Code;
using TagSet = kr.ac.kaist.swrc.jhannanum.share.TagSet;
using System.Collections.Generic;
using System.Diagnostics;
namespace kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.ChartMorphAnalyzer
{
	
	/// <summary> This class is for the lattice style morpheme chart which is a internal data structure for morphological analysis without backtracking.</summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class MorphemeChart
	{
		public MorphemeChart()
		{
		}

		/// <summary> A morpheme node in the lattice style chart.</summary>
		/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
		/// </author>
		public class Morpheme
		{
			public Morpheme(MorphemeChart enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(MorphemeChart enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
				connection = new int[kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.ChartMorphAnalyzer.MorphemeChart.MAX_MORPHEME_CONNECTION];
			}
			private MorphemeChart enclosingInstance;
			public MorphemeChart Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			/// <summary>morpheme tag </summary>
			public int tag;
			
			/// <summary>phoneme </summary>
			public int phoneme;
			
			/// <summary>the index of the next node </summary>
			public int nextPosition;
			
			/// <summary>the type of the next morpheme </summary>
			public int nextTagType;
			
			/// <summary>the state of current processing </summary>
			public int state;
			
			/// <summary>the number of morphemes connected </summary>
			public int connectionCount;
			
			/// <summary>the list of the morphemes connected </summary>
			public int[] connection;
			
			/// <summary>plain string </summary>
			public System.String str = "";
		}
		
		/// <summary>the reserved word for replacement of Chinese characters </summary>
		private const System.String CHI_REPLACE = "HAN_CHI";
		
		/// <summary>the reserved word for replacement of English alphabets </summary>
		private const System.String ENG_REPLACE = "HAN_ENG";
		
		/// <summary>the list for the replacement of Chinese character </summary>
		private LinkedList < String > chiReplacementList = null;
		
		/// <summary>the list for the replacement of English alphabets </summary>
		private LinkedList < String > engReplacementList = null;
		
		/// <summary>the index for replacement of English alphabets </summary>
		private int engReplaceIndex = 0;
		
		/// <summary>the index for replacement of Chinese characters </summary>
		private int chiReplaceIndex = 0;
		
		/// <summary>the maximum number of connections between one morpheme and others </summary>
		private const int MAX_MORPHEME_CONNECTION = 30;
		
		/// <summary>the maximum number of morpheme nodes in the chart </summary>
		private const int MAX_MORPHEME_CHART = 2046;
		
		/// <summary>the processing state - incomplete </summary>
		private const int MORPHEME_STATE_INCOMPLETE = 2;
		
		/// <summary>the processing state - success </summary>
		private const int MORPHEME_STATE_SUCCESS = 1;
		
		/// <summary>the maximum number of analysis results </summary>
		private const int MAX_CANDIDATE_NUM = 100000;
		
		/// <summary>the processing state - fail </summary>
		private const int MORPHEME_STATE_FAIL = 0;
		
		/// <summary>the morpheme chart </summary>
		public Morpheme[] chart = null;
		
		/// <summary>the last index of the chart </summary>
		public int chartEnd = 0;
		
		/// <summary>the morpheme tag set </summary>
		private TagSet tagSet = null;
		
		/// <summary>the connection rules </summary>
		private Connection connection = null;
		
		/// <summary>segment position </summary>
		private SegmentPosition sp = null;
		
		/// <summary>string buffer </summary>
		private System.String bufString = "";
		
		/// <summary>path of segmentation </summary>
        private int[] segmentPath = new int[SegmentPosition.MAX_SEGMENT];
		
		/// <summary>chart expansion </summary>
		private Exp exp = null;
		
		/// <summary>system morpheme dictionary </summary>
		private Trie systemDic = null;
		
		/// <summary>user morpheme dictionary </summary>
		private Trie userDic = null;
		
		/// <summary>number dictionary - automata </summary>
		private NumberDic numDic = null;
		
		/// <summary>SIMple Trie Index </summary>
		private Simti simti = null;
		
		/// <summary>the number of analysis results printed </summary>
		private int printResultCnt = 0;
		
		/// <summary>the list of eojeols analyzed </summary>
		private LinkedList < Eojeol > resEojeols = null;
		
		/// <summary>the list of morphemes analyzed </summary>
		private List< String > resMorphemes = null;
		
		/// <summary>the list of morpheme tags analyzed </summary>
		private List< String > resTags = null;
		
		/// <summary> Constructor.</summary>
		/// <param name="tagSet">- the morpheme tag set
		/// </param>
		/// <param name="connection">- the morpheme connection rules
		/// </param>
		/// <param name="systemDic">- the system morpheme dictionary
		/// </param>
		/// <param name="userDic">- the user morpheme dictionary
		/// </param>
		/// <param name="numDic">- the number dictionary
		/// </param>
		/// <param name="simti">- the SIMple Trie Index
		/// </param>
		/// <param name="resEojeolList">- the list of eojeols to store the analysis result
		/// </param>
        public MorphemeChart(TagSet tagSet, Connection connection, Trie systemDic, Trie userDic, NumberDic numDic, Simti simti, LinkedList<Eojeol> resEojeolList)
        {
            chart = new Morpheme[MAX_MORPHEME_CHART];
            for (int i = 0; i < MAX_MORPHEME_CHART; i++)
            {
                chart[i] = new Morpheme(this);
            }

            this.sp = new SegmentPosition();
            this.tagSet = tagSet;
            this.connection = connection;
            this.exp = new Exp(this, tagSet);
            this.systemDic = systemDic;
            this.userDic = userDic;
            this.numDic = numDic;
            this.simti = simti;
            this.resEojeols = resEojeolList;

            resMorphemes = new List<String>();
            resTags = new List<String>();

            chiReplacementList = new LinkedList<String>();
            engReplacementList = new LinkedList<String>();
        }
		
		/// <summary> Adds a new morpheme to the chart.</summary>
		/// <param name="tag">- the morpheme tag ID
		/// </param>
		/// <param name="phoneme">- phoneme
		/// </param>
		/// <param name="nextPosition">- the index of next morpheme
		/// </param>
		/// <param name="nextTagType">- the tag type of next morpheme
		/// </param>
		/// <returns> the last index of the chart
		/// </returns>
		public virtual int addMorpheme(int tag, int phoneme, int nextPosition, int nextTagType)
		{
			chart[chartEnd].tag = tag;
			chart[chartEnd].phoneme = phoneme;
			chart[chartEnd].nextPosition = nextPosition;
			chart[chartEnd].nextTagType = nextTagType;
			chart[chartEnd].state = MORPHEME_STATE_INCOMPLETE;
			chart[chartEnd].connectionCount = 0;
			return chartEnd++;
		}
		
		/// <summary> It inserts the reverse of the given string to the SIMTI data structure.</summary>
		/// <param name="str">- string to insert to the SIMTI structure
		/// </param>
		/// <returns> the index of the next morpheme
		/// </returns>
		public virtual int altSegment(System.String str)
		{
			int prev = 0;
			int next = 0;
			int match;
			int len;
			int to;
			
			len = str.Length;
			
			System.String rev = "";
			for (int i = len - 1; i >= 0; i--)
			{
				rev += str[i];
			}
			
			char[] revStrArray = rev.ToCharArray();
			
			match = simti.search(revStrArray);
			to = simti.fetch(rev.Substring(0, (match) - (0)).ToCharArray());
			
			for (int i = 0; i < str.Length; i++)
			{
				if (len <= match)
				{
					break;
				}
				next = sp.addPosition(str[i]);
				if (prev != 0)
				{
					sp.setPositionLink(prev, next);
				}
				
				simti.insert(rev.Substring(0, (len) - (0)).ToCharArray(), next);
				prev = next;
				len--;
			}
			
			if (prev != 0)
			{
				sp.setPositionLink(prev, to);
			}
			
			return simti.fetch(revStrArray);
		}
		
		/// <summary> It performs morphological analysis on the morpheme chart constructed.</summary>
		/// <returns> the number of analysis results
		/// </returns>
		public virtual int analyze()
		{
			int res = 0;
			
			res = analyze(0, TagSet.TAG_TYPE_ALL);
			
			if (res > 0)
			{
				return res;
			}
			else
			{
				return analyzeUnknown();
			}
		}
		
		/// <summary> It performs morphological anlysis on the morpheme chart from the specified index in the chart.</summary>
		/// <param name="chartIndex">- the index of the chart to analyze
		/// </param>
		/// <param name="tagType">- the type of next morpheme
		/// </param>
		/// <returns> the number of analysis results
		/// </returns>
		private int analyze(int chartIndex, int tagType)
		{
			int from, to;
			int i, j, x, y;
			int mp;
			char c;
			int nc_idx;
			TNODE node;
            LinkedList<INFO> infoList = null;
			INFO info = null;
			
			int sidx = 1;
			int uidx = 1;
			int nidx = 1;
			Position fromPos = null;
			Position toPos = null;
			Morpheme morph = chart[chartIndex];
			from = morph.nextPosition;
			fromPos = sp.getPosition(from);
			
			switch (sp.getPosition(from).state)
			{
				
				default: 
					return 0;
					
					/* dictionary search */
				
				
				case SegmentPosition.SP_STATE_N: 
					i = 0;
					bufString = "";
					
					// searches all combinations of words segmented through the dictionaries
					for (to = from; to != SegmentPosition.POSITION_START_KEY; to = sp.nextPosition(to))
					{
						toPos = sp.getPosition(to);
						c = toPos.key;
						
						if (sidx != 0)
						{
							sidx = systemDic.node_look(c, sidx);
						}
						if (uidx != 0)
						{
							uidx = userDic.node_look(c, uidx);
						}
						if (nidx != 0)
						{
							nidx = numDic.node_look(c, nidx);
						}
						
						toPos.sIndex = sidx;
						toPos.uIndex = uidx;
						toPos.nIndex = nidx;
						
						bufString += c;
						segmentPath[i++] = to;
					}
					
					nidx = 0;
					
					for (; i > 0; i--)
					{
						to = segmentPath[i - 1];
						toPos = sp.getPosition(to);
						
						// system dictionary
						if (toPos.sIndex != 0)
						{
							node = systemDic.get_node(toPos.sIndex);
							if ((infoList = node.info_list) != null)
							{
								for (j = 0; j < infoList.Count; j++)
								{
									info = infoList.Get_Renamed(j);
									
									nc_idx = addMorpheme(info.tag, info.phoneme, sp.nextPosition(to), 0);
									chart[nc_idx].str = bufString.Substring(0, (i) - (0));
									fromPos.morpheme[fromPos.morphCount++] = nc_idx;
								}
							}
						}
						
						// user dictionary
						if (toPos.uIndex != 0)
						{
							node = userDic.get_node(toPos.uIndex);
							if ((infoList = node.info_list) != null)
							{
                                for (j = 0; j < infoList.Count; j++)
								{
									info = infoList.Get_Renamed(j);
									nc_idx = addMorpheme(info.tag, info.phoneme, sp.nextPosition(to), 0);
									chart[nc_idx].str = bufString.Substring(0, (i) - (0));
									fromPos.morpheme[fromPos.morphCount++] = nc_idx;
								}
							}
						}
						
						// number dictionary
						if (nidx == 0 && toPos.nIndex != 0)
						{
							if (numDic.isNum(toPos.nIndex))
							{
								nc_idx = addMorpheme(tagSet.numTag, TagSet.PHONEME_TYPE_ALL, sp.nextPosition(to), 0);
								chart[nc_idx].str = bufString.Substring(0, (i) - (0));
								fromPos.morpheme[fromPos.morphCount++] = nc_idx;
								nidx = toPos.nIndex;
							}
							else
							{
								nidx = 0;
							}
						}
					}
					
					fromPos.state = SegmentPosition.SP_STATE_D;
					
					/* chart expansion regarding various rules */
					goto case SegmentPosition.SP_STATE_D;
				
				case SegmentPosition.SP_STATE_D: 
					exp.prule(from, morph.str, bufString, sp);
					sp.getPosition(from).state = SegmentPosition.SP_STATE_R;
					
					/* recursive processing */
					goto case SegmentPosition.SP_STATE_R;
				
				case SegmentPosition.SP_STATE_R: 
					x = 0;
					for (i = 0; i < fromPos.morphCount; i++)
					{
						mp = fromPos.morpheme[i];
						
						// It prevents a recursive call for '습니다', which needs to be improved.
						if (tagSet.checkTagType(tagType, chart[mp].tag) == false)
						{
							continue;
						}
						
						// It prevents some redundant processing
						if (chart[mp].state == MORPHEME_STATE_INCOMPLETE)
						{
							y = analyze(mp, chart[mp].nextTagType);
							x += y;
							
							if (y != 0)
							{
								chart[mp].state = MORPHEME_STATE_SUCCESS;
							}
							else
							{
								chart[mp].state = MORPHEME_STATE_FAIL;
							}
						}
						else
						{
							x += chart[mp].connectionCount;
						}
					}
					
					if (x == 0)
					{
						if (tagType == TagSet.TAG_TYPE_ALL)
						{
							fromPos.state = SegmentPosition.SP_STATE_F;
						}
						return 0;
					}
					
					if (tagType == TagSet.TAG_TYPE_ALL)
					{
						fromPos.state = SegmentPosition.SP_STATE_M;
					}
					
					/* connecton rule */
					goto case SegmentPosition.SP_STATE_M;
				
				case SegmentPosition.SP_STATE_M: 
					for (i = 0; i < fromPos.morphCount; i++)
					{
						mp = fromPos.morpheme[i];
						
						if (chart[mp].state == MORPHEME_STATE_SUCCESS && connection.checkConnection(tagSet, morph.tag, chart[mp].tag, morph.str.Length, chart[mp].str.Length, morph.nextTagType))
						{
							morph.connection[morph.connectionCount++] = mp;
						}
					}
					break;
				}
			return morph.connectionCount;
		}
		
		/// <summary> It segments all phonemes, and tags 'unknown' to each segment, and then performs chart analysis,
		/// so that the eojeols that consist of morphems not in the dictionaries can be processed.
		/// </summary>
		/// <returns> the number of analysis results
		/// </returns>
		private int analyzeUnknown()
		{
			int i;
			int nc_idx;
			
			bufString = "";
			
			Position pos_1 = sp.getPosition(1);
			
			for (i = 1; i != 0; i = sp.nextPosition(i))
			{
				Position pos = sp.getPosition(i);
				
				bufString += pos.key;
				
				//			if (Code.isChoseong(pos.key)) {
				//				continue;
				//			}
				
				nc_idx = addMorpheme(tagSet.unkTag, TagSet.PHONEME_TYPE_ALL, sp.nextPosition(i), TagSet.TAG_TYPE_ALL);
				chart[nc_idx].str = bufString;
				
				pos_1.morpheme[pos_1.morphCount++] = nc_idx;
				pos_1.state = SegmentPosition.SP_STATE_R;
			}
			
			chart[0].connectionCount = 0;
			
			return analyze(0, 0);
		}
		
		/// <summary> Checks the specified morpheme is exist in the morpheme chart.</summary>
		/// <param name="morpheme">- the list of indices of the morphemes to check
		/// </param>
		/// <param name="morphemeLen">- the length of the list
		/// </param>
		/// <param name="tag">- morpheme tag ID
		/// </param>
		/// <param name="phoneme">- phoneme	
		/// </param>
		/// <param name="nextPosition">- the index of the next morpheme
		/// </param>
		/// <param name="nextTagType">- the type of the next morpheme tag
		/// </param>
		/// <param name="str">- plain string
		/// </param>
		/// <returns> true: the morpheme is in the chart, false: not exist
		/// </returns>
		public virtual bool checkChart(int[] morpheme, int morphemeLen, int tag, int phoneme, int nextPosition, int nextTagType, System.String str)
		{
			for (int i = 0; i < morphemeLen; i++)
			{
				Morpheme morph = chart[morpheme[i]];
				if (morph.tag == tag && morph.phoneme == phoneme && morph.nextPosition == nextPosition && morph.nextTagType == nextTagType && morph.str.Equals(str))
				{
					return true;
				}
			}
			return false;
		}
		
		/// <summary> Generates the morphological analysis result based on the morpheme chart where the analysis is performed.</summary>
		public virtual void  getResult()
		{
			
			printResultCnt = 0;
			printChart(0);
		}
		
		/// <summary> Initializes the morpheme chart with the specified word.</summary>
		/// <param name="word">- the plain string of an eojeol to analyze
		/// </param>
		public virtual void  init(System.String word)
		{
			simti.init();
			word = preReplace(word);
			sp.init(Code.toTripleString(word), simti);
			
			chartEnd = 0;
			Position p = sp.getPosition(0);
			p.morpheme[p.morphCount++] = chartEnd;
			chart[chartEnd].tag = tagSet.iwgTag;
			chart[chartEnd].phoneme = 0;
			chart[chartEnd].nextPosition = 1;
			chart[chartEnd].nextTagType = 0;
			chart[chartEnd].state = MORPHEME_STATE_SUCCESS;
			chart[chartEnd].connectionCount = 0;
			chart[chartEnd].str = "";
			chartEnd++;
		}
		
		/// <summary> It expands the morpheme chart to deal with the phoneme change phenomenon.</summary>
		/// <param name="from">- the index of the start segment position
		/// </param>
		/// <param name="front">- the front part of the string
		/// </param>
		/// <param name="back">- the next part of the string
		/// </param>
		/// <param name="ftag">- the morpheme tag of the front part
		/// </param>
		/// <param name="btag">- the morpheme tag of the next part
		/// </param>
		/// <param name="phoneme">- phoneme
		/// </param>
		public virtual void  phonemeChange(int from, System.String front, System.String back, int ftag, int btag, int phoneme)
		{
			TNODE node = null;
			int size = 0;
			bool x, y;
			int next;
			int nc_idx;
			
			// searches the system dictionary for the front part
			node = systemDic.fetch(front.ToCharArray());
			if (node != null && node.info_list != null)
			{
				size = node.info_list.Count;
			}
			
			Position pos = sp.getPosition(from);
			
			for (int i = 0; i < size; i++)
			{
                INFO info = node.info_list.Get_Renamed(i);
				
				// comparison of the morpheme tag of the front part
				x = tagSet.checkTagType(ftag, info.tag);
				
				// comparison of the phoneme of the front part
				y = tagSet.checkPhonemeType(phoneme, info.phoneme);
				
				if (x && y)
				{
					next = altSegment(back);
					
					if (checkChart(pos.morpheme, pos.morphCount, info.tag, info.phoneme, next, btag, front) == false)
					{
						nc_idx = addMorpheme(info.tag, info.phoneme, next, btag);
						chart[nc_idx].str = front;
						pos.morpheme[pos.morphCount++] = nc_idx;
					}
					else
					{
						System.Console.Error.WriteLine("phonemeChange: exit");
						System.Environment.Exit(0);
					}
				}
			}
		}
		
		/// <summary> It generates the final mophological analysis result from the morpheme chart.</summary>
		/// <param name="chartIndex">- the start index of the chart to generate final result
		/// </param>
		private void  printChart(int chartIndex)
		{
			int i;
			Morpheme morph = chart[chartIndex];
			int engCnt = 0;
			int chiCnt = 0;
			
			if (chartIndex == 0)
			{
				for (i = 0; i < morph.connectionCount; i++)
				{
					resMorphemes.Clear();
					resTags.Clear();
					printChart(morph.connection[i]);
				}
			}
			else
			{
				System.String morphStr = Code.toString(morph.str.ToCharArray());
				int idx = 0;
				engCnt = 0;
				chiCnt = 0;
				while (idx != - 1)
				{
					if ((idx = morphStr.IndexOf(ENG_REPLACE)) != - 1)
					{
						engCnt++;
                        morphStr = morphStr.ReplaceFirst(ENG_REPLACE, engReplacementList.Get_Renamed(engReplaceIndex++));
					}
					else if ((idx = morphStr.IndexOf(CHI_REPLACE)) != - 1)
					{
						chiCnt++;
                        morphStr = morphStr.ReplaceFirst(CHI_REPLACE, chiReplacementList.Get_Renamed(chiReplaceIndex++));
					}
				}
				
				resMorphemes.Add(morphStr);
                resTags.Add(tagSet.getTagName(morph.tag));
				
				for (i = 0; i < morph.connectionCount && printResultCnt < MAX_CANDIDATE_NUM; i++)
				{
					if (morph.connection[i] == 0)
					{
						System.String[] mArray = resMorphemes.ToArray();
						System.String[] tArray = resTags.ToArray();
                        resEojeols.AddLast(new Eojeol(mArray, tArray));
						
						printResultCnt++;
					}
					else
					{
						printChart(morph.connection[i]);
					}
				}
				
				resMorphemes.RemoveAt(resMorphemes.Count - 1);
                resTags.RemoveAt(resTags.Count - 1);
				if (engCnt > 0)
				{
					engReplaceIndex -= engCnt;
				}
				if (chiCnt > 0)
				{
					chiReplaceIndex -= chiCnt;
				}
			}
		}
		
		/// <summary> It prints the all data in the chart to the console.</summary>
		public virtual void  printMorphemeAll()
		{
			System.Console.Error.WriteLine("chartEnd: " + chartEnd);
			for (int i = 0; i < chartEnd; i++)
			{
				System.Console.Error.WriteLine("chartID: " + i);
                Trace.Write(
                    string.Format("{0}/{1}.{2} nextPosition={3} nextTagType={4} state={5} ", Code.toString(chart[i].str.ToCharArray()), tagSet.getTagName(chart[i].tag), tagSet.getIrregularName(chart[i].phoneme), Code.toCompatibilityJamo(sp.getPosition(chart[i].nextPosition).key), tagSet.getTagName(chart[i].nextTagType), chart[i].state));

				System.Console.Error.Write("connection=");
				for (int j = 0; j < chart[i].connectionCount; j++)
				{
                    Trace.Write(chart[i].connection[j] + ", ");
				}
                Trace.Write(Environment.NewLine);
			}
		}
		
		/// <summary> Replaces the English alphabets and Chinese characters in the specified string with the reserved words.</summary>
		/// <param name="str">- the string to replace English and Chinese characters
		/// </param>
		/// <returns> the string in which English and Chinese characters were replace with the reserved words
		/// </returns>
		private System.String preReplace(System.String str)
		{
			System.String result = "";
			bool engFlag = false;
			bool chiFlag = false;
			System.String buf = "";
			
			engReplacementList.Clear();
			chiReplacementList.Clear();
			engReplaceIndex = 0;
			chiReplaceIndex = 0;
			
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				
				if (((c >= 'a' && c <= 'z') || c >= 'A' && c <= 'Z'))
				{
					/* English Alphabets */
					if (engFlag)
					{
						buf += c;
					}
					else
					{
						if (engFlag)
						{
							engFlag = false;
							engReplacementList.AddLast(buf);
							buf = "";
						}
						result += ENG_REPLACE;
						buf += c;
						engFlag = true;
					}
				}
				else if (((c >= 0x2E80 && c <= 0x2EFF) || (c >= 0x3400 && c <= 0x4DBF)) || (c >= 0x4E00 && c < 0x9FBF) || (c >= 0xF900 && c <= 0xFAFF) && chiFlag)
				{
					/* Chinese Characters */
					if (chiFlag)
					{
						buf += c;
					}
					else
					{
						if (chiFlag)
						{
							chiFlag = false;
                            chiReplacementList.AddLast(buf);
							buf = "";
						}
						result += CHI_REPLACE;
						buf += c;
						chiFlag = true;
					}
				}
				else
				{
					result += c;
					if (engFlag)
					{
						engFlag = false;
                        engReplacementList.AddLast(buf);
						buf = "";
					}
					if (chiFlag)
					{
						chiFlag = false;
                        chiReplacementList.AddLast(buf);
						buf = "";
					}
				}
			}
			if (engFlag)
			{
                engReplacementList.AddLast(buf);
			}
			if (chiFlag)
			{
                chiReplacementList.AddLast(buf);
			}
			return result;
		}
	}
}