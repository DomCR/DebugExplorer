using EnvDTE;

namespace DebugExplorer.Tests.Data
{
	public class ExpressionMock : Expression
	{
		public string Name { get; }

		public string Type { get; }

		public Expressions DataMembers { get; }

		public string Value { get; set; }

		public bool IsValidValue { get; }

		public DTE DTE { get; }

		public Debugger Parent { get; }

		public Expressions Collection { get; }

		public ExpressionMock(string name, object value)
		{
			this.Name = name;
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
				case string:
					return new ExpressionMock(name, value);
				default:
					throw new Exception();
			}
		}

		public override string ToString()
		{
			return $"{Type}:{Value}";
		}
	}
}
