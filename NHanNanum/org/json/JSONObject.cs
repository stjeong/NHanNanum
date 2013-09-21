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
using System.Reflection;
namespace org.json
{

    /// <summary> A JSONObject is an unordered collection of name/value pairs. Its
    /// external form is a string wrapped in curly braces with colons between the
    /// names and values, and commas between the values and names. The internal form
    /// is an object having <code>get</code> and <code>opt</code> methods for
    /// accessing the values by name, and <code>put</code> methods for adding or
    /// replacing values by name. The values can be any of these types:
    /// <code>Boolean</code>, <code>JSONArray</code>, <code>JSONObject</code>,
    /// <code>Number</code>, <code>String</code>, or the <code>JSONObject.NULL</code>
    /// object. A JSONObject constructor can be used to convert an external form
    /// JSON text into an internal form whose values can be retrieved with the
    /// <code>get</code> and <code>opt</code> methods, or to convert values into a
    /// JSON text using the <code>put</code> and <code>toString</code> methods.
    /// A <code>get</code> method returns a value if one can be found, and throws an
    /// exception if one cannot be found. An <code>opt</code> method returns a
    /// default value instead of throwing an exception, and so is useful for
    /// obtaining optional values.
    /// <p>
    /// The generic <code>get()</code> and <code>opt()</code> methods return an
    /// object, which you can cast or query for type. There are also typed
    /// <code>get</code> and <code>opt</code> methods that do type checking and type
    /// coercion for you.
    /// <p>
    /// The <code>put</code> methods adds values to an object. For example, <pre>
    /// myString = new JSONObject().put("JSON", "Hello, World!").toString();</pre>
    /// produces the string <code>{"JSON": "Hello, World"}</code>.
    /// <p>
    /// The texts produced by the <code>toString</code> methods strictly conform to
    /// the JSON syntax rules.
    /// The constructors are more forgiving in the texts they will accept:
    /// <ul>
    /// <li>An extra <code>,</code>&nbsp;<small>(comma)</small> may appear just
    /// before the closing brace.</li>
    /// <li>Strings may be quoted with <code>'</code>&nbsp;<small>(single
    /// quote)</small>.</li>
    /// <li>Strings do not need to be quoted at all if they do not begin with a quote
    /// or single quote, and if they do not contain leading or trailing spaces,
    /// and if they do not contain any of these characters:
    /// <code>{ } [ ] / \ : , = ; #</code> and if they do not look like numbers
    /// and if they are not the reserved words <code>true</code>,
    /// <code>false</code>, or <code>null</code>.</li>
    /// <li>Keys can be followed by <code>=</code> or <code>=></code> as well as
    /// by <code>:</code>.</li>
    /// <li>Values can be followed by <code>;</code> <small>(semicolon)</small> as
    /// well as by <code>,</code> <small>(comma)</small>.</li>
    /// <li>Numbers may have the <code>0x-</code> <small>(hex)</small> prefix.</li>
    /// </ul>
    /// </summary>
    /// <author>  JSON.org
    /// </author>
    /// <version>  2010-05-17
    /// </version>
    public class JSONObject
    {

        /// <summary> JSONObject.NULL is equivalent to the value that JavaScript calls null,
        /// whilst Java's null is equivalent to the value that JavaScript calls
        /// undefined.
        /// </summary>
        private sealed class Null : System.ICloneable
        {

            /// <summary> There is only intended to be a single instance of the NULL object,
            /// so the clone method returns itself.
            /// </summary>
            /// <returns>     NULL.
            /// </returns>
            public System.Object Clone()
            {
                return this;
            }


            /// <summary> A Null object is equal to the null value and to itself.</summary>
            /// <param name="object">   An object to test for nullness.
            /// </param>
            /// <returns> true if the object parameter is the JSONObject.NULL object
            /// or null.
            /// </returns>
            public override bool Equals(System.Object object_Renamed)
            {
                return object_Renamed == null || object_Renamed == this;
            }


            /// <summary> Get the "null" string value.</summary>
            /// <returns> The string "null".
            /// </returns>
            public override System.String ToString()
            {
                return "null";
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }


        /// <summary> The map where the JSONObject's properties are kept.</summary>
        private System.Collections.IDictionary map;


        /// <summary> It is sometimes more convenient and less ambiguous to have a
        /// <code>NULL</code> object than to use Java's <code>null</code> value.
        /// <code>JSONObject.NULL.equals(null)</code> returns <code>true</code>.
        /// <code>JSONObject.NULL.toString()</code> returns <code>"null"</code>.
        /// </summary>
        public static readonly System.Object NULL = new Null();


        /// <summary> Construct an empty JSONObject.</summary>
        public JSONObject()
        {
            this.map = new System.Collections.Hashtable();
        }


        /// <summary> Construct a JSONObject from a subset of another JSONObject.
        /// An array of strings is used to identify the keys that should be copied.
        /// Missing keys are ignored.
        /// </summary>
        /// <param name="jo">A JSONObject.
        /// </param>
        /// <param name="names">An array of strings.
        /// </param>
        /// <throws>  JSONException  </throws>
        /// <exception cref="JSONException">If a value is a non-finite number or if a name is duplicated.
        /// </exception>
        public JSONObject(JSONObject jo, System.String[] names)
            : this()
        {
            for (int i = 0; i < names.Length; i += 1)
            {
                try
                {
                    putOnce(names[i], jo.opt(names[i]));
                }
                catch (System.Exception)
                {
                }
            }
        }


        /// <summary> Construct a JSONObject from a JSONTokener.</summary>
        /// <param name="x">A JSONTokener object containing the source string.
        /// </param>
        /// <throws>  JSONException If there is a syntax error in the source string </throws>
        /// <summary>  or a duplicated key.
        /// </summary>
        public JSONObject(JSONTokener x)
            : this()
        {
            char c;
            System.String key;

            if (x.nextClean() != '{')
            {
                throw x.syntaxError("A JSONObject text must begin with '{'");
            }
            for (; ; )
            {
                c = x.nextClean();
                switch (c)
                {

                    case (char)(0):
                        throw x.syntaxError("A JSONObject text must end with '}'");

                    case '}':
                        return;

                    default:
                        x.back();
                        key = x.nextValue().ToString();
                        break;

                }

                /*
                * The key is followed by ':'. We will also tolerate '=' or '=>'.
                */

                c = x.nextClean();
                if (c == '=')
                {
                    if (x.next() != '>')
                    {
                        x.back();
                    }
                }
                else if (c != ':')
                {
                    throw x.syntaxError("Expected a ':' after a key");
                }
                putOnce(key, x.nextValue());

                /*
                * Pairs are separated by ','. We will also tolerate ';'.
                */

                switch (x.nextClean())
                {

                    case ';':
                    case ',':
                        if (x.nextClean() == '}')
                        {
                            return;
                        }
                        x.back();
                        break;

                    case '}':
                        return;

                    default:
                        throw x.syntaxError("Expected a ',' or '}'");

                }
            }
        }


