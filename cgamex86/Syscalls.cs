using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;

namespace cgamex86 {
	public class Syscalls {
		[DllExport("dllEntry", CallingConvention = CallingConvention.Cdecl)]
		public static void dllEntry(IntPtr Ret, IntPtr SyscallTable, IntPtr C) {

		}
	}
}