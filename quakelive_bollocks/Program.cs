using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Hackery;

namespace quakelive_launcher {
	class Program {
		static void Main(string[] Args) {
			try {
				SafeMain(Args);
			} catch (Exception E) {
				MessageBox.Show(E.ToString(), E.GetType().FullName, MessageBoxButtons.OK);
				Environment.Exit(1);
			}
			Environment.Exit(0);
		}

		static void SafeMain(string[] Args) {
			if (Args.Length == 0)
				Args = new[] { "cliq3" };
			string Dev = " +set developer 1 +set fs_debug 1 +set logfile 3";
			string CmdLine = "quakelive_steam +set sv_pure 0 +set sv_vac 0 +set fs_game \"" + Args[0] + "\"" + Dev;
			Process QL = Magic.CreateProcess("quakelive_steam.exe", CmdLine, ProcessCreationFlags.CREATE_SUSPENDED);
			Magic.Inject(QL, "hackfest.dll", "Init");
		}
	}
}