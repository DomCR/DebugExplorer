using DebugExplorer.Tests.Data;
using Newtonsoft.Json.Linq;

namespace DebugExplorer.Tests
{
	public class JObjectGeneratorTests
	{
		public static TheoryData<ExpressionMock> PrimitiveCases { get; }

		public static TheoryData<object> Cases { get; }

		static JObjectGeneratorTests()
		{
			PrimitiveCases = new TheoryData<ExpressionMock>
			{
				ExpressionMock.CreatePrimitive<short>(),
				ExpressionMock.CreatePrimitive<int>(),
				ExpressionMock.CreatePrimitive<string>(value: "this is a random value")
			};

			Cases = new TheoryData<object>
			{
				(short)10,
				(int)12,
				(double)15,
				(long)16,
			};
		}

		[Theory]
		[MemberData(nameof(Cases))]
		public void Hello(object c)
		{
			JObject jo = new JObject(c);
		}

		[Theory]
		[MemberData(nameof(PrimitiveCases))]
		public void GenerateTest(ExpressionMock mock)
		{
			JObjectGenerator generator = new JObjectGenerator();
			JObject jo = generator.Generate(mock);

		}
	}
}