        /// <summary> Construct a JSONObject from a Map.
        /// 
        /// </summary>
        /// <param name="map">A map object that can be used to initialize the contents of
        /// the JSONObject.
        /// </param>
        /// <throws>  JSONException  </throws>
        public JSONObject(System.Collections.IDictionary map)
        {
            this.map = new System.Collections.Hashtable();
            if (map != null)
            {
                System.Collections.IEnumerator i = new SupportClass.HashSetSupport(map).GetEnumerator();
                while (i.MoveNext())
                {
                    System.Collections.DictionaryEntry e = (System.Collections.DictionaryEntry)i.Current;
                    this.map[e.Key] = wrap(e.Value);
                }
            }
        }


        /// <summary> Construct a JSONObject from an Object using bean getters.
        /// It reflects on all of the public methods of the object.
        /// For each of the methods with no parameters and a name starting
        /// with <code>"get"</code> or <code>"is"</code> followed by an uppercase letter,
        /// the method is invoked, and a key and the value returned from the getter method
        /// are put into the new JSONObject.
        /// 
        /// The key is formed by removing the <code>"get"</code> or <code>"is"</code> prefix.
        /// If the second remaining character is not upper case, then the first
        /// character is converted to lower case.
        /// 
        /// For example, if an object has a method named <code>"getName"</code>, and
        /// if the result of calling <code>object.getName()</code> is <code>"Larry Fine"</code>,
        /// then the JSONObject will contain <code>"name": "Larry Fine"</code>.
        /// 
        /// </summary>
        /// <param name="bean">An object that has getter methods that should be used
        /// to make a JSONObject.
        /// </param>
        public JSONObject(System.Object bean)
            : this()
        {
            populateMap(bean);
        }


        /// <summary> Construct a JSONObject from an Object, using reflection to find the
        /// public members. The resulting JSONObject's keys will be the strings
        /// from the names array, and the values will be the field values associated
        /// with those keys in the object. If a key is not found or not visible,
        /// then it will not be copied into the new JSONObject.
        /// </summary>
        /// <param name="object">An object that has fields that should be used to make a
        /// JSONObject.
        /// </param>
        /// <param name="names">An array of strings, the names of the fields to be obtained
        /// from the object.
        /// </param>
        public JSONObject(System.Object object_Renamed, System.String[] names)
            : this()
        {
            System.Type c = object_Renamed.GetType();
            for (int i = 0; i < names.Length; i += 1)
            {
                System.String name = names[i];
                try
                {
                    putOpt(name, c.GetField(name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).GetValue(object_Renamed));
                }
                catch (System.Exception)
                {
                }
            }
        }


        /// <summary> Construct a JSONObject from a source JSON text string.
        /// This is the most commonly used JSONObject constructor.
        /// </summary>
        /// <param name="source">   A string beginning
        /// with <code>{</code>&nbsp;<small>(left brace)</small> and ending
        /// with <code>}</code>&nbsp;<small>(right brace)</small>.
        /// </param>
        /// <exception cref="JSONException">If there is a syntax error in the source
        /// string or a duplicated key.
        /// </exception>
        public JSONObject(System.String source)
            : this(new JSONTokener(source))
        {
        }


        /// <summary> Accumulate values under a key. It is similar to the put method except
        /// that if there is already an object stored under the key then a
        /// JSONArray is stored under the key to hold all of the accumulated values.
        /// If there is already a JSONArray, then the new value is appended to it.
        /// In contrast, the put method replaces the previous value.
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <param name="value">An object to be accumulated under the key.
        /// </param>
        /// <returns> this.
        /// </returns>
        /// <throws>  JSONException If the value is an invalid number </throws>
        /// <summary>  or if the key is null.
        /// </summary>
        public virtual JSONObject accumulate(System.String key, System.Object value_Renamed)
        {
            testValidity(value_Renamed);
            System.Object o = opt(key);
            if (o == null)
            {
                put(key, value_Renamed is JSONArray ? new JSONArray().put(value_Renamed) : value_Renamed);
            }
            else if (o is JSONArray)
            {
                ((JSONArray)o).put(value_Renamed);
            }
            else
            {
                put(key, new JSONArray().put(o).put(value_Renamed));
            }
            return this;
        }


        /// <summary> Append values to the array under a key. If the key does not exist in the
        /// JSONObject, then the key is put in the JSONObject with its value being a
        /// JSONArray containing the value parameter. If the key was already
        /// associated with a JSONArray, then the value parameter is appended to it.
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <param name="value">An object to be accumulated under the key.
        /// </param>
        /// <returns> this.
        /// </returns>
        /// <throws>  JSONException If the key is null or if the current value </throws>
        /// <summary>  associated with the key is not a JSONArray.
        /// </summary>
        public virtual JSONObject append(System.String key, System.Object value_Renamed)
        {
            testValidity(value_Renamed);
            System.Object o = opt(key);
            if (o == null)
            {
                put(key, new JSONArray().put(value_Renamed));
            }
            else if (o is JSONArray)
            {
                put(key, ((JSONArray)o).put(value_Renamed));
            }
            else
            {
                throw new JSONException("JSONObject[" + key + "] is not a JSONArray.");
            }
            return this;
        }


        /// <summary> Produce a string from a double. The string "null" will be returned if
        /// the number is not finite.
        /// </summary>
        /// <param name="d">A double.
        /// </param>
        /// <returns> A String.
        /// </returns>
        static public System.String doubleToString(double d)
        {
            if (System.Double.IsInfinity(d) || System.Double.IsNaN(d))
            {
                return "null";
            }

            // Shave off trailing zeros and decimal point, if possible.

            System.String s = d.ToString();
            if (s.IndexOf('.') > 0 && s.IndexOf('e') < 0 && s.IndexOf('E') < 0)
            {
                while (s.EndsWith("0"))
                {
                    s = s.Substring(0, (s.Length - 1) - (0));
                }
                if (s.EndsWith("."))
                {
                    s = s.Substring(0, (s.Length - 1) - (0));
                }
            }
            return s;
        }


