using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Migrate")]
[assembly: AssemblyProduct("Migrate")]

[assembly: AssemblyVersion("2.2.0.0")]
[assembly: AssemblyFileVersion("2.2.0.0")]

[assembly: CLSCompliant(true)]

[assembly: ComVisible(false)]

[assembly: NeutralResourcesLanguage("en-US")]

// use sn.exe -Tp assembly.dll (from the SDK to figure out this nice number)
[assembly: InternalsVisibleTo("MigSharp.NUnit")]
