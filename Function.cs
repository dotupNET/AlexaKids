using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Alexa.NET.Response;
using Alexa.NET.Request;
using Alexa.NET;
using Alexa.NET.Request.Type;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AlexaKids
{
	public class Function
	{

		/// <summary>
		/// A simple function that takes a string and does a ToUpper
		/// </summary>
		/// <param name="input"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		//public string FunctionHandler(string input, ILambdaContext context)
		//{
		//	return input?.ToUpper();
		//}
		public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
		{
			var intentRequest = input.Request as IntentRequest;
			var name = intentRequest.Intent.Slots["Name"].Value;
			var action = intentRequest.Intent.Slots["Action"].Value;

			var speak = new AlexaSpeak();

			foreach (var item in intentRequest.Intent.Slots)
			{
				speak.Normal($"{item.Value.Name}");
				speak.Break(AlexaSpeak.BreakStrength.x_strong);
				speak.Normal(item.Value.Value);
				speak.Break(AlexaSpeak.BreakStrength.x_strong);
			}

			speak
				.Break(AlexaSpeak.BreakStrength.x_strong)
				.Normal($"Hallo {name}")
				.Normal($"Du musst jetzt also {action}")
				//				.Volume("Guten morgen", AlexaSpeak.ProsodyVolume.soft)
				.Break(AlexaSpeak.BreakStrength.medium)
				.Pitch("Was machen wir denn jetzt?", AlexaSpeak.ProsodyPitch.low)
				.Break(AlexaSpeak.BreakStrength.medium)
				.Volume(speak.GetWhisper("Wollen wir uns verstecken?"), AlexaSpeak.ProsodyVolume.x_loud)
			;

			var speech = new Alexa.NET.Response.SsmlOutputSpeech();
			//speech.Ssml = "<speak>Today is <say-as interpret-as=\"date\">????0922</say-as>.<break strength=\"x-strong\"/>I hope you have a good day.</speak>";
			speech.Ssml = speak.ToString();

			// create the response using the ResponseBuilder
			var finalResponse = ResponseBuilder.Tell(speech);
			return finalResponse;


		}
	}
}