        /// <summary> Get the value object associated with a key.
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <returns>      The object associated with the key.
        /// </returns>
        /// <throws>    JSONException if the key is not found. </throws>
        public virtual System.Object get_Renamed(System.String key)
        {
            System.Object o = opt(key);
            if (o == null)
            {
                throw new JSONException("JSONObject[" + quote(key) + "] not found.");
            }
            return o;
        }


        /// <summary> Get the boolean value associated with a key.
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <returns>      The truth.
        /// </returns>
        /// <throws>    JSONException </throws>
        /// <summary>  if the value is not a Boolean or the String "true" or "false".
        /// </summary>
        public virtual bool getBoolean(System.String key)
        {
            System.Object o = get_Renamed(key);
            if (o.Equals(false) || (o is System.String && ((System.String)o).ToUpper().Equals("false".ToUpper())))
            {
                return false;
            }
            else if (o.Equals(true) || (o is System.String && ((System.String)o).ToUpper().Equals("true".ToUpper())))
            {
                return true;
            }
            throw new JSONException("JSONObject[" + quote(key) + "] is not a Boolean.");
        }


        /// <summary> Get the double value associated with a key.</summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <returns>      The numeric value.
        /// </returns>
        /// <throws>  JSONException if the key is not found or </throws>
        /// <summary>  if the value is not a Number object and cannot be converted to a number.
        /// </summary>
        public virtual double getDouble(System.String key)
        {
            System.Object o = get_Renamed(key);
            try
            {
                return o is System.ValueType ? System.Convert.ToDouble(((System.ValueType)o)) : System.Double.Parse((System.String)o);
            }
            catch (System.Exception e)
            {
                throw new JSONException("JSONObject[" + quote(key) + "] is not a number.");
            }
        }


        /// <summary> Get the int value associated with a key. 
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <returns>      The integer value.
        /// </returns>
        /// <throws>    JSONException if the key is not found or if the value cannot </throws>
        /// <summary>  be converted to an integer.
        /// </summary>
        public virtual int getInt(System.String key)
        {
            System.Object o = get_Renamed(key);
            try
            {
                return o is System.ValueType ? System.Convert.ToInt32(((System.ValueType)o)) : System.Int32.Parse((System.String)o);
            }
            catch (System.Exception e)
            {
                throw new JSONException("JSONObject[" + quote(key) + "] is not an int.");
            }
        }


        /// <summary> Get the JSONArray value associated with a key.
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <returns>      A JSONArray which is the value.
        /// </returns>
        /// <throws>    JSONException if the key is not found or </throws>
        /// <summary>  if the value is not a JSONArray.
        /// </summary>
        public virtual JSONArray getJSONArray(System.String key)
        {
            System.Object o = get_Renamed(key);
            if (o is JSONArray)
            {
                return (JSONArray)o;
            }
            throw new JSONException("JSONObject[" + quote(key) + "] is not a JSONArray.");
        }


        /// <summary> Get the JSONObject value associated with a key.
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <returns>      A JSONObject which is the value.
        /// </returns>
        /// <throws>    JSONException if the key is not found or </throws>
        /// <summary>  if the value is not a JSONObject.
        /// </summary>
        public virtual JSONObject getJSONObject(System.String key)
        {
            System.Object o = get_Renamed(key);
            if (o is JSONObject)
            {
                return (JSONObject)o;
            }
            throw new JSONException("JSONObject[" + quote(key) + "] is not a JSONObject.");
        }


        /// <summary> Get the long value associated with a key. 
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <returns>      The long value.
        /// </returns>
        /// <throws>    JSONException if the key is not found or if the value cannot </throws>
        /// <summary>  be converted to a long.
        /// </summary>
        public virtual long getLong(System.String key)
        {
            System.Object o = get_Renamed(key);
            try
            {
                return o is System.ValueType ? System.Convert.ToInt64(((System.ValueType)o)) : System.Int64.Parse((System.String)o);
            }
            catch (System.Exception e)
            {
                throw new JSONException("JSONObject[" + quote(key) + "] is not a long.");
            }
        }


        /// <summary> Get an array of field names from a JSONObject.
        /// 
        /// </summary>
        /// <returns> An array of field names, or null if there are no names.
        /// </returns>
        public static System.String[] getNames(JSONObject jo)
        {
            int length = jo.length();
            if (length == 0)
            {
                return null;
            }
            System.Collections.IEnumerator i = jo.keys();
            System.String[] names = new System.String[length];
            int j = 0;
            while (i.MoveNext())
            {
                names[j] = ((System.String)i.Current);
                j += 1;
            }
            return names;
        }


        /// <summary> Get an array of field names from an Object.
        /// 
        /// </summary>
        /// <returns> An array of field names, or null if there are no names.
        /// </returns>
        public static System.String[] getNames(System.Object object_Renamed)
        {
            if (object_Renamed == null)
            {
                return null;
            }
            System.Type klass = object_Renamed.GetType();
            System.Reflection.FieldInfo[] fields = klass.GetFields();
            int length = fields.Length;
            if (length == 0)
            {
                return null;
            }
            System.String[] names = new System.String[length];
            for (int i = 0; i < length; i += 1)
            {
                names[i] = fields[i].Name;
            }
            return names;
        }


        /// <summary> Get the string associated with a key.
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <returns>      A string which is the value.
        /// </returns>
        /// <throws>    JSONException if the key is not found. </throws>
        public virtual System.String getString(System.String key)
        {
            return get_Renamed(key).ToString();
        }


        /// <summary> Determine if the JSONObject contains a specific key.</summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <returns>      true if the key exists in the JSONObject.
        /// </returns>
        public virtual bool has(System.String key)
        {
            return this.map.Contains(key);
        }


