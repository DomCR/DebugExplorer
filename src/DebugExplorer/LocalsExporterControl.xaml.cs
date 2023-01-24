using DebugExplorer.ObjectWrappers;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace DebugExplorer
{
	/// <summary>
	/// Interaction logic for LocalsExporterControl.
	/// </summary>
	public partial class LocalsExporterControl : UserControl
	{
		private static DebuggerEvents _debuggerEvents;

		public ObservableCollection<ExpressionWrapper> ExpressionWrappers { get; } = new ObservableCollection<ExpressionWrapper>();

		public string JsonText { get; private set; }

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

		/// <summary>
		/// Handles click on the button by displaying a message box.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event args.</param>
		[SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
		[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
		private void button1_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(
				string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
				"LocalsExporter");
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
	}
}