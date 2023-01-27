using EnvDTE;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace DebugExplorer.Tests.Data
{
	//https://stackoverflow.com/questions/30574322/memberdata-tests-show-up-as-one-test-instead-of-many
	public class ExpressionMock : Expression, IXunitSerializable
	{
		public string Name { get; }

		public string Type { get; }

		public Expressions DataMembers { get; }

		public string Value { get; set; }

		public bool IsValidValue { get; }

		public DTE DTE { get; }

		public Debugger Parent { get; }

		public Expressions Collection { get; }

		public string JsonFomrat { get; }

		[Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
		public ExpressionMock()
		{
		}

		public ExpressionMock(string name, object value)
		{
			this.Name = name;
			this.Value = value.ToString();
			this.Type = value.GetType().Name;
			this.JsonFomrat = JsonConvert.SerializeObject(value);
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

		public void Deserialize(IXunitSerializationInfo info)
		{
			info.GetValue<string>(nameof(Name));
			info.GetValue<string>(nameof(Value));
			info.GetValue<string>(nameof(Type));
			info.GetValue<string>(nameof(JsonFomrat));
		}

		public void Serialize(IXunitSerializationInfo info)
		{
			info.AddValue(nameof(Name), Name);
		}
	}
}