        /// <summary> Increment a property of a JSONObject. If there is no such property,
        /// create one with a value of 1. If there is such a property, and if
        /// it is an Integer, Long, Double, or Float, then add one to it.
        /// </summary>
        /// <param name="key"> A key string.
        /// </param>
        /// <returns> this.
        /// </returns>
        /// <throws>  JSONException If there is already a property with this name </throws>
        /// <summary> that is not an Integer, Long, Double, or Float.
        /// </summary>
        public virtual JSONObject increment(System.String key)
        {
            System.Object value_Renamed = opt(key);
            if (value_Renamed == null)
            {
                put(key, 1);
            }
            else
            {
                if (value_Renamed is System.Int32)
                {
                    put(key, ((System.Int32)value_Renamed) + 1);
                }
                else if (value_Renamed is System.Int64)
                {
                    put(key, (long)((System.Int64)value_Renamed) + 1);
                }
                else if (value_Renamed is System.Double)
                {
                    put(key, ((System.Double)value_Renamed) + 1);
                }
                else if (value_Renamed is System.Single)
                {
                    put(key, (float)((System.Single)value_Renamed) + 1);
                }
                else
                {
                    throw new JSONException("Unable to increment [" + key + "].");
                }
            }
            return this;
        }


        /// <summary> Determine if the value associated with the key is null or if there is
        /// no value.
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <returns>      true if there is no value associated with the key or if
        /// the value is the JSONObject.NULL object.
        /// </returns>
        public virtual bool isNull(System.String key)
        {
            return JSONObject.NULL.Equals(opt(key));
        }


        /// <summary> Get an enumeration of the keys of the JSONObject.
        /// 
        /// </summary>
        /// <returns> An iterator of the keys.
        /// </returns>
        public virtual System.Collections.IEnumerator keys()
        {
            return new SupportClass.HashSetSupport(this.map.Keys).GetEnumerator();
        }


        /// <summary> Get the number of keys stored in the JSONObject.
        /// 
        /// </summary>
        /// <returns> The number of keys in the JSONObject.
        /// </returns>
        public virtual int length()
        {
            return this.map.Count;
        }


        /// <summary> Produce a JSONArray containing the names of the elements of this
        /// JSONObject.
        /// </summary>
        /// <returns> A JSONArray containing the key strings, or null if the JSONObject
        /// is empty.
        /// </returns>
        public virtual JSONArray names()
        {
            JSONArray ja = new JSONArray();
            System.Collections.IEnumerator keyIter = keys();
            while (keyIter.MoveNext())
            {
                ja.put(keyIter.Current);
            }
            return ja.length() == 0 ? null : ja;
        }

        /// <summary> Produce a string from a Number.</summary>
        /// <param name="n">A Number
        /// </param>
        /// <returns> A String.
        /// </returns>
        /// <throws>  JSONException If n is a non-finite number. </throws>
        static public System.String numberToString(System.ValueType n)
        {
            if (n == null)
            {
                throw new JSONException("Null pointer");
            }
            testValidity(n);

            // Shave off trailing zeros and decimal point, if possible.

            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            System.String s = n.ToString();
            if (s.IndexOf('.') > 0 && s.IndexOf('e') < 0 && s.IndexOf('E') < 0)
            {
                while (s.EndsWith("0"))
                {
                    s = s.Substring(0, (s.Length - 1) - (0));
                }
                if (s.EndsWith("."))
                {
                    s = s.Substring(0, (s.Length - 1) - (0));
                }
            }
            return s;
        }


        /// <summary> Get an optional value associated with a key.</summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <returns>      An object which is the value, or null if there is no value.
        /// </returns>
        public virtual System.Object opt(System.String key)
        {
            return key == null ? null : this.map[key];
        }


        /// <summary> Get an optional boolean associated with a key.
        /// It returns false if there is no such key, or if the value is not
        /// Boolean.TRUE or the String "true".
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <returns>      The truth.
        /// </returns>
        public virtual bool optBoolean(System.String key)
        {
            return optBoolean(key, false);
        }


        /// <summary> Get an optional boolean associated with a key.
        /// It returns the defaultValue if there is no such key, or if it is not
        /// a Boolean or the String "true" or "false" (case insensitive).
        /// 
        /// </summary>
        /// <param name="key">             A key string.
        /// </param>
        /// <param name="defaultValue">    The default.
        /// </param>
        /// <returns>      The truth.
        /// </returns>
        public virtual bool optBoolean(System.String key, bool defaultValue)
        {
            try
            {
                return getBoolean(key);
            }
            catch (System.Exception e)
            {
                return defaultValue;
            }
        }


        /// <summary> Get an optional double associated with a key,
        /// or NaN if there is no such key or if its value is not a number.
        /// If the value is a string, an attempt will be made to evaluate it as
        /// a number.
        /// 
        /// </summary>
        /// <param name="key">  A string which is the key.
        /// </param>
        /// <returns>      An object which is the value.
        /// </returns>
        public virtual double optDouble(System.String key)
        {
            return optDouble(key, System.Double.NaN);
        }


        /// <summary> Get an optional double associated with a key, or the
        /// defaultValue if there is no such key or if its value is not a number.
        /// If the value is a string, an attempt will be made to evaluate it as
        /// a number.
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <param name="defaultValue">    The default.
        /// </param>
        /// <returns>      An object which is the value.
        /// </returns>
        public virtual double optDouble(System.String key, double defaultValue)
        {
            try
            {
                System.Object o = opt(key);
                //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                return o is System.ValueType ? System.Convert.ToDouble(((System.ValueType)o)) : System.Double.Parse((System.String)o);
            }
            catch (System.Exception e)
            {
                return defaultValue;
            }
        }


        /// <summary> Get an optional int value associated with a key,
        /// or zero if there is no such key or if the value is not a number.
        /// If the value is a string, an attempt will be made to evaluate it as
        /// a number.
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <returns>      An object which is the value.
        /// </returns>
        public virtual int optInt(System.String key)
        {
            return optInt(key, 0);
        }


        /// <summary> Get an optional int value associated with a key,
        /// or the default if there is no such key or if the value is not a number.
        /// If the value is a string, an attempt will be made to evaluate it as
        /// a number.
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <param name="defaultValue">    The default.
        /// </param>
        /// <returns>      An object which is the value.
        /// </returns>
        public virtual int optInt(System.String key, int defaultValue)
        {
            try
            {
                return getInt(key);
            }
            catch (System.Exception e)
            {
                return defaultValue;
            }
        }


        /// <summary> Get an optional JSONArray associated with a key.
        /// It returns null if there is no such key, or if its value is not a
        /// JSONArray.
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <returns>      A JSONArray which is the value.
        /// </returns>
        public virtual JSONArray optJSONArray(System.String key)
        {
            System.Object o = opt(key);
            return o is JSONArray ? (JSONArray)o : null;
        }


