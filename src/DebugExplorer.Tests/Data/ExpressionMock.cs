using EnvDTE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugExplorer.Tests.Data
{
	public class ExpressionMock : EnvDTE.Expression
	{
		public string Name { get; private set; }

		public string Type { get; private set; }

		public Expressions DataMembers { get; }

		public string Value { get; set; }

		public bool IsValidValue { get; }

		public DTE DTE { get; }

		public Debugger Parent { get; }

		public Expressions Collection { get; }

		public ExpressionMock(object value)
		{
			this.Value = value.ToString();
			this.Type = value.GetType().Name;
		}

		public static ExpressionMock CreatePrimitive<T>(string name = null, T value = default)
		{
			switch (value)
			{
				case short:
				case ushort:
				case int:
				case uint:
					return new ExpressionMock(value);
				case string:
					return new ExpressionMock(value);
				default:
					break;
			}

			return null;
		}

		public override string ToString()
		{
			return $"{Type}:{Value}";
		}
	}
}
