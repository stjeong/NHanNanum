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
using PlainSentence = kr.ac.kaist.swrc.jhannanum.comm.PlainSentence;
using SetOfSentences = kr.ac.kaist.swrc.jhannanum.comm.SetOfSentences;
using JSONReader = kr.ac.kaist.swrc.jhannanum.share.JSONReader;
using TagSet = kr.ac.kaist.swrc.jhannanum.share.TagSet;
using System.Collections.Generic;
using PetShop.Data;
namespace kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.ChartMorphAnalyzer
{
	
	/// <summary> Chart-based Morphological Analyzer.
	/// 
	/// It is a morphological analyzer plug-in which is a major plug-in of phase 2 in HanNanum work flow.
	/// This uses the lattice-style chart as a internal data structure, which makes it possible to
	/// do morphological analysis without back tracking.
	/// 
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class ChartMorphAnalyzer : kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.MorphAnalyzer
	{
		/// <summary> Returns the name of the morphological analysis plug-in.</summary>
		/// <returns> the name of the morphological analysis plug-in.
		/// </returns>
		virtual public System.String Name
		{
			get
			{
				return PLUG_IN_NAME;
			}
			
		}
		/// <summary>Name of this plug-in. </summary>
		private const System.String PLUG_IN_NAME = "MorphAnalyzer";
		
		/// <summary>Pre-analyzed dictionary. </summary>
		private AnalyzedDic analyzedDic = null;
		
		/// <summary>Default morpheme dictionary. </summary>
		private Trie systemDic = null;
		
		/// <summary>Additional morpheme dictionary that users can modify for their own purpose. </summary>
		private Trie userDic = null;
		
		/// <summary>Number dictionary, which is actually a automata. </summary>
		private NumberDic numDic = null;
		
		/// <summary>Morpheme tag set </summary>
		private TagSet tagSet = null;
		
		/// <summary>Connection rules between morphemes. </summary>
		private Connection connection = null;
		
		/// <summary>Impossible connection rules. </summary>
		private ConnectionNot connectionNot = null;
		
		/// <summary>Lattice-style morpheme chart. </summary>
		private MorphemeChart chart = null;
		
		/// <summary>SIMTI structure for reverse segment position. </summary>
		private Simti simti = null;
		
		/// <summary>The file path for the impossible connection rules. </summary>
		private System.String fileConnectionsNot = "";
		
		/// <summary>The file path for the connection rules. </summary>
		private System.String fileConnections = "";
		
		/// <summary>The file path for the pre-analyzed dictionary. </summary>
		private System.String fileDicAnalyzed = "";
		
		/// <summary>The file path for the default morpheme dictionary. </summary>
		private System.String fileDicSystem = "";
		
		/// <summary>The file path for the user morpheme dictionary. </summary>
		private System.String fileDicUser = "";
		
		/// <summary>The file path for the tag set. </summary>
		private System.String fileTagSet = "";
		
		/// <summary>Eojeol list </summary>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private LinkedList < Eojeol > eojeolList = null;
		
		/// <summary>Post-processor to deal with some exceptions </summary>
		private PostProcessor postProc = null;
		
		/// <summary> It processes the input plain eojeol by analyzing it or searching the pre-analyzed dictionary.</summary>
		/// <param name="plainEojeol">- plain eojeol to analyze
		/// </param>
		/// <returns> the morphologically analyzed eojeol list
		/// </returns>
		private Eojeol[] processEojeol(System.String plainEojeol)
		{
			System.String analysis = analyzedDic.get_Renamed(plainEojeol);
			
			eojeolList.Clear();
			
			if (analysis != null)
			{
				// the eojeol was registered in the pre-analyzed dictionary
                StringTokenizer st = new StringTokenizer(analysis, "^");
				while (st.HasMoreTokens)
				{
					System.String analyzed = st.NextToken;
                    System.String[] tokens = analyzed.Split("\\+|/");
					
					System.String[] morphemes = new System.String[tokens.Length / 2];
					System.String[] tags = new System.String[tokens.Length / 2];
					
					for (int i = 0, j = 0; i < morphemes.Length; i++)
					{
						morphemes[i] = tokens[j++];
						tags[i] = tokens[j++];
					}
					Eojeol eojeol = new Eojeol(morphemes, tags);
                    eojeolList.AddLast(eojeol);
				}
			}
			else
			{
				// analyze the input plain eojeol
				chart.init(plainEojeol);
				chart.analyze();
				chart.getResult();
			}
			
			return eojeolList.ToArray();
		}
		
		/// <summary> Analyzes the specified plain sentence, and returns all the possible analysis results.</summary>
		/// <returns> all the possible morphological analysis results
		/// </returns>
		public virtual SetOfSentences morphAnalyze(PlainSentence ps)
		{
            StringTokenizer st = new StringTokenizer(ps.Sentence, " \t");
			
			System.String plainEojeol = null;
			int eojeolNum = st.Count;
			
			List< String > plainEojeolArray = new List< String >(eojeolNum);
			List< Eojeol [] > eojeolSetArray = new List< Eojeol [] >(eojeolNum);
			
			while (st.HasMoreTokens)
			{
				plainEojeol = st.NextToken;
				
				plainEojeolArray.Add(plainEojeol);
				eojeolSetArray.Add(processEojeol(plainEojeol));
			}
			
			SetOfSentences sos = new SetOfSentences(ps.DocumentID, ps.SentenceID, ps.EndOfDocument, plainEojeolArray, eojeolSetArray);
			
			sos = postProc.doPostProcessing(sos);
			
			return sos;
		}
		
		/// <summary> Initializes the Chart-based Morphological Analyzer plug-in.</summary>
		/// <param name="baseDir">- the path for base directory, which should have the 'conf' and 'data' directory
		/// </param>
		/// <param name="configFile">- the path for the configuration file (relative path to the base directory)
		/// </param>
		public virtual void  initialize(System.String baseDir, System.String configFile)
		{
			JSONReader json = new JSONReader(configFile);
			
			fileDicSystem = baseDir + "/" + json.getValue("dic_system");
			fileDicUser = baseDir + "/" + json.getValue("dic_user");
			fileConnections = baseDir + "/" + json.getValue("connections");
			fileConnectionsNot = baseDir + "/" + json.getValue("connections_not");
			fileDicAnalyzed = baseDir + "/" + json.getValue("dic_analyzed");
			fileTagSet = baseDir + "/" + json.getValue("tagset");
			
			tagSet = new TagSet();
			tagSet.init(fileTagSet, TagSet.TAG_SET_KAIST);
			
			connection = new Connection();
			connection.init(fileConnections, tagSet.TagCount, tagSet);
			
			connectionNot = new ConnectionNot();
			connectionNot.init(fileConnectionsNot, tagSet);
			
			analyzedDic = new AnalyzedDic();
			analyzedDic.readDic(fileDicAnalyzed);
			
			systemDic = new Trie(Trie.DEFAULT_TRIE_BUF_SIZE_SYS);
			systemDic.read_dic(fileDicSystem, tagSet);
			
			userDic = new Trie(Trie.DEFAULT_TRIE_BUF_SIZE_USER);
			userDic.read_dic(fileDicUser, tagSet);
			
			numDic = new NumberDic();
			simti = new Simti();
			simti.init();
			eojeolList = new LinkedList < Eojeol >();
			
			chart = new MorphemeChart(tagSet, connection, systemDic, userDic, numDic, simti, eojeolList);
			
			postProc = new PostProcessor();
		}
		
		/// <summary> It is called right before the work flow ends.</summary>
		public virtual void  shutdown()
		{
		}
	}
}