        /// <summary> Get an optional JSONObject associated with a key.
        /// It returns null if there is no such key, or if its value is not a
        /// JSONObject.
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <returns>      A JSONObject which is the value.
        /// </returns>
        public virtual JSONObject optJSONObject(System.String key)
        {
            System.Object o = opt(key);
            return o is JSONObject ? (JSONObject)o : null;
        }


        /// <summary> Get an optional long value associated with a key,
        /// or zero if there is no such key or if the value is not a number.
        /// If the value is a string, an attempt will be made to evaluate it as
        /// a number.
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <returns>      An object which is the value.
        /// </returns>
        public virtual long optLong(System.String key)
        {
            return optLong(key, 0);
        }


        /// <summary> Get an optional long value associated with a key,
        /// or the default if there is no such key or if the value is not a number.
        /// If the value is a string, an attempt will be made to evaluate it as
        /// a number.
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <param name="defaultValue">    The default.
        /// </param>
        /// <returns>      An object which is the value.
        /// </returns>
        public virtual long optLong(System.String key, long defaultValue)
        {
            try
            {
                return getLong(key);
            }
            catch (System.Exception e)
            {
                return defaultValue;
            }
        }


        /// <summary> Get an optional string associated with a key.
        /// It returns an empty string if there is no such key. If the value is not
        /// a string and is not null, then it is coverted to a string.
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <returns>      A string which is the value.
        /// </returns>
        public virtual System.String optString(System.String key)
        {
            return optString(key, "");
        }


        /// <summary> Get an optional string associated with a key.
        /// It returns the defaultValue if there is no such key.
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <param name="defaultValue">    The default.
        /// </param>
        /// <returns>      A string which is the value.
        /// </returns>
        public virtual System.String optString(System.String key, System.String defaultValue)
        {
            System.Object o = opt(key);
            return o != null ? o.ToString() : defaultValue;
        }


        private void populateMap(System.Object bean)
        {
            System.Type klass = bean.GetType();

            // If klass is a System class then set includeSuperClass to false. 

            bool includeSuperClass = klass.GetType() != null;

            System.Reflection.MethodInfo[] methods = (includeSuperClass) ? klass.GetMethods() : klass.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Static);
            for (int i = 0; i < methods.Length; i += 1)
            {
                try
                {
                    System.Reflection.MethodInfo method = methods[i];
                    if (method.IsPublic)
                    {
                        System.String name = method.Name;
                        System.String key = "";
                        if (name.StartsWith("get"))
                        {
                            if (name.Equals("getClass") || name.Equals("getDeclaringClass"))
                            {
                                key = "";
                            }
                            else
                            {
                                key = name.Substring(3);
                            }
                        }
                        else if (name.StartsWith("is"))
                        {
                            key = name.Substring(2);
                        }
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.reflect.Method.getParameterTypes' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        if (key.Length > 0 && System.Char.IsUpper(key[0]) && method.GetParameters().Length == 0)
                        {
                            if (key.Length == 1)
                            {
                                key = key.ToLower();
                            }
                            else if (!System.Char.IsUpper(key[1]))
                            {
                                key = key.Substring(0, (1) - (0)).ToLower() + key.Substring(1);
                            }

                            System.Object result = method.Invoke(bean, (System.Object[])null);

                            map[key] = wrap(result);
                        }
                    }
                }
                catch (System.Exception ignore)
                {
                }
            }
        }


        /// <summary> Put a key/boolean pair in the JSONObject.
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <param name="value">A boolean which is the value.
        /// </param>
        /// <returns> this.
        /// </returns>
        /// <throws>  JSONException If the key is null. </throws>
        public virtual JSONObject put(System.String key, bool value_Renamed)
        {
            put(key, (System.Object)(value_Renamed ? true : false));
            return this;
        }


        /// <summary> Put a key/value pair in the JSONObject, where the value will be a
        /// JSONArray which is produced from a Collection.
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <param name="value">A Collection value.
        /// </param>
        /// <returns>      this.
        /// </returns>
        /// <throws>  JSONException </throws>
        public virtual JSONObject put(System.String key, System.Collections.ICollection value_Renamed)
        {
            put(key, new JSONArray(value_Renamed));
            return this;
        }


        /// <summary> Put a key/double pair in the JSONObject.
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <param name="value">A double which is the value.
        /// </param>
        /// <returns> this.
        /// </returns>
        /// <throws>  JSONException If the key is null or if the number is invalid. </throws>
        public virtual JSONObject put(System.String key, double value_Renamed)
        {
            put(key, (System.Object)value_Renamed);
            return this;
        }


        /// <summary> Put a key/int pair in the JSONObject.
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <param name="value">An int which is the value.
        /// </param>
        /// <returns> this.
        /// </returns>
        /// <throws>  JSONException If the key is null. </throws>
        public virtual JSONObject put(System.String key, int value_Renamed)
        {
            put(key, (System.Object)value_Renamed);
            return this;
        }


        /// <summary> Put a key/long pair in the JSONObject.
        /// 
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <param name="value">A long which is the value.
        /// </param>
        /// <returns> this.
        /// </returns>
        /// <throws>  JSONException If the key is null. </throws>
        public virtual JSONObject put(System.String key, long value_Renamed)
        {
            put(key, (System.Object)value_Renamed);
            return this;
        }


        /// <summary> Put a key/value pair in the JSONObject, where the value will be a
        /// JSONObject which is produced from a Map.
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <param name="value">A Map value.
        /// </param>
        /// <returns>      this.
        /// </returns>
        /// <throws>  JSONException </throws>
        public virtual JSONObject put(System.String key, System.Collections.IDictionary value_Renamed)
        {
            put(key, new JSONObject(value_Renamed));
            return this;
        }


        /// <summary> Put a key/value pair in the JSONObject. If the value is null,
        /// then the key will be removed from the JSONObject if it is present.
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <param name="value">An object which is the value. It should be of one of these
        /// types: Boolean, Double, Integer, JSONArray, JSONObject, Long, String,
        /// or the JSONObject.NULL object.
        /// </param>
        /// <returns> this.
        /// </returns>
        /// <throws>  JSONException If the value is non-finite number </throws>
        /// <summary>  or if the key is null.
        /// </summary>
        public virtual JSONObject put(System.String key, System.Object value_Renamed)
        {
            if (key == null)
            {
                throw new JSONException("Null key.");
            }
            if (value_Renamed != null)
            {
                testValidity(value_Renamed);
                this.map[key] = value_Renamed;
            }
            else
            {
                remove(key);
            }
            return this;
        }


