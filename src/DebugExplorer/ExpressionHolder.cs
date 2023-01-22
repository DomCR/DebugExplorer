using EnvDTE;
using System;

namespace DebugExplorer
{
	public class ExpressionHolder
	{
		public string Name { get; }

		private Expression _expression;

		public ExpressionHolder(Expression expression)
		{
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

			this._expression = expression;
	
			this.Name = expression.Name;
		}
	}
}
