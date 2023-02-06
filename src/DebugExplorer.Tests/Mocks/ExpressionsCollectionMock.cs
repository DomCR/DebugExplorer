using EnvDTE;
using System.Collections;

namespace DebugExplorer.Tests.Mocks
{
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
