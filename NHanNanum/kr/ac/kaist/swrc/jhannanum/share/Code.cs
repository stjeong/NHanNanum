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
using System.Collections.Generic;
namespace kr.ac.kaist.swrc.jhannanum.share
{

    /// <summary> This class is for code conversion. HanNanum internally uses triple encoding, which represents
    /// an Korean eumjeol with three characters - CHOSEONG(beginning consonant), JUNGSEONG(vowel), JONGSEONG(final consonant).
    /// This class converts the Korean encoding from unicode to triple encoding, and vice versa.
    /// 
    /// </summary>
    /// <seealso cref="<a href="http://www.utf8-chartable.de/unicode-utf8-table.pl">http://www.utf8-chartable.de/unicode-utf8-table.pl</a>">
    /// </seealso>
    /// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
    /// </author>
    public class Code
    {
        /// <summary>triple encoding </summary>
        public const int ENCODING_TRIPLE = 0;

        /// <summary>unicode </summary>
        public const int ENCODING_UNICODE = 1;

        /// <summary>CHOSEONG(beginning consonant) </summary>
        public const int JAMO_CHOSEONG = 0;

        /// <summary>JUNGSEONG(vowel) </summary>
        public const int JAMO_JUNGSEONG = 1;

        /// <summary>JONGSEONG(final consonant) </summary>
        public const int JAMO_JONGSEONG = 2;

        /// <summary>hangul filler in unicode </summary>
        public const char HANGUL_FILLER = (char)(0x3164);

        /// <summary>the list of CHOSEONG - beginning consonant </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'CHOSEONG_LIST'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private static readonly char[] CHOSEONG_LIST = new char[] { 'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };

        /// <summary>the list of JONGSEONG - final consonant </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'JONGSEONG_LIST '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private static readonly char[] JONGSEONG_LIST = new char[] { HANGUL_FILLER, 'ㄱ', 'ㄲ', 'ㄳ', 'ㄴ', 'ㄵ', 'ㄶ', 'ㄷ', 'ㄹ', 'ㄺ', 'ㄻ', 'ㄼ', 'ㄽ', 'ㄾ', 'ㄿ', 'ㅀ', 'ㅁ', 'ㅂ', 'ㅄ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };

        /// <summary>the list of JONGSEONG for reverse </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'CHOSEONG_LIST_REV'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private static readonly sbyte[] CHOSEONG_LIST_REV = new sbyte[] { 0, 1, -1, 2, -1, -1, 3, 4, 5, -1, -1, -1, -1, -1, -1, -1, 6, 7, 8, -1, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };

        /// <summary>the list of JONGSEONG for reverse </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'JONGSEONG_LIST_REV'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private static readonly sbyte[] JONGSEONG_LIST_REV = new sbyte[] { 1, 2, 3, 4, 5, 6, 7, -1, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, -1, 18, 19, 20, 21, 22, -1, 23, 24, 25, 26, 27 };

        /// <summary> It changes the encoding of text file between UTF-8 and the triple encoding.</summary>
        /// <param name="srcFileName">- the input file
        /// </param>
        /// <param name="desFileName">- the output file
        /// </param>
        /// <param name="srcEncoding">- the encoding of input file: ENCODING_UNICODE or ENCODING_TRIPLE
        /// </param>
        /// <param name="desEncoding">- the encoding of input file: ENCODING_UNICODE or ENCODING_TRIPLE
        /// </param>
        /// <throws>  IOException </throws>
        public static void convertFile(System.String srcFileName, System.String desFileName, int srcEncoding, int desEncoding)
        {
            System.IO.StreamReader br = new System.IO.StreamReader(
                new System.IO.FileStream(srcFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read), System.Text.Encoding.UTF8);

            System.IO.StreamWriter bw = new System.IO.StreamWriter(
                new System.IO.FileStream(desFileName, System.IO.FileMode.Create), System.Text.Encoding.UTF8);
            System.String line = null;

            if (srcEncoding == ENCODING_UNICODE && desEncoding == ENCODING_TRIPLE)
            {
                while ((line = br.ReadLine()) != null)
                {
                    char[] buf = toTripleArray(line);
                    bw.Write(buf);
                    bw.Write('\n');
                }
            }
            else if (srcEncoding == ENCODING_TRIPLE && desEncoding == ENCODING_UNICODE)
            {
                while ((line = br.ReadLine()) != null)
                {
                    System.String buf = toString(line.ToCharArray());
                    bw.Write(buf);
                    bw.Write('\n');
                }
            }
            br.Close();
            bw.Close();
        }

