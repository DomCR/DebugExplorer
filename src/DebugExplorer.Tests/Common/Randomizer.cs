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
				case short:
					break;
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