        /// <summary> Put a key/value pair in the JSONObject, but only if the key and the
        /// value are both non-null, and only if there is not already a member
        /// with that name.
        /// </summary>
        /// <param name="key">
        /// </param>
        /// <param name="value">
        /// </param>
        /// <returns> his.
        /// </returns>
        /// <throws>  JSONException if the key is a duplicate </throws>
        public virtual JSONObject putOnce(System.String key, System.Object value_Renamed)
        {
            if (key != null && value_Renamed != null)
            {
                if (opt(key) != null)
                {
                    throw new JSONException("Duplicate key \"" + key + "\"");
                }
                put(key, value_Renamed);
            }
            return this;
        }


        /// <summary> Put a key/value pair in the JSONObject, but only if the
        /// key and the value are both non-null.
        /// </summary>
        /// <param name="key">  A key string.
        /// </param>
        /// <param name="value">An object which is the value. It should be of one of these
        /// types: Boolean, Double, Integer, JSONArray, JSONObject, Long, String,
        /// or the JSONObject.NULL object.
        /// </param>
        /// <returns> this.
        /// </returns>
        /// <throws>  JSONException If the value is a non-finite number. </throws>
        public virtual JSONObject putOpt(System.String key, System.Object value_Renamed)
        {
            if (key != null && value_Renamed != null)
            {
                put(key, value_Renamed);
            }
            return this;
        }