        /// <summary> It checks whether the specified character is choseong.</summary>
        /// <param name="c">- the character to check
        /// </param>
        /// <returns> true: the specified character is choseong, false: not choseong
        /// </returns>
        public static bool isChoseong(char c)
        {
            if (c >= 0x1100 && c <= 0x1112)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary> It checks whether the specified character is jongseong.</summary>
        /// <param name="c">- the character to check
        /// </param>
        /// <returns> true: the specified character is jongseong, false: not jongseong
        /// </returns>
        public static bool isJongseong(char c)
        {
            if (c >= 0x11A8 && c <= 0x11C2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary> It checks whether the specified character is jungseong.</summary>
        /// <param name="c">- the character to check
        /// </param>
        /// <returns> true: the specified character is jungseong, false: not jungseong
        /// </returns>
        public static bool isJungseong(char c)
        {
            if (c >= 0x1161 && c <= 0x1175)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary> It changes the specified jongseong to choseong.</summary>
        /// <param name="jongseong">- the final consonant
        /// </param>
        /// <returns> the consonant which is changed from jongseong to choseong
        /// </returns>
        public static char toChoseong(char jongseong)
        {
            if (jongseong >= 0x11A8 && jongseong <= 0x11C2)
            {
                jongseong -= (char)(0x11A7);
                // 종성
                char tmp = JONGSEONG_LIST[jongseong];
                tmp -= (char)(0x3131);
                if (CHOSEONG_LIST_REV[tmp] != -1)
                {
                    return (char)(CHOSEONG_LIST_REV[tmp] + 0x1100);
                }
            }
            return jongseong;
        }

        /// <summary> Changes the unicode Hangul jamo to unicode compatibility Hangul jamo.</summary>
        /// <param name="jamo">- unicode Hangul jamo
        /// </param>
        /// <returns> the compatibility Hangul jamo
        /// </returns>
        public static char toCompatibilityJamo(char jamo)
        {
            if (jamo >= 0x1100 && jamo < 0x1100 + CHOSEONG_LIST.Length)
            {
                return CHOSEONG_LIST[jamo - 0x1100];
            }
            if (jamo >= 0x1161 && jamo <= 0x1175)
            {
                return (char)(jamo - 0x1161 + 0x314F);
            }
            if (jamo == 0)
            {
                return HANGUL_FILLER;
            }
            else
            {
                if (jamo >= 0x11A8 && jamo < 0x11A7 + JONGSEONG_LIST.Length)
                {
                    return JONGSEONG_LIST[jamo - 0x11A7];
                }
            }
            return jamo;
        }

        /// <summary> It changes the unicode Hangul compatibility jamo to Hangul jamo - choseong, jungseong, or jongseong.</summary>
        /// <param name="jamo">- the unicode Hangul compatibility jamo
        /// </param>
        /// <param name="flag">- JAMO_CHOSEONG or JAMO_JUNGSEONG or JAMO_JONGSEONG
        /// </param>
        /// <returns> the unicode Hangul jamo
        /// </returns>
        public static char toJamo(char jamo, int flag)
        {
            char result = (char)(0);
            switch (flag)
            {

                case JAMO_CHOSEONG:
                    if (jamo >= 0 && jamo <= 0x12)
                    {
                        result = (char)(jamo + 0x1100);
                    }
                    break;

                case JAMO_JUNGSEONG:
                    if (jamo >= 0 && jamo <= 0x14)
                    {
                        result = (char)(jamo + 0x1161);
                    }
                    break;

                case JAMO_JONGSEONG:
                    if (jamo >= 1 && jamo <= 0x1B)
                    {
                        result = (char)(jamo + 0x11A7);
                    }
                    break;
            }
            return result;
        }

        /// <summary> Converts the encoding of the text from Hangul triple encoding to unicode.</summary>
        /// <param name="tripleArray">- the text with the Hangul triple encoding
        /// </param>
        /// <returns> the unicode text
        /// </returns>
        public static System.String toString(char[] tripleArray)
        {
            System.String result = "";
            int i = 0;
            int len = tripleArray.Length;

            int cho;
            int jung;
            int jong;

            if (len == 0)
            {
                return "";
            }

            char c = tripleArray[i];

            while (i < len)
            {
                if (c >= 0x1100 && c <= 0x1112)
                {
                    cho = c - 0x1100;

                    if (++i < len)
                    {
                        c = tripleArray[i];
                    }
                    if (c >= 0x1161 && c <= 0x1175 && i < len)
                    {
                        jung = c - 0x1161;

                        if (++i < len)
                        {
                            c = tripleArray[i];
                        }
                        if (c >= 0x11A8 && c <= 0x11C2 && i < len)
                        {
                            jong = c - 0x11A7;

                            // choseong + jungseong + jongseong
                            result += (char)(0xAC00 + (cho * 21 * 28) + (jung * 28) + jong);
                            if (++i < len)
                            {
                                c = tripleArray[i];
                            }
                        }
                        else
                        {
                            // choseong + jungseong
                            result += (char)(0xAC00 + (cho * 21 * 28) + (jung * 28));
                        }
                    }
                    else
                    {
                        // choseong: a single choseong is represented as ^consonant
                        char tmp = CHOSEONG_LIST[cho];
                        if (tmp == 'ㅃ' || tmp == 'ㅉ' || tmp == 'ㄸ')
                        {
                            result += CHOSEONG_LIST[cho];
                        }
                        else
                        {
                            result += ("^" + CHOSEONG_LIST[cho]);
                        }
                    }
                }
                else if (c >= 0x1161 && c <= 0x1175 && i < len)
                {
                    jung = c - 0x1161;

                    // jungseong
                    result += (char)(jung + 0x314F);

                    if (++i < len)
                    {
                        c = tripleArray[i];
                    }
                }
                else if (c >= 0x11A8 && c <= 0x11C2 && i < len)
                {
                    jong = c - 0x11A7;

                    // jongseong
                    result += JONGSEONG_LIST[jong];

                    if (++i < len)
                    {
                        c = tripleArray[i];
                    }
                }
                else
                {
                    result += c;

                    if (++i < len)
                    {
                        c = tripleArray[i];
                    }
                }
            }
            return result;
        }

        /// <summary> Converts the encoding of the text from Hangul triple encoding to unicode.</summary>
        /// <param name="tripleArray">- the text with the Hangul triple encoding
        /// </param>
        /// <param name="len">- the length of text to convert
        /// </param>
        /// <returns> the unicode text
        /// </returns>
        public static System.String toString(char[] tripleArray, int len)
        {
            System.String result = "";
            int i = 0;

            int cho;
            int jung;
            int jong;

            char c = tripleArray[i++];

            while (i < len)
            {
                if (c >= 0x1100 && c <= 0x1112 && i < len)
                {
                    cho = c - 0x1100;
                    c = tripleArray[i++];
                    if (c >= 0x1161 && c <= 0x1175 && i < len)
                    {
                        jung = c - 0x1161;
                        c = tripleArray[i++];
                        if (c >= 0x11A8 && c <= 0x11C2 && i < len)
                        {
                            jong = c - 0x11A7;
                            // choseong + jungseong + jongseong
                            result += (char)(0xAC00 + (cho * 21 * 28) + (jung * 28) + jong);
                            c = tripleArray[i++];
                        }
                        else
                        {
                            // choseong + jongseong
                            result += (char)(0xAC00 + (cho * 21 * 28) + (jung * 28));
                        }
                    }
                    else
                    {
                        // choseong: a single choseong is represented as ^consonant
                        char tmp = CHOSEONG_LIST[cho];
                        if (tmp == 'ㅃ' || tmp == 'ㅉ' || tmp == 'ㄸ')
                        {
                            result += CHOSEONG_LIST[cho];
                        }
                        else
                        {
                            result += ("^" + CHOSEONG_LIST[cho]);
                        }
                    }
                }
                else if (c >= 0x1161 && c <= 0x1175 && i < len)
                {
                    jung = c - 0x1161;
                    // jungseong
                    result += (char)(jung + 0x314F);
                    c = tripleArray[i++];
                }
                else if (c >= 0x11A8 && c <= 0x11C2 && i < len)
                {
                    jong = c - 0x11A7;
                    // jongseong
                    result += JONGSEONG_LIST[jong];
                    c = tripleArray[i++];
                }
                else
                {
                    result += c;
                    c = tripleArray[i++];
                }
            }
            return result;
        }

        /// <summary> It combines the specified choseong, jungseong, and jongseong to one unicode Hangul syllable. </summary>
        /// <param name="cho">- beginning consonant
        /// </param>
        /// <param name="jung">- vowel
        /// </param>
        /// <param name="jong">- final consonant
        /// </param>
        /// <returns> the combined Hangul syllable
        /// </returns>
        public static char toSyllable(char cho, char jung, char jong)
        {
            if (cho >= 0x1100 && cho <= 0x1112)
            {
                cho -= (char)(0x1100);
                if (jung >= 0x1161 && jung <= 0x1175)
                {
                    jung -= (char)(0x1161);
                    if (jong >= 0x11A8 && jong <= 0x11C2)
                    {
                        jong -= (char)(0x11A8);
                        // choseong + jungseong + jongseong
                        return (char)(0xAC00 + (cho * 21 * 28) + (jung * 28) + jong);
                    }
                    else
                    {
                        // choseong + jungseong
                        return (char)(0xAC00 + (cho * 21 * 28) + (jung * 28));
                    }
                }
                else
                {
                    // choseong
                    return CHOSEONG_LIST[cho];
                }
            }
            else if (jung >= 0x1161 && jung <= 0x1175)
            {
                jung -= (char)(0x1161);
                // jungseong
                return (char)(jung + 0x314F);
            }
            else if (jong >= 0x11A8 && jong <= 0x11C2)
            {
                jong -= (char)(0x11A);
                // jongseong
                return JONGSEONG_LIST[jong];
            }
            return HANGUL_FILLER;
        }

        /// <summary> It converts the encoding of the specified text from unicode to triple encoding.</summary>
        /// <param name="str">- the unicode text
        /// </param>
        /// <returns> the text represented in the Hangul triple encoding
        /// </returns>
        public static char[] toTripleArray(System.String str)
        {
            char[] result = null;

            List<char> charList = new List<char>();
            char c = (char)(0);
            char cho;
            char jung;
            char jong;

            for (int i = 0; i < str.Length; i++)
            {
                c = str[i];

                if (c >= 0xAC00 && c <= 0xD7AF)
                {
                    int combined = c - 0xAC00;
                    if ((cho = toJamo((char)(combined / (21 * 28)), JAMO_CHOSEONG)) != 0)
                    {
                        charList.Add(cho);
                    }
                    combined %= (21 * 28);
                    if ((jung = toJamo((char)(combined / 28), JAMO_JUNGSEONG)) != 0)
                    {
                        charList.Add(jung);
                    }
                    if ((jong = toJamo((char)(combined % 28), JAMO_JONGSEONG)) != 0)
                    {
                        charList.Add(jong);
                    }
                }
                else if (c >= 0x3131 && c <= 0x314E)
                {
                    c -= (char)(0x3131);
                    if (JONGSEONG_LIST_REV[c] != -1)
                    {
                        // a single consonant is regarded as a final consonant
                        charList.Add((char)(JONGSEONG_LIST_REV[c] + 0x11A7));
                    }
                    else if (CHOSEONG_LIST_REV[c] != -1)
                    {
                        // a single consonant which can not be a final consonant becomes a beginning consonant
                        charList.Add((char)(CHOSEONG_LIST_REV[c] + 0x1100));
                    }
                    else
                    {
                        // exception (if it occur, the conversion array has some problem)
                        charList.Add((char)(c + 0x3131));
                    }
                }
                else if (c >= 0x314F && c <= 0x3163)
                {
                    // a single vowel changes jungseong
                    charList.Add((char)(c - 0x314F + 0x1161));
                }
                else if (c == '^' && str.Length > i + 1 && str[i + 1] >= 0x3131 && str[i + 1] <= 0x314E)
                {
                    // ^consonant changes to choseong
                    c = (char)(str[i + 1] - 0x3131);
                    if (CHOSEONG_LIST_REV[c] != -1)
                    {
                        charList.Add((char)(CHOSEONG_LIST_REV[c] + 0x1100));
                        i++;
                    }
                    else
                    {
                        charList.Add('^');
                    }
                }
                else
                {
                    // other characters
                    charList.Add(c);
                }
            }

            result = new char[charList.Count];

            IEnumerator<char> iter = charList.GetEnumerator();

            for (int i = 0; iter.MoveNext(); i++)
            {
                result[i] = iter.Current;
            }

            return result;
        }

        /// <summary> It returns the unicode representation of triple encoding text.</summary>
        /// <param name="str">- the unicode text
        /// </param>
        /// <returns> the unicode representation of triple encoding text
        /// </returns>
        public static System.String toTripleString(System.String str)
        {
            char[] charArrays = toTripleArray(str);
            string txt = new string(charArrays);
            return txt;
        }
    }
}