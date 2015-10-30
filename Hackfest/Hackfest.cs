using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;
using Hackery;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace hackfest {
	[UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
	delegate IntPtr GetProcAddressFunc(IntPtr Lib, string Proc);

	[UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
	delegate IntPtr LoadLibraryFunc(string Name);

	[UnmanagedFunctionPointer(CallingConvention.Winapi)]
	delegate bool FreeLibraryFunc(IntPtr Lib);

	public class Hackfest {
		static HashSet<Delegate> Delegates = new HashSet<Delegate>();
		static object Lock = new object();
		static string[] SkipDlls, SDKDlls;
		static HookHandle GetProcAddressHook, LoadLibraryHook, FreeLibraryHook;

		static HookHandle HookKernel32(string Func, Delegate NewFunc) {
			Delegates.Add(NewFunc);
			return HookHandle.CreateHook("kernel32.dll", Func, Marshal.GetFunctionPointerForDelegate(NewFunc));
		}

		static string GetModuleName(IntPtr Mod) {
			StringBuilder SB = new StringBuilder(1024);
			Kernel32.GetModuleFileName(Mod, SB);
			return Path.GetFileName(SB.ToString().Trim()).ToUpper();
		}

		[DllExport(CallingConvention = CallingConvention.Winapi)]
		public static void Init(IntPtr Arg) {
			//Debugger.Launch();

			Kernel32.AllocConsole();
			Environment.SetEnvironmentVariable("SteamAppId", "282440");
			Console.WriteLine(Environment.CommandLine);

			SkipDlls = new string[] { "WINMM.DLL", "DINPUT8.DLL", "DINPUT.DLL",
				"DDRAW.DLL", "XINPUT9_1_0.DLL", "OPENGL32.DLL", "USER32.DLL",
				"KERNELBASE.DLL", "KERNEL32.DLL", "DWMAPI.DLL", "GDI32.DLL",
				"PSAPI.DLL", "SETUPAPI.DLL" };
			SDKDlls = new string[] { "CGAMEX86.DLL", "QAGAMEX86.DLL", "UIX86.DLL" };

			LoadLibraryHook = HookKernel32("LoadLibraryA", (LoadLibraryFunc)_LoadLibrary);
			LoadLibraryHook.Hook();
			Magic.ResumeProcess(Process.GetCurrentProcess());
		}

		static IntPtr _GetProcAddress(IntPtr Lib, string Proc) {
			lock (Lock) {
				GetProcAddressHook.Unhook();
				//string ModName = GetModuleName(Lib);
				IntPtr Ret = Kernel32.GetProcAddress(Lib, Proc);
				GetProcAddressHook.Hook();
				return Ret;
			}
		}

		static IntPtr _LoadLibrary(string Name) {
			lock (Lock) {
				LoadLibraryHook.Unhook();

				string ModName = Path.GetFileName(Name).ToUpper();
				if (ModName == "OPENGL32.DLL" && FreeLibraryHook == null) {
					FreeLibraryHook = HookKernel32("FreeLibrary", (FreeLibraryFunc)_FreeLibrary);
					FreeLibraryHook.Hook();
					GetProcAddressHook = HookKernel32("GetProcAddress", (GetProcAddressFunc)_GetProcAddress);
					GetProcAddressHook.Hook();
				}

				IntPtr Ret = IntPtr.Zero;

				if (SDKDlls.Contains(ModName)) {
					Name = "cliq3\\bin\\" + Path.GetFileName(Name);
					Ret = Kernel32.GetModuleHandle(Name);
				}

				if (Ret == IntPtr.Zero)
					Ret = Kernel32.LoadLibrary(Name);
				LoadLibraryHook.Hook();
				return Ret;
			}
		}

		static bool _FreeLibrary(IntPtr Lib) {
			lock (Lock) {
				FreeLibraryHook.Unhook();

				string ModName = GetModuleName(Lib);
				if (ModName == "OPENGL32.DLL")
					HookHandle.DisposeAllHandles();

				bool Ret = true;
				if (!SDKDlls.Contains(ModName))
					Ret = Kernel32.FreeLibrary(Lib);

				FreeLibraryHook.Hook();
				return Ret;
			}
		}
	}
}