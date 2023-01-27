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

		public List<ExpressionWrapper> DataMembers { get; private set; } = new List<ExpressionWrapper>();

		private Expression _expression;

		public ExpressionWrapper(Expression expression)
		{
#if !TEST_ENV
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
#endif

			this._expression = expression;

			this.Name = expression.Name;
			this.Type = expression.Type;
			this.Value = expression.Value;
		}

		public void ProcessDataMembers()
		{
#if !TEST_ENV
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
#endif

			if (_expression.Collection != null)
			{
				foreach (Expression item in _expression.Collection)
				{
					//this.Collection.Add(new ExpressionWrapper(item));
				}
			}

			if (_expression.DataMembers != null && this.Value != "null")
			{
				foreach (Expression item in _expression.DataMembers)
				{
					this.DataMembers.Add(new ExpressionWrapper(item));
				}
			}
		}

		public string JsonFomrat()
		{
			return "Json Placeholder";
		}
	}
}
