using EnvDTE;
using System;
using System.Collections.Generic;

namespace DebugExplorer.ObjectWrappers
{
	public class ExpressionWrapper
	{
		public string Name { get; }

		public string Type { get; }

		public string Value { get; }

		public List<ExpressionWrapper> Collection { get; } = new List<ExpressionWrapper>();

		public List<ExpressionWrapper> DataMembers { get; } = new List<ExpressionWrapper>();

		public ExpressionWrapper(Expression expression)
		{
#if !TEST_ENV
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
#endif

			this.Name = expression.Name;
			this.Type = expression.Type;
			this.Value = expression.Value;

			if (expression.Collection != null)
			{
				foreach (Expression item in expression.Collection)
				{
					//this.Collection.Add(new ExpressionWrapper(item));
				}
			}

			if (expression.DataMembers != null && this.Value != "null")
			{
				foreach (Expression item in expression.DataMembers)
				{
					this.DataMembers.Add(new ExpressionWrapper(item));
				}
			}
		}
	}
}
