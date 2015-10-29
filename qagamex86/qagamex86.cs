using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;

namespace qagamex86 {
	class qagamex86 {
		[DllExport("dllEntry", CallingConvention = CallingConvention.Cdecl)]
		public static void dllEntry(ReturnInfo RetInfo, DispatchTable SyscallTable, IntPtr Unknown) {

		}
	}
}