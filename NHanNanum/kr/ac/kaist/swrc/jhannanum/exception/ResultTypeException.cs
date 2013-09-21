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
namespace kr.ac.kaist.swrc.jhannanum.exception
{

    /// <summary> This exception occurs when the return type of HanNanum work flow is not matched correctly.
    /// A work flow has several phases, so it returns the result with the relevant type according to the last plug-in
    /// on the work flow. It means that the work flow may return different types of result. 
    /// 
    /// </summary>
    /// <author>  Sangwon Park (hudoni@world.kaist.ac.kr), CILab, SWRC, KAIST
    /// </author>
    [Serializable]
    public class ResultTypeException : System.Exception
    {
        public override System.String Message
        {
            get
            {
                return "The workflow ends in phase-" + phase + " so '" + objName[phase] + "' is required to store the result properly.";
            }
        }

        /// <summary> serialVersionUID</summary>
        private const long serialVersionUID = 1L;

        /// <summary> The array of the phase names.</summary>
        private static readonly System.String[] objName = new System.String[]
        { null, "PlainSentence", "SetOfSentences", "Sentence" };

        /// <summary> The analysis phase of the work flow.</summary>
        private int phase = 0;

        /// <summary> Constructor.</summary>
        /// <param name="phase">- analysis phase of the work flow
        /// </param>
        public ResultTypeException(int phase)
        {
            this.phase = phase;
        }
    }
}