using DebugExplorer.ObjectWrappers;
using DebugExplorer.Tests.Common;
using DebugExplorer.Tests.Data;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Newtonsoft.Json.Linq;

namespace DebugExplorer.Tests.ObjectWrappers
{
	[Collection(MockedVS.Collection)]
	public class ExpressionWrapperTests
	{
		public static TheoryData<ExpressionMock> PrimitiveVariables { get; }

		public static TheoryData<Type, object> Primitives { get; }

		static ExpressionWrapperTests()
		{
			Randomizer rand = new Randomizer(0);

			PrimitiveVariables = new TheoryData<ExpressionMock>
			{
				ExpressionMock.CreatePrimitive<short>(rand.RandomString(3), rand.Next<short>()),
				ExpressionMock.CreatePrimitive<int>(),
				ExpressionMock.CreatePrimitive(value: "this is a random value")
			};

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
				{typeof(string), rand.RandomString(10) }
		};
		}

		public ExpressionWrapperTests(GlobalServiceProvider sp)
		{
			sp.Reset();
		}

		[Theory]
		[MemberData(nameof(PrimitiveVariables), Skip = "Not serialized")]
		public async Task PrimitiveVariablesTestAsync(ExpressionMock mock)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			ExpressionWrapper generator = new ExpressionWrapper(mock);
			string json = generator.JsonFomrat();

			Assert.Equal(mock.JsonFomrat, json);
		}

		[Theory]
		[MemberData(nameof(Primitives))]
		public async Task PrimitiveTestAsync(Type t, object value)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			ExpressionMock mock = new ExpressionMock("my_prop", value);

			ExpressionWrapper wrapper = new ExpressionWrapper(mock);

			Assert.True(wrapper.IsPrimitive);
			Assert.Equal(mock.JsonFomrat, wrapper.JsonFomrat());
		}
	}
}