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
using Hackery;
using hackfest;

namespace uix86 {
	class uix86 {
		[DllExport("dllEntry", CallingConvention = CallingConvention.Cdecl)]
		public static void dllEntry(ReturnInfo RetInfo, DispatchTable SyscallTable, IntPtr Unknown) {
			SDK.UIInit(SyscallTable);
			SDK.PRINT(Colors.Red + "[UI] Hello Quake Live!\n");

			/*SDK.CVAR_SET_NUM("fage_num", 42);
			SDK.CVAR_SET("fage", "fage_string");*/

			IntPtr _uix86 = Kernel32.LoadLibrary("cliq3\\orig\\_uix86.dll");
			dllEntryFunc _dllEntry = Kernel32.GetProcAddress<dllEntryFunc>(_uix86, "dllEntry");
			_dllEntry(RetInfo, SyscallTable, Unknown);
		}
	}
}