        /// <summary> Produce a string in double quotes with backslash sequences in all the
        /// right places. A backslash will be inserted within </, allowing JSON
        /// text to be delivered in HTML. In JSON text, a string cannot contain a
        /// control character or an unescaped quote or backslash.
        /// </summary>
        /// <param name="string">A String
        /// </param>
        /// <returns>  A String correctly formatted for insertion in a JSON text.
        /// </returns>
        public static System.String quote(System.String string_Renamed)
        {
            if (string_Renamed == null || string_Renamed.Length == 0)
            {
                return "\"\"";
            }

            char b;
            char c = (char)(0);
            int i;
            int len = string_Renamed.Length;
            System.Text.StringBuilder sb = new System.Text.StringBuilder(len + 4);
            System.String t;

            sb.Append('"');
            for (i = 0; i < len; i += 1)
            {
                b = c;
                c = string_Renamed[i];
                switch (c)
                {

                    case '\\':
                    case '"':
                        sb.Append('\\');
                        sb.Append(c);
                        break;

                    case '/':
                        if (b == '<')
                        {
                            sb.Append('\\');
                        }
                        sb.Append(c);
                        break;

                    case '\b':
                        sb.Append("\\b");
                        break;

                    case '\t':
                        sb.Append("\\t");
                        break;

                    case '\n':
                        sb.Append("\\n");
                        break;

                    case '\f':
                        sb.Append("\\f");
                        break;

                    case '\r':
                        sb.Append("\\r");
                        break;

                    default:
                        if (c < ' ' || (c >= '\u0080' && c < '\u00a0') || (c >= '\u2000' && c < '\u2100'))
                        {
                            t = "000" + System.Convert.ToString(c, 16);
                            sb.Append("\\u" + t.Substring(t.Length - 4));
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;

                }
            }
            sb.Append('"');
            return sb.ToString();
        }

        /// <summary> Remove a name and its value, if present.</summary>
        /// <param name="key">The name to be removed.
        /// </param>
        /// <returns> The value that was associated with the name,
        /// or null if there was no value.
        /// </returns>
        public virtual System.Object remove(System.String key)
        {
            System.Object tempObject;
            tempObject = this.map[key];
            this.map.Remove(key);
            return tempObject;
        }

        /// <summary> Get an enumeration of the keys of the JSONObject.
        /// The keys will be sorted alphabetically.
        /// 
        /// </summary>
        /// <returns> An iterator of the keys.
        /// </returns>
        public virtual System.Collections.IEnumerator sortedKeys()
        {
            //UPGRADE_TODO: Class 'java.util.TreeSet' was converted to 'SupportClass.TreeSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilTreeSet'"
            //UPGRADE_TODO: Method 'java.util.Map.keySet' was converted to 'SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilMapkeySet'"
            return new SupportClass.TreeSetSupport(new SupportClass.HashSetSupport(this.map.Keys)).GetEnumerator();
        }

        /// <summary> Try to convert a string into a number, boolean, or null. If the string
        /// can't be converted, return the string.
        /// </summary>
        /// <param name="s">A String.
        /// </param>
        /// <returns> A simple JSON value.
        /// </returns>
        static public System.Object stringToValue(System.String s)
        {
            if (s.Equals(""))
            {
                return s;
            }
            if (s.ToUpper().Equals("true".ToUpper()))
            {
                return true;
            }
            if (s.ToUpper().Equals("false".ToUpper()))
            {
                return false;
            }
            if (s.ToUpper().Equals("null".ToUpper()))
            {
                return JSONObject.NULL;
            }

            /*
            * If it might be a number, try converting it. 
            * We support the non-standard 0x- convention. 
            * If a number cannot be produced, then the value will just
            * be a string. Note that the 0x-, plus, and implied string
            * conventions are non-standard. A JSON parser may accept
            * non-JSON forms as long as it accepts all correct JSON forms.
            */

            char b = s[0];
            if ((b >= '0' && b <= '9') || b == '.' || b == '-' || b == '+')
            {
                if (b == '0' && s.Length > 2 && (s[1] == 'x' || s[1] == 'X'))
                {
                    try
                    {
                        //UPGRADE_TODO: Method 'java.lang.Integer.parseInt' was converted to 'System.Convert.ToInt32' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                        return (System.Int32)System.Convert.ToInt32(s.Substring(2), 16);
                    }
                    catch (System.Exception ignore)
                    {
                    }
                }
                try
                {
                    if (s.IndexOf('.') > -1 || s.IndexOf('e') > -1 || s.IndexOf('E') > -1)
                    {
                        return System.Double.Parse(s);
                    }
                    else
                    {
                        System.Int64 myLong = System.Int64.Parse(s);
                        if ((long)myLong == (int)myLong)
                        {
                            return (System.Int32)myLong;
                        }
                        else
                        {
                            return myLong;
                        }
                    }
                }
                catch (System.Exception ignore)
                {
                }
            }
            return s;
        }


        /// <summary> Throw an exception if the object is an NaN or infinite number.</summary>
        /// <param name="o">The object to test.
        /// </param>
        /// <throws>  JSONException If o is a non-finite number. </throws>
        internal static void testValidity(System.Object o)
        {
            if (o != null)
            {
                if (o is System.Double)
                {
                    if (System.Double.IsInfinity(((System.Double)o)) || System.Double.IsNaN(((System.Double)o)))
                    {
                        throw new JSONException("JSON does not allow non-finite numbers.");
                    }
                }
                else if (o is System.Single)
                {
                    if (System.Single.IsInfinity(((System.Single)o)) || System.Single.IsNaN(((System.Single)o)))
                    {
                        throw new JSONException("JSON does not allow non-finite numbers.");
                    }
                }
            }
        }


        /// <summary> Produce a JSONArray containing the values of the members of this
        /// JSONObject.
        /// </summary>
        /// <param name="names">A JSONArray containing a list of key strings. This
        /// determines the sequence of the values in the result.
        /// </param>
        /// <returns> A JSONArray of values.
        /// </returns>
        /// <throws>  JSONException If any of the values are non-finite numbers. </throws>
        public virtual JSONArray toJSONArray(JSONArray names)
        {
            if (names == null || names.length() == 0)
            {
                return null;
            }
            JSONArray ja = new JSONArray();
            for (int i = 0; i < names.length(); i += 1)
            {
                ja.put(this.opt(names.getString(i)));
            }
            return ja;
        }

        /// <summary> Make a JSON text of this JSONObject. For compactness, no whitespace
        /// is added. If this would not result in a syntactically correct JSON text,
        /// then null will be returned instead.
        /// <p>
        /// Warning: This method assumes that the data structure is acyclical.
        /// 
        /// </summary>
        /// <returns> a printable, displayable, portable, transmittable
        /// representation of the object, beginning
        /// with <code>{</code>&nbsp;<small>(left brace)</small> and ending
        /// with <code>}</code>&nbsp;<small>(right brace)</small>.
        /// </returns>
        public override System.String ToString()
        {
            try
            {
                System.Collections.IEnumerator keyIter = keys();
                System.Text.StringBuilder sb = new System.Text.StringBuilder("{");

                while (keyIter.MoveNext())
                {
                    if (sb.Length > 1)
                    {
                        sb.Append(',');
                    }
                    System.Object o = keyIter.Current;
                    sb.Append(quote(o.ToString()));
                    sb.Append(':');
                    sb.Append(valueToString(this.map[o]));
                }
                sb.Append('}');
                return sb.ToString();
            }
            catch (System.Exception e)
            {
                return null;
            }
        }


        /// <summary> Make a prettyprinted JSON text of this JSONObject.
        /// <p>
        /// Warning: This method assumes that the data structure is acyclical.
        /// </summary>
        /// <param name="indentFactor">The number of spaces to add to each level of
        /// indentation.
        /// </param>
        /// <returns> a printable, displayable, portable, transmittable
        /// representation of the object, beginning
        /// with <code>{</code>&nbsp;<small>(left brace)</small> and ending
        /// with <code>}</code>&nbsp;<small>(right brace)</small>.
        /// </returns>
        /// <throws>  JSONException If the object contains an invalid number. </throws>
        public virtual System.String toString(int indentFactor)
        {
            return toString(indentFactor, 0);
        }


        /// <summary> Make a prettyprinted JSON text of this JSONObject.
        /// <p>
        /// Warning: This method assumes that the data structure is acyclical.
        /// </summary>
        /// <param name="indentFactor">The number of spaces to add to each level of
        /// indentation.
        /// </param>
        /// <param name="indent">The indentation of the top level.
        /// </param>
        /// <returns> a printable, displayable, transmittable
        /// representation of the object, beginning
        /// with <code>{</code>&nbsp;<small>(left brace)</small> and ending
        /// with <code>}</code>&nbsp;<small>(right brace)</small>.
        /// </returns>
        /// <throws>  JSONException If the object contains an invalid number. </throws>
        internal virtual System.String toString(int indentFactor, int indent)
        {
            int j;
            int n = length();
            if (n == 0)
            {
                return "{}";
            }
            System.Collections.IEnumerator keys = sortedKeys();
            System.Text.StringBuilder sb = new System.Text.StringBuilder("{");
            int newindent = indent + indentFactor;
            System.Object o;
            if (n == 1)
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                o = keys.Current;
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                sb.Append(quote(o.ToString()));
                sb.Append(": ");
                sb.Append(valueToString(this.map[o], indentFactor, indent));
            }
            else
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                while (keys.MoveNext())
                {
                    //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                    o = keys.Current;
                    if (sb.Length > 1)
                    {
                        sb.Append(",\n");
                    }
                    else
                    {
                        sb.Append('\n');
                    }
                    for (j = 0; j < newindent; j += 1)
                    {
                        sb.Append(' ');
                    }
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    sb.Append(quote(o.ToString()));
                    sb.Append(": ");
                    sb.Append(valueToString(this.map[o], indentFactor, newindent));
                }
                if (sb.Length > 1)
                {
                    sb.Append('\n');
                    for (j = 0; j < indent; j += 1)
                    {
                        sb.Append(' ');
                    }
                }
            }
            sb.Append('}');
            return sb.ToString();
        }


