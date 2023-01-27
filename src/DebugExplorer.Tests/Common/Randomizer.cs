using System;
using System.Collections.Generic;
using System.Text;

namespace DebugExplorer.Tests.Common
{
	public class Randomizer : Random
	{
		public Randomizer() : base() { }

		public Randomizer(int seed) : base(seed) { }

		public T Next<T>()
		{
			T value = default(T);

			switch (value)
			{
				case byte:
					return (T)Convert.ChangeType(this.Next(byte.MinValue, byte.MaxValue), typeof(T));
				case short:
					return (T)Convert.ChangeType(this.Next(short.MinValue, short.MaxValue), typeof(T));
			}

			return value;
		}

		public string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[this.Next(s.Length)]).ToArray());
		}
	}
}
