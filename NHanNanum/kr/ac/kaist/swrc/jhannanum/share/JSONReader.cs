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
using JSONArray = org.json.JSONArray;
using JSONException = org.json.JSONException;
using JSONObject = org.json.JSONObject;
namespace kr.ac.kaist.swrc.jhannanum.share
{
	
	/// <summary> <code>JSONReader</code> is for reading data from the configuration files for each plug-in.
	/// 
	/// </summary>
	/// <seealso cref="<a href="http://json.org">http://json.org</a>">
	/// </seealso>
	/// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
	/// </author>
	public class JSONReader
	{
		/// <summary> Returns the name of the plug-in.</summary>
		/// <returns> the name of the plug-in
		/// </returns>
		/// <throws>  JSONException </throws>
		virtual public System.String Name
		{
			get
			{
				return json.getString("name");
			}
			
		}
		/// <summary> Returns the version of the plug-in.</summary>
		/// <returns> the version of the plug-in
		/// </returns>
		/// <throws>  JSONException </throws>
		virtual public System.String Version
		{
			get
			{
				return json.getString("version");
			}
			
		}
		/// <summary> Returns the author of the plug-in</summary>
		/// <returns> the author of the plug-in
		/// </returns>
		/// <throws>  JSONException </throws>
		virtual public System.String Author
		{
			get
			{
				System.String res = "";
				
				JSONArray array = json.getJSONArray("author");
				JSONObject obj = null;
				for (int i = 0; i < array.length(); i++)
				{
					if (i > 0)
					{
						res += ", ";
					}
					obj = array.getJSONObject(i);
					if (!obj.getString("email").Equals("null"))
					{
						res += (obj.getString("name") + "<" + obj.getString("email") + ">");
					}
					else
					{
						res += obj.getString("name");
					}
				}
				return res;
			}
			
		}
		/// <summary> Returns the description of the plug-in</summary>
		/// <returns> the description of the plug-in
		/// </returns>
		/// <throws>  JSONException </throws>
		virtual public System.String Description
		{
			get
			{
				return json.getString("description");
			}
			
		}
		/// <summary> Returns the type of the plug-in</summary>
		/// <returns> the type of the plug-in
		/// </returns>
		/// <throws>  JSONException </throws>
		virtual public System.String Type
		{
			get
			{
				return json.getString("type");
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Returns the file path of the configuration file.</summary>
		/// <returns> the file path of the configuration file
		/// </returns>
		/// <summary> Sets the configuration file for a plug-in.</summary>
		/// <param name="filePath">- the configuration file path
		/// </param>
		virtual public System.String FilePath
		{
			get
			{
				return filePath;
			}
			
			set
			{
				this.filePath = value;
			}
			
		}
		/// <summary>the path of the json file </summary>
		private System.String filePath = null;
		
		/// <summary>json object </summary>
		private JSONObject json = null;
		
		/// <summary> Constructor.</summary>
		/// <param name="filePath">- the file path of the plug-in configuration file
		/// </param>
		/// <throws>  JSONException </throws>
		/// <throws>  IOException </throws>
		public JSONReader(System.String filePath)
		{
			System.IO.StreamReader reader = new System.IO.StreamReader(filePath, System.Text.Encoding.UTF8);
			System.Text.StringBuilder buf = new System.Text.StringBuilder();
			char[] cbuf = new char[4096];
			int idx = 0;
			
			while ((idx = reader.Read((System.Char[]) cbuf, 0, cbuf.Length)) > 1)
			{
				buf.Append(cbuf, 0, idx);
			}
			json = new JSONObject(buf.ToString());
			
			reader.Close();
		}
		
		/// <summary> Returns the value mapped with the specified key.</summary>
		/// <returns> the value mapped with the specified key
		/// </returns>
		/// <throws>  JSONException </throws>
		public virtual System.String getValue(System.String key)
		{
			return json.getString(key);
		}
	}
}