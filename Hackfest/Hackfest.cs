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

namespace hackfest {
	public class Hackfest {
		[DllExport(CallingConvention = CallingConvention.Winapi)]
		public static void Init(IntPtr Arg) {
			Kernel32.AllocConsole();
			Environment.SetEnvironmentVariable("SteamAppId", "282440");
			Console.WriteLine(Environment.CommandLine);

			// TODO: Magic

			Magic.ResumeProcess(Process.GetCurrentProcess());
		}
	}
}