using System;
using System.Collections.Generic;
using System.Text;

namespace AlexaKids
{
	public class AlexaSpeak
	{
		public enum SayAsInterpreter
		{
			// Spell out each letter.
			characters,
			spell_out,
			cardinal, number, //: Interpret the value as a cardinal number.
			ordinal,//: Interpret the value as an ordinal number.
			digits,//: Spell each digit separately .
			fraction,//: Interpret the value as a fraction. This works for both common fractions (such as 3/20) and mixed fractions(such as 1+1/2).
			unit,//: Interpret a value as a measurement.The value should be either a number or fraction followed by a unit (with no space in between) or just a unit.
			date,//: Interpret the value as a date. Specify the format with the format attribute.
			time,//: Interpret a value such as 1'21" as duration in minutes and seconds.
			telephone,//: Interpret a value as a 7-digit or 10-digit telephone number.This can also handle extensions (for example, 2025551212x345).
			address,//: Interpret a value as part of street address.
			interjection,//: Interpret the value as an interjection. Alexa speaks the text in a more expressive voice. For optimal results, only use the supported interjections and surround each one with a pause. For example: <say-as interpret-as="interjection">Wow.</say-as>. Speechcons are supported for the languages listed below.
			expletive//: “Bleep” out the content inside the tag
		}

		public enum ProsodyRate
		{
			x_slow, slow, medium, fast, x_fast
		}

		public enum ProsodyPitch
		{
			x_low, low, medium, high, x_high
		}

		public enum ProsodyVolume
		{
			silent, x_soft, soft, medium, loud, x_loud
		}

		public enum BreakStrength
		{
			none, //: No pause should be outputted. This can be used to remove a pause that would normally occur (such as after a period).
			x_weak,//: No pause should be outputted(same as none).
			weak,//: Treat adjacent words as if separated by a single comma(equivalent to medium).
			medium,//: Treat adjacent words as if separated by a single comma.
			strong,//: Make a sentence break (equivalent to using the <s> tag).
			x_strong,//: Make
		}

		public enum EmphasisLevel
		{
			// Increase the volume and slow down the speaking rate so the speech is louder and slower.
			strong,
			// Increase the volume and slow down the speaking rate, but not as much as when set to strong. This is used as a default if level is not provided.
			moderate,
			// Decrease the volume and speed up the speaking rate. The speech is softer and faster.
			reduced
		}

		private readonly StringBuilder textToSpeak;

		public AlexaSpeak()
		{
			this.textToSpeak = new StringBuilder();
		}

		public AlexaSpeak Normal(string text)
		{
			textToSpeak.AppendLine(text);
			return this;
		}

		public AlexaSpeak Whisper(string text)
		{
			var formated = $"<amazon:effect name=\"whispered\">{text}</amazon:effect>";
			textToSpeak.AppendLine(formated);
			return this;
		}

		public string GetWhisper(string text)
		{
			var formated = $"<amazon:effect name=\"whispered\">{text}</amazon:effect>";
			return formated;
		}

		public AlexaSpeak Break(TimeSpan duration)
		{
			var milliseconds = duration.TotalMilliseconds;

			if (milliseconds > 10000)
				throw new ArgumentOutOfRangeException("milliseconds > 10000");

			var text = $"<break time=\"{milliseconds}ms\"/>";
			textToSpeak.AppendLine(text);
			return this;
		}

		public AlexaSpeak Break(BreakStrength strength)
		{
			var value = strength.ToString().Replace("_", "-");
			var text = $"<break strength=\"{value}\"/>";
			textToSpeak.AppendLine(text);
			return this;
		}

		public AlexaSpeak Emphasize(string text, EmphasisLevel level)
		{
			var formated = $"<emphasis level=\"{level.ToString()}\">{text}</emphasis>";
			textToSpeak.AppendLine(formated);
			return this;
		}

		public AlexaSpeak Paragraph(string text)
		{
			var formated = $"<p>{text}</p>";
			textToSpeak.AppendLine(formated);
			return this;
		}

		public AlexaSpeak Rate(string text, ProsodyRate rate)
		{
			textToSpeak.AppendLine(Prosody("rate", rate.ToString(), text));
			return this;
		}

		public AlexaSpeak Pitch(string text, ProsodyPitch pitch)
		{
			textToSpeak.AppendLine(Prosody("pitch", pitch.ToString(), text));
			return this;
		}

		public AlexaSpeak Volume(string text, ProsodyVolume volume)
		{
			textToSpeak.AppendLine(Prosody("volume", volume.ToString(), text));
			return this;
		}

		public AlexaSpeak Sentence(string text)
		{
			var formated = $"<s>{text}</s>";
			textToSpeak.AppendLine(formated);
			return this;
		}

		public AlexaSpeak SayAs(string text, SayAsInterpreter interpreter)
		{
			var interpretAs = interpreter.ToString().Replace("_", "-");
			var formated = $"<say-as interpret-as=\"{interpretAs}\">{text}</say-as>";
			textToSpeak.AppendLine(formated);
			return this;
		}

		private string Prosody(string key, string value, string text)
		{
			value = value.ToString().Replace("_", "-");
			var formated = $"<prosody {key}=\"{value}\">{text}</prosody>";
			return formated;
		}

		public override string ToString()
		{
			return $"<speak>{textToSpeak.ToString()}</speak>";
		}
	}
}