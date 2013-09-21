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

using PlainSentence = kr.ac.kaist.swrc.jhannanum.comm.PlainSentence;
using Sentence = kr.ac.kaist.swrc.jhannanum.comm.Sentence;
using SetOfSentences = kr.ac.kaist.swrc.jhannanum.comm.SetOfSentences;
using ResultTypeException = kr.ac.kaist.swrc.jhannanum.exception.ResultTypeException;
using MorphAnalyzerThread = kr.ac.kaist.swrc.jhannanum.thread.MorphAnalyzerThread;
using MorphemeProcThread = kr.ac.kaist.swrc.jhannanum.thread.MorphemeProcThread;
using PlainTextProcThread = kr.ac.kaist.swrc.jhannanum.thread.PlainTextProcThread;
using PosProcThread = kr.ac.kaist.swrc.jhannanum.thread.PosProcThread;
using PosTaggerThread = kr.ac.kaist.swrc.jhannanum.thread.PosTaggerThread;
using Spring.Threading.Collections.Generic;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using kr.ac.kaist.swrc.jhannanum.comm;
namespace kr.ac.kaist.swrc.jhannanum.hannanum
{

    /// <summary> This class is for the HanNanum work flow, which can be set up with more than one
    /// HanNanum plug-in. The work flow can be used with the following steps:<br>
    /// <br>
    /// 1. Create the work flow using one of the constructors with suitable configurations.<br>
    /// 2. Set the plug-ins up on the work flow regarding the purpose of analysis and the characteristics of input.<br>
    /// 3. Activate the work flow in the multi-thread mode or the single thread mode.<br>
    /// 4. Analyze the target text with the work flow.<br>
    /// 5. Get the result with some relevant data type or string representation.<br>
    /// 6. Repeat the step 4-5 you need it.<br>
    /// 7. Close the work flow when it will not be used anymore.<br>
    /// <br>
    /// Take a look the demo program - kr.ac.kaist.swrc.jhannanum.demo.WorkflowWithHMMTagger for an example.<br>
    /// <br>
    /// </summary>
    /// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, Kaist
    /// </author>
    public class Workflow
    {
        virtual public System.String ResultOfSentence
        {
            get
            {
                System.String res = null;

                try
                {
                    switch (outputPhaseNum)
                    {
                        case 1:
                            LinkedBlockingQueue<PlainSentence> out1 = queuePhase1[outputQueueNum];
                            res = out1.Take().ToString();
                            break;
                        case 2:
                            LinkedBlockingQueue<SetOfSentences> out2 = queuePhase2[outputQueueNum];
                            res = out2.Take().ToString();
                            break;
                        case 3:
                            LinkedBlockingQueue<Sentence> out3 = queuePhase3[outputQueueNum];
                            res = out3.Take().ToString();
                            break;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }

                return res;
            }

        }
        /// <summary> Returns the analysis result list for all sentence in the result. When you use this method,
        /// you need to pay attention on the size of the data. If the size of data is big, it may show
        /// lower performance than using getResultOfSentence() repeatedly.
        /// 
        /// It returns the sting representation of the result. If you want to reuse the result, the string should be
        /// parsed, which requires extra program codes and causes overhead. To solve this problem, consider to use 
        /// "<T> LinkedList<T> getResultOfDocument(T a)" instead.
        /// 
        /// </summary>
        /// <returns> the list of the analysis result for all sentences in the document
        /// </returns>
        /// <throws>  ResultTypeException </throws>
        virtual public System.String ResultOfDocument
        {
            get
            {
                System.Text.StringBuilder buf = new System.Text.StringBuilder();

                try
                {
                    switch (outputPhaseNum)
                    {
                        case 1:
                            LinkedBlockingQueue<PlainSentence> out1 = queuePhase1[outputQueueNum];
                            while (true)
                            {
                                PlainSentence ps = out1.Take();
                                buf.Append(ps);
                                buf.Append('\n');

                                if (ps.EndOfDocument)
                                {
                                    break;
                                }
                            }
                            break;
                        case 2:
                            LinkedBlockingQueue<SetOfSentences> out2 = queuePhase2[outputQueueNum];
                            while (true)
                            {
                                SetOfSentences sos = out2.Take();
                                buf.Append(sos);
                                buf.Append('\n');

                                if (sos.EndOfDocument)
                                {
                                    break;
                                }
                            }
                            break;
                        case 3:
                            LinkedBlockingQueue<Sentence> out3 = queuePhase3[outputQueueNum];
                            while (true)
                            {
                                Sentence sent = out3.Take();
                                buf.Append(sent);
                                buf.Append('\n');

                                if (sent.EndOfDocument)
                                {
                                    break;
                                }
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }

                return buf.ToString();
            }

        }
        /// <summary> The default value for the maximum number of the supplement plug-ins on each phase.</summary>
        public static int MAX_SUPPLEMENT_PLUGIN_NUM = 8;

        /// <summary> The maximum number of the supplement plug-ins of each phase.</summary>
        private int maxSupplementPluginNum = 0;

        /// <summary> The flag for the thread mode. true: multi-thread mode, false: single-thread mode.</summary>
        private bool isThreadMode = false;

        /// <summary> The analysis phase of the work flow.</summary>
        private int outputPhaseNum = 0;

        /// <summary> The number of the plug-ins for the last phase of the work flow.</summary>
        private int outputQueueNum = 0;

        /// <summary> Plug-in thread list.</summary>
        private LinkedList<SupportClass.ThreadClass> threadList = null;


        /* Major Plugins */

        /// <summary> The second phase, major plug-in - morphological analyzer.</summary>
        private kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.MorphAnalyzer morphAnalyzer = null;

        /// <summary> The configuration file for the morphological analyzer.</summary>
        private System.String morphAnalyzerConfFile = null;

        /// <summary> The third phase, major plug-in - POS tagger.</summary>
        private kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.PosTagger.PosTagger posTagger = null;

        /// <summary> The configuration file for the POS tagger.</summary>
        private System.String posTaggerConfFile = null;


        /* Supplement Plugins */

        /// <summary> The first phase, supplement plug-ins, plain text processors.</summary>
        private kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.PlainTextProcessor[] plainTextProcessors = null;

        /// <summary> The configuration files for the plain text processors.</summary>
        private System.String[] plainTextProcessorsConfFiles = null;

        /// <summary> The number of the plain text processors.</summary>
        private int plainTextPluginCnt = 0;

        /// <summary> The second phase, supplement plug-ins, morpheme processors.</summary>
        private kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.MorphemeProcessor.MorphemeProcessor[] morphemeProcessors = null;

        /// <summary> The configuration files for the morpheme processors.</summary>
        private System.String[] morphemeProcessorsConfFiles = null;

        /// <summary> The number of the morpheme processors.</summary>
        private int morphemePluginCnt = 0;

        /// <summary> The third phase, supplement plug-ins, pos processors.</summary>
        private kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PosProcessor.PosProcessor[] posProcessors = null;

        /// <summary> The configuration file for the pos processors.</summary>
        private System.String[] posProcessorConfFiles = null;

        /// <summary> The number of pos processors.</summary>
        private int posPluginCnt = 0;

        /// <summary> It is true when the work flow is ready for analysis.</summary>
        private bool isInitialized = false;

        /// <summary> The path for the base directory data and configuration files.</summary>
        private System.String baseDir = null;


        /* Communication Queues */

        /// <summary> The communication queues for the fist phase plug-ins.</summary>
        List<LinkedBlockingQueue<PlainSentence>> queuePhase1 = null;

        /// <summary> The communication queues for the second phase plug-ins.</summary>
        List<LinkedBlockingQueue<SetOfSentences>> queuePhase2 = null;

        /// <summary> The communication queues for the third phase plug-ins.</summary>
        List<LinkedBlockingQueue<Sentence>> queuePhase3 = null;

        /// <summary> Constructor.
        /// The maximum number of supplement plug-ins for each phase is set up with Workflow.MAX_SUPPLEMENT_PLUGIN_NUM.
        /// </summary>
        public Workflow()
        {
            this.maxSupplementPluginNum = MAX_SUPPLEMENT_PLUGIN_NUM;

            plainTextProcessors = new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.PlainTextProcessor[maxSupplementPluginNum];
            morphemeProcessors = new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.MorphemeProcessor.MorphemeProcessor[maxSupplementPluginNum];
            posProcessors = new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PosProcessor.PosProcessor[maxSupplementPluginNum];
            plainTextProcessorsConfFiles = new System.String[maxSupplementPluginNum];
            morphemeProcessorsConfFiles = new System.String[maxSupplementPluginNum];
            posProcessorConfFiles = new System.String[maxSupplementPluginNum];

            queuePhase1 = new List<LinkedBlockingQueue<PlainSentence>>(maxSupplementPluginNum);
            
            queuePhase2 = new List<LinkedBlockingQueue<SetOfSentences>>(maxSupplementPluginNum + 1);
            
            queuePhase3 = new List<LinkedBlockingQueue<Sentence>>(maxSupplementPluginNum + 1);
            
            threadList = new LinkedList<SupportClass.ThreadClass>();

            isInitialized = true;

            this.baseDir = ".";
        }

        /// <summary> Constructor.
        /// The maximum number of supplement plug-ins for each phase is set up with Workflow.MAX_SUPPLEMENT_PLUGIN_NUM.
        /// </summary>
        /// <param name="baseDir">- the path for base directory, which should have the 'conf' and 'data' directory
        /// </param>
        public Workflow(System.String baseDir)
        {
            this.maxSupplementPluginNum = MAX_SUPPLEMENT_PLUGIN_NUM;

            plainTextProcessors = new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.PlainTextProcessor[maxSupplementPluginNum];
            morphemeProcessors = new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.MorphemeProcessor.MorphemeProcessor[maxSupplementPluginNum];
            posProcessors = new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PosProcessor.PosProcessor[maxSupplementPluginNum];
            plainTextProcessorsConfFiles = new System.String[maxSupplementPluginNum];
            morphemeProcessorsConfFiles = new System.String[maxSupplementPluginNum];
            posProcessorConfFiles = new System.String[maxSupplementPluginNum];


            queuePhase1 = new List<LinkedBlockingQueue<PlainSentence>>(maxSupplementPluginNum);

            queuePhase2 = new List<LinkedBlockingQueue<SetOfSentences>>(maxSupplementPluginNum + 1);

            queuePhase3 = new List<LinkedBlockingQueue<Sentence>>(maxSupplementPluginNum + 1);


            threadList = new LinkedList<SupportClass.ThreadClass>();

            isInitialized = true;

            this.baseDir = baseDir;
        }

        /// <summary> Constructor.</summary>
        /// <param name="baseDir">- the path for base directory, which should have the 'conf' and 'data' directory
        /// </param>
        /// <param name="maxSupplementPluginNum">- the maximum number of supplement plug-ins for each phase
        /// </param>
        public Workflow(System.String baseDir, int maxSupplementPluginNum)
        {
            this.maxSupplementPluginNum = maxSupplementPluginNum;

            plainTextProcessors = new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.PlainTextProcessor[maxSupplementPluginNum];
            morphemeProcessors = new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.MorphemeProcessor.MorphemeProcessor[maxSupplementPluginNum];
            posProcessors = new kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PosProcessor.PosProcessor[maxSupplementPluginNum];
            plainTextProcessorsConfFiles = new System.String[maxSupplementPluginNum];
            morphemeProcessorsConfFiles = new System.String[maxSupplementPluginNum];
            posProcessorConfFiles = new System.String[maxSupplementPluginNum];

            queuePhase1 = new List<LinkedBlockingQueue<PlainSentence>>(maxSupplementPluginNum);
            queuePhase2 = new List<LinkedBlockingQueue<SetOfSentences>>(maxSupplementPluginNum + 1);
            queuePhase3 = new List<LinkedBlockingQueue<Sentence>>(maxSupplementPluginNum + 1);

            threadList = new LinkedList<SupportClass.ThreadClass>();

            isInitialized = true;

            this.baseDir = baseDir;
        }

        /// <summary> Sets the morphological analyzer plug-in, which is the major plug-in on second phase,
        /// on the work flow.
        /// </summary>
        /// <param name="ma">- the morphological analyzer plug-in
        /// </param>
        /// <param name="configFile">- the path for the configuration file (relative path to the base directory)
        /// </param>
        public virtual void setMorphAnalyzer(kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.MorphAnalyzer ma, System.String configFile)
        {
            morphAnalyzer = ma;
            morphAnalyzerConfFile = baseDir + "/" + configFile;
        }

        /// <summary> Sets the POS tagger plug-in, which is the major plug-in on the third phase, on the work flow.</summary>
        /// <param name="tagger">- the POS tagger plug-in
        /// </param>
        /// <param name="configFile">- the path for the configuration file (relative path to the base directory)
        /// </param>
        public virtual void setPosTagger(kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.PosTagger.PosTagger tagger, System.String configFile)
        {
            posTagger = tagger;
            posTaggerConfFile = baseDir + "/" + configFile;
        }

        /// <summary> Appends the plain text processor plug-in, which is the supplement plug-in on the first phase, on the work flow.</summary>
        /// <param name="plugin">- the plain text processor plug-in
        /// </param>
        /// <param name="configFile">- the path for the configuration file (relative path to the base directory)
        /// </param>
        public virtual void appendPlainTextProcessor(kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.PlainTextProcessor plugin, System.String configFile)
        {
            plainTextProcessorsConfFiles[plainTextPluginCnt] = baseDir + "/" + configFile;
            plainTextProcessors[plainTextPluginCnt++] = plugin;
        }

        /// <summary> Appends the morpheme processor plug-in, which is the supplement plug-in on the second phase, on the work flow.</summary>
        /// <param name="plugin">- the morpheme processor plug-in
        /// </param>
        /// <param name="configFile">- the path for the configuration file (relative path to the base directory)
        /// </param>
        public virtual void appendMorphemeProcessor(kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.MorphemeProcessor.MorphemeProcessor plugin, System.String configFile)
        {
            morphemeProcessorsConfFiles[morphemePluginCnt] = baseDir + "/" + configFile;
            morphemeProcessors[morphemePluginCnt++] = plugin;
        }

        /// <summary> Appends the POS processor plug-in, which is the supplement plug-in on the third phase, on the work flow.</summary>
        /// <param name="plugin">- the plain POS processor plug-in
        /// </param>
        /// <param name="configFile">- the path for the configuration file (relative path to the base directory)
        /// </param>
        public virtual void appendPosProcessor(kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PosProcessor.PosProcessor plugin, System.String configFile)
        {
            posProcessorConfFiles[posPluginCnt] = baseDir + "/" + configFile;
            posProcessors[posPluginCnt++] = plugin;
        }

        /// <summary> It activates the work flow with the plug-ins that were set up. The work flow can be activated in
        /// the thread mode where each plug-in works on its own thread. It may show better performance
        /// in the machines with multi-processor.
        /// 
        /// </summary>
        /// <param name="threadMode">- true: multi-thread mode, false: sigle thread mode
        /// </param>
        /// <throws>  Exception  </throws>
        public virtual void activateWorkflow(bool threadMode)
        {
            if (threadMode)
            {
                isThreadMode = true;

                // initialize the first phase supplement plug-ins and the communication queues
                LinkedBlockingQueue<PlainSentence> in1 = null;
                LinkedBlockingQueue<PlainSentence> out1 = new LinkedBlockingQueue<PlainSentence>();

                queuePhase1.Add(out1);

                for (int i = 0; i < plainTextPluginCnt; i++)
                {
                    in1 = out1;
                    out1 = new LinkedBlockingQueue<PlainSentence>();
                    queuePhase1.Add(out1);

                    plainTextProcessors[i].initialize(baseDir, plainTextProcessorsConfFiles[i]);
                    threadList.AddLast(new PlainTextProcThread(plainTextProcessors[i], in1, out1));
                }

                if (morphAnalyzer == null)
                {
                    outputPhaseNum = 1;
                    outputQueueNum = plainTextPluginCnt;
                    runThreads();
                    return;
                }
                in1 = out1;

                // initialize the second phase major plug-in and the communication queues
                LinkedBlockingQueue<SetOfSentences> in2 = null;
                LinkedBlockingQueue<SetOfSentences> out2 = new LinkedBlockingQueue<SetOfSentences>();

                queuePhase2.Add(out2);
                morphAnalyzer.initialize(baseDir, morphAnalyzerConfFile);

                threadList.AddLast(new MorphAnalyzerThread(morphAnalyzer, in1, out2));

                // initialize the second phase supplement plug-ins and the communication queues
                for (int i = 0; i < morphemePluginCnt; i++)
                {
                    in2 = out2;
                    out2 = new LinkedBlockingQueue<SetOfSentences>();

                    queuePhase2.Add(out2);
                    morphemeProcessors[i].initialize(baseDir, morphemeProcessorsConfFiles[i]);

                    threadList.AddLast(new MorphemeProcThread(morphemeProcessors[i], in2, out2));
                }

                if (posTagger == null)
                {
                    outputPhaseNum = 2;
                    outputQueueNum = morphemePluginCnt;
                    runThreads();
                    return;
                }
                in2 = out2;

                // initialize the third phase major plug-in and the communication queues
                LinkedBlockingQueue<Sentence> in3 = null;
                LinkedBlockingQueue<Sentence> out3 = new LinkedBlockingQueue<Sentence>();

                posTagger.initialize(baseDir, posTaggerConfFile);
                queuePhase3.Add(out3);

                threadList.AddLast(new PosTaggerThread(posTagger, in2, out3));

                // initialize the third phase supplement plug-ins and the communication queues
                for (int i = 0; i < posPluginCnt; i++)
                {
                    in3 = out3;
                    out3 = new LinkedBlockingQueue<Sentence>();

                    queuePhase3.Add(out3);
                    posProcessors[i].initialize(baseDir, posProcessorConfFiles[i]);

                    threadList.AddLast(new PosProcThread(posProcessors[i], in3, out3));
                }

                outputPhaseNum = 3;
                outputQueueNum = posPluginCnt;
                runThreads();
            }
            else
            {
                isThreadMode = false;

                // initialize the first phase supplement plug-ins and the communication queues
                queuePhase1.Add(new LinkedBlockingQueue<PlainSentence>());

                for (int i = 0; i < plainTextPluginCnt; i++)
                {
                    plainTextProcessors[i].initialize(baseDir, plainTextProcessorsConfFiles[i]);
                    queuePhase1.Add(new LinkedBlockingQueue<PlainSentence>());
                }

                if (morphAnalyzer == null)
                {
                    outputPhaseNum = 1;
                    outputQueueNum = plainTextPluginCnt;
                    return;
                }

                // initialize the second phase major plug-in and the communication queue
                morphAnalyzer.initialize(baseDir, morphAnalyzerConfFile);
                queuePhase2.Add(new LinkedBlockingQueue<SetOfSentences>());

                // initialize the second phase supplement plug-ins and the communication queues
                for (int i = 0; i < morphemePluginCnt; i++)
                {
                    morphemeProcessors[i].initialize(baseDir, morphemeProcessorsConfFiles[i]);
                    queuePhase2.Add(new LinkedBlockingQueue<SetOfSentences>());
                }

                if (posTagger == null)
                {
                    outputPhaseNum = 2;
                    outputQueueNum = morphemePluginCnt;
                    return;
                }

                // initialize the third phase major plug-in and the communication queue
                posTagger.initialize(baseDir, posTaggerConfFile);
                queuePhase3.Add(new LinkedBlockingQueue<Sentence>());

                // initialize the third phase supplement plug-in and the communication queues
                for (int i = 0; i < posPluginCnt; i++)
                {
                    posProcessors[i].initialize(baseDir, posProcessorConfFiles[i]);
                    queuePhase3.Add(new LinkedBlockingQueue<Sentence>());
                }

                outputPhaseNum = 3;
                outputQueueNum = posPluginCnt;
            }
        }

        /// <summary> It starts the threads for each plug-in on the work flow, when the work flow was activated
        /// with the multi-thread mode.
        /// </summary>
        private void runThreads()
        {
            foreach (SupportClass.ThreadClass th in threadList)
            {
                th.Start();
            }
        }

        /// <summary> It ends the threads for each plug-in on the work flow. The shutdown() methods of each plug-in
        /// are called before they end.
        /// </summary>
        public virtual void close()
        {
            if (isThreadMode)
            {
                foreach (SupportClass.ThreadClass th in threadList)
                {
                    th.Interrupt();
                }
                threadList.Clear();
            }
        }

        /// <summary> It removes the plug-ins on the work flow.</summary>
        public virtual void clear()
        {
            close();

            if (isInitialized)
            {
                queuePhase1.Clear();
                queuePhase2.Clear();
                queuePhase3.Clear();
                isThreadMode = false;
                outputPhaseNum = 0;
                outputQueueNum = 0;
                plainTextPluginCnt = 0;
                morphemePluginCnt = 0;
                posPluginCnt = 0;
                morphAnalyzer = null;
                posTagger = null;
            }
        }

        /// <summary> It adds the specified input text to the input queue of the work flow. After this method,
        /// you are allowed to get the analysis result by using one of the following methods:
        /// 
        /// - getResultOfSentence() : to get the result for one sentence at the front of result queue
        /// - getResultOfDocument() : to get the entire result for all sentences
        /// 
        /// If the input document is not small, getResultOfDocument() may show lower performance, and it
        /// could be better to call getResultOfSentence() repeatedly. You need to pay attention on this.
        /// 
        /// </summary>
        /// <param name="document">- sequence of sentences separated with newlines.
        /// </param>
        public virtual void analyze(System.String document)
        {
            System.String[] strArray = document.Split("\n");
            LinkedBlockingQueue<PlainSentence> queue = queuePhase1[0];

            if (queue == null)
            {
                return;
            }

            for (int i = 0; i < strArray.Length - 1; i++)
            {
                queue.Add(new PlainSentence(0, i, false, strArray[i].Trim()));
            }
            queue.Add(new PlainSentence(0, strArray.Length - 1, true, strArray[strArray.Length - 1].Trim()));

            if (!isThreadMode)
            {
                analyzeInSingleThread();
            }
        }

        /// <summary> It adds the specified input text to the input queue of the work flow. After this method,
        /// you are allowed to get the analysis result by using one of the following methods:
        /// 
        /// - getResultOfSentence() : to get the result for one sentence at the front of result queue
        /// - getResultOfDocument() : to get the entire result for all sentences
        /// 
        /// If the input document is not small, getResultOfDocument() may show lower performance, and it
        /// could be better to call getResultOfSentence() repeatedly. You need to pay attention on this.
        /// 
        /// </summary>
        /// <param name="document">- the path for the text file to be analyzed
        /// </param>
        /// <throws>  IOException </throws>
        public virtual void analyze(System.IO.FileInfo document)
        {
            System.IO.StreamReader br = new System.IO.StreamReader(document.FullName, System.Text.Encoding.UTF8);
            LinkedBlockingQueue<PlainSentence> queue = queuePhase1[0];

            if (queue == null)
            {
                return;
            }

            System.String line = null;
            int i = 0;

            while ((line = br.ReadLine()) != null)
            {
                if (br.Peek() != -1)
                {
                    queue.Add(new PlainSentence(0, i++, false, line.Trim()));
                }
                else
                {
                    queue.Add(new PlainSentence(0, i++, true, line.Trim()));
                    break;
                }
            }

            br.Close();

            if (!isThreadMode)
            {
                analyzeInSingleThread();
            }
        }

        /// <summary> Returns the analysis result for one sentence at the top of the result queue. You can call this method
        /// repeatedly to get the result for remaining sentences in the input document. If there is no result,
        /// this method will be blocked until a new result comes.
        /// 
        /// It stores the specified object with the analysis result. The return type of the object depends on the
        /// analysis phase of the work flow so you must give the relevant type of parameter.
        /// 
        /// In this way, you can get the analysis result with a relevant object, so you don't need to parse the result string
        /// again. If you just want to see the result, consider to use "String getResultOfSentence()" instead.
        /// 
        /// </summary>
        /// <param name="<T>">- One of PlainSentence (for the first phase), Sentence (for the second phase), and SetOfSentences (for the third phase).
        /// </param>
        /// <param name="a">- the object to get the result
        /// </param>
        /// <returns> the analysis result for one sentence at front
        /// </returns>
        /// <throws>  ResultTypeException </throws>
        public T getResultOfSentence<T>(T a) where T : class
        {
            Type objClass = a.GetType();

            try
            {
                if (typeof(PlainSentence).Equals(objClass))
                {
                    if (outputPhaseNum != 1)
                    {
                        throw new ResultTypeException(outputPhaseNum);
                    }
                    LinkedBlockingQueue<PlainSentence> queue = queuePhase1[outputQueueNum];
                    a = queue.Take() as T;
                }
                else if (typeof(SetOfSentences).Equals(objClass))
                {
                    if (outputPhaseNum != 2)
                    {
                        throw new ResultTypeException(outputPhaseNum);
                    }
                    LinkedBlockingQueue<SetOfSentences> queue = queuePhase2[outputQueueNum];
                    a = queue.Take() as T;
                }
                else if (typeof(Sentence).Equals(objClass))
                {
                    if (outputPhaseNum != 3)
                    {
                        throw new ResultTypeException(outputPhaseNum);
                    }
                    LinkedBlockingQueue<Sentence> queue = queuePhase3[outputQueueNum];
                    a = queue.Take() as T;
                }
                else
                {
                    throw new ResultTypeException(outputPhaseNum);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

            return a;
        }

        /// <summary> Returns the analysis result list for all sentence in the result. When you use this method,
        /// you need to pay attention on the size of the data. If the size of data is big, it may show
        /// lower performance than using getResultOfSentence() repeatedly.
        /// 
        /// The return type of the object depends on the analysis phase of the work flow so you must give
        /// the relevant type of parameter. In this way, you can get the analysis result with a relevant
        /// object, so you don't need to parse the result string again. If you just want to see the result,
        /// consider to use "String getResultOfDocument()" instead.
        /// 
        /// </summary>
        /// <param name="<T>">- One of PlainSentence (for the first phase), Sentence (for the second phase), and SetOfSentences (for the third phase).
        /// </param>
        /// <param name="a">- the object to specify the return type
        /// </param>
        /// <returns> the list of the analysis result for all sentences in the document
        /// </returns>
        /// <throws>  ResultTypeException </throws>
        public LinkedList<T> getResultOfDocument<T>(T a)  where T : class
        {
            Type objClass = a.GetType();
            LinkedList<T> list = new LinkedList<T>();

            try
            {
                if (typeof(PlainSentence).Equals(objClass))
                {
                    if (outputPhaseNum != 1)
                    {
                        throw new ResultTypeException(outputPhaseNum);
                    }
                    LinkedBlockingQueue<PlainSentence> queue = queuePhase1[outputQueueNum];
                    while (true)
                    {
                        PlainSentence ps = queue.Take();
                        list.AddLast(ps as T);
                        if (ps.EndOfDocument)
                        {
                            break;
                        }
                    }
                }
                else if (typeof(SetOfSentences).Equals(objClass))
                {
                    if (outputPhaseNum != 2)
                    {
                        throw new ResultTypeException(outputPhaseNum);
                    }
                    LinkedBlockingQueue<SetOfSentences> queue = queuePhase2[outputQueueNum];
                    while (true)
                    {
                        SetOfSentences sos = queue.Take();
                        list.AddLast(sos as T);
                        if (sos.EndOfDocument)
                        {
                            break;
                        }
                    }
                }
                else if (typeof(Sentence).Equals(objClass))
                {
                    if (outputPhaseNum != 3)
                    {
                        throw new ResultTypeException(outputPhaseNum);
                    }
                    LinkedBlockingQueue<Sentence> queue = queuePhase3[outputQueueNum];
                    while (true)
                    {
                        Sentence sent = queue.Take();
                        list.AddLast(sent as T);
                        if (sent.EndOfDocument)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    throw new ResultTypeException(outputPhaseNum);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            return list;
        }

        /// <summary> Analyze the text in the single thread. </summary>
        private void analyzeInSingleThread()
        {
            // first phase
            if (plainTextPluginCnt == 0)
            {
                return;
            }

            LinkedBlockingQueue<PlainSentence> inQueue1 = null;
            LinkedBlockingQueue<PlainSentence> outQueue1 = null;

            PlainSentence ps = null;
            outQueue1 = queuePhase1[0];

            for (int i = 0; i < plainTextPluginCnt; i++)
            {
                inQueue1 = outQueue1;
                outQueue1 = queuePhase1[i + 1];

                while ((ps = inQueue1.Poll()) != null)
                {
                    if ((ps = plainTextProcessors[i].doProcess(ps)) != null)
                    {
                        outQueue1.Add(ps);
                    }

                    while (plainTextProcessors[i].hasRemainingData())
                    {
                        if ((ps = plainTextProcessors[i].doProcess(null)) != null)
                        {
                            outQueue1.Add(ps);
                        }
                    }

                    if ((ps = plainTextProcessors[i].flush()) != null)
                    {
                        outQueue1.Add(ps);
                    }
                }
            }

            // second phase
            if (morphAnalyzer == null)
            {
                return;
            }

            LinkedBlockingQueue<SetOfSentences> inQueue2 = null;
            LinkedBlockingQueue<SetOfSentences> outQueue2 = null;
            SetOfSentences sos = null;
            inQueue1 = outQueue1;
            outQueue2 = queuePhase2[0];

            while ((ps = inQueue1.Poll()) != null)
            {
                if ((sos = morphAnalyzer.morphAnalyze(ps)) != null)
                {
                    outQueue2.Add(sos);
                }
            }

            if (morphemePluginCnt == 0)
            {
                return;
            }

            for (int i = 0; i < morphemePluginCnt; i++)
            {
                inQueue2 = outQueue2;
                outQueue2 = queuePhase2[i + 1];

                while ((sos = inQueue2.Poll()) != null)
                {
                    if ((sos = morphemeProcessors[i].doProcess(sos)) != null)
                    {
                        outQueue2.Add(sos);
                    }
                }
            }

            // third phase
            if (posTagger == null)
            {
                return;
            }

            LinkedBlockingQueue<Sentence> inQueue3 = null;
            LinkedBlockingQueue<Sentence> outQueue3 = null;
            Sentence sent = null;
            inQueue2 = outQueue2;
            outQueue3 = queuePhase3[0];

            while ((sos = inQueue2.Poll()) != null)
            {
                if ((sent = posTagger.tagPOS(sos)) != null)
                {
                    outQueue3.Add(sent);
                }
            }

            if (posPluginCnt == 0)
            {
                return;
            }

            for (int i = 0; i < posPluginCnt; i++)
            {
                inQueue3 = outQueue3;
                outQueue3 = queuePhase3[i + 1];

                while ((sent = inQueue3.Poll()) != null)
                {
                    if ((sent = posProcessors[i].doProcess(sent)) != null)
                    {
                        outQueue3.Add(sent);
                    }
                }
            }
        }
    }
}