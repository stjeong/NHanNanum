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
using Code = kr.ac.kaist.swrc.jhannanum.share.Code;
using TagSet = kr.ac.kaist.swrc.jhannanum.share.TagSet;
namespace kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.ChartMorphAnalyzer
{
	
	/// <summary> This class for expansion of morphological analysis regarding rules such as
	/// elision, contractions, and irregular rules.
	/// 
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class Exp
	{
		/*
		* TAG_TYPE_YONGS : TAG_TYPE_YONGS
		* TAG_TYPE_EOMIES : TAG_TYPE_EOMIES
		* IRR_TYPE_S : IRR_TYPE_S
		* ᆮ Irregular rule : IRR_TYPE_D
		* ᆸ Irregular rule : IRR_TYPE_B
		* ᇂ Irregular rule : IRR_TYPE_H
		* 르 Irregular rule : IRR_TYPE_REU
		* 러 Irregular rule : IRR_TYPE_REO
		*/
		
		/// <summary> The last index of pset</summary>
		private int pset_end = 0;
		
		/// <summary> The lattice style morpheme chart</summary>
		private MorphemeChart mc = null;
		
		/// <summary> Morpheme tag set</summary>
		private TagSet tagSet = null;
		
		/// <summary> The list for expansion rules.</summary>
		private System.String[][] pset = new System.String[][]{new System.String[]{"초성", "ᄀᄁᄂᄃᄄᄅᄆᄇᄈᄉᄊᄋᄌᄍᄎᄏᄐᄑᄒ"}, new System.String[]{"종성", "ᆨᆩᆪᆫᆬᆭᆮᆯᆰᆱᆲᆳᆴᆵᆶᆷᆸᆹᆺᆻᆼᆽᆾᆿᇀᇁᇂ"}, new System.String[]{"중성", "ᅡᅣᅥᅧᅩᅭᅮᅲᅳᅵᅢᅤᅦᅨᅬᅱᅴᅪᅯᅫᅰ"}, new System.String[]{"음성모음", "ᅥᅮᅧᅲᅦᅯᅱᅨ"}, new System.String[]{"양성모음", "ᅡᅩᅣᅢᅪᅬᅤ"}, new System.String[]{"중성모음", "ᅳᅵ"}, new System.String[]{"rule_것l", ""}, new System.String[]{"rule_것", "ᄂᄆᄅᆫᆯᆸ"}, new System.String[]{"rule_것r", ""}, new System.String[]{"l11", "ᅡᅣᅥᅧᅩᅭᅮᅲᅳᅵᅢᅤᅦᅨᅬᅱᅴᅪᅯᅫᅰ"}, new System.String[]{"11", " ᆫᆯᆷᆸᄂᄉ"}, new System.String[]{"r11", ""}, new System.String[]{"l11-1", "ᅡᅣᅥᅧᅩᅭᅮᅲᅳᅵᅢᅤᅦᅨᅬᅱᅴᅪᅯᅫᅰ"}, new System.String[]{"11-1", "ᄂᄉ"}, new System.String[]{"r11-1", ""}, new System.String[]{"l12", ""}, new System.String[]{"12", "ᅡᅥ"}, new System.String[]{"r12", ""}, new System.String[]{"l13", ""}, new System.String[]{"13", "ᅡ"}, new System.String[]{"r13", ""}, new System.String[]{"l14", ""}, new System.String[]{"14", "ᅥᅦᅧᅢ"}, new System.String[]{"r14", ""}, new System.String[]{"l21", "ᆯ"}, new System.String[]{"21", "ᄋ"}, new System.String[]{"r21", "ᅥᅡᅳ"}, new System.String[]{"l22", "ᅡᅥᅮᅳᅵ"}, new System.String[]{"22", "ᄋ"}, new System.String[]{"r22", "ᅥᅡᅳ"}, new System.String[]{"l23", "ᄋ"}, new System.String[]{"23", "ᅮ"}, new System.String[]{"r23", ""}, new System.String[]{"l24", "ᄋ"}, new System.String[]{"24", "ᅪ"}, new System.String[]{"r24", ""}, new System.String[]{"l25", "ᄋ"}, new System.String[]{"25", "ᅯ"}, new System.String[]{"r25", ""}, new System.String[]{"l26", "ᄀᄃᄅᄆᄋ"}, new System.String[]{"26", "ᅡᅣ"}, new System.String[]{"r26", ""}, new System.String[]{"l27", "ᄀᄃᄅᄆᄄᄋ"}, new System.String[]{"27", "ᅢᅤ"}, new System.String[]{"r27", ""}, new System.String[]{"l28", "ᄀᄃᄅᄆᄄᄋ"}, new System.String[]{"28", "ᅥ"}, new System.String[]{"r28", ""}, new System.String[]{"l29", "ᆯ"}, new System.String[]{"29", "ᄅ"}, new System.String[]{"r29", "ᅥᅡ"}, new System.String[]{"l30", "ᅳ"}, new System.String[]{"30", "ᄅ"}, new System.String[]{"r30", "ᅥ"}, new 
			System.String[]{"l31", "ᄑ"}, new System.String[]{"31", "ᅥ"}, new System.String[]{"r31", ""}, new System.String[]{"l32", "ᄒ"}, new System.String[]{"32", "ᅡ"}, new System.String[]{"r32", "ᄋ"}, new System.String[]{"l33", "ᄒ"}, new System.String[]{"33", "ᅢ"}, new System.String[]{"r33", ""}, new System.String[]{"l51", ""}, new System.String[]{"51", "ᅪᅯ"}, new System.String[]{"r51", ""}, new System.String[]{"l52", ""}, new System.String[]{"52", "ᅫ"}, new System.String[]{"r52", ""}, new System.String[]{"l53", ""}, new System.String[]{"53", "ᅧ"}, new System.String[]{"r53", ""}, new System.String[]{"l54", "ᆯᅡᅣᅥᅧᅩᅭᅮᅲᅳᅵᅢᅤᅦᅨᅬᅱᅴᅪᅯᅫᅰ"}, new System.String[]{"54", " ᆫᆯᆷᆸᄂᄅᄆᄉᄋ"}, new System.String[]{"r54", ""}};
		
		/// <summary> Constructor.</summary>
		/// <param name="mc">- the lattice style morpheme chart
		/// </param>
		/// <param name="tagSet">- morpheme tag set
		/// </param>
		public Exp(MorphemeChart mc, TagSet tagSet)
		{
			this.mc = mc;
			this.tagSet = tagSet;
			pset_end = pset.Length;
		}
		
		/// <summary> Inserts the string str2 to the specified position of the string str1.</summary>
		/// <param name="str1">- the string where the other string is going to be inserted
		/// </param>
		/// <param name="cur">- the index of the str1 for insertion
		/// </param>
		/// <param name="str2">- the string to insert
		/// </param>
		/// <returns> the new string that str2 is inserted to str1 at the specified position
		/// </returns>
		private System.String insert(System.String str1, int cur, System.String str2)
		{
			return str1.Substring(0, (cur) - (0)) + str2 + str1.Substring(cur);
		}
		
		/// <summary> It checks pset whether the rule is applied to the specified index of given string.</summary>
		/// <param name="base">- the string to check
		/// </param>
		/// <param name="idx">- the index of the string
		/// </param>
		/// <param name="rule">- the name of the rule to check
		/// </param>
		/// <returns> 0 - the rule is not applied, otherwise applied
		/// </returns>
		private int pcheck(System.String base_Renamed, int idx, System.String rule)
		{
			char c;
			
			if (idx < base_Renamed.Length)
			{
				c = base_Renamed[idx];
			}
			else
			{
				c = '\x0000';
			}
			
			for (int i = 0; i < pset_end; i++)
			{
				if (pset[i][0].Equals(rule))
				{
					if (pset[i][1].Length == 0)
					{
						return 1;
					}
					else
					{
						int index = pset[i][1].IndexOf((System.Char) c);
						if (index == - 1)
						{
							return 0;
						}
						else
						{
							return index + 1;
						}
					}
				}
			}
			return 0;
		}
		
		/// <summary> Check the rules on the given string, and expand the morpheme chart.</summary>
		/// <param name="from">- the index for the start of segment position
		/// </param>
		/// <param name="str1">- the front part of the string
		/// </param>
		/// <param name="str2">- the next part of the string
		/// </param>
		/// <param name="sp">- the segment position
		/// </param>
		public virtual void  prule(int from, System.String str1, System.String str2, SegmentPosition sp)
		{
			int i;
			rule_NP(from, str1, str2);
			// sp.printPosition();
			
			for (i = 0; i < str2.Length; i++)
			{
				rule_rem(from, str1, str2, i);
				// sp.printPosition();
				rule_irr_word(from, str1, str2, i);
				// sp.printPosition();
				rule_irr_word2(from, str1, str2, i);
				// sp.printPosition();
				rule_shorten(from, str1, str2, i);
				// sp.printPosition();
				rule_eomi_u(from, str1, str2, i);
				// sp.printPosition();
				rule_johwa(from, str1, str2, i);
				// sp.printPosition();
				rule_i(from, str1, str2, i);
				// sp.printPosition();
				rule_gut(from, str1, str2, i);
				// sp.printPosition();
			}
		}
		
		/// <summary> Replaces the character at the specified position of the string str1
		/// with the first character of the string str2.
		/// </summary>
		/// <param name="str1">- base string
		/// </param>
		/// <param name="cur">- index of the character to 
		/// </param>
		/// <param name="str2">- the first character of the string is used to replace
		/// </param>
		/// <returns> the string with the new character replaced
		/// </returns>
		private System.String replace(System.String str1, int cur, System.String str2)
		{
			char[] array = str1.ToCharArray();
			
			if (str2.Length == 0)
			{
				System.Console.Error.WriteLine("Exp.java: replace(): s is to short");
				System.Environment.Exit(0);
			}

			array[cur] = str2[0];

            return new string(array);
		}
		
		/// <summary> It expands the morpheme chart regarding the elision rule '으', '스', '느'.</summary>
		/// <param name="from">- the start index for the segment position
		/// </param>
		/// <param name="prev">- the passed part of the string
		/// </param>
		/// <param name="str">- the next part of the string to check
		/// </param>
		/// <param name="cur">- the current index of the string for checking the rules
		/// </param>
		private void  rule_eomi_u(int from, System.String prev, System.String str, int cur)
		{
			System.String buf;
			System.String buf2;
			System.String new_str;
			
			if (cur > str.Length)
			{
				return ;
			}
			
			if ((cur > 0 && pcheck(str, cur - 1, "l54") != 0) && pcheck(str, cur, "54") != 0 && pcheck(str, cur + 1, "r54") != 0)
			{
				new_str = insert(str, cur, "으");
				buf = new_str.Substring(0, (cur) - (0));
				buf2 = new_str.Substring(cur);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, 0);
			}
			if ((cur > 0 && pcheck(str, cur - 1, "l54") != 0) && strncmp(str, cur, "ᆸ니", 0, 3) == 0)
			{
				new_str = insert(str, cur, "스");
				buf = new_str.Substring(0, (cur) - (0));
				buf2 = new_str.Substring(cur);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, 0);
			}
			if ((cur > 0 && pcheck(str, cur - 1, "l54") != 0) && strncmp(str, cur, "ᆫ다", 0, 3) == 0)
			{
				new_str = insert(str, cur, "느");
				buf = new_str.Substring(0, (cur) - (0));
				buf2 = new_str.Substring(cur);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, 0);
			}
		}
		
		/// <summary> It expands the morpheme chart regarding the rules about '것'.</summary>
		/// <param name="from">- the start index for the segment position
		/// </param>
		/// <param name="prev">- the passed part of the string
		/// </param>
		/// <param name="str">- the next part of the string to check
		/// </param>
		/// <param name="cur">- the current index of the string for checking the rules
		/// </param>
		private void  rule_gut(int from, System.String prev, System.String str, int cur)
		{
			System.String buf;
			System.String buf2;
			System.String new_str;
			
			if (cur >= str.Length)
			{
				return ;
			}
			
			if (cur > 1 && strncmp(str, cur - 2, "거", 0, 2) == 0 && pcheck(str, cur, "rule_것") != 0)
			{
				
				if (str[cur] == 'ᆸ')
				{
					if (strncmp(str, cur, "ᆸ니", 0, 3) == 0)
					{
						new_str = insert(str, cur, "ᆺ이");
						buf = new_str.Substring(0, (cur + 1) - (0));
						buf2 = new_str.Substring(cur + 1);
						// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
						mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JP, 0);
					}
				}
				else
				{
					if (strncmp(str, cur, "ᆯ로", 0, 3) == 0)
					{
						new_str = replace(str, cur, "ᆺ");
						new_str = insert(new_str, cur + 1, "으");
						buf = new_str.Substring(0, (cur + 1) - (0));
						buf2 = new_str.Substring(cur + 1);
						// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
						mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JOSA, 0);
					}
					else if (str[cur] == 'ᆯ' || str[cur] == 'ᆫ')
					{
						if (str.Length != cur + 1)
						{
							new_str = insert(str, cur, "ᆺ이");
							buf = new_str.Substring(0, (cur + 1) - (0));
							buf2 = new_str.Substring(cur + 1);
							// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
							mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JP, 0);
						}
						
						new_str = insert(str, cur, "ᆺ으");
						buf = new_str.Substring(0, (cur + 1) - (0));
						buf2 = new_str.Substring(cur + 1);
						// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
						mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JOSA, 0);
					}
					else
					{
						new_str = insert(str, cur, "ᆺ이");
						buf = new_str.Substring(0, (cur + 1) - (0));
						buf2 = new_str.Substring(cur + 1);
						// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
						mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JP, 0);
					}
				}
			}
		}
		
		/// <summary> It expands the morpheme chart regarding the rule '이'.</summary>
		/// <param name="from">- the start index for the segment position
		/// </param>
		/// <param name="prev">- the passed part of the string
		/// </param>
		/// <param name="str">- the next part of the string to check
		/// </param>
		/// <param name="cur">- the current index of the string for checking the rules
		/// </param>
		private void  rule_i(int from, System.String prev, System.String str, int cur)
		{
			System.String buf;
			System.String buf2;
			System.String new_str;
			
			if (cur + 2 > str.Length)
			{
				return ;
			}
			
			if ((prev != null && prev.Length != 0 && cur == 0) && pcheck(prev, prev.Length - 1, "중성") != 0)
			{
				
				if (strncmp(str, 0, "여", 0, 2) == 0)
				{
					new_str = replace(str, cur + 1, "ᅥ");
					new_str = insert(new_str, cur + 1, "ᅵᄋ");
					buf = new_str.Substring(0, (cur + 2) - (0));
					buf2 = new_str.Substring(cur + 2);
					// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
					mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_JP, TagSet.TAG_TYPE_EOMIES, 0);
				}
				else
				{
					if (pcheck(str, 0, "종성") != 0 || strncmp(str, 0, "는", 0, 3) == 0 || strncmp(str, 0, "은", 0, 3) == 0 || strncmp(str, 0, "음", 0, 3) == 0 || strncmp(str, 2, "는", 0, 3) == 0)
						return ;
					// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
					mc.phonemeChange(from, "이", str, TagSet.TAG_TYPE_JP, TagSet.TAG_TYPE_EOMIES, 0);
					buf = "이" + str;
					rule_eomi_u(from, prev, buf, cur + 2);
				}
			}
		}
		
		/// <summary> It expands the morpheme chart regarding the irregular rules about 'ㄷ', 'ㅅ', 'ㅂ', 'ㅎ', '르', '러'.</summary>
		/// <param name="from">- the start index for the segment position
		/// </param>
		/// <param name="prev">- the passed part of the string
		/// </param>
		/// <param name="str">- the next part of the string to check
		/// </param>
		/// <param name="cur">- the current index of the string for checking the rules
		/// </param>
		private void  rule_irr_word(int from, System.String prev, System.String str, int cur)
		{
			System.String buf;
			System.String buf2;
			System.String new_str;
			int len = str.Length;
			
			/* 'ᆮ' irregular rule */
			if ((cur > 0 && cur <= len && pcheck(str, cur - 1, "l21") != 0) && pcheck(str, cur, "21") != 0 && pcheck(str, cur + 1, "r21") != 0)
			{
				new_str = replace(str, cur - 1, "ᆮ");
				buf = new_str.Substring(0, (cur) - (0));
				buf2 = new_str.Substring(cur);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, tagSet.IRR_TYPE_D);
			}
			
			/* 'ᆺ' irregular rule */
			if ((cur > 0 && cur < len && pcheck(str, cur - 1, "l22") != 0) && pcheck(str, cur, "22") != 0 && pcheck(str, cur + 1, "r22") != 0)
			{
				new_str = insert(str, cur, "ᆺ");
				buf = new_str.Substring(0, (cur + 1) - (0));
				buf2 = new_str.Substring(cur + 1);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, tagSet.IRR_TYPE_S);
			}
			
			/* 'ㅂ' irregular rule */
			if ((cur > 0 && cur <= len && pcheck(str, cur - 1, "l23") != 0) && pcheck(str, cur, "23") != 0 && pcheck(str, cur + 1, "r23") != 0)
			{
				new_str = replace(str, cur, "ᅳ");
				new_str = insert(new_str, cur - 1, "ᆸ");
				buf = new_str.Substring(0, (cur) - (0));
				buf2 = new_str.Substring(cur);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, tagSet.IRR_TYPE_B);
			}
			
			/* 'ᆸ' irregular rule */
			if ((cur > 0 && cur <= len && pcheck(str, cur - 1, "l24") != 0) && pcheck(str, cur, "24") != 0 && pcheck(str, cur + 1, "r24") != 0)
			{
				new_str = replace(str, cur, "ᅥ");
				new_str = insert(new_str, cur - 1, "ᆸ");
				buf = new_str.Substring(0, (cur) - (0));
				buf2 = new_str.Substring(cur);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, tagSet.IRR_TYPE_B);
			}
			
			/* 'ㅂ' irregular rule */
			if ((cur > 0 && cur <= len && pcheck(str, cur - 1, "l25") != 0) && pcheck(str, cur, "25") != 0 && pcheck(str, cur + 1, "r25") != 0)
			{
				new_str = replace(str, cur, "ᅥ");
				new_str = insert(new_str, cur - 1, "ᆸ");
				buf = new_str.Substring(0, (cur) - (0));
				buf2 = new_str.Substring(cur);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, tagSet.IRR_TYPE_B);
			}
			
			/* 'ᇂ' irregular rule */
			if ((cur > 0 && cur + 1 < len && pcheck(str, cur - 1, "l26") != 0) && pcheck(str, cur, "26") != 0 && pcheck(str, cur + 1, "r26") != 0)
			{
				new_str = insert(str, cur + 1, "ᇂ으");
				buf = new_str.Substring(0, (cur + 2) - (0));
				buf2 = new_str.Substring(cur + 2);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, tagSet.IRR_TYPE_H);
			}
			
			/* 'ㅎ' irregular rule */
			if ((cur > 0 && cur + 1 < len && pcheck(str, cur - 1, "l27") != 0) && pcheck(str, cur, "27") != 0 && pcheck(str, cur + 1, "r27") != 0)
			{
				if (str[cur] == 'ᅢ')
				{
					new_str = replace(str, cur, "ᅡ");
				}
				else
				{
					new_str = replace(str, cur, "ᅣ");
				}
				new_str = insert(new_str, cur + 1, "ᇂ어");
				buf = new_str.Substring(0, (cur + 2) - (0));
				buf2 = new_str.Substring(cur + 2);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, tagSet.IRR_TYPE_H);
				//			이운재 추가
				if (str[cur] == 'ᅢ')
				{
					new_str = replace(str, cur, "ᅥ");
				}
				else
				{
					new_str = replace(str, cur, "ᅧ");
				}
				new_str = insert(new_str, cur + 1, "ᇂ어");
				buf = new_str.Substring(0, (cur + 2) - (0));
				buf2 = new_str.Substring(cur + 2);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, tagSet.IRR_TYPE_H);
			}
			
			/* 'ㅎ' irregular rule */
			if ((cur > 0 && cur + 1 < len && pcheck(str, cur - 1, "l28") != 0) && pcheck(str, cur, "28") != 0 && pcheck(str, cur + 1, "r28") != 0)
			{
				new_str = replace(str, cur, "ᅥ");
				new_str = insert(new_str, cur + 1, "ᇂᄋ");
				buf = new_str.Substring(0, (cur + 2) - (0));
				buf2 = new_str.Substring(cur + 2);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, tagSet.IRR_TYPE_H);
			}
			
			
			/* '르' irregular rule */
			if ((cur > 0 && cur < len && pcheck(str, cur - 1, "l29") != 0) && pcheck(str, cur, "29") != 0 && pcheck(str, cur + 1, "r29") != 0)
			{
				new_str = replace(str, cur, "ᅳ");
				if (new_str[cur + 1] == 'ᅡ')
					new_str = new_str.Substring(0, (cur + 1) - (0)) + 'ᅥ' + new_str.Substring(cur + 2);
				new_str = insert(new_str, cur + 1, "ᄋ");
				new_str = new_str.Substring(0, (cur - 1) - (0)) + Code.toChoseong(new_str[cur - 1]) + new_str.Substring(cur);
				
				buf = new_str.Substring(0, (cur + 1) - (0));
				buf2 = new_str.Substring(cur + 1);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, tagSet.IRR_TYPE_REU);
			}
			
			/* '러' irregular rule */
			if ((cur > 0 && cur <= len && pcheck(str, cur - 1, "l30") != 0) && pcheck(str, cur, "30") != 0 && pcheck(str, cur + 1, "r30") != 0 && (cur - 2 >= 0 && str[cur - 2] == 'ᄅ'))
			{
				new_str = replace(str, cur, "ᄋ");
				buf = new_str.Substring(0, (cur) - (0));
				buf2 = new_str.Substring(cur);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, tagSet.IRR_TYPE_REO);
			}
		}
		
		/// <summary> It expands the morpheme chart regarding the irregular rules about '우', '여'.</summary>
		/// <param name="from">- the start index for the segment position
		/// </param>
		/// <param name="prev">- the passed part of the string
		/// </param>
		/// <param name="str">- the next part of the string to check
		/// </param>
		/// <param name="cur">- the current index of the string for checking the rules
		/// </param>
		private void  rule_irr_word2(int from, System.String prev, System.String str, int cur)
		{
			System.String buf;
			System.String buf2;
			System.String new_str;
			
			if (cur >= str.Length)
			{
				return ;
			}
			
			/* '우' irregular rule */
			if ((cur > 0 && pcheck(str, cur - 1, "l31") != 0) && pcheck(str, cur, "31") != 0 && pcheck(str, cur + 1, "r31") != 0)
			{
				new_str = replace(str, cur, "ᅮ");
				new_str = insert(new_str, cur + 1, "어");
				buf = new_str.Substring(0, (cur + 1) - (0));
				buf2 = new_str.Substring(cur + 1);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, 0);
			}
			
			/* '여' irregular rule */
			if ((cur > 0 && pcheck(str, cur - 1, "l32") != 0) && pcheck(str, cur, "32") != 0 && pcheck(str, cur + 1, "r32") != 0 && str[cur + 2] == 'ᅧ')
			{
				new_str = replace(str, cur + 2, "ᅥ");
				buf = new_str.Substring(0, (cur + 1) - (0));
				buf2 = new_str.Substring(cur + 1);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, 0);
			}
			
			/* '여' irregular rule */
			if ((cur > 0 && pcheck(str, cur - 1, "l33") != 0) && pcheck(str, cur, "33") != 0 && pcheck(str, cur + 1, "r33") != 0)
			{
				new_str = replace(str, cur, "ᅡ");
				new_str = insert(new_str, cur + 1, "어");
				buf = new_str.Substring(0, (cur + 1) - (0));
				buf2 = new_str.Substring(cur + 1);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, 0);
			}
		}
		
		/// <summary> It expands the morpheme chart regarding the vowel harmony rules.</summary>
		/// <param name="from">- the start index for the segment position
		/// </param>
		/// <param name="prev">- the passed part of the string
		/// </param>
		/// <param name="str">- the next part of the string to check
		/// </param>
		/// <param name="cur">- the current index of the string for checking the rules
		/// </param>
		private void  rule_johwa(int from, System.String prev, System.String str, int cur)
		{
			System.String buf;
			System.String buf2;
			System.String new_str;
			if (cur > 0 && pcheck(str, cur - 1, "양성모음") != 0)
			{
				if (cur + 2 < str.Length && str[cur + 1] == 'ᄋ' && str[cur + 2] == 'ᅡ')
				{
					new_str = replace(str, cur + 2, "ᅥ");
					buf = new_str.Substring(0, (cur + 1) - (0));
					buf2 = new_str.Substring(cur + 1);
					// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
					mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, 0);
				}
				else if (cur + 1 < str.Length && str[cur] == 'ᄋ' && str[cur + 1] == 'ᅡ')
				{
					new_str = replace(str, cur + 1, "ᅥ");
					buf = new_str.Substring(0, (cur) - (0));
					buf2 = new_str.Substring(cur);
					// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
					mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, 0);
				}
			}
		}
		
		/// <summary> It expands the morpheme chart regarding the rules about personal pronoun.</summary>
		/// <param name="from">- the start index for the segment position
		/// </param>
		/// <param name="prev">- the passed part of the string
		/// </param>
		/// <param name="str">- the next part of the string to check
		/// </param>
		private void  rule_NP(int from, System.String prev, System.String str)
		{
			System.String buf;
			
			if (strncmp(str, 0, "내가", 0, 4) == 0)
			{
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()));
				mc.phonemeChange(from, "나", str + 2, TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JOSA, 0);
			}
			else if (strncmp(str, 0, "네가", 0, 4) == 0)
			{
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()));
				mc.phonemeChange(from, "너", str + 2, TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JOSA, 0);
			}
			else if (strncmp(str, 0, "제가", 0, 4) == 0)
			{
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()));
				mc.phonemeChange(from, "저", str + 2, TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JOSA, 0);
			}
			else if (strcmp(str, 0, "내", 0) == 0)
			{
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()));
				mc.phonemeChange(from, "나", "의", TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JOSA, 0);
			}
			else if (strcmp(str, 0, "네", 0) == 0)
			{
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()));
				mc.phonemeChange(from, "너", "의", TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JOSA, 0);
			}
			else if (strcmp(str, 0, "제", 0) == 0)
			{
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()));
				mc.phonemeChange(from, "저", "의", TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JOSA, 0);
			}
			else if (strncmp(str, 0, "내게", 0, 4) == 0)
			{
				buf = "에" + str.Substring(2);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()));
				mc.phonemeChange(from, "나", buf, TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JOSA, 0);
			}
			else if (strncmp(str, 0, "네게", 0, 4) == 0)
			{
				buf = "에" + str.Substring(2);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()));
				mc.phonemeChange(from, "너", buf, TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JOSA, 0);
			}
			else if (strncmp(str, 0, "제게", 0, 4) == 0)
			{
				buf = "에" + str.Substring(2);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()));
				mc.phonemeChange(from, "저", buf, TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JOSA, 0);
			}
			else if (strncmp(str, 0, "나", 0, 2) == 0)
			{
				if (str.Length == 3 && str[2] == 'ᆫ')
				{
					// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()));
					mc.phonemeChange(from, "나", "는", TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JOSA, 0);
				}
				else if (str.Length == 3 && str[2] == 'ᆯ')
				{
					// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()));
					mc.phonemeChange(from, "나", "를", TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JOSA, 0);
				}
			}
			else if (strncmp(str, 0, "너", 0, 2) == 0)
			{
				if (str.Length == 3 && str[2] == 'ᆫ')
				{
					// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()));
					mc.phonemeChange(from, "너", "는", TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JOSA, 0);
				}
				else if (str.Length == 3 && str[2] == 'ᆯ')
				{
					// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()));
					mc.phonemeChange(from, "너", "를", TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JOSA, 0);
				}
			}
			else if (strncmp(str, 0, "누구", 0, 4) == 0)
			{
				if (str.Length == 5 && str[4] == 'ᆫ')
				{
					// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()));
					mc.phonemeChange(from, "누구", "는", TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JOSA, 0);
				}
				else if (str.Length == 5 && str[4] == 'ᆯ')
				{
					// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()));
					mc.phonemeChange(from, "누구", "를", TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JOSA, 0);
				}
			}
			else if (strcmp(str, 0, "무언가", 0) == 0)
			{
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()));
				mc.phonemeChange(from, "무엇", "인가", TagSet.TAG_TYPE_NBNP, TagSet.TAG_TYPE_JOSA, 0);
			}
		}
		
		/// <summary> It expands the morpheme chart regarding the elision rules about 'ㄹ', 'ㅡ', 'ㅏ', 'ㅓ'.</summary>
		/// <param name="from">- the start index for the segment position
		/// </param>
		/// <param name="prev">- the passed part of the string
		/// </param>
		/// <param name="str">- the next part of the string to check
		/// </param>
		/// <param name="cur">- the current index of the string for checking the rules
		/// </param>
		private void  rule_rem(int from, System.String prev, System.String str, int cur)
		{
			System.String buf;
			System.String buf2;
			System.String new_str;
			
			if (cur >= str.Length)
			{
				return ;
			}
			
			/* 'ㄹ' elision rule */
			if ((cur > 0 && pcheck(str, cur - 1, "l11") != 0) && (pcheck(str, cur, "11") != 0 || strncmp(str, cur, "오", 0, 2) == 0) && pcheck(str, cur + 1, "r11") != 0)
			{
				
				System.String buf3;
				new_str = insert(str, cur, "ᆯ");
				buf3 = new_str;
				
				buf = new_str.Substring(0, (cur + 1) - (0));
				buf2 = new_str.Substring(cur + 1);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, 0);
				rule_eomi_u(from, prev, buf3, cur + 1);
			}
			
			/* 'ㅡ' elision rule */
			if ((cur > 0 && pcheck(str, cur - 1, "l12") != 0) && pcheck(str, cur, "12") != 0 && pcheck(str, cur + 1, "r12") != 0 || (cur == 1 && str[cur] != 'ᅡ'))
			{
				new_str = replace(str, cur, "ᅥ");
				new_str = insert(new_str, cur, "ᅳᄋ");
				buf = new_str.Substring(0, (cur + 1) - (0));
				buf2 = new_str.Substring(cur + 1);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, 0);
			}
			
			/* 'ㅏ' elision rule */
			if ((cur > 0 && pcheck(str, cur - 1, "l13") != 0) && pcheck(str, cur, "13") != 0 && pcheck(str, cur + 1, "r13") != 0)
			{
				new_str = insert(str, cur + 1, "어");
				buf = new_str.Substring(0, (cur + 1) - (0));
				buf2 = new_str.Substring(cur + 1);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, 0);
			}
			
			/* 'ㅓ' elision rule */
			if ((cur > 0 && pcheck(str, cur - 1, "l14") != 0) && pcheck(str, cur, "14") != 0 && pcheck(str, cur + 1, "r14") != 0)
			{
				new_str = insert(str, cur + 1, "어");
				buf = new_str.Substring(0, (cur + 1) - (0));
				buf2 = new_str.Substring(cur + 1);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, 0);
			}
		}
		
		/// <summary> It expands the morpheme chart regarding the contration rules about 'ㅗ', 'ㅜ', 'ㅚ', 'ㅣ'.</summary>
		/// <param name="from">- the start index for the segment position
		/// </param>
		/// <param name="prev">- the passed part of the string
		/// </param>
		/// <param name="str">- the next part of the string to check
		/// </param>
		/// <param name="cur">- the current index of the string for checking the rules
		/// </param>
		private void  rule_shorten(int from, System.String prev, System.String str, int cur)
		{
			System.String buf;
			System.String buf2;
			System.String new_str;
			if (cur >= str.Length)
			{
				return ;
			}
			
			/* 'ㅗ', 'ㅜ' contraction rule */
			if ((cur > 0 && pcheck(str, cur - 1, "l51") != 0) && pcheck(str, cur, "51") != 0 && pcheck(str, cur + 1, "r51") != 0)
			{
				if (str[cur] == 'ᅪ')
				{
					new_str = replace(str, cur, "ᅩ");
				}
				else
				{
					new_str = replace(str, cur, "ᅮ");
				}
				new_str = insert(new_str, cur + 1, "어");
				buf = new_str.Substring(0, (cur + 1) - (0));
				buf2 = new_str.Substring(cur + 1);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, 0);
			}
			
			/* 'ㅚ' contraction rule */
			if ((cur > 0 && pcheck(str, cur - 1, "l52") != 0) && pcheck(str, cur, "52") != 0 && pcheck(str, cur + 1, "r52") != 0)
			{
				new_str = replace(str, cur, "ᅬ");
				new_str = insert(new_str, cur + 1, "어");
				buf = new_str.Substring(0, (cur + 1) - (0));
				buf2 = new_str.Substring(cur + 1);
				// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
				mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, 0);
			}
			
			/* 'ㅣ' contraction rule */
			if (cur > 0)
			{
				if (((cur > 1 || (str[cur - 1] != 'ᄋ')) && pcheck(str, cur - 1, "l53") != 0) && pcheck(str, cur, "53") != 0 && pcheck(str, cur + 1, "r53") != 0)
				{
					
					new_str = replace(str, cur, "ᅵ");
					new_str = insert(new_str, cur + 1, "어");
					buf = new_str.Substring(0, (cur + 1) - (0));
					buf2 = new_str.Substring(cur + 1);
					// System.out.println("Prev: " + Code.toString(prev.toCharArray()) + ", " + "Str: " + Code.toString(str.toCharArray()) + ", " + "Cur: " + cur);
					mc.phonemeChange(from, buf, buf2, TagSet.TAG_TYPE_YONGS, TagSet.TAG_TYPE_EOMIES, 0);
				}
			}
		}
		
		/// <summary> C style string compare method.</summary>
		/// <param name="s1">- string 1
		/// </param>
		/// <param name="i1">- start index of string 1 for comparing
		/// </param>
		/// <param name="s2">- string 2
		/// </param>
		/// <param name="i2">- strart index of string 2 for comparing
		/// </param>
		/// <returns> 0 : equal, > 0 : string 1 is higher in alphabetical order, < 0 : string 1 is lower in alphabetical order
		/// </returns>
		private int strcmp(System.String s1, int i1, System.String s2, int i2)
		{
			int l1 = s1.Length - i1;
			int l2 = s2.Length - i2;
			
			int len = l1;
			bool diff = false;
			
			if (len > l2)
			{
				len = l2;
			}
			
			while (len-- > 0)
			{
				if (s1[i1++] != s2[i2++])
				{
					diff = true;
					break;
				}
			}
			
			if (diff == false && l1 != l2)
			{
				if (l1 > l2)
				{
					return s1[i1];
				}
				else
				{
					return - s2[i2];
				}
			}
			return s1[i1 - 1] - s2[i2 - 1];
		}
		
		/// <summary> C style string compare method for the specified length.</summary>
		/// <param name="s1">- string 1
		/// </param>
		/// <param name="i1">- start index of string 1 for comparing
		/// </param>
		/// <param name="s2">- string 2
		/// </param>
		/// <param name="i2">- strart index of string 2 for comparing
		/// </param>
		/// <param name="len">- the number of characters to compare
		/// </param>
		/// <returns> 0 : equal, > 0 : string 1 is higher in alphabetical order, < 0 : string 1 is lower in alphabetical order
		/// </returns>
		private int strncmp(System.String s1, int i1, System.String s2, int i2, int len)
		{
			if (s1.Length - i1 < len)
			{
				return 1;
			}
			else if (s2.Length - i2 < len)
			{
				return - 1;
			}
			while (len-- > 0)
			{
				if (s1[i1++] != s2[i2++])
				{
					break;
				}
			}
			return s1[i1 - 1] - s2[i2 - 1];
		}
	}
}