using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace org.json
{

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

    /// <summary> A JSONTokener takes a source string and extracts characters and tokens from
    /// it. It is used by the JSONObject and JSONArray constructors to parse
    /// JSON source strings.
    /// </summary>
    /// <author>  JSON.org
    /// </author>
    /// <version>  2010-02-02
    /// </version>
    public class JSONTokener
    {
        private int character;
        private bool eof;
        private int index;
        private int line;
        private char previous;
        private StreamReader reader;
        private bool usePrevious;

        /// <summary> Construct a JSONTokener from a reader.
        /// 
        /// </summary>
        /// <param name="reader">    A reader.
        /// </param>errors.
        public JSONTokener(StreamReader reader)
        {
            this.reader = reader;
            this.eof = false;
            this.usePrevious = false;
            this.previous = (char)(0);
            this.index = 0;
            this.character = 1;
            this.line = 1;
        }


        /// <summary> Construct a JSONTokener from a string.
        /// 
        /// </summary>
        /// <param name="s">    A source string.
        /// </param>
        public JSONTokener(System.String s)
            : this(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(s)), Encoding.UTF8))
        {
        }


        /// <summary> Back up one character. This provides a sort of lookahead capability,
        /// so that you can test for a digit or letter before attempting to parse
        /// the next number or identifier.
        /// </summary>
        public virtual void back()
        {
            if (usePrevious || index <= 0)
            {
                throw new JSONException("Stepping back two steps is not supported");
            }
            this.index -= 1;
            this.character -= 1;
            this.usePrevious = true;
            this.eof = false;
        }


        /// <summary> Get the hex value of a character (base16).</summary>
        /// <param name="c">A character between '0' and '9' or between 'A' and 'F' or
        /// between 'a' and 'f'.
        /// </param>
        /// <returns>  An int between 0 and 15, or -1 if c was not a hex digit.
        /// </returns>
        public static int dehexchar(char c)
        {
            if (c >= '0' && c <= '9')
            {
                return c - '0';
            }
            if (c >= 'A' && c <= 'F')
            {
                return c - ('A' - 10);
            }
            if (c >= 'a' && c <= 'f')
            {
                return c - ('a' - 10);
            }
            return -1;
        }

        public virtual bool end()
        {
            return eof && !usePrevious;
        }


        /// <summary> Determine if the source string still contains characters that next()
        /// can consume.
        /// </summary>
        /// <returns> true if not yet at the end of the source.
        /// </returns>
        public virtual bool more()
        {
            next();
            if (end())
            {
                return false;
            }
            back();
            return true;
        }


        /// <summary> Get the next character in the source string.
        /// 
        /// </summary>
        /// <returns> The next character, or 0 if past the end of the source string.
        /// </returns>
        public virtual char next()
        {
            int c;
            if (this.usePrevious)
            {
                this.usePrevious = false;
                c = this.previous;
            }
            else
            {
                try
                {
                    //UPGRADE_TODO: Method 'java.io.Reader.read' was converted to 'System.IO.StreamReader.Read' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioReaderread'"
                    c = this.reader.Read();
                }
                catch (System.IO.IOException exception)
                {
                    throw new JSONException(exception);
                }

                if (c <= 0)
                {
                    // End of stream
                    this.eof = true;
                    c = 0;
                }
            }
            this.index += 1;
            if (this.previous == '\r')
            {
                this.line += 1;
                this.character = c == '\n' ? 0 : 1;
            }
            else if (c == '\n')
            {
                this.line += 1;
                this.character = 0;
            }
            else
            {
                this.character += 1;
            }
            this.previous = (char)c;
            return this.previous;
        }


        /// <summary> Consume the next character, and check that it matches a specified
        /// character.
        /// </summary>
        /// <param name="c">The character to match.
        /// </param>
        /// <returns> The character.
        /// </returns>
        /// <throws>  JSONException if the character does not match. </throws>
        public virtual char next(char c)
        {
            char n = next();
            if (n != c)
            {
                throw syntaxError("Expected '" + c + "' and instead saw '" + n + "'");
            }
            return n;
        }


        /// <summary> Get the next n characters.
        /// 
        /// </summary>
        /// <param name="n">    The number of characters to take.
        /// </param>
        /// <returns>      A string of n characters.
        /// </returns>
        /// <throws>  JSONException </throws>
        /// <summary>   Substring bounds error if there are not
        /// n characters remaining in the source string.
        /// </summary>
        public virtual System.String next(int n)
        {
            if (n == 0)
            {
                return "";
            }

            char[] buffer = new char[n];
            int pos = 0;

            while (pos < n)
            {
                buffer[pos] = next();
                if (end())
                {
                    throw syntaxError("Substring bounds error");
                }
                pos += 1;
            }
            return new System.String(buffer);
        }


        /// <summary> Get the next char in the string, skipping whitespace.</summary>
        /// <throws>  JSONException </throws>
        /// <returns>  A character, or 0 if there are no more characters.
        /// </returns>
        public virtual char nextClean()
        {
            for (; ; )
            {
                char c = next();
                if (c == 0 || c > ' ')
                {
                    return c;
                }
            }
        }


        /// <summary> Return the characters up to the next close quote character.
        /// Backslash processing is done. The formal JSON format does not
        /// allow strings in single quotes, but an implementation is allowed to
        /// accept them.
        /// </summary>
        /// <param name="quote">The quoting character, either
        /// <code>"</code>&nbsp;<small>(double quote)</small> or
        /// <code>'</code>&nbsp;<small>(single quote)</small>.
        /// </param>
        /// <returns>      A String.
        /// </returns>
        /// <throws>  JSONException Unterminated string. </throws>
        public virtual System.String nextString(char quote)
        {
            char c;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (; ; )
            {
                c = next();
                switch (c)
                {

                    case (char)(0):
                    case '\n':
                    case '\r':
                        throw syntaxError("Unterminated string");

                    case '\\':
                        c = next();
                        switch (c)
                        {

                            case 'b':
                                sb.Append('\b');
                                break;

                            case 't':
                                sb.Append('\t');
                                break;

                            case 'n':
                                sb.Append('\n');
                                break;

                            case 'f':
                                sb.Append('\f');
                                break;

                            case 'r':
                                sb.Append('\r');
                                break;

                            case 'u':
                                //UPGRADE_TODO: Method 'java.lang.Integer.parseInt' was converted to 'System.Convert.ToInt32' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                                sb.Append((char)System.Convert.ToInt32(next(4), 16));
                                break;

                            case '"':
                            case '\'':
                            case '\\':
                            case '/':
                                sb.Append(c);
                                break;

                            default:
                                throw syntaxError("Illegal escape.");

                        }
                        break;

                    default:
                        if (c == quote)
                        {
                            return sb.ToString();
                        }
                        sb.Append(c);
                        break;

                }
            }
        }


        /// <summary> Get the text up but not including the specified character or the
        /// end of line, whichever comes first.
        /// </summary>
        /// <param name="d">A delimiter character.
        /// </param>
        /// <returns>   A string.
        /// </returns>
        public virtual System.String nextTo(char d)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (; ; )
            {
                char c = next();
                if (c == d || c == 0 || c == '\n' || c == '\r')
                {
                    if (c != 0)
                    {
                        back();
                    }
                    return sb.ToString().Trim();
                }
                sb.Append(c);
            }
        }


        /// <summary> Get the text up but not including one of the specified delimiter
        /// characters or the end of line, whichever comes first.
        /// </summary>
        /// <param name="delimiters">A set of delimiter characters.
        /// </param>
        /// <returns> A string, trimmed.
        /// </returns>
        public virtual System.String nextTo(System.String delimiters)
        {
            char c;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (; ; )
            {
                c = next();
                if (delimiters.IndexOf((System.Char)c) >= 0 || c == 0 || c == '\n' || c == '\r')
                {
                    if (c != 0)
                    {
                        back();
                    }
                    return sb.ToString().Trim();
                }
                sb.Append(c);
            }
        }


        /// <summary> Get the next value. The value can be a Boolean, Double, Integer,
        /// JSONArray, JSONObject, Long, or String, or the JSONObject.NULL object.
        /// </summary>
        /// <throws>  JSONException If syntax error. </throws>
        /// <summary> 
        /// </summary>
        /// <returns> An object.
        /// </returns>
        public virtual System.Object nextValue()
        {
            char c = nextClean();
            System.String s;

            switch (c)
            {

                case '"':
                case '\'':
                    return nextString(c);

                case '{':
                    back();
                    return new JSONObject(this);

                case '[':
                case '(':
                    back();
                    return new JSONArray(this);
            }

            /*
            * Handle unquoted text. This could be the values true, false, or
            * null, or it can be a number. An implementation (such as this one)
            * is allowed to also accept non-standard forms.
            *
            * Accumulate characters until we reach the end of the text or a
            * formatting character.
            */

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            while (c >= ' ' && ",:]}/\\\"[{;=#".IndexOf((System.Char)c) < 0)
            {
                sb.Append(c);
                c = next();
            }
            back();

            s = sb.ToString().Trim();
            if (s.Equals(""))
            {
                throw syntaxError("Missing value");
            }
            return JSONObject.stringToValue(s);
        }


        /// <summary> Skip characters until the next character is the requested character.
        /// If the requested character is not found, no characters are skipped.
        /// </summary>
        /// <param name="to">A character to skip to.
        /// </param>
        /// <returns> The requested character, or zero if the requested character
        /// is not found.
        /// </returns>
        public virtual char skipTo(char to)
        {
            char c;
            Dictionary<StreamReader, long> marks = new Dictionary<StreamReader, long>();

            try
            {
                int startIndex = this.index;
                int startCharacter = this.character;
                int startLine = this.line;

                reader.Mark(System.Int32.MaxValue, marks);
                do
                {
                    c = next();
                    if (c == 0)
                    {
                        reader.Reset(marks);
                        this.index = startIndex;
                        this.character = startCharacter;
                        this.line = startLine;
                        return c;
                    }
                }
                while (c != to);
            }
            catch (System.IO.IOException exc)
            {
                throw new JSONException(exc);
            }

            back();
            return c;
        }


        /// <summary> Make a JSONException to signal a syntax error.
        /// 
        /// </summary>
        /// <param name="message">The error message.
        /// </param>
        /// <returns>  A JSONException object, suitable for throwing
        /// </returns>
        public virtual JSONException syntaxError(System.String message)
        {
            return new JSONException(message + ToString());
        }


        /// <summary> Make a printable string of this JSONTokener.
        /// 
        /// </summary>
        /// <returns> " at {index} [character {character} line {line}]"
        /// </returns>
        public override System.String ToString()
        {
            return " at " + index + " [character " + this.character + " line " + this.line + "]";
        }
    }
}