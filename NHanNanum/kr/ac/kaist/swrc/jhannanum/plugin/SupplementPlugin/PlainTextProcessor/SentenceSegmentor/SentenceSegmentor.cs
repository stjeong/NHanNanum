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
namespace kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.SentenceSegmentor
{

    /// <summary> This plug-in reads a document which consists of more than one sentence, and recognize the end of each sentence
    /// based on punctuation marks. So if punctuation marks were not used correctly in the sentences, this plug-in
    /// will not work well. </br>
    /// </br>
    /// It considers '.', '!', '?' as the marks for the end of sentence, but these symbols can be used in other purpose,
    /// so it deals with those problems. </br>
    /// </br>
    /// For example, </br>
    /// - 12.42 : number </br>
    /// - A. Introduction : section title </br>
    /// - I'm fine... : ellipsis </br>
    /// - U.S. : abbreviation </br>
    /// </br>
    /// It is a Plain Text Processor plug-in which is a supplement plug-in of phase 1 in HanNanum work flow.
    /// 
    /// </summary>
    /// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
    /// </author>
    public class SentenceSegmentor : kr.ac.kaist.swrc.jhannanum.plugin.SupplementPlugin.PlainTextProcessor.PlainTextProcessor
    {
        /// <summary>the ID of the document </summary>
        private int documentID = 0;

        /// <summary>the ID of the sentence </summary>
        private int sentenceID = 0;

        /// <summary>the flag to check if there is remaining data in the input buffer </summary>
        private bool hasRemainingData_Renamed_Field = false;

        /// <summary>the buffer for storing intermediate results </summary>
        private System.String bufRes = null;

        /// <summary>the buffer for storing the remaining part after one sentence returned </summary>
        private System.String[] bufEojeols = null;

        /// <summary>the index of the buffer for storing the remaining part </summary>
        private int bufEojeolsIdx = 0;

        /// <summary>the flag to check whether current sentence is the end of document </summary>
        private bool endOfDocument = false;

        /// <summary> Checks if the specified symbol can appear with previous symbols.</summary>
        /// <param name="c">- the character to check
        /// </param>
        /// <returns> true: if the character can come together with the previous symbols, false: not possible
        /// </returns>
        private bool isSym(char c)
        {
            switch (c)
            {
                case ')': return true;
                case ']': return true;
                case '}': return true;
                case '?': return true;
                case '!': return true;
                case '.': return true;
                case '\'': return true;
                case '\"': return true;
            
            }
            return false;
        }


        /// <summary> It recognizes the end of each sentence and return the first sentence.</summary>
        /// <param name="ps">- the plain sentence which can consist of several sentences
        /// </param>
        /// <returns> the first sentence recognized 
        /// </returns>
        public virtual PlainSentence doProcess(PlainSentence ps)
        {
            System.String[] eojeols = null;
            System.String res = null;
            bool isFirstEojeol = true;
            bool isEOS = false;
            int i = 0;
            int j = 0;

            if (bufEojeols != null)
            {
                eojeols = bufEojeols;
                i = bufEojeolsIdx;

                bufEojeols = null;
                bufEojeolsIdx = 0;
            }
            else
            {
                if (ps == null)
                {
                    return null;
                }

                if (documentID != ps.DocumentID)
                {
                    documentID = ps.DocumentID;
                    sentenceID = 0;
                }

                System.String str = null;
                if ((str = ps.Sentence) == null)
                {
                    return null;
                }
                eojeols = str.Split("\\s");

                endOfDocument = ps.EndOfDocument;
            }

            for (; isEOS == false && i < eojeols.Length; i++)
            {
                if (!eojeols[i].Matches(".*(\\.|\\!|\\?).*"))
                {
                    // the eojeol doesn't have '.', '!', '?'
                    if (isFirstEojeol)
                    {
                        res = eojeols[i];
                        isFirstEojeol = false;
                    }
                    else
                    {
                        res += (" " + eojeols[i]);
                    }
                }
                else
                {
                    // the eojeol has '.', '!', '?'
                    char[] ca = eojeols[i].ToCharArray();

                    for (j = 0; isEOS == false && j < ca.Length; j++)
                    {
                        switch (ca[j])
                        {

                            case '.':
                                if (j == 1)
                                {
                                    // ellipsis
                                    continue;
                                }
                                if (j > 0)
                                {
                                    // abbreviation
                                    if (System.Char.IsLower(ca[j - 1]) || System.Char.IsUpper(ca[j - 1]))
                                    {
                                        continue;
                                    }
                                }
                                if (j < ca.Length - 1)
                                {
                                    // number
                                    if (System.Char.IsDigit(ca[j + 1]))
                                    {
                                        continue;
                                    }
                                }
                                isEOS = true;
                                break;

                            case '!':
                                isEOS = true;
                                break;

                            case '?':
                                isEOS = true;
                                break;
                        }

                        if (isEOS)
                        {
                            if (isFirstEojeol)
                            {
                                res = eojeols[i].Substring(0, (j) - (0)) + " " + ca[j];
                                isFirstEojeol = false;
                            }
                            else
                            {
                                res += (" " + eojeols[i].Substring(0, (j) - (0)) + " " + ca[j]);
                            }

                            // a sequence of symbols such as '...', '?!!'
                            while (j < ca.Length - 1)
                            {
                                if (isSym(ca[j + 1]))
                                {
                                    j++;
                                    res += ca[j];
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    if (isEOS == false)
                    {
                        if (isFirstEojeol)
                        {
                            res = eojeols[i];
                            isFirstEojeol = false;
                        }
                        else
                        {
                            res += (" " + eojeols[i]);
                        }
                    }
                }
            }

            i--;
            j--;

            if (isEOS)
            {
                // the remaining part of an eojeol after the end of sentence is stored in the buffer
                if (j + 1 < eojeols[i].Length)
                {
                    eojeols[i] = eojeols[i].Substring(j + 1);
                    bufEojeols = eojeols;
                    bufEojeolsIdx = i;
                    hasRemainingData_Renamed_Field = true;
                }
                else
                {
                    if (i == eojeols.Length - 1)
                    {
                        // all eojeols were processed
                        hasRemainingData_Renamed_Field = false;
                    }
                    else
                    {
                        // if there were some eojeols not processed, they were stored in the buffer
                        bufEojeols = eojeols;
                        bufEojeolsIdx = i + 1;
                        hasRemainingData_Renamed_Field = true;
                    }
                }

                if (bufRes == null)
                {
                    return new PlainSentence(documentID, sentenceID++, !hasRemainingData_Renamed_Field && endOfDocument, res);
                }
                else
                {
                    res = bufRes + " " + res;
                    bufRes = null;
                    return new PlainSentence(documentID, sentenceID++, !hasRemainingData_Renamed_Field && endOfDocument, res);
                }
            }
            else
            {
                if (res != null && res.Length > 0)
                {
                    bufRes = res;
                }
                hasRemainingData_Renamed_Field = false;
                return null;
            }
        }
        public virtual void initialize(System.String baseDir, System.String configFile)
        {

        }
        public virtual void shutdown()
        {
        }
        public virtual PlainSentence flush()
        {
            if (bufRes != null)
            {
                System.String res = bufRes;
                bufRes = null;
                hasRemainingData_Renamed_Field = false;
                return new PlainSentence(documentID, sentenceID++, !hasRemainingData_Renamed_Field && endOfDocument, res);
            }
            else
            {
                return null;
            }
        }
        public virtual bool hasRemainingData()
        {
            return hasRemainingData_Renamed_Field;
        }
    }
}