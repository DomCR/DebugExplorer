using EnvDTE;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using Xunit.Abstractions;

namespace DebugExplorer.Tests.Mocks
{
	//https://stackoverflow.com/questions/30574322/memberdata-tests-show-up-as-one-test-instead-of-many
	public class ExpressionMock : Expression, IXunitSerializable
	{
		public string Name { get; }

		public string Type { get; }

		public Expressions DataMembers { get { return this._dataMembers; } }

		public string Value { get; set; }

		public bool IsValidValue { get; }

		public DTE DTE { get; }

		public Debugger Parent { get; }

		public Expressions Collection { get; }

		public string JsonFomrat { get; }

		public object OriginalValue { get; }

		private ExpressionsCollectionMock _dataMembers = new ExpressionsCollectionMock();

		[Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
		public ExpressionMock()
		{
		}

		public ExpressionMock(string name, object value)
		{
			this.Name = name;
			this.Value = value?.ToString();
			this.OriginalValue = value;
			this.Type = value?.GetType().Name;
			this.JsonFomrat = JsonConvert.SerializeObject(value);
		}

		public ExpressionMock(Person person)
		{
			this.Name = "prop_name";
			this.Value = typeof(Person).FullName;
			this.Type = typeof(Person).Name;

			this._dataMembers.Add(new ExpressionMock(nameof(person.Name), person.Name));
			this._dataMembers.Add(new ExpressionMock(nameof(person.Surname), person.Surname));
			this._dataMembers.Add(new ExpressionMock(nameof(person.Age), person.Age));
			this._dataMembers.Add(new ExpressionMock(nameof(person.Amount), person.Amount));

			this.JsonFomrat = JsonConvert.SerializeObject(person, Formatting.Indented);
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
			info.AddValue(nameof(Value), Value);
			info.AddValue(nameof(Type), Type);
		}

		public static ExpressionMock CreateExpressionObject()
		{
			ExpressionMock expression = new ExpressionMock();

			throw new NotImplementedException();
		}

		public class ExpressionsCollectionMock : Expressions
		{
			public DTE DTE { get; }

			public Debugger Parent { get; }

			public int Count { get { return this._expressions.Count; } }

			private List<ExpressionMock> _expressions = new List<ExpressionMock>();

			public void Add(ExpressionMock exp)
			{
				this._expressions.Add(exp);
			}

			public Expression Item(object index)
			{
				throw new NotImplementedException();
			}

			public IEnumerator GetEnumerator()
			{
				return _expressions.GetEnumerator();
			}
		}
	}
}
