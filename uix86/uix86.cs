using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
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
			File.AppendAllLines("INFO.txt", new string[] { Str });
		}

		[DllExport("dllEntry", CallingConvention = CallingConvention.Cdecl)]
		public static void dllEntry(ReturnInfo RetInfo, DispatchTable SyscallTable, IntPtr Unknown) {
			IntPtr _cgamex86 = Native.LoadLibrary("cliq3\\orig\\uix86.dll");
			dllEntryFunc _dllEntry = Native.GetProcAddress<dllEntryFunc>(_cgamex86, "dllEntry");

			msg("Redirrecting dllEntry");
			_dllEntry(RetInfo, SyscallTable, Unknown);

			PrintFunc Print = SyscallTable.GetFunc<PrintFunc>(Syscalls.UI_PRINT);
			Print("Hello UI World!\n");
		}

		static void FreeModule(string Path) {
			IntPtr Hnd;
			while ((Hnd = Native.GetModuleHandle(Path)) != IntPtr.Zero)
				Native.FreeLibrary(Hnd);
			msg("Free'd: " + Path);
		}
	}
}