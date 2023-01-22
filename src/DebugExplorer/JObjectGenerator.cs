using System;
using System.Diagnostics;
using EnvDTE;
using Newtonsoft.Json.Linq;

namespace DebugExplorer
{
	public class JObjectGenerator
	{
		public int TimeOutInSeconds { get; } = 10;

		public JObject JObject { get; private set; }

		private Stopwatch _runtimeTimer { get; } = new Stopwatch();

		public JObjectGenerator()
		{
			//Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

			this.JObject = new JObject();
		}

		public JObject Generate(Expression expression)
		{
			JObject result = null;

			try
			{
				//Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
				result = generateJObject(expression);
			}
			catch (Exception ex)
			{
				Debug.Fail(ex.Message);
			}

			return result;
		}

		private JObject generateJObject(Expression expression)
		{
			if (_runtimeTimer.ElapsedMilliseconds > (TimeOutInSeconds * 1000))
			{
				throw new TimeoutException("Timeout while generating JSON.");
			}

			if (isValue(expression, out object value))
			{
				return new JObject(value);
			}

			return null;
		}

		private bool isValue(Expression exp, out object value)
		{
			switch (exp.Type.Trim('?'))
			{
				case nameof(System.String):
					value = exp.Value;
					return true;
				case nameof(System.Int16):
					value = short.Parse(exp.Value);
					return true;
				default:
					Debug.Fail($"value type not implemented for {nameof(JObjectGenerator)}", exp.Type.Trim('?'));
					value = null;
					return false;
			}
		}
	}
}
