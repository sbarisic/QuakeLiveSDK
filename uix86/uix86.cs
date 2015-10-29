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

	[UnmanagedFunctionPointer(CallingConvention.Winapi)]
	delegate bool FreeLibFunc(IntPtr H);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	delegate int InitFunc(int A, int B, int C);

	class uix86 {
		static void msg(string Str) {
			if (Print != null)
				Print(Str + "\n");
			File.AppendAllLines("INFO.txt", new string[] { Str });
		}

		static string ModuleToString(IntPtr Mod) {
			StringBuilder FileName = new StringBuilder(1024);
			Native.GetModuleFileName(Mod, FileName);
			return Mod + " - " + FileName.ToString().Trim();
		}

		static bool FreeLibHooked(IntPtr H) {
			FreeLibraryHook.Unhook();
			msg("FreeLibrary(" + ModuleToString(H) + ")");

			IntPtr ThisHandle = Native.GetModuleHandle("uix86.dll");
			if (H == ThisHandle) {
				IntPtr _uix86 = Native.GetModuleHandle("_uix86.dll");
				if (_uix86 != IntPtr.Zero) {
					msg("FreeLibrary(" + ModuleToString(_uix86) + ")");
					Native.FreeLibrary(_uix86);
				}

				FreeModule("uix86.dll");
				return true;
			}

			bool Ret = Native.FreeLibrary(H);
			FreeLibraryHook.Hook();
			return Ret;
		}

		static PrintFunc Print;
		static HookHandle FreeLibraryHook;
		static IntPtr _uix86;

		[DllExport("dllEntry", CallingConvention = CallingConvention.Cdecl)]
		public static void dllEntry(ReturnInfo RetInfo, DispatchTable SyscallTable, IntPtr Unknown) {
			//WaitForDebugger();
			Print = SyscallTable.GetFunc<PrintFunc>(Syscalls.UI_PRINT);
			msg("-- Initializing");

			msg(ModuleToString(Native.GetModuleHandle("uix86.dll")));
			_uix86 = Native.LoadLibrary("cliq3\\orig\\_uix86.dll");
			msg(ModuleToString(_uix86));
			dllEntryFunc _dllEntry = Native.GetProcAddress<dllEntryFunc>(_uix86, "dllEntry");


			msg("Hookin' shit");
			{
				IntPtr kernel32 = Native.LoadLibrary("kernel32.dll");
				IntPtr FreeLib = Native.GetProcAddress(kernel32, "FreeLibrary");
				FreeLibraryHook = HookHandle.CreateHook(FreeLib, (Delegate)new FreeLibFunc(FreeLibHooked));
			}

			msg("Calling dllEntry");
			{
				_dllEntry(RetInfo, SyscallTable, Unknown);
			}

			DispatchTable DT = RetInfo.GetDispatchTable();
			{
				/*InitFunc Init = DT.GetFunc<InitFunc>(0);
				DT.SetFunc(0, new InitFunc((A, B, C) => {
					msg(string.Format("Init({0}, {1}, {2})", A, B, C));
					return Init(A, B, C);
				}));*/

				ShutdownFunc Shutdown = DT.GetFunc<ShutdownFunc>(1);
				DT.SetFunc(1, new ShutdownFunc(() => {
					int Ret = Shutdown();
					msg("Shutdown: " + Ret);
					{
						//DT.SetFunc(0, Init);
						DT.SetFunc(1, Shutdown);
						Print = null;
					}
					//Native.FreeLibrary(Native.GetModuleHandle("uix86.dll"));
					return Ret;
				}));
			}

			msg("[C#] Hello Quake Live World!");
		}

		static void WaitForDebugger() {
			msg("Waiting for debugger");
			while (!Debugger.IsAttached)
				;
			msg("Debugger found");
		}

		static void FreeModule(string Path) {
			IntPtr Hnd;
			while ((Hnd = Native.GetModuleHandle(Path)) != IntPtr.Zero)
				Native.FreeLibrary(Hnd);
			msg("Free'd: " + Path);
		}
	}
}