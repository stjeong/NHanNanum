using System;
namespace org.json
{
	
	/// <summary> The JSONException is thrown by the JSON.org classes when things are amiss.</summary>
	/// <author>  JSON.org
	/// </author>
	/// <version>  2008-09-18
	/// </version>
	[Serializable]
	public class JSONException:System.Exception
	{
		//UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
		virtual public System.Exception Cause
		{
			get
			{
				return this.cause;
			}
			
		}
		/// <summary> </summary>
		private const long serialVersionUID = 0;
		//UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
		private System.Exception cause;
		
		/// <summary> Constructs a JSONException with an explanatory message.</summary>
		/// <param name="message">Detail about the reason for the exception.
		/// </param>
		public JSONException(System.String message):base(message)
		{
		}
		
		//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
		//UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
		public JSONException(System.Exception t):base(t.Message)
		{
			this.cause = t;
		}
	}
}