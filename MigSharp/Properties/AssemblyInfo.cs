using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("MigSharp")]
[assembly: AssemblyProduct("MigSharp")]

[assembly: AssemblyVersion("2.2.0.0")]
[assembly: AssemblyFileVersion("2.2.0.0")]

[assembly: CLSCompliant(true)]

[assembly: ComVisible(false)]

[assembly: NeutralResourcesLanguage("en-US")]

// use sn.exe -Tp assembly.dll (from the SDK to figure out this nice number)
[assembly: InternalsVisibleTo("MigSharp.NUnit")]
[assembly: InternalsVisibleTo("MigSharp.SQLite.NUnit")]
[assembly: InternalsVisibleTo("MigSharp.SqlServer.NUnit")]
[assembly: InternalsVisibleTo("MigSharp.SqlServerCe.NUnit")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
