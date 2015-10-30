using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Hackery;

namespace quakelive_launcher {
	class Program {
		/*static void Print(string Txt, ConsoleColor Fg = ConsoleColor.Gray) {
			ConsoleColor Old = Console.ForegroundColor;
			Console.ForegroundColor = Fg;
			Console.WriteLine(Txt);
			Console.ForegroundColor = Old;
		}*/

		static void Main(string[] args) {
			if (args.Length == 0)
				args = new[] { "cliq3" };
			string CmdLine = "quakelive_steam +set sv_pure 0 +set sv_vac 0 +set fs_game \"" + args[0] + "\"";
			Process QL = Magic.CreateProcess("quakelive_steam.exe", CmdLine, ProcessCreationFlags.CREATE_SUSPENDED);
			Magic.Inject(QL, "hackfest.dll", "Init");
			Environment.Exit(0);
		}
	}
}