using DebugExplorer.ObjectWrappers;
using DebugExplorer.Tests.Common;
using DebugExplorer.Tests.Data;
using Microsoft.VisualStudio.Shell;
using Newtonsoft.Json.Linq;

namespace DebugExplorer.Tests.ObjectWrappers
{
	public class ExpressionWrapperTests
	{
		public static TheoryData<ExpressionMock> PrimitiveVariables { get; }

		static ExpressionWrapperTests()
		{
			Randomizer rand = new Randomizer();

			PrimitiveVariables = new TheoryData<ExpressionMock>
			{
				ExpressionMock.CreatePrimitive<short>(rand.RandomString(3), rand.Next<short>()),
				ExpressionMock.CreatePrimitive<int>(),
				ExpressionMock.CreatePrimitive(value: "this is a random value")
			};
		}

		[Theory]
		[MemberData(nameof(PrimitiveVariables))]
		public async Task PrimitiveTestAsync(ExpressionMock mock)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			ExpressionWrapper generator = new ExpressionWrapper(mock);
			string json = generator.JsonFomrat();

			Assert.Equal(mock.JsonFomrat, json);
		}
	}
}