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
using kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PosProcessor;
using Spring.Threading.Collections.Generic;
using Sentence = kr.ac.kaist.swrc.jhannanum.comm.Sentence;
namespace kr.ac.kaist.swrc.jhannanum.thread
{

    /// <summary> This class makes the POS processor plug-in to run on a thread.</summary>
    /// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
    /// </author>
    public class PosProcThread : SupportClass.ThreadClass
    {
        public PosProcThread()
        {
        }
        /// <summary>the POS processor plug-in </summary>
        private kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PosProcessor.PosProcessor posProcessor = null;

        /// <summary>input queue </summary>
        private LinkedBlockingQueue<Sentence> in_Renamed;

        /// <summary>output queue </summary>
        private LinkedBlockingQueue<Sentence> out_Renamed;

        /// <summary> Constructor.</summary>
        /// <param name="posProcessor">- the POS processor plug-in
        /// </param>
        /// <param name="in">- input queue
        /// </param>
        /// <param name="out">- output queue
        /// </param>
        public PosProcThread(PosProcessor posProcessor, LinkedBlockingQueue<Sentence> in_Renamed, LinkedBlockingQueue<Sentence> out_Renamed)
        {
            this.posProcessor = posProcessor;
            this.in_Renamed = in_Renamed;
            this.out_Renamed = out_Renamed;
        }

        override public void Run()
        {
            Sentence sent = null;

            try
            {
                while (true)
                {
                    sent = in_Renamed.Take();

                    if ((sent = posProcessor.doProcess(sent)) != null)
                    {
                        out_Renamed.Add(sent);
                    }
                }
            }
            catch (System.Threading.ThreadInterruptedException e)
            {
                posProcessor.shutdown();
            }
        }
    }
}