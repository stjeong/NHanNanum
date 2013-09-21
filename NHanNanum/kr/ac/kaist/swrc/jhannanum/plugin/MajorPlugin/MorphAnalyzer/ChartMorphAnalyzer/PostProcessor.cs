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
using SetOfSentences = kr.ac.kaist.swrc.jhannanum.comm.SetOfSentences;
using Code = kr.ac.kaist.swrc.jhannanum.share.Code;
using System.Collections.Generic;
namespace kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.ChartMorphAnalyzer
{

    /// <summary> This class is for post processing of morphological analysis.</summary>
    /// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
    /// </author>
    public class PostProcessor
    {
        /// <summary>the triple character representation of '하' </summary>
        private System.String HA = null;

        /// <summary>the triple character representation of '아' </summary>
        private System.String AR = null;

        /// <summary>the triple character representation of '어' </summary>
        private System.String A_ = null;

        /// <summary>the triple character representation of 'ㅏㅑㅗ' </summary>
        private System.String PV = null;

        /// <summary>the triple character representation of '끄뜨쓰크트' </summary>
        private System.String XEU = null;

        /// <summary>the triple character representation of '돕' </summary>
        private System.String DOB = null;

        /// <summary>the triple character representation of '곱' </summary>
        private System.String GOB = null;

        /// <summary>the triple character representation of '으' </summary>
        private System.String EU = null;

        /// <summary>the triple character representation of '습니' </summary>
        private System.String SU = null;

        /// <summary>the triple character representation of '는다' </summary>
        private System.String NU = null;

        /// <summary> Constructor.</summary>
        public PostProcessor()
        {
            HA = Code.toTripleString("하");
            AR = Code.toTripleString("아");
            A_ = Code.toTripleString("어");
            PV = Code.toTripleString("ㅏㅑㅗ");
            XEU = Code.toTripleString("끄뜨쓰크트");
            DOB = Code.toTripleString("돕");
            GOB = Code.toTripleString("곱");
            EU = Code.toTripleString("으");
            SU = Code.toTripleString("습니");
            NU = Code.toTripleString("는다");
        }

        /// <summary> It does post processing of morphological analysis to deal with some exceptions.</summary>
        /// <param name="sos">- the result of morphological analysis
        /// </param>
        /// <returns> the result of morphological analysis with post processing
        /// </returns>
        public virtual SetOfSentences doPostProcessing(SetOfSentences sos)
        {

            List<Eojeol[]> eojeolSetArray = sos.getEojeolSetArray();

            IEnumerator<Eojeol[]> iter = eojeolSetArray.GetEnumerator();

            while (iter.MoveNext())
            {
                Eojeol[] eojeolSet = iter.Current;
                System.String prevMorph = "";

                for (int i = 0; i < eojeolSet.Length; i++)
                {
                    Eojeol eojeol = eojeolSet[i];
                    System.String[] morphemes = eojeol.Morphemes;
                    System.String[] tags = eojeol.Tags;

                    for (int j = 0; j < eojeol.length; j++)
                    {
                        System.String tri = Code.toTripleString(morphemes[j]);
                        if (tags[j].StartsWith("e"))
                        {
                            int prevLen = prevMorph.Length;

                            if (tri.StartsWith(A_))
                            {
                                /* 어 -> 아 */
                                if (prevLen >= 4 && prevMorph[prevLen - 1] == EU[1] && !isXEU(prevMorph[prevLen - 2]) && ((Code.isJungseong(prevMorph[prevLen - 3]) && isPV(prevMorph[prevLen - 3])) || (Code.isJongseong(prevMorph[prevLen - 3]) && isPV(prevMorph[prevLen - 4]))))
                                {
                                    morphemes[j] = Code.toString(AR.ToCharArray());
                                }
                                else if (prevLen >= 3 && prevMorph[prevLen - 1] == DOB[2] && (prevMorph.Substring(prevLen - 3).Equals(DOB) == false || prevMorph.Substring(prevLen - 3).Equals(GOB) == false))
                                {
                                    /* for 'ㅂ' irregular */
                                }
                                else if (prevLen >= 2 && prevMorph.Substring(prevLen - 2).Equals(HA))
                                {
                                }
                                else if (prevLen >= 2 && ((Code.isJungseong(prevMorph[prevLen - 1]) && isPV(prevMorph[prevLen - 1])) || (Code.isJongseong(prevMorph[prevLen - 1]) && isPV(prevMorph[prevLen - 2]))))
                                {
                                    // final consonant or not
                                    morphemes[j] = Code.toString(AR.ToCharArray());
                                }
                            }
                            else if (tri.StartsWith(EU.Substring(0, (2) - (0))) || tri.StartsWith(SU.Substring(0, (4) - (0))) || tri.StartsWith(NU.Substring(0, (4) - (0))))
                            {
                                /* elision of '으', '스', '느' */
                                if (prevLen >= 2 && (Code.isJungseong(prevMorph[prevLen - 1]) || prevMorph[prevLen - 1] == 0x11AF))
                                {
                                    morphemes[j] = Code.toString(tri.Substring(2).ToCharArray());
                                }
                            }
                        }

                        prevMorph = Code.toTripleString(morphemes[j]);
                    }
                }
            }

            return sos;
        }

        /// <summary> Checks whether the specified character is one of 'ㅏ', 'ㅑ', 'ㅗ'.</summary>
        /// <param name="c">- the character to check
        /// </param>
        /// <returns> true: the character is one of 'ㅏ', 'ㅑ', 'ㅗ', false: not one of the characters
        /// </returns>
        private bool isPV(char c)
        {
            if (PV.IndexOf((System.Char)c) == -1)
            {
                return false;
            }
            return true;
        }

        /// <summary> Checks whether the specified character is one of '끄', '뜨', '쓰', '크', '트'.</summary>
        /// <param name="c">- the character to check
        /// </param>
        /// <returns> true: the character is one of '끄', '뜨', '쓰', '크', '트', false: not one of the characters
        /// </returns>
        private bool isXEU(char c)
        {
            if (XEU.IndexOf((System.Char)c) == -1)
            {
                return false;
            }
            return true;
        }
    }
}