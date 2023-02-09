using DebugExplorer.ObjectWrappers;
using DebugExplorer.Tests.Common;
using DebugExplorer.Tests.Mocks;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;

namespace DebugExplorer.Tests.ObjectWrappers
{
	[Collection(MockedVS.Collection)]
	public class ExpressionWrapperTests
	{
		public static TheoryData<Type, object> Primitives { get; }

		private ITestOutputHelper _output;

		static ExpressionWrapperTests()
		{
			Randomizer rand = new Randomizer(0);

			Primitives = new TheoryData<Type, object>
			{
				{typeof(byte), rand.Next<byte>() },
				{typeof(short), rand.Next<short>() },
				{typeof(ushort), rand.Next<ushort>() },
				{typeof(int), rand.Next<int>() },
				{typeof(uint), rand.Next<uint>() },
				{typeof(long), rand.Next<long>() },
				{typeof(ulong), rand.Next<ulong>() },
				{typeof(double), rand.Next<double>() },
				{typeof(float), rand.Next<float>() },
				{typeof(string), $"{rand.RandomString(10)}" },
				{typeof(string), null },
				{typeof(string), "null" }
			};
		}

		public ExpressionWrapperTests(GlobalServiceProvider sp, ITestOutputHelper output)
		{
			sp.Reset();
			this._output = output;
		}

		[Theory]
		[MemberData(nameof(Primitives))]
		public async Task PrimitiveTestAsync(Type t, object value)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			ExpressionMock mock = new ExpressionMock("my_prop", value, t);

			ExpressionWrapper wrapper = new ExpressionWrapper(mock);

			Assert.True(wrapper.IsPrimitive);
			Assert.Equal(t.Name, wrapper.Type);
			Assert.Equal(mock.JsonFomrat, wrapper.JsonFomrat());
		}

		[Theory]
		[MemberData(nameof(Primitives))]
		public async Task GetValueTestAsync(Type t, object value)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			ExpressionMock mock = new ExpressionMock("my_prop", value, t);

			ExpressionWrapper wrapper = new ExpressionWrapper(mock);

			Assert.True(wrapper.IsPrimitive);
			Assert.Equal(t.Name, wrapper.Type);
			Assert.Equal(mock.OriginalValue, wrapper.GetValue());
		}

		[Fact]
		public async Task ListToJsonTestAsync()
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			List<string> list = new List<string>();
			list.Add("a");
			list.Add("b");
			list.Add("c");
			list.Add("d");

			ExpressionMock mock = new ExpressionMock("my_list", list);

			ExpressionWrapper wrapper = new ExpressionWrapper(mock);
			wrapper.ProcessDataMembers();

			Assert.False(wrapper.IsPrimitive);

			JObject jo = (JObject)wrapper.ToJsonToken();
			this._output.WriteLine(jo.ToString(Formatting.Indented));
		}

		[Fact]
		public async Task ObjectToJsonTestAsync()
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			Person p = new Person();
			p.Name = "person_name";
			p.Surname = "person_surname";

			ExpressionMock mock = new ExpressionMock(p);

			ExpressionWrapper wrapper = new ExpressionWrapper(mock);
			wrapper.ProcessDataMembers();

			Assert.False(wrapper.IsPrimitive);

			JObject jo = (JObject)wrapper.ToJsonToken();
			this._output.WriteLine(jo.ToString(Formatting.Indented));
		}

		[Fact]
		public async Task ObjectWithListToJsonTestAsync()
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			Person p = new Person();
			p.Name = "person_name";
			p.Surname = "person_surname";
			p.Tags.Add("tag_1");
			p.Tags.Add("tag_2");
			p.Tags.Add("tag_3");

			ExpressionMock mock = new ExpressionMock(p);

			ExpressionWrapper wrapper = new ExpressionWrapper(mock);
			wrapper.ProcessDataMembers();

			Assert.False(wrapper.IsPrimitive);

			JObject jo = (JObject)wrapper.ToJsonToken();
			this._output.WriteLine(jo.ToString(Formatting.Indented));
		}

		[Fact]
		public async Task ObjectWithDictionaryToJsonTestAsync()
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			Person p = new Person();
			p.Name = "person_name";
			p.Surname = "person_surname";
			p.Ids.Add(0, "my_id_0");
			p.Ids.Add(1, "my_id_1");
			p.Ids.Add(2, "my_id_2");

			ExpressionMock mock = new ExpressionMock(p);

			ExpressionWrapper wrapper = new ExpressionWrapper(mock);
			wrapper.ProcessDataMembers();

			Assert.False(wrapper.IsPrimitive);

			JObject jo = (JObject)wrapper.ToJsonToken();
			this._output.WriteLine(jo.ToString(Formatting.Indented));
		}

		[Fact]
		public async Task ObjectWithInfiniteLoopToJsonTestAsync()
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			Person p = new Person();
			p.Name = "person_name";
			p.Surname = "person_surname";

			p.Family.Add(p);

			ExpressionMock mock = new ExpressionMock(p);

			ExpressionWrapper wrapper = new ExpressionWrapper(mock);
			wrapper.ProcessDataMembers();

			Assert.False(wrapper.IsPrimitive);

			JObject jo = (JObject)wrapper.ToJsonToken();
			this._output.WriteLine(jo.ToString(Formatting.Indented));
		}
	}
}