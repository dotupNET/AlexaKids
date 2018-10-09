using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AlexaKids
{

	public abstract class Enumeration : IComparable
	{
		private int value;
		private string name;

		protected Enumeration()
		{ }

		protected Enumeration(string name, int value)
		{
			this.value = value;
			this.name = name;
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public int Value
		{
			get { return value; }
			set { this.value = value; }
		}

		public override string ToString()
		{
			return name;
		}

		public static bool operator ==(Enumeration value1, Enumeration value2)
		{
			return value1 != null && value1.Equals(value2);
		}

		public static bool operator !=(Enumeration value1, Enumeration value2)
		{
			return value1 != null && !value1.Equals(value2);
		}

		public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
		{
			var type = typeof(T);
			var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

			foreach (var field in fields)
			{
				var result = field.GetValue(new T()) as T;

				if (result != null)
					yield return result;
			}
		}

		public override bool Equals(object obj)
		{
			var other = obj as Enumeration;

			if (other == null)
				return false;

			var isSameType = GetType().Equals(obj.GetType());

			var valueMatches = value.Equals(other.Value);

			return isSameType && valueMatches;
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}

		public static T FromValue<T>(int value) where T : Enumeration, new()
		{
			var result = GetAll<T>().FirstOrDefault(item => item.Value == value);
			if (result == null)
				throw new Exception($"Value <{value}> not found.");
			return result;
		}

		public static T FromName<T>(string name) where T : Enumeration, new()
		{
			var result = GetAll<T>().FirstOrDefault(item => item.Name == name);
			if (result == null)
				throw new Exception($"Name <{name}> not found.");
			return result;
		}

		public int CompareTo(object other)
		{
			return Value.CompareTo(((Enumeration)other).Value);
		}
	}
}
