using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;
using System.Threading;
using Hackery;

namespace qagamex86 {
	class qagamex86 {
		[DllExport("dllEntry", CallingConvention = CallingConvention.Cdecl)]
		public static void dllEntry(ReturnInfo RetInfo, DispatchTable SyscallTable, IntPtr Unknown) {
			SDK.QAGameInit(SyscallTable);
			SDK.PRINT(Colors.Red + "[QAGame] Hello Quake Live!\n");

			IntPtr _uix86 = Kernel32.LoadLibrary("cliq3\\orig\\_qagamex86.dll");
			dllEntryFunc _dllEntry = Kernel32.GetProcAddress<dllEntryFunc>(_uix86, "dllEntry");
			_dllEntry(RetInfo, SyscallTable, Unknown);
		}
	}
}