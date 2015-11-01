using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
public delegate void G_CVAR_VARIABLE_STRING_BUFFER_Func(string Str, StringBuilder SB, int Size);

[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
public delegate void G_CVAR_SET_Func(string CVar, string Val);

[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
public delegate void CVAR_SET_NUM_Func(string CVar, float Val);

[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
public delegate int G_CVAR_VARIABLE_INTEGER_VALUE_Func(string CVar);

public static partial class SDK {
	/*public static string G_CVAR_VARIABLE_STRING_BUFFER(string Name, int BufferSize = 80) {
		StringBuilder SB = new StringBuilder(BufferSize);
		int Num = (int)Syscalls_G.G_CVAR_SET + 7;
		QAGameTable.GetFunc<G_CVAR_VARIABLE_STRING_BUFFER_Func>(Num)(Name, SB, SB.Length);
		return SB.ToString();
	}

	public static int G_CVAR_VARIABLE_INTEGER_VALUE(string Name) {
		return QAGameTable.GetFunc<G_CVAR_VARIABLE_INTEGER_VALUE_Func>(2)(Name);
	}*/

	public static void CVAR_SET_NUM(string CVar, float Num) {
		QAGameTable.GetFunc<CVAR_SET_NUM_Func>((int)Syscalls.CVAR_SET_NUM)(CVar, Num);
	}

	public static void CVAR_SET(string CVar, string Val) {
		QAGameTable.GetFunc<G_CVAR_SET_Func>((int)Syscalls.CVAR_SET)(CVar, Val);
	}

	/*public static void TEST() {
		G_CVAR_SET("fagefage", "42");
		Console.WriteLine("fagefage: {0}", G_CVAR_VARIABLE_INTEGER_VALUE("fagefage"));
		//QAGameTable.GetFunc<PrintFunc>((int)Syscalls_G.G_SEND_CONSOLE_COMMAND)("echo ^2Hello World!\n");
	}*/
}