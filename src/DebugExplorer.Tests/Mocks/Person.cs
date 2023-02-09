namespace DebugExplorer.Tests.Mocks
{
	public class Person
	{
		public string Name { get; set; }

		public string Surname { get; set; }

		public int Age { get; set; }

		public double Amount { get; set; }

		public int[] Array { get; set; }

		public List<string> Tags { get; set; } = new List<string>();

		public List<Person> Family { get; set; } = new List<Person>();

		public Dictionary<int, string> Ids { get; set; } = new Dictionary<int, string>();

		private string _secret;
	}
}
