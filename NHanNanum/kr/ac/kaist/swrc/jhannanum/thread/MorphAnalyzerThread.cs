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
using kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer;
using Spring.Threading.Collections.Generic;
using PlainSentence = kr.ac.kaist.swrc.jhannanum.comm.PlainSentence;
using SetOfSentences = kr.ac.kaist.swrc.jhannanum.comm.SetOfSentences;
namespace kr.ac.kaist.swrc.jhannanum.thread
{

    /// <summary> This class makes the morphological analyzer plug-in to run on a thread.</summary>
    /// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
    /// </author>
    public class MorphAnalyzerThread : SupportClass.ThreadClass
    {
        /// <summary>the morphological analyzer plug-in </summary>
        private kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.MorphAnalyzer ma = null;

        /// <summary>input queue </summary>
        private LinkedBlockingQueue<PlainSentence> in_Renamed;

        /// <summary>output queue </summary>
        private LinkedBlockingQueue<SetOfSentences> out_Renamed;

        /// <summary> Constructor.</summary>
        /// <param name="ma">- the morphological analyzer plug-in
        /// </param>
        /// <param name="in">- input queue
        /// </param>
        /// <param name="out">- output queue
        /// </param>
        public MorphAnalyzerThread(MorphAnalyzer ma, LinkedBlockingQueue<PlainSentence> in_Renamed,
            LinkedBlockingQueue<SetOfSentences> out_Renamed)
        {
            this.ma = ma;
            this.in_Renamed = in_Renamed;
            this.out_Renamed = out_Renamed;
        }

        override public void Run()
        {
            PlainSentence ps = null;
            SetOfSentences sos = null;

            try
            {
                while (true)
                {
                    ps = in_Renamed.Take();

                    if ((sos = ma.morphAnalyze(ps)) != null)
                    {
                        out_Renamed.Add(sos);
                    }
                }
            }
            catch (System.Threading.ThreadInterruptedException e)
            {
                ma.shutdown();
            }
        }
    }
}