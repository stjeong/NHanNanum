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
using kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor;
using Spring.Threading.Collections.Generic;
using PlainSentence = kr.ac.kaist.swrc.jhannanum.comm.PlainSentence;
namespace kr.ac.kaist.swrc.jhannanum.thread
{

    /// <summary> This class makes the plain text processor plug-in to run on a thread.</summary>
    /// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
    /// </author>
    public class PlainTextProcThread : SupportClass.ThreadClass
    {
        /// <summary>the plain text processor plug-in </summary>
        private kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.PlainTextProcessor plainTextProcessor = null;

        /// <summary>input queue </summary>
        private LinkedBlockingQueue<PlainSentence> in_Renamed;

        /// <summary>output queue </summary>
        private LinkedBlockingQueue<PlainSentence> out_Renamed;

        /// <summary> Constructor.</summary>
        /// <param name="plainTextProcessor">- the plain text processor plug-in
        /// </param>
        /// <param name="in">- input queue
        /// </param>
        /// <param name="out">- output queue
        /// </param>
        public PlainTextProcThread(PlainTextProcessor plainTextProcessor, LinkedBlockingQueue<PlainSentence> in_Renamed,
            LinkedBlockingQueue<PlainSentence> out_Renamed)
        {
            this.plainTextProcessor = plainTextProcessor;
            this.in_Renamed = in_Renamed;
            this.out_Renamed = out_Renamed;
        }

        override public void Run()
        {
            PlainSentence ps = null;

            try
            {
                while (true)
                {
                    ps = in_Renamed.Take();

                    if ((ps = plainTextProcessor.doProcess(ps)) != null)
                    {
                        out_Renamed.Add(ps);
                    }

                    while (plainTextProcessor.hasRemainingData())
                    {
                        if ((ps = plainTextProcessor.doProcess(null)) != null)
                        {
                            out_Renamed.Add(ps);
                        }
                    }

                    if ((ps = plainTextProcessor.flush()) != null)
                    {
                        out_Renamed.Add(ps);
                    }
                }
            }
            catch (System.Threading.ThreadInterruptedException e)
            {
                plainTextProcessor.shutdown();
            }
        }
    }
}