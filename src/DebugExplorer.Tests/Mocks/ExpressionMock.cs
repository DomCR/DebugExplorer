using EnvDTE;
using Moq;
using Newtonsoft.Json;

namespace DebugExplorer.Tests.Mocks
{
	public class ExpressionMock : Expression
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

		public ExpressionMock(string name, object value, Type type = null)
		{
			this.Name = name;
			this.OriginalValue = value;

			if (type != null)
			{
				this.Type = type.Name;
			}
			else
			{
				this.Type = value?.GetType().Name;
			}

			if (value is string s && !s.StartsWith("Count ="))
			{
				this.Value = $"\"{s}\"";
			}
			else
			{
				this.Value = value?.ToString();
			}

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

			var tags = new ExpressionMock(nameof(person.Tags), "Count = 0", person.Tags.GetType());
			this._dataMembers.Add(tags);

			for (int i = 0; i < person.Tags.Count; i++)
			{
				tags._dataMembers.Add(new ExpressionMock($"[{i}]", person.Tags[i]));
			}

			this.JsonFomrat = JsonConvert.SerializeObject(person, Formatting.Indented);
		}

		public override string ToString()
		{
			return $"{Type}:{Value}";
		}
	}
}
