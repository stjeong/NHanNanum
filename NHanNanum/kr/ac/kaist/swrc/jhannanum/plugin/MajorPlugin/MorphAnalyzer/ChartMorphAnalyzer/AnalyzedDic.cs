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
using System.Text;
using PetShop.Data;
namespace kr.ac.kaist.swrc.jhannanum.plugin.MajorPlugin.MorphAnalyzer.ChartMorphAnalyzer
{
	
	/// <summary> This class is the data structure for the pre-analyzed dictionary.
	/// 
	/// </summary>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class AnalyzedDic
	{
		private Dictionary < String, String > dictionary;
		
		/// <summary> Constructor.</summary>
		public AnalyzedDic()
		{
            dictionary = new Dictionary<String, String>();
		}
		
		/// <summary> Constructor. It loads the pre-analyzed dictionary from data file to the hash table.
		/// The file format of dictionary should be like this: "ITEM\tCONTENT\n"
		/// 
		/// </summary>
		/// <param name="dictionaryFileName">- the path for the pre-analyzed dictionary file
		/// </param>
		/// <throws>  UnsupportedEncodingException </throws>
		/// <throws>  FileNotFoundException </throws>
		/// <throws>  IOException </throws>
		public AnalyzedDic(System.String dictionaryFileName)
		{
            dictionary = new Dictionary<String, String>();
			
			this.readDic(dictionaryFileName);
		}
		
		/// <summary> Cleans the data in the hash table.</summary>
		public virtual void  clear()
		{
			dictionary.Clear();
		}
		
		/// <summary> Returns the contents according to the specified item.</summary>
		/// <param name="item">- item to search
		/// </param>
		/// <returns> the dictionary contents for the search item
		/// </returns>
		public virtual System.String get_Renamed(System.String item)
		{
            string value;
            dictionary.TryGetValue(item, out value);
            return value;
		}
		
		/// <summary> It loads the pre-analyzed dictionary from data file to the hash table.
		/// The file format of dictionary should be like this: "ITEM\tCONTENT\n"
		/// </summary>
		/// <param name="dictionaryFileName">- the path for the pre-analyzed dictionary file
		/// </param>
		/// <throws>  UnsupportedEncodingException </throws>
		/// <throws>  FileNotFoundException </throws>
		/// <throws>  IOException </throws>
		public virtual void  readDic(System.String dictionaryFileName)
		{
			dictionary.Clear();
			System.String str = "";
			
			System.IO.StreamReader in_Renamed = new System.IO.StreamReader(
                new System.IO.FileStream(dictionaryFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                , Encoding.UTF8);
			
			while ((str = in_Renamed.ReadLine()) != null)
			{
				str.Trim();
				if (str.Equals(""))
				{
					continue;
				}

                StringTokenizer tok = new StringTokenizer(str, "\t");
				System.String key = tok.NextToken;
				System.String value_Renamed = "";
				while (tok.HasMoreTokens)
				{
                    value_Renamed += (tok.NextToken + "\n");
				}
                dictionary[key] = value_Renamed.Trim();
			}
		}
	}
}