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
using kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.PosTagger;
using Spring.Threading.Collections.Generic;
using Sentence = kr.ac.kaist.swrc.jhannanum.comm.Sentence;
using SetOfSentences = kr.ac.kaist.swrc.jhannanum.comm.SetOfSentences;
namespace kr.ac.kaist.swrc.jhannanum.thread
{

    /// <summary> This class makes the POS tagger plug-in to run on a thread.</summary>
    /// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
    /// </author>
    public class PosTaggerThread : SupportClass.ThreadClass
    {
        /// <summary>the POS tagger plug-in </summary>
        private kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.PosTagger.PosTagger tagger = null;

        /// <summary>input queue </summary>
        private LinkedBlockingQueue<SetOfSentences> in_Renamed;

        /// <summary>output queue </summary>
        private LinkedBlockingQueue<Sentence> out_Renamed;

        /// <summary> Constructor.</summary>
        /// <param name="tagger">- the POS tagger plug-in
        /// </param>
        /// <param name="in">- input queue
        /// </param>
        /// <param name="out">- output queue
        /// </param>
        public PosTaggerThread(PosTagger tagger, LinkedBlockingQueue<SetOfSentences> in_Renamed,
        LinkedBlockingQueue<Sentence> out_Renamed)
        {
            this.tagger = tagger;
            this.in_Renamed = in_Renamed;
            this.out_Renamed = out_Renamed;
        }

        override public void Run()
        {
            SetOfSentences sos = null;
            Sentence sent = null;

            try
            {
                while (true)
                {
                    sos = in_Renamed.Take();

                    if ((sent = tagger.tagPOS(sos)) != null)
                    {
                        out_Renamed.Add(sent);
                    }
                }
            }
            catch (System.Threading.ThreadInterruptedException e)
            {
                tagger.shutdown();
            }
        }
    }
}