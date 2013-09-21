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
using Sentence = kr.ac.kaist.swrc.jhannanum.comm.Sentence;
using SetOfSentences = kr.ac.kaist.swrc.jhannanum.comm.SetOfSentences;
using JSONReader = kr.ac.kaist.swrc.jhannanum.share.JSONReader;
using System.Collections.Generic;
namespace kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.PosTagger.HmmPosTagger
{
	
	/// <summary> Hidden Markov Model based Part Of Speech Tagger.
	/// 
	/// It is a POS Tagger plug-in which is a major plug-in of phase 3 in HanNanum work flow. It uses
	/// Hidden Markov Model regarding the features of Korean Eojeol to choose the most promising morphological
	/// analysis results of each eojeol for entire sentence.
	/// 
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class HMMTagger : kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.PosTagger.PosTagger
	{
		/// <summary> Node for the markov model.</summary>
		/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
		/// </author>
		private class MNode
		{
			public MNode(HMMTagger enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(HMMTagger enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private HMMTagger enclosingInstance;
			public HMMTagger Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
			}
			/// <summary>eojeol </summary>
			private Eojeol eojeol;
            public Eojeol Eojeol
            {
                get { return eojeol; }
                set { eojeol = value; }
            }

			/// <summary>eojeol tag </summary>
			private System.String wp_tag;
            public String Wp_Tag
            {
                get { return wp_tag; }
                set { wp_tag = value; }
            }

			/// <summary>the probability of this node - P(T, W) </summary>
			private double prob_wt;
            public double Prob_Wt
            {
                get { return prob_wt; }
                set { prob_wt = value; }
            }

			/// <summary>the accumulated probability from start to this node </summary>
            private double prob;
            public double Prob
            {
                get { return prob; }
                set { prob = value; }
            }

			/// <summary>back pointer for viterbi algorithm </summary>
			private int backptr;
            public int Backptr
            {
                get { return backptr; }
                set { backptr = value; }
            }

			/// <summary>the index for the next sibling </summary>
			private int sibling;
            public int Sibling
            {
                get { return sibling; }
                set { sibling = value; }
            }
		}
		
		/// <summary> Header of an eojeol.</summary>
		/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
		/// </author>
		private class WPhead
		{
			public WPhead(HMMTagger enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(HMMTagger enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private HMMTagger enclosingInstance;
			public HMMTagger Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			/// <summary>the index of the node for an eojeol </summary>
			private int mnode;
            public int MNode
            {
                get { return mnode; }
                set { mnode = value; }
            }
		}
		
		/// <summary>log 0.01 - smoothing factor </summary>
		private static double SF = - 4.60517018598809136803598290936873;
		
		/// <summary>the list of nodes for each eojeol </summary>
		private WPhead[] wp = null;
		
		/// <summary>the last index of eojeol list </summary>
		private int wp_end = 0;
		
		/// <summary>the nodes for the markov model </summary>
		private MNode[] mn = null;
		
		/// <summary>the last index of the markov model  </summary>
		private int mn_end = 0;
		
		/// <summary>for the probability P(W|T) </summary>
		private ProbabilityDBM pwt_pos_tf = null;
		
		/// <summary>for the probability P(T|T) </summary>
		private ProbabilityDBM ptt_pos_tf = null;
		
		/// <summary>for the probability P(T|T) for eojeols </summary>
		private ProbabilityDBM ptt_wp_tf = null;
		
		/// <summary>the statistic file for the probability P(T|W) for morphemes </summary>
		private System.String PWT_POS_TDBM_FILE;
		
		/// <summary>the statistic file for the probability P(T|T) for morphemes </summary>
		private System.String PTT_POS_TDBM_FILE;
		
		/// <summary>the statistic file for the probability P(T|T) for eojeols </summary>
		private System.String PTT_WP_TDBM_FILE;
		
		/// <summary>the default probability </summary>
		private const double PCONSTANT = - 20.0;
		
		/// <summary>lambda value </summary>
		private const double LAMBDA = 0.9;
		
		/// <summary>lambda 1 </summary>
		private static readonly double Lambda1 = LAMBDA;
		
		/// <summary>lambda 2 </summary>
		private static readonly double Lambda2 = 1.0 - LAMBDA;
		public virtual Sentence tagPOS(SetOfSentences sos)
		{
			int v = 0, prev_v = 0, w = 0;
            List<string> plainEojeolArray = sos.getPlainEojeolArray();
			List< Eojeol [] > eojeolSetArray = sos.getEojeolSetArray();
			
			// initialization
			reset();
			
            IEnumerator<string> plainEojeolIter = plainEojeolArray.GetEnumerator();
			foreach (Eojeol [] eojeolSet in eojeolSetArray)
			{
				System.String plainEojeol = null;
				if (plainEojeolIter.MoveNext())
				{
					plainEojeol = plainEojeolIter.Current;
				}
				else
				{
					break;
				}
				w = new_wp(plainEojeol);
				
				for (int i = 0; i < eojeolSet.Length; i++)
				{
					System.String now_tag;
					double probability;
					
					now_tag = PhraseTag.getPhraseTag(eojeolSet[i].Tags);
					probability = compute_wt(eojeolSet[i]);
					
					v = new_mnode(eojeolSet[i], now_tag, probability);
					if (i == 0)
					{
						wp[w].MNode = v;
						prev_v = v;
					}
					else
					{
						mn[prev_v].Sibling = v;
						prev_v = v;
					}
				}
			}
			
			// gets the final result by running viterbi
			return end_sentence(sos);
		}

		public virtual void  initialize(System.String baseDir, System.String configFile)
		{
			wp = new WPhead[5000];
			for (int i = 0; i < 5000; i++)
			{
				wp[i] = new WPhead(this);
			}
			wp_end = 1;
			
			mn = new MNode[10000];
			for (int i = 0; i < 10000; i++)
			{
				mn[i] = new MNode(this);
			}
			mn_end = 1;
			
			JSONReader json = new JSONReader(configFile);
			PWT_POS_TDBM_FILE = baseDir + "/" + json.getValue("pwt.pos");
			PTT_POS_TDBM_FILE = baseDir + "/" + json.getValue("ptt.pos");
			PTT_WP_TDBM_FILE = baseDir + "/" + json.getValue("ptt.wp");
			
			pwt_pos_tf = new ProbabilityDBM(PWT_POS_TDBM_FILE);
			ptt_wp_tf = new ProbabilityDBM(PTT_WP_TDBM_FILE);
			ptt_pos_tf = new ProbabilityDBM(PTT_POS_TDBM_FILE);
		}

		public virtual void  shutdown()
		{
			
		}
		
		/// <summary> Computes P(T_i, W_i) of the specified eojeol.</summary>
		/// <param name="eojeol">- the eojeol to compute the probability
		/// </param>
		/// <returns> P(T_i, W_i) of the specified eojeol
		/// </returns>
		private double compute_wt(Eojeol eojeol)
		{
			double current = 0.0, tbigram, tunigram, lexicon;
			
			System.String tag;
			System.String bitag;
			System.String oldtag;
			
			tag = eojeol.getTag(0);
			
			/* the probability of P(t1|t0) */
			bitag = "bnk-" + tag;
			
			double[] prob = null;
			
			if ((prob = ptt_pos_tf.get_Renamed(bitag)) != null)
			{
				/* current = P(t1|t0) */
				tbigram = prob[0];
			}
			else
			{
				/* current = P(t1|t0) = 0.01 */
				tbigram = PCONSTANT;
			}
			
			/* the probability of P(t1) */
			if ((prob = ptt_pos_tf.get_Renamed(tag)) != null)
			{
				/* current = P(t1) */
				tunigram = prob[0];
			}
			else
			{
				/* current = P(t1) = 0.01 */
				tunigram = PCONSTANT;
			}
			
			/* the probability of P(w|t) */
			if ((prob = pwt_pos_tf.get_Renamed(eojeol.getMorpheme(0) + "/" + tag)) != null)
			{
				/* current *= P(w|t1) */
				lexicon = prob[0];
			}
			else
			{
				/* current = P(w|t1) = 0.01 */
				lexicon = PCONSTANT;
			}
			
			/*                              
			* current = P(w|t1) * P(t1|t0) ~= P(w|t1) * (P(t1|t0))^Lambda1 * (P(t1))^Lambda2 (Lambda1 + Lambda2 = 1)
			*/
			//		current = lexicon + Lambda1*tbigram + Lambda2*tunigram;
			
			/* 
			* current = P(w|t1)/P(t1) * P(t1|t0)/P(t1)
			*/
			//		current = lexicon - tunigram + tbigram - tunigram;
			
			/* 
			* current = P(w|t1) * P(t1|t0)
			*/
			//		current = lexicon + tbigram ;
			
			/* 
			* current = P(w|t1) * P(t1|t0) / P(t1)
			*/
			current = lexicon + tbigram - tunigram;
			oldtag = tag;
			
			
			for (int i = 1; i < eojeol.length; i++)
			{
				tag = eojeol.getTag(i);
				
				/* P(t_i|t_i-1) */
				bitag = oldtag + "-" + tag;
				
				if ((prob = ptt_pos_tf.get_Renamed(bitag)) != null)
				{
					tbigram = prob[0];
				}
				else
				{
					tbigram = PCONSTANT;
				}
				
				/* P(w|t) */
				if ((prob = pwt_pos_tf.get_Renamed(eojeol.getMorpheme(i) + "/" + tag)) != null)
				{
					/* current *= P(w|t) */
					lexicon = prob[0];
				}
				else
				{
					lexicon = PCONSTANT;
				}
				
				/* P(t) */
				if ((prob = ptt_pos_tf.get_Renamed(tag)) != null)
				{
					/* current = P(t) */
					tunigram = prob[0];
				}
				else
				{
					/* current = P(t)=0.01 */
					tunigram = PCONSTANT;
				}
				
				//			current += lexicon - tunigram + tbigram - tunigram;
				//			current += lexicon + tbigram;
				current += lexicon + tbigram - tunigram;
				
				oldtag = tag;
			}
			
			/* the blank at the end of eojeol */
			bitag = tag + "-bnk";
			
			/* P(bnk|t_last) */
			if ((prob = ptt_pos_tf.get_Renamed(bitag)) != null)
			{
				tbigram = prob[0];
			}
			else
			{
				tbigram = PCONSTANT;
			}
			
			/* P(bnk) */
			if ((prob = ptt_pos_tf.get_Renamed("bnk")) != null)
			{
				tunigram = prob[0];
			}
			else
			{
				tunigram = PCONSTANT;
			}
			
			/* P(w|bnk) = 1, and ln(1) = 0 */
			//		current += 0 - tunigram + tbigram - tunigram;
			//		current += 0 + tbigram;
			current += 0 + tbigram - tunigram;
			
			return current;
		}
		
		/// <summary> Runs viterbi to get the final morphological analysis result which has the highest probability.</summary>
		/// <param name="sos">- all the candidates of morphological analysis
		/// </param>
		/// <returns> the final morphological analysis result which has the highest probability
		/// </returns>
		private Sentence end_sentence(SetOfSentences sos)
		{
			int i, j, k;
			
			/* Ceartes the last node */
			i = new_wp(" ");
            wp[i].MNode = new_mnode(null, "SF", 0);
			
			/* Runs viterbi */
			for (i = 1; i < wp_end - 1; i++)
			{
                for (j = wp[i].MNode; j != 0; j = mn[j].Sibling)
				{
                    for (k = wp[i + 1].MNode; k != 0; k = mn[k].Sibling)
					{
						update_prob_score(j, k);
					}
				}
			}
			
			i = sos.length;
			Eojeol[] eojeols = new Eojeol[i];
            for (k = wp[i].MNode; k != 0; k = mn[k].Backptr)
			{
				eojeols[--i] = mn[k].Eojeol;
			}
			
			return new Sentence(sos.DocumentID, sos.SentenceID, sos.EndOfDocument, sos.getPlainEojeolArray().ToArray(), eojeols);
		}
		
		/// <summary> Adds a new node for the markov model.</summary>
		/// <param name="eojeol">- the eojeol to add
		/// </param>
		/// <param name="wp_tag">- the eojeol tag
		/// </param>
		/// <param name="prob">- the probability P(w|t)
		/// </param>
		/// <returns> the index of the new node
		/// </returns>
		private int new_mnode(Eojeol eojeol, System.String wp_tag, double prob)
		{
            mn[mn_end].Eojeol = eojeol;
			mn[mn_end].Wp_Tag = wp_tag;
			mn[mn_end].Prob_Wt = prob;
            mn[mn_end].Backptr = 0;
            mn[mn_end].Sibling = 0;
			return mn_end++;
		}
		
		/// <summary> Adds a new header of an eojeol.</summary>
		/// <param name="str">- the plain string of the eojeol
		/// </param>
		/// <returns> the index of the new header
		/// </returns>
		private int new_wp(System.String str)
		{
            wp[wp_end].MNode = 0;
			return wp_end++;
		}
		
		/// <summary> Resets the model.</summary>
		private void  reset()
		{
			wp_end = 1;
			mn_end = 1;
		}
		
		/// <summary> Updates the probability regarding the transition between two eojeols.</summary>
		/// <param name="from">- the previous eojeol
		/// </param>
		/// <param name="to">- the current eojeol
		/// </param>
		private void  update_prob_score(int from, int to)
		{
			double PTT;
			double[] prob = null;
			double P;
			
			/* the traisition probability P(T_i,T_i-1) */
            prob = ptt_wp_tf.get_Renamed(mn[from].Wp_Tag + "-" + mn[to].Wp_Tag);
			
			if (prob == null)
			{
				/* ln(0.01). Smoothing Factor */
				PTT = SF;
			}
			else
			{
				PTT = prob[0];
			}
			
			/* P(T_i,T_i-1) / P(T_i) */
			prob = ptt_wp_tf.get_Renamed(mn[to].Wp_Tag);
			
			if (prob != null)
			{
				PTT -= prob[0];
			}
			
			/* P(T_i,T_i-1) / (P(T_i) * P(T_i-1)) */
			//		prob = ptt_wp_tf.get(mn[from].wp_tag);
			//		
			//		if (prob != null) {
			//			PTT -= prob[0];
			//		}
			
			if (mn[from].Backptr == 0)
			{
                mn[from].Prob = mn[from].Prob_Wt;
			}
			
			/* 
			* P = the accumulated probability to the previous eojeol * transition probability * the probability of current eojeol
			* PTT = P(T_i|T_i-1) / P(T_i)
			* mn[to].prob_wt = P(T_i, W_i)
			*/
            P = mn[from].Prob + PTT + mn[to].Prob_Wt;
			
			// for debugging
			//		System.out.format("P:%f\t%s(%d:%s):%f -> %f -> %s(%d:%s):%f\n", P, mn[from].eojeol, 
			//				from, mn[from].wp_tag, mn[from].prob, PTT, 
			//				mn[to].eojeol, to, mn[to].wp_tag, mn[to].prob_wt );

            if (mn[to].Backptr == 0 || P > mn[to].Prob)
			{
                mn[to].Backptr = from;
				mn[to].Prob = P;
			}
		}
	}
}