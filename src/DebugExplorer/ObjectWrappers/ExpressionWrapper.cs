using EnvDTE;
using Microsoft.VisualStudio.OLE.Interop;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DebugExplorer.ObjectWrappers
{
	public class ExpressionWrapper
	{
		public const string NullToken = "null";

		public string Name { get; }

		public string Type { get; }

		public string ValueAsString { get; }

		public bool IsPrimitive
		{
			get
			{
				switch (Type)
				{
					case nameof(System.String):
					case nameof(System.Byte):
					case nameof(System.Int16):
					case nameof(System.UInt16):
					case nameof(System.Int32):
					case nameof(System.UInt32):
					case nameof(System.Int64):
					case nameof(System.UInt64):
					case nameof(System.Double):
					case nameof(System.Single):
						return true;
					default:
						return false;
				}
			}
		}

		public List<ExpressionWrapper> Collection { get; } = new List<ExpressionWrapper>();

		public List<ExpressionWrapper> DataMembers { get; private set; } = new List<ExpressionWrapper>();

		private Expression _expression;

		public ExpressionWrapper(Expression expression)
		{
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

			this._expression = expression;

			this.Name = expression.Name;
			this.Type = expression.Type;
			this.ValueAsString = expression.Value;
		}

		public void ProcessDataMembers()
		{
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

			if (_expression.Collection != null)
			{
				foreach (Expression item in _expression.Collection)
				{
					var s = item.Name;
					var v = item.Value;
					var t = item.Type;
					//this.Collection.Add(new ExpressionWrapper(item));
				}
			}

			if (_expression.DataMembers != null && this.ValueAsString.Equals(NullToken, StringComparison.OrdinalIgnoreCase))
			{
				foreach (Expression item in _expression.DataMembers)
				{
					this.DataMembers.Add(new ExpressionWrapper(item));
				}
			}
		}

		public object GetValue()
		{
			if (this.ValueAsString is null || this.ValueAsString.Equals(NullToken, StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}

			switch (Type)
			{
				case nameof(System.String):
					return this.ValueAsString;
				case nameof(System.Byte):
					return byte.Parse(this.ValueAsString);
				case nameof(System.Int16):
					return short.Parse(this.ValueAsString);
				case nameof(System.UInt16):
					return ushort.Parse(this.ValueAsString);
				case nameof(System.Int32):
					return int.Parse(this.ValueAsString);
				case nameof(System.UInt32):
					return uint.Parse(this.ValueAsString);
				case nameof(System.Int64):
					return long.Parse(this.ValueAsString);
				case nameof(System.UInt64):
					return ulong.Parse(this.ValueAsString);
				case nameof(System.Double):
					return double.Parse(this.ValueAsString);
				case nameof(System.Single):
					return float.Parse(this.ValueAsString);
				default:
					return null;
			}
		}

		public string JsonFomrat()
		{
			if (this.IsPrimitive)
			{
				if (this.Type.Equals(nameof(System.String)))
				{
					return $"\"{this.ValueAsString}\"";
				}

				return this.ValueAsString;
			}

			JObject jobject = (JObject)this.ToJsonToken();
			return jobject.ToString();
		}

		public JToken ToJsonToken()
		{
			if (this.IsPrimitive)
			{
				return new JValue(this.ValueAsString);
			}

			JObject jobject = new JObject();
			foreach (var item in this.DataMembers)
			{
				jobject.Add(item.Name, item.ToJsonToken());
			}

			return jobject;
		}
	}
}
