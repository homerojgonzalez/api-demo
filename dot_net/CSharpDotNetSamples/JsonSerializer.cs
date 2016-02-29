﻿using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic ;
using System.Web.Script.Serialization;

namespace AspNetClientEncryptionExample
{
	public class JsonSerializer
	{
		public static string GetSeralizedString<T>(T obj) 
		{ 
			var jsSerializer = new JavaScriptSerializer();

			//converting request into JSON string
			var requestJSON = jsSerializer.Serialize(obj);
				
			//Optional - Display Json Request 
			System.Web.HttpContext.Current.Response.Write ("<br>" + "Json Request: " + requestJSON + "<br>");

			return requestJSON; 
		}

		public static void AssignError(TempResponse tempResponse, PayTraceBasicResponse basicResponse)
		{
			basicResponse.ErrorMsg = tempResponse.ErrorMessage;
		}

		public static T DeserializeResponse<T>(TempResponse tempResponse)
		{ 
			T returnObject = default(T);

			var jsSerializer= new JavaScriptSerializer ();

			//optional - Display the Json Response
			System.Web.HttpContext.Current.Response.Write ("<br>" + "Json Response: " + tempResponse.JsonResponse + "<br>");

			if (null != tempResponse.JsonResponse) 
			{
				// parse JSON data into C# obj
				returnObject = jsSerializer.Deserialize<T>(tempResponse.JsonResponse);
			}

			return returnObject; 
		}
	}

}

