using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Diagnostics;
using RGiesecke.DllExport;

namespace uix86 {
	[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	delegate void PrintFunc(string Str);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	delegate int ShutdownFunc();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	delegate int InitFunc(int A, int B, int C);

	class uix86 {
		static void msg(string Str) {
			if (Print != null)
				Print(Str + "\n");
			Console.WriteLine(Str);
			//File.AppendAllLines("INFO.txt", new string[] { Str });
		}

		static PrintFunc Print;
		static IntPtr _uix86;

		[DllExport("dllEntry", CallingConvention = CallingConvention.Cdecl)]
		public static void dllEntry(ReturnInfo RetInfo, DispatchTable SyscallTable, IntPtr Unknown) {
			//WaitForDebugger();
			Print = SyscallTable.GetFunc<PrintFunc>(Syscalls.UI_PRINT);
			msg("-- Initializing");

			_uix86 = Native.LoadLibrary("cliq3\\orig\\_uix86.dll");
			dllEntryFunc _dllEntry = Native.GetProcAddress<dllEntryFunc>(_uix86, "dllEntry");

			msg("Calling dllEntry");
			_dllEntry(RetInfo, SyscallTable, Unknown);

			msg("[C#] Hello Quake Live World!");
		}

		static void WaitForDebugger() {
			msg("Waiting for debugger");
			while (!Debugger.IsAttached)
				;
			msg("Debugger found");
		}
	}
}