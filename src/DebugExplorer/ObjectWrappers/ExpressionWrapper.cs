using EnvDTE;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DebugExplorer.ObjectWrappers
{
	public class ExpressionWrapper
	{
		public const string NullToken = "null";

		private static int _maxDepth = 100;

		private Regex _collectionFormat = new Regex("Count = \\d+");

		public string Name { get; }

		public string Type { get; }

		public string ValueAsString { get; }

		public bool IsPrimitive
		{
			get
			{
				switch (Type)
				{
					case "string":
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

		public bool IsNull { get { return this.ValueAsString.Equals("null"); } }

		public bool IsCollection { get { return _collectionFormat.IsMatch(this.ValueAsString); } }

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

		public void ProcessDataMembers(int depth = 0)
		{
			if (depth > _maxDepth)
			{
				return;
			}

			this.DataMembers.Clear();

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

			if (_expression.DataMembers != null && !this.ValueAsString.Equals(NullToken, StringComparison.OrdinalIgnoreCase))
			{
				foreach (Expression item in _expression.DataMembers)
				{
					ExpressionWrapper member = new ExpressionWrapper(item);

					this.DataMembers.Add(member);
					member.ProcessDataMembers(depth + 1);
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
				case "string":
				case nameof(System.String):
					string value = this.ValueAsString.Remove(0, 1).Remove(this.ValueAsString.Length - 2);
					return value;
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
				StringBuilder sb = new StringBuilder();
				StringWriter sw = new StringWriter(sb);
				JsonWriter jsonWriter = new JsonTextWriter(sw);

				jsonWriter.WriteValue(GetValue());

				return sb.ToString();
			}

			JObject jobject = (JObject)this.ToJsonToken();
			return jobject.ToString();
		}

		public JToken ToJsonToken()
		{
			if (this.IsPrimitive)
			{
				return new JValue(this.GetValue());
			}

			if (this.IsNull)
			{
				return null;
			}

			JObject jobject = new JObject();
			if (this.IsCollection)
			{
				JArray arr = new JArray();
				foreach (var item in this.DataMembers)
				{
					arr.Add(item.ToJsonToken());
				}

				return arr;
			}
			else
			{
				foreach (var item in this.DataMembers)
				{
					jobject.Add(item.Name, item.ToJsonToken());
				}
			}

			return jobject;
		}
	}
}
