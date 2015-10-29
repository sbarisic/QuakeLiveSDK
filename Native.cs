using System;
using System.Runtime.InteropServices;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void dllEntryFunc(ReturnInfo RetInfo, DispatchTable SyscallTable, IntPtr C);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void CdeclAction();

static class Native {
	[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
	public static extern IntPtr GetModuleHandle(string ModuleName);

	[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
	public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string FileName);

	[DllImport("kernel32", SetLastError = true)]
	public static extern bool FreeLibrary(IntPtr Module);

	[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true)]
	public static extern IntPtr GetProcAddress(IntPtr Module, string ProcName);

	public static T GetProcAddress<T>(IntPtr Module, string ProcName) where T : class {
		if (!typeof(Delegate).IsAssignableFrom(typeof(T)))
			throw new Exception("Type has to be a delegate");

		return Marshal.GetDelegateForFunctionPointer(GetProcAddress(Module, ProcName), typeof(T)) as T;
	}
}