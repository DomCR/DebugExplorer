using DebugExplorer.ObjectWrappers;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace DebugExplorer
{
	/// <summary>
	/// Interaction logic for LocalsExporterControl.
	/// </summary>
	public partial class LocalsExporterControl : UserControl, INotifyPropertyChanged
	{
		private static DebuggerEvents _debuggerEvents;

		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<ExpressionWrapper> ExpressionWrappers { get; } = new ObservableCollection<ExpressionWrapper>();

		public string JsonText
		{
			get { return this._jsonText; }
			private set
			{
				this._jsonText = value;
				this.notifyPropertyChanged();
			}
		}

		public string _jsonText;

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalsExporterControl"/> class.
		/// </summary>
		public LocalsExporterControl()
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			this.InitializeComponent();

			//Attach component to the debugger
			DTE dte = (DTE)Package.GetGlobalService(typeof(DTE));
			_debuggerEvents = dte.Events.DebuggerEvents;
			_debuggerEvents.OnEnterBreakMode += debuggerEvents_OnEnterBreakMode;
		}

		private void debuggerEvents_OnEnterBreakMode(dbgEventReason reason, ref dbgExecutionAction executionAction)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			this.ExpressionWrappers.Clear();

			DTE dte = (DTE)Package.GetGlobalService(typeof(DTE));
			if (dte.Debugger.CurrentStackFrame == null)
			{
				return;
			}

			foreach (EnvDTE.Expression local in dte.Debugger.CurrentStackFrame.Locals)
			{
				this.ExpressionWrappers.Add(new ExpressionWrapper(local));
			}
		}

		private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListView ls = sender as ListView;
			ExpressionWrapper selected = ls.SelectedItem as ExpressionWrapper;

			if (selected == null) { return; }

			try
			{
				selected.ProcessDataMembers();
				this.JsonText = selected.JsonFomrat();
			}
			catch (Exception ex)
			{
				this.JsonText = ex.ToString();
			}
		}

		private void notifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}