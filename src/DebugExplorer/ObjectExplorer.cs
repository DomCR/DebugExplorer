﻿using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DebugExplorer
{
	public sealed class ObjectExplorer
	{
		private static DebuggerEvents _debuggerEvents;

		public static async Task InitializeAsync(AsyncPackage package)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

			DTE dte = (DTE)Package.GetGlobalService(typeof(DTE));
			_debuggerEvents = dte.Events.DebuggerEvents;
			_debuggerEvents.OnEnterBreakMode += debuggerEvents_OnEnterBreakMode;
		}

		private static void debuggerEvents_OnEnterBreakMode(dbgEventReason Reason, ref dbgExecutionAction ExecutionAction)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			DTE dte = (DTE)Package.GetGlobalService(typeof(DTE));
			if (dte.Debugger.CurrentStackFrame == null)
			{
				return;
			}

			JObjectGenerator generator = new JObjectGenerator();
			foreach (Expression local in dte.Debugger.CurrentStackFrame.Locals)
			{
				var json = generator.Generate(local);
				Debug.WriteLine(json);
			}
		}
	}
}