        /// <summary> Make a JSON text of an Object value. If the object has an
        /// value.toJSONString() method, then that method will be used to produce
        /// the JSON text. The method is required to produce a strictly
        /// conforming text. If the object does not contain a toJSONString
        /// method (which is the most common case), then a text will be
        /// produced by other means. If the value is an array or Collection,
        /// then a JSONArray will be made from it and its toJSONString method
        /// will be called. If the value is a MAP, then a JSONObject will be made
        /// from it and its toJSONString method will be called. Otherwise, the
        /// value's toString method will be called, and the result will be quoted.
        /// 
        /// <p>
        /// Warning: This method assumes that the data structure is acyclical.
        /// </summary>
        /// <param name="value">The value to be serialized.
        /// </param>
        /// <returns> a printable, displayable, transmittable
        /// representation of the object, beginning
        /// with <code>{</code>&nbsp;<small>(left brace)</small> and ending
        /// with <code>}</code>&nbsp;<small>(right brace)</small>.
        /// </returns>
        /// <throws>  JSONException If the value is or contains an invalid number. </throws>
        internal static System.String valueToString(System.Object value_Renamed)
        {
            if (value_Renamed == null || value_Renamed.Equals(null))
            {
                return "null";
            }
            if (value_Renamed is JSONString)
            {
                System.Object o;
                try
                {
                    o = ((JSONString)value_Renamed).toJSONString();
                }
                catch (System.Exception e)
                {
                    throw new JSONException(e);
                }
                if (o is System.String)
                {
                    return (System.String)o;
                }
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                throw new JSONException("Bad value from toJSONString: " + o);
            }
            if (value_Renamed is System.ValueType)
            {
                return numberToString((System.ValueType)value_Renamed);
            }
            if (value_Renamed is System.Boolean || value_Renamed is JSONObject || value_Renamed is JSONArray)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                return value_Renamed.ToString();
            }
            if (value_Renamed is System.Collections.IDictionary)
            {
                return new JSONObject((System.Collections.IDictionary)value_Renamed).ToString();
            }
            if (value_Renamed is System.Collections.ICollection)
            {
                return new JSONArray((System.Collections.ICollection)value_Renamed).ToString();
            }
            if (value_Renamed.GetType().IsArray)
            {
                return new JSONArray(value_Renamed).ToString();
            }
            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            return quote(value_Renamed.ToString());
        }


        /// <summary> Make a prettyprinted JSON text of an object value.
        /// <p>
        /// Warning: This method assumes that the data structure is acyclical.
        /// </summary>
        /// <param name="value">The value to be serialized.
        /// </param>
        /// <param name="indentFactor">The number of spaces to add to each level of
        /// indentation.
        /// </param>
        /// <param name="indent">The indentation of the top level.
        /// </param>
        /// <returns> a printable, displayable, transmittable
        /// representation of the object, beginning
        /// with <code>{</code>&nbsp;<small>(left brace)</small> and ending
        /// with <code>}</code>&nbsp;<small>(right brace)</small>.
        /// </returns>
        /// <throws>  JSONException If the object contains an invalid number. </throws>
        internal static System.String valueToString(System.Object value_Renamed, int indentFactor, int indent)
        {
            if (value_Renamed == null || value_Renamed.Equals(null))
            {
                return "null";
            }
            try
            {
                if (value_Renamed is JSONString)
                {
                    System.Object o = ((JSONString)value_Renamed).toJSONString();
                    if (o is System.String)
                    {
                        return (System.String)o;
                    }
                }
            }
            catch (System.Exception ignore)
            {
            }
            if (value_Renamed is System.ValueType)
            {
                return numberToString((System.ValueType)value_Renamed);
            }
            if (value_Renamed is System.Boolean)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                return value_Renamed.ToString();
            }
            if (value_Renamed is JSONObject)
            {
                return ((JSONObject)value_Renamed).toString(indentFactor, indent);
            }
            if (value_Renamed is JSONArray)
            {
                return ((JSONArray)value_Renamed).toString(indentFactor, indent);
            }
            if (value_Renamed is System.Collections.IDictionary)
            {
                return new JSONObject((System.Collections.IDictionary)value_Renamed).toString(indentFactor, indent);
            }
            if (value_Renamed is System.Collections.ICollection)
            {
                return new JSONArray((System.Collections.ICollection)value_Renamed).toString(indentFactor, indent);
            }
            if (value_Renamed.GetType().IsArray)
            {
                return new JSONArray(value_Renamed).toString(indentFactor, indent);
            }
            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            return quote(value_Renamed.ToString());
        }


        /// <summary> Wrap an object, if necessary. If the object is null, return the NULL 
        /// object. If it is an array or collection, wrap it in a JSONArray. If 
        /// it is a map, wrap it in a JSONObject. If it is a standard property 
        /// (Double, String, et al) then it is already wrapped. Otherwise, if it 
        /// comes from one of the java packages, turn it into a string. And if 
        /// it doesn't, try to wrap it in a JSONObject. If the wrapping fails,
        /// then null is returned.
        /// 
        /// </summary>
        /// <param name="object">The object to wrap
        /// </param>
        /// <returns> The wrapped value
        /// </returns>
        internal static System.Object wrap(System.Object object_Renamed)
        {
            try
            {
                if (object_Renamed == null)
                {
                    return NULL;
                }
                if (object_Renamed is JSONObject || object_Renamed is JSONArray || NULL.Equals(object_Renamed) || object_Renamed is JSONString || object_Renamed is System.SByte || object_Renamed is System.Char || object_Renamed is System.Int16 || object_Renamed is System.Int32 || object_Renamed is System.Int64 || object_Renamed is System.Boolean || object_Renamed is System.Single || object_Renamed is System.Double || object_Renamed is System.String)
                {
                    return object_Renamed;
                }

                if (object_Renamed is System.Collections.ICollection)
                {
                    return new JSONArray((System.Collections.ICollection)object_Renamed);
                }
                if (object_Renamed.GetType().IsArray)
                {
                    return new JSONArray(object_Renamed);
                }
                if (object_Renamed is System.Collections.IDictionary)
                {
                    return new JSONObject((System.Collections.IDictionary)object_Renamed);
                }
                
                
                Assembly objectPackage = object_Renamed.GetType().Assembly;
                
                System.String objectPackageName = (objectPackage != null ? objectPackage.FullName : "");
                
                if (objectPackageName.StartsWith("java.") || objectPackageName.StartsWith("javax."))
                {
                    
                    return object_Renamed.ToString();
                }
                return new JSONObject(object_Renamed);
            }
            catch (System.Exception)
            {
                return null;
            }
        }


        /// <summary> Write the contents of the JSONObject as JSON text to a writer.
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
                System.Collections.IEnumerator keyIter = keys();
                writer.Write('{');

                while (keyIter.MoveNext())
                {
                    if (b)
                    {
                        writer.Write(',');
                    }
                    System.Object k = keyIter.Current;
                    writer.Write(quote(k.ToString()));
                    writer.Write(':');
                    System.Object v = this.map[k];
                    if (v is JSONObject)
                    {
                        ((JSONObject)v).write(writer);
                    }
                    else if (v is JSONArray)
                    {
                        ((JSONArray)v).write(writer);
                    }
                    else
                    {
                        writer.Write(valueToString(v));
                    }
                    b = true;
                }
                writer.Write('}');
                return writer;
            }
            catch (System.IO.IOException exception)
            {
                throw new JSONException(exception);
            }
        }
    }
}