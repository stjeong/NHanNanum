/*
Copyright (c) 2002 JSON.org

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

The Software shall be used for Good, not Evil.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/
using System;
namespace org.json
{
	
	/// <summary> A JSONArray is an ordered sequence of values. Its external text form is a
	/// string wrapped in square brackets with commas separating the values. The
	/// internal form is an object having <code>get</code> and <code>opt</code>
	/// methods for accessing the values by index, and <code>put</code> methods for
	/// adding or replacing values. The values can be any of these types:
	/// <code>Boolean</code>, <code>JSONArray</code>, <code>JSONObject</code>,
	/// <code>Number</code>, <code>String</code>, or the
	/// <code>JSONObject.NULL object</code>.
	/// <p>
	/// The constructor can convert a JSON text into a Java object. The
	/// <code>toString</code> method converts to JSON text.
	/// <p>
	/// A <code>get</code> method returns a value if one can be found, and throws an
	/// exception if one cannot be found. An <code>opt</code> method returns a
	/// default value instead of throwing an exception, and so is useful for
	/// obtaining optional values.
	/// <p>
	/// The generic <code>get()</code> and <code>opt()</code> methods return an
	/// object which you can cast or query for type. There are also typed
	/// <code>get</code> and <code>opt</code> methods that do type checking and type
	/// coercion for you.
	/// <p>
	/// The texts produced by the <code>toString</code> methods strictly conform to
	/// JSON syntax rules. The constructors are more forgiving in the texts they will
	/// accept:
	/// <ul> 
	/// <li>An extra <code>,</code>&nbsp;<small>(comma)</small> may appear just
	/// before the closing bracket.</li>
	/// <li>The <code>null</code> value will be inserted when there
	/// is <code>,</code>&nbsp;<small>(comma)</small> elision.</li>
	/// <li>Strings may be quoted with <code>'</code>&nbsp;<small>(single
	/// quote)</small>.</li>
	/// <li>Strings do not need to be quoted at all if they do not begin with a quote
	/// or single quote, and if they do not contain leading or trailing spaces,
	/// and if they do not contain any of these characters:
	/// <code>{ } [ ] / \ : , = ; #</code> and if they do not look like numbers
	/// and if they are not the reserved words <code>true</code>,
	/// <code>false</code>, or <code>null</code>.</li>
	/// <li>Values can be separated by <code>;</code> <small>(semicolon)</small> as
	/// well as by <code>,</code> <small>(comma)</small>.</li>
	/// <li>Numbers may have the 
	/// <code>0x-</code> <small>(hex)</small> prefix.</li>
	/// </ul>
	/// </summary>
	/// <author>  JSON.org
	/// </author>
	/// <version>  2009-04-14
	/// </version>
	public class JSONArray
	{
		
		
		/// <summary> The arrayList where the JSONArray's properties are kept.</summary>
		private System.Collections.ArrayList myArrayList;
		
		
		/// <summary> Construct an empty JSONArray.</summary>
		public JSONArray()
		{
			this.myArrayList = new System.Collections.ArrayList();
		}
		
		/// <summary> Construct a JSONArray from a JSONTokener.</summary>
		/// <param name="x">A JSONTokener
		/// </param>
		/// <throws>  JSONException If there is a syntax error. </throws>
		public JSONArray(JSONTokener x):this()
		{
			char c = x.nextClean();
			char q;
			if (c == '[')
			{
				q = ']';
			}
			else if (c == '(')
			{
				q = ')';
			}
			else
			{
				throw x.syntaxError("A JSONArray text must start with '['");
			}
			if (x.nextClean() == ']')
			{
				return ;
			}
			x.back();
			for (; ; )
			{
				if (x.nextClean() == ',')
				{
					x.back();
					this.myArrayList.Add(null);
				}
				else
				{
					x.back();
					this.myArrayList.Add(x.nextValue());
				}
				c = x.nextClean();
				switch (c)
				{
					
					case ';': 
					case ',': 
						if (x.nextClean() == ']')
						{
							return ;
						}
						x.back();
						break;
					
					case ']': 
					case ')': 
						if (q != c)
						{
							throw x.syntaxError("Expected a '" + q + "'");
						}
						return ;
					
					default: 
						throw x.syntaxError("Expected a ',' or ']'");
					
				}
			}
		}
		
		
		/// <summary> Construct a JSONArray from a source JSON text.</summary>
		/// <param name="source">    A string that begins with
		/// <code>[</code>&nbsp;<small>(left bracket)</small>
		/// and ends with <code>]</code>&nbsp;<small>(right bracket)</small>.
		/// </param>
		/// <throws>  JSONException If there is a syntax error. </throws>
		public JSONArray(System.String source):this(new JSONTokener(source))
		{
		}
		
		
		/// <summary> Construct a JSONArray from a Collection.</summary>
		/// <param name="collection">    A Collection.
		/// </param>
		public JSONArray(System.Collections.ICollection collection)
		{
			this.myArrayList = new System.Collections.ArrayList();
			if (collection != null)
			{
				System.Collections.IEnumerator iter = collection.GetEnumerator();
				//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
				while (iter.MoveNext())
				{
					//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
					System.Object o = iter.Current;
					this.myArrayList.Add(JSONObject.wrap(o));
				}
			}
		}
		
		
		/// <summary> Construct a JSONArray from an array</summary>
		/// <throws>  JSONException If not an array. </throws>
		public JSONArray(System.Object array):this()
		{
			if (array.GetType().IsArray)
			{
				int length = ((System.Array) array).Length;
				for (int i = 0; i < length; i += 1)
				{
					this.put(JSONObject.wrap(((System.Array) array).GetValue(i)));
				}
			}
			else
			{
				throw new JSONException("JSONArray initial value should be a string or collection or array.");
			}
		}
		
		
		/// <summary> Get the object value associated with an index.</summary>
		/// <param name="index">The index must be between 0 and length() - 1.
		/// </param>
		/// <returns> An object value.
		/// </returns>
		/// <throws>  JSONException If there is no value for the index. </throws>
		public virtual System.Object get_Renamed(int index)
		{
			System.Object o = opt(index);
			if (o == null)
			{
				throw new JSONException("JSONArray[" + index + "] not found.");
			}
			return o;
		}
		
		
		/// <summary> Get the boolean value associated with an index.
		/// The string values "true" and "false" are converted to boolean.
		/// 
		/// </summary>
		/// <param name="index">The index must be between 0 and length() - 1.
		/// </param>
		/// <returns>      The truth.
		/// </returns>
		/// <throws>  JSONException If there is no value for the index or if the </throws>
		/// <summary>  value is not convertable to boolean.
		/// </summary>
		public virtual bool getBoolean(int index)
		{
			System.Object o = get_Renamed(index);
			if (o.Equals(false) || (o is System.String && ((System.String) o).ToUpper().Equals("false".ToUpper())))
			{
				return false;
			}
			else if (o.Equals(true) || (o is System.String && ((System.String) o).ToUpper().Equals("true".ToUpper())))
			{
				return true;
			}
			throw new JSONException("JSONArray[" + index + "] is not a Boolean.");
		}
		
		
		/// <summary> Get the double value associated with an index.
		/// 
		/// </summary>
		/// <param name="index">The index must be between 0 and length() - 1.
		/// </param>
		/// <returns>      The value.
		/// </returns>
		/// <throws>    JSONException If the key is not found or if the value cannot </throws>
		/// <summary>  be converted to a number.
		/// </summary>
		public virtual double getDouble(int index)
		{
			System.Object o = get_Renamed(index);
			try
			{
				return o is System.ValueType?System.Convert.ToDouble(((System.ValueType) o)):System.Double.Parse((System.String) o);
			}
			catch (System.Exception e)
			{
				throw new JSONException("JSONArray[" + index + "] is not a number.");
			}
		}
		
		
		/// <summary> Get the int value associated with an index.
		/// 
		/// </summary>
		/// <param name="index">The index must be between 0 and length() - 1.
		/// </param>
		/// <returns>      The value.
		/// </returns>
		/// <throws>    JSONException If the key is not found or if the value cannot </throws>
		/// <summary>  be converted to a number.
		/// if the value cannot be converted to a number.
		/// </summary>
		public virtual int getInt(int index)
		{
			System.Object o = get_Renamed(index);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return o is System.ValueType?System.Convert.ToInt32(((System.ValueType) o)):(int) getDouble(index);
		}
		
		
		/// <summary> Get the JSONArray associated with an index.</summary>
		/// <param name="index">The index must be between 0 and length() - 1.
		/// </param>
		/// <returns>      A JSONArray value.
		/// </returns>
		/// <throws>  JSONException If there is no value for the index. or if the </throws>
		/// <summary> value is not a JSONArray
		/// </summary>
		public virtual JSONArray getJSONArray(int index)
		{
			System.Object o = get_Renamed(index);
			if (o is JSONArray)
			{
				return (JSONArray) o;
			}
			throw new JSONException("JSONArray[" + index + "] is not a JSONArray.");
		}
		
		
		/// <summary> Get the JSONObject associated with an index.</summary>
		/// <param name="index">subscript
		/// </param>
		/// <returns>      A JSONObject value.
		/// </returns>
		/// <throws>  JSONException If there is no value for the index or if the </throws>
		/// <summary> value is not a JSONObject
		/// </summary>
		public virtual JSONObject getJSONObject(int index)
		{
			System.Object o = get_Renamed(index);
			if (o is JSONObject)
			{
				return (JSONObject) o;
			}
			throw new JSONException("JSONArray[" + index + "] is not a JSONObject.");
		}
		
		
		/// <summary> Get the long value associated with an index.
		/// 
		/// </summary>
		/// <param name="index">The index must be between 0 and length() - 1.
		/// </param>
		/// <returns>      The value.
		/// </returns>
		/// <throws>    JSONException If the key is not found or if the value cannot </throws>
		/// <summary>  be converted to a number.
		/// </summary>
		public virtual long getLong(int index)
		{
			System.Object o = get_Renamed(index);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return o is System.ValueType?System.Convert.ToInt64(((System.ValueType) o)):(long) getDouble(index);
		}
		
		
		/// <summary> Get the string associated with an index.</summary>
		/// <param name="index">The index must be between 0 and length() - 1.
		/// </param>
		/// <returns>      A string value.
		/// </returns>
		/// <throws>  JSONException If there is no value for the index. </throws>
		public virtual System.String getString(int index)
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			return get_Renamed(index).ToString();
		}
		
		
		/// <summary> Determine if the value is null.</summary>
		/// <param name="index">The index must be between 0 and length() - 1.
		/// </param>
		/// <returns> true if the value at the index is null, or if there is no value.
		/// </returns>
		public virtual bool isNull(int index)
		{
			return JSONObject.NULL.Equals(opt(index));
		}
		
		
		/// <summary> Make a string from the contents of this JSONArray. The
		/// <code>separator</code> string is inserted between each element.
		/// Warning: This method assumes that the data structure is acyclical.
		/// </summary>
		/// <param name="separator">A string that will be inserted between the elements.
		/// </param>
		/// <returns> a string.
		/// </returns>
		/// <throws>  JSONException If the array contains an invalid number. </throws>
		public virtual System.String join(System.String separator)
		{
			int len = length();
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			
			for (int i = 0; i < len; i += 1)
			{
				if (i > 0)
				{
					sb.Append(separator);
				}
				sb.Append(JSONObject.valueToString(this.myArrayList[i]));
			}
			return sb.ToString();
		}
		
		
		/// <summary> Get the number of elements in the JSONArray, included nulls.
		/// 
		/// </summary>
		/// <returns> The length (or size).
		/// </returns>
		public virtual int length()
		{
			return this.myArrayList.Count;
		}
		
		
		/// <summary> Get the optional object value associated with an index.</summary>
		/// <param name="index">The index must be between 0 and length() - 1.
		/// </param>
		/// <returns>      An object value, or null if there is no
		/// object at that index.
		/// </returns>
		public virtual System.Object opt(int index)
		{
			return (index < 0 || index >= length())?null:this.myArrayList[index];
		}
		
		
		/// <summary> Get the optional boolean value associated with an index.
		/// It returns false if there is no value at that index,
		/// or if the value is not Boolean.TRUE or the String "true".
		/// 
		/// </summary>
		/// <param name="index">The index must be between 0 and length() - 1.
		/// </param>
		/// <returns>      The truth.
		/// </returns>
		public virtual bool optBoolean(int index)
		{
			return optBoolean(index, false);
		}
		
		
		/// <summary> Get the optional boolean value associated with an index.
		/// It returns the defaultValue if there is no value at that index or if
		/// it is not a Boolean or the String "true" or "false" (case insensitive).
		/// 
		/// </summary>
		/// <param name="index">The index must be between 0 and length() - 1.
		/// </param>
		/// <param name="defaultValue">    A boolean default.
		/// </param>
		/// <returns>      The truth.
		/// </returns>
		public virtual bool optBoolean(int index, bool defaultValue)
		{
			try
			{
				return getBoolean(index);
			}
			catch (System.Exception e)
			{
				return defaultValue;
			}
		}
		
		
		/// <summary> Get the optional double value associated with an index.
		/// NaN is returned if there is no value for the index,
		/// or if the value is not a number and cannot be converted to a number.
		/// 
		/// </summary>
		/// <param name="index">The index must be between 0 and length() - 1.
		/// </param>
		/// <returns>      The value.
		/// </returns>
		public virtual double optDouble(int index)
		{
			return optDouble(index, System.Double.NaN);
		}
		
		
		/// <summary> Get the optional double value associated with an index.
		/// The defaultValue is returned if there is no value for the index,
		/// or if the value is not a number and cannot be converted to a number.
		/// 
		/// </summary>
		/// <param name="index">subscript
		/// </param>
		/// <param name="defaultValue">    The default value.
		/// </param>
		/// <returns>      The value.
		/// </returns>
		public virtual double optDouble(int index, double defaultValue)
		{
			try
			{
				return getDouble(index);
			}
			catch (System.Exception e)
			{
				return defaultValue;
			}
		}
		
		
		/// <summary> Get the optional int value associated with an index.
		/// Zero is returned if there is no value for the index,
		/// or if the value is not a number and cannot be converted to a number.
		/// 
		/// </summary>
		/// <param name="index">The index must be between 0 and length() - 1.
		/// </param>
		/// <returns>      The value.
		/// </returns>
		public virtual int optInt(int index)
		{
			return optInt(index, 0);
		}
		
		
		/// <summary> Get the optional int value associated with an index.
		/// The defaultValue is returned if there is no value for the index,
		/// or if the value is not a number and cannot be converted to a number.
		/// </summary>
		/// <param name="index">The index must be between 0 and length() - 1.
		/// </param>
		/// <param name="defaultValue">    The default value.
		/// </param>
		/// <returns>      The value.
		/// </returns>
		public virtual int optInt(int index, int defaultValue)
		{
			try
			{
				return getInt(index);
			}
			catch (System.Exception e)
			{
				return defaultValue;
			}
		}
		
		
		/// <summary> Get the optional JSONArray associated with an index.</summary>
		/// <param name="index">subscript
		/// </param>
		/// <returns>      A JSONArray value, or null if the index has no value,
		/// or if the value is not a JSONArray.
		/// </returns>
		public virtual JSONArray optJSONArray(int index)
		{
			System.Object o = opt(index);
			return o is JSONArray?(JSONArray) o:null;
		}
		
		
		/// <summary> Get the optional JSONObject associated with an index.
		/// Null is returned if the key is not found, or null if the index has
		/// no value, or if the value is not a JSONObject.
		/// 
		/// </summary>
		/// <param name="index">The index must be between 0 and length() - 1.
		/// </param>
		/// <returns>      A JSONObject value.
		/// </returns>
		public virtual JSONObject optJSONObject(int index)
		{
			System.Object o = opt(index);
			return o is JSONObject?(JSONObject) o:null;
		}
		
		
		/// <summary> Get the optional long value associated with an index.
		/// Zero is returned if there is no value for the index,
		/// or if the value is not a number and cannot be converted to a number.
		/// 
		/// </summary>
		/// <param name="index">The index must be between 0 and length() - 1.
		/// </param>
		/// <returns>      The value.
		/// </returns>
		public virtual long optLong(int index)
		{
			return optLong(index, 0);
		}
		
		
		/// <summary> Get the optional long value associated with an index.
		/// The defaultValue is returned if there is no value for the index,
		/// or if the value is not a number and cannot be converted to a number.
		/// </summary>
		/// <param name="index">The index must be between 0 and length() - 1.
		/// </param>
		/// <param name="defaultValue">    The default value.
		/// </param>
		/// <returns>      The value.
		/// </returns>
		public virtual long optLong(int index, long defaultValue)
		{
			try
			{
				return getLong(index);
			}
			catch (System.Exception e)
			{
				return defaultValue;
			}
		}
		
		
		/// <summary> Get the optional string value associated with an index. It returns an
		/// empty string if there is no value at that index. If the value
		/// is not a string and is not null, then it is coverted to a string.
		/// 
		/// </summary>
		/// <param name="index">The index must be between 0 and length() - 1.
		/// </param>
		/// <returns>      A String value.
		/// </returns>
		public virtual System.String optString(int index)
		{
			return optString(index, "");
		}
		
		
		/// <summary> Get the optional string associated with an index.
		/// The defaultValue is returned if the key is not found.
		/// 
		/// </summary>
		/// <param name="index">The index must be between 0 and length() - 1.
		/// </param>
		/// <param name="defaultValue">    The default value.
		/// </param>
		/// <returns>      A String value.
		/// </returns>
		public virtual System.String optString(int index, System.String defaultValue)
		{
			System.Object o = opt(index);
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			return o != null?o.ToString():defaultValue;
		}
		
		
		/// <summary> Append a boolean value. This increases the array's length by one.
		/// 
		/// </summary>
		/// <param name="value">A boolean value.
		/// </param>
		/// <returns> this.
		/// </returns>
		public virtual JSONArray put(bool value_Renamed)
		{
			put((System.Object) (value_Renamed?true:false));
			return this;
		}
		
		
		/// <summary> Put a value in the JSONArray, where the value will be a
		/// JSONArray which is produced from a Collection.
		/// </summary>
		/// <param name="value">A Collection value.
		/// </param>
		/// <returns>      this.
		/// </returns>
		public virtual JSONArray put(System.Collections.ICollection value_Renamed)
		{
			put(new JSONArray(value_Renamed));
			return this;
		}
		
		
		/// <summary> Append a double value. This increases the array's length by one.
		/// 
		/// </summary>
		/// <param name="value">A double value.
		/// </param>
		/// <throws>  JSONException if the value is not finite. </throws>
		/// <returns> this.
		/// </returns>
		public virtual JSONArray put(double value_Renamed)
		{
			System.Double d = (double) value_Renamed;
			JSONObject.testValidity((System.Object) d);
			put((System.Object) d);
			return this;
		}
		
		
		/// <summary> Append an int value. This increases the array's length by one.
		/// 
		/// </summary>
		/// <param name="value">An int value.
		/// </param>
		/// <returns> this.
		/// </returns>
		public virtual JSONArray put(int value_Renamed)
		{
			put((System.Object) value_Renamed);
			return this;
		}
		
		
		/// <summary> Append an long value. This increases the array's length by one.
		/// 
		/// </summary>
		/// <param name="value">A long value.
		/// </param>
		/// <returns> this.
		/// </returns>
		public virtual JSONArray put(long value_Renamed)
		{
			put((System.Object) value_Renamed);
			return this;
		}
		
		
		/// <summary> Put a value in the JSONArray, where the value will be a
		/// JSONObject which is produced from a Map.
		/// </summary>
		/// <param name="value">A Map value.
		/// </param>
		/// <returns>      this.
		/// </returns>
		public virtual JSONArray put(System.Collections.IDictionary value_Renamed)
		{
			put(new JSONObject(value_Renamed));
			return this;
		}
		
		
		/// <summary> Append an object value. This increases the array's length by one.</summary>
		/// <param name="value">An object value.  The value should be a
		/// Boolean, Double, Integer, JSONArray, JSONObject, Long, or String, or the
		/// JSONObject.NULL object.
		/// </param>
		/// <returns> this.
		/// </returns>
		public virtual JSONArray put(System.Object value_Renamed)
		{
			this.myArrayList.Add(value_Renamed);
			return this;
		}
		
		
		/// <summary> Put or replace a boolean value in the JSONArray. If the index is greater
		/// than the length of the JSONArray, then null elements will be added as
		/// necessary to pad it out.
		/// </summary>
		/// <param name="index">The subscript.
		/// </param>
		/// <param name="value">A boolean value.
		/// </param>
		/// <returns> this.
		/// </returns>
		/// <throws>  JSONException If the index is negative. </throws>
		public virtual JSONArray put(int index, bool value_Renamed)
		{
			put(index, (System.Object) (value_Renamed?true:false));
			return this;
		}
		
		
		/// <summary> Put a value in the JSONArray, where the value will be a
		/// JSONArray which is produced from a Collection.
		/// </summary>
		/// <param name="index">The subscript.
		/// </param>
		/// <param name="value">A Collection value.
		/// </param>
		/// <returns>      this.
		/// </returns>
		/// <throws>  JSONException If the index is negative or if the value is </throws>
		/// <summary> not finite.
		/// </summary>
		public virtual JSONArray put(int index, System.Collections.ICollection value_Renamed)
		{
			put(index, new JSONArray(value_Renamed));
			return this;
		}
		
		
		/// <summary> Put or replace a double value. If the index is greater than the length of
		/// the JSONArray, then null elements will be added as necessary to pad
		/// it out.
		/// </summary>
		/// <param name="index">The subscript.
		/// </param>
		/// <param name="value">A double value.
		/// </param>
		/// <returns> this.
		/// </returns>
		/// <throws>  JSONException If the index is negative or if the value is </throws>
		/// <summary> not finite.
		/// </summary>
		public virtual JSONArray put(int index, double value_Renamed)
		{
			put(index, (System.Object) value_Renamed);
			return this;
		}
		
		
		/// <summary> Put or replace an int value. If the index is greater than the length of
		/// the JSONArray, then null elements will be added as necessary to pad
		/// it out.
		/// </summary>
		/// <param name="index">The subscript.
		/// </param>
		/// <param name="value">An int value.
		/// </param>
		/// <returns> this.
		/// </returns>
		/// <throws>  JSONException If the index is negative. </throws>
		public virtual JSONArray put(int index, int value_Renamed)
		{
			put(index, (System.Object) value_Renamed);
			return this;
		}
		
		
		/// <summary> Put or replace a long value. If the index is greater than the length of
		/// the JSONArray, then null elements will be added as necessary to pad
		/// it out.
		/// </summary>
		/// <param name="index">The subscript.
		/// </param>
		/// <param name="value">A long value.
		/// </param>
		/// <returns> this.
		/// </returns>
		/// <throws>  JSONException If the index is negative. </throws>
		public virtual JSONArray put(int index, long value_Renamed)
		{
			put(index, (System.Object) value_Renamed);
			return this;
		}
		
		
		/// <summary> Put a value in the JSONArray, where the value will be a
		/// JSONObject which is produced from a Map.
		/// </summary>
		/// <param name="index">The subscript.
		/// </param>
		/// <param name="value">The Map value.
		/// </param>
		/// <returns>      this.
		/// </returns>
		/// <throws>  JSONException If the index is negative or if the the value is </throws>
		/// <summary>  an invalid number.
		/// </summary>
		public virtual JSONArray put(int index, System.Collections.IDictionary value_Renamed)
		{
			put(index, new JSONObject(value_Renamed));
			return this;
		}
		
		
		/// <summary> Put or replace an object value in the JSONArray. If the index is greater
		/// than the length of the JSONArray, then null elements will be added as
		/// necessary to pad it out.
		/// </summary>
		/// <param name="index">The subscript.
		/// </param>
		/// <param name="value">The value to put into the array. The value should be a
		/// Boolean, Double, Integer, JSONArray, JSONObject, Long, or String, or the
		/// JSONObject.NULL object.
		/// </param>
		/// <returns> this.
		/// </returns>
		/// <throws>  JSONException If the index is negative or if the the value is </throws>
		/// <summary>  an invalid number.
		/// </summary>
		public virtual JSONArray put(int index, System.Object value_Renamed)
		{
			JSONObject.testValidity(value_Renamed);
			if (index < 0)
			{
				throw new JSONException("JSONArray[" + index + "] not found.");
			}
			if (index < length())
			{
				this.myArrayList[index] = value_Renamed;
			}
			else
			{
				while (index != length())
				{
					put(JSONObject.NULL);
				}
				put(value_Renamed);
			}
			return this;
		}
		
		
		/// <summary> Remove an index and close the hole.</summary>
		/// <param name="index">The index of the element to be removed.
		/// </param>
		/// <returns> The value that was associated with the index,
		/// or null if there was no value.
		/// </returns>
		public virtual System.Object remove(int index)
		{
			System.Object o = opt(index);
			this.myArrayList.RemoveAt(index);
			return o;
		}
		
		
		/// <summary> Produce a JSONObject by combining a JSONArray of names with the values
		/// of this JSONArray.
		/// </summary>
		/// <param name="names">A JSONArray containing a list of key strings. These will be
		/// paired with the values.
		/// </param>
		/// <returns> A JSONObject, or null if there are no names or if this JSONArray
		/// has no values.
		/// </returns>
		/// <throws>  JSONException If any of the names are null. </throws>
		public virtual JSONObject toJSONObject(JSONArray names)
		{
			if (names == null || names.length() == 0 || length() == 0)
			{
				return null;
			}
			JSONObject jo = new JSONObject();
			for (int i = 0; i < names.length(); i += 1)
			{
				jo.put(names.getString(i), this.opt(i));
			}
			return jo;
		}
		
		
		/// <summary> Make a JSON text of this JSONArray. For compactness, no
		/// unnecessary whitespace is added. If it is not possible to produce a
		/// syntactically correct JSON text then null will be returned instead. This
		/// could occur if the array contains an invalid number.
		/// <p>
		/// Warning: This method assumes that the data structure is acyclical.
		/// 
		/// </summary>
		/// <returns> a printable, displayable, transmittable
		/// representation of the array.
		/// </returns>
		public override System.String ToString()
		{
			try
			{
				return '[' + join(",") + ']';
			}
			catch (System.Exception e)
			{
				return null;
			}
		}
		
		
		/// <summary> Make a prettyprinted JSON text of this JSONArray.
		/// Warning: This method assumes that the data structure is acyclical.
		/// </summary>
		/// <param name="indentFactor">The number of spaces to add to each level of
		/// indentation.
		/// </param>
		/// <returns> a printable, displayable, transmittable
		/// representation of the object, beginning
		/// with <code>[</code>&nbsp;<small>(left bracket)</small> and ending
		/// with <code>]</code>&nbsp;<small>(right bracket)</small>.
		/// </returns>
		/// <throws>  JSONException </throws>
		public virtual System.String toString(int indentFactor)
		{
			return toString(indentFactor, 0);
		}
		
		
		/// <summary> Make a prettyprinted JSON text of this JSONArray.
		/// Warning: This method assumes that the data structure is acyclical.
		/// </summary>
		/// <param name="indentFactor">The number of spaces to add to each level of
		/// indentation.
		/// </param>
		/// <param name="indent">The indention of the top level.
		/// </param>
		/// <returns> a printable, displayable, transmittable
		/// representation of the array.
		/// </returns>
		/// <throws>  JSONException </throws>
		internal virtual System.String toString(int indentFactor, int indent)
		{
			int len = length();
			if (len == 0)
			{
				return "[]";
			}
			int i;
			System.Text.StringBuilder sb = new System.Text.StringBuilder("[");
			if (len == 1)
			{
				sb.Append(JSONObject.valueToString(this.myArrayList[0], indentFactor, indent));
			}
			else
			{
				int newindent = indent + indentFactor;
				sb.Append('\n');
				for (i = 0; i < len; i += 1)
				{
					if (i > 0)
					{
						sb.Append(",\n");
					}
					for (int j = 0; j < newindent; j += 1)
					{
						sb.Append(' ');
					}
					sb.Append(JSONObject.valueToString(this.myArrayList[i], indentFactor, newindent));
				}
				sb.Append('\n');
				for (i = 0; i < indent; i += 1)
				{
					sb.Append(' ');
				}
			}
			sb.Append(']');
			return sb.ToString();
		}
		
		
		/// <summary> Write the contents of the JSONArray as JSON text to a writer.
		/// For compactness, no whitespace is added.
		/// <p>
		/// Warning: This method assumes that the data structure is acyclical.
		/// 
		/// </summary>
		/// <returns> The writer.
		/// </returns>
		/// <throws>  JSONException </throws>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Writer' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public virtual System.IO.StreamWriter write(System.IO.StreamWriter writer)
		{
			try
			{
				bool b = false;
				int len = length();
				
				writer.Write('[');
				
				for (int i = 0; i < len; i += 1)
				{
					if (b)
					{
						writer.Write(',');
					}
					System.Object v = this.myArrayList[i];
					if (v is JSONObject)
					{
						((JSONObject) v).write(writer);
					}
					else if (v is JSONArray)
					{
						((JSONArray) v).write(writer);
					}
					else
					{
						writer.Write(JSONObject.valueToString(v));
					}
					b = true;
				}
				writer.Write(']');
				return writer;
			}
			catch (System.IO.IOException e)
			{
				throw new JSONException(e);
			}
		}
	}
}