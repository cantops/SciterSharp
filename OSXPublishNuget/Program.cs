﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using SciterSharp;

namespace OSXPublishNuget
{
	class MainClass
	{
		public static void Main(string[] args)
		{
            Environment.CurrentDirectory = "/Users/midiway/Downloads/desk/SciterSharp/SciterSharp";
			string nuspec = File.ReadAllText("SciterSharpOSX.nuspec");
			nuspec = Regex.Replace(nuspec,
						  "<version>.*?</version>",
						  "<version>" + LibVersion.AssemblyVersion + "</version>",
			              RegexOptions.None);
			File.WriteAllText("SciterSharpOSX.nuspec", nuspec);

			Process.Start("nuget", "pack SciterSharpOSX.nuspec").WaitForExit();
            Process.Start("nuget", "push SciterSharpOSX." + LibVersion.AssemblyVersion + ".nupkg 7ce52cf9-2dac-412b-8d82-037facc016ff -Source nuget.org").WaitForExit();
		}
	}
}