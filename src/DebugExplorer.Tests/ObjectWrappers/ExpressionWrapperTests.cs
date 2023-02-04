using DebugExplorer.ObjectWrappers;
using DebugExplorer.Tests.Common;
using DebugExplorer.Tests.Mocks;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Moq;
using Newtonsoft.Json.Linq;

namespace DebugExplorer.Tests.ObjectWrappers
{
	[Collection(MockedVS.Collection)]
	public class ExpressionWrapperTests
	{
		public static TheoryData<Type, object> Primitives { get; }

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
				{typeof(string), rand.RandomString(10) },
				{typeof(string), null },
				{typeof(string), "null" }
			};
		}

		public ExpressionWrapperTests(GlobalServiceProvider sp)
		{
			sp.Reset();
		}

		[Theory]
		[MemberData(nameof(Primitives))]
		public async Task PrimitiveTestAsync(Type t, object value)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			ExpressionMock mock = new ExpressionMock("my_prop", value);

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

			ExpressionMock mock = new ExpressionMock("my_prop", value);

			ExpressionWrapper wrapper = new ExpressionWrapper(mock);

			Assert.True(wrapper.IsPrimitive);
			Assert.Equal(t.Name, wrapper.Type);
			Assert.Equal(mock.OriginalValue, wrapper.GetValue());
		}

		[Fact]
		public async Task SimpleObjectTestAsync()
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			Person p = new Person();
			p.Name = "person_name";
			p.Surname = "person_surname";

			ExpressionMock mock = new ExpressionMock(p);

			ExpressionWrapper wrapper = new ExpressionWrapper(mock);
			wrapper.ProcessDataMembers();

			JObject jo = (JObject)wrapper.ToJsonToken();

			Assert.False(wrapper.IsPrimitive);
			Assert.Equal(mock.JsonFomrat, wrapper.JsonFomrat());
		}
	}
}