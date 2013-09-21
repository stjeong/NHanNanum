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
using HMMTagger = kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.PosTagger.HmmPosTagger.HMMTagger;
using UnknownProcessor = kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.MorphemeProcessor.UnknownMorphProcessor.UnknownProcessor;
namespace kr.ac.kaist.swrc.jhannanum.hannanum
{
	
	/// <summary> This class makes it easy to create a HanNanum work flow for general use. There are several
	/// predefined work flows so users who don't want to set up the work flow manually can use those
	/// general work flows: Morphological Analysis, POS Tagging, and Noun Extraction.<br>
	/// <br>
	/// HanNanum work flow can be used through the following steps with the WorkflowFactory:<br>
	/// <br>
	/// 1. Create the work flow WorkflowFactory.<br>
	/// 2. Activate the work flow in the multi-thread mode or the single thread mode.<br>
	/// 3. Analyze the target text with the work flow.<br>
	/// 4. Get the result with some relevant data type or string representation.<br>
	/// 5. Repeat the step 4-5 you need it.<br>
	/// 6. Close the work flow when it will not be used anymore.<br>
	/// <br>
	/// Take a look the demo program - kr.ac.kaist.swrc.jhannanum.demo.PredefinedWorkflow for an example.<br>
	/// <br>
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class WorkflowFactory
	{
		/// <summary> Predefined work flow for POS tagging.</summary>
		public const int WORKFLOW_HMM_POS_TAGGER = 0x01;
		
		/// <summary> Predefined work flow for morphological analysis.</summary>
		public const int WORKFLOW_MORPH_ANALYZER = 0x02;
		
		/// <summary> Predefined work flow for noun extraction.</summary>
		public const int WORKFLOW_NOUN_EXTRACTOR = 0x03;
		
		/// <summary> Predefined work flow for simple POS tagging with 22 tags.</summary>
		public const int WORKFLOW_POS_SIMPLE_22 = 0x04;
		
		/// <summary> Predefined work flow for simple POS tagging with 9 tags.</summary>
		public const int WORKFLOW_POS_SIMPLE_09 = 0x05;
		
		/// <summary> Returns the predefined work flow according to the specified flag.</summary>
		/// <param name="workflowFlag">- One of WORKFLOW_HMM_TAGGER, WORKFLOW_MORPH_ONLY, WORKFLOW_NOUN_EXTRACT
		/// </param>
		/// <returns> the instance of predefined work flow
		/// </returns>
		public static Workflow getPredefinedWorkflow(int workflowFlag)
		{
			Workflow workflow = new Workflow();
			
			switch (workflowFlag)
			{
				
				case WORKFLOW_HMM_POS_TAGGER: 
					workflow.appendPlainTextProcessor(new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.SentenceSegmentor.SentenceSegmentor(), null);
					workflow.appendPlainTextProcessor(new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.InformalSentenceFilter.InformalSentenceFilter(), null);
					
					workflow.setMorphAnalyzer(new kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.ChartMorphAnalyzer.ChartMorphAnalyzer(), "conf/plugin/MajorPlugin/MorphAnalyzer/ChartMorphAnalyzer.json");
					workflow.appendMorphemeProcessor(new UnknownProcessor(), null);
					
					workflow.setPosTagger(new HMMTagger(), "conf/plugin/MajorPlugin/PosTagger/HmmPosTagger.json");
					break;
				
				case WORKFLOW_MORPH_ANALYZER: 
					workflow.appendPlainTextProcessor(new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.SentenceSegmentor.SentenceSegmentor(), null);
					workflow.appendPlainTextProcessor(new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.InformalSentenceFilter.InformalSentenceFilter(), null);
					
					workflow.setMorphAnalyzer(new kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.ChartMorphAnalyzer.ChartMorphAnalyzer(), "conf/plugin/MajorPlugin/MorphAnalyzer/ChartMorphAnalyzer.json");
					workflow.appendMorphemeProcessor(new UnknownProcessor(), null);
					break;
				
				case WORKFLOW_NOUN_EXTRACTOR: 
					workflow.appendPlainTextProcessor(new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.SentenceSegmentor.SentenceSegmentor(), null);
					workflow.appendPlainTextProcessor(new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.InformalSentenceFilter.InformalSentenceFilter(), null);
					
					workflow.setMorphAnalyzer(new kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.ChartMorphAnalyzer.ChartMorphAnalyzer(), "conf/plugin/MajorPlugin/MorphAnalyzer/ChartMorphAnalyzer.json");
					workflow.appendMorphemeProcessor(new UnknownProcessor(), null);
					
					workflow.setPosTagger(new HMMTagger(), "conf/plugin/MajorPlugin/PosTagger/HmmPosTagger.json");
					workflow.appendPosProcessor(new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PosProcessor.NounExtractor.NounExtractor(), null);
					break;
				
				case WORKFLOW_POS_SIMPLE_22: 
					workflow.appendPlainTextProcessor(new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.SentenceSegmentor.SentenceSegmentor(), null);
					workflow.appendPlainTextProcessor(new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.InformalSentenceFilter.InformalSentenceFilter(), null);
					
					workflow.setMorphAnalyzer(new kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.ChartMorphAnalyzer.ChartMorphAnalyzer(), "conf/plugin/MajorPlugin/MorphAnalyzer/ChartMorphAnalyzer.json");
					workflow.appendMorphemeProcessor(new UnknownProcessor(), null);
					
					workflow.setPosTagger(new HMMTagger(), "conf/plugin/MajorPlugin/PosTagger/HmmPosTagger.json");
					workflow.appendPosProcessor(new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PosProcessor.SimplePOSResult22.SimplePOSResult22(), null);
					break;
				
				case WORKFLOW_POS_SIMPLE_09: 
					workflow.appendPlainTextProcessor(new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.SentenceSegmentor.SentenceSegmentor(), null);
					workflow.appendPlainTextProcessor(new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.InformalSentenceFilter.InformalSentenceFilter(), null);
					
					workflow.setMorphAnalyzer(new kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.ChartMorphAnalyzer.ChartMorphAnalyzer(), "conf/plugin/MajorPlugin/MorphAnalyzer/ChartMorphAnalyzer.json");
					workflow.appendMorphemeProcessor(new UnknownProcessor(), null);
					
					workflow.setPosTagger(new HMMTagger(), "conf/plugin/MajorPlugin/PosTagger/HmmPosTagger.json");
					workflow.appendPosProcessor(new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PosProcessor.SimplePOSResult09.SimplePOSResult09(), null);
					break;
				}
			
			return workflow;
		}
	}
}