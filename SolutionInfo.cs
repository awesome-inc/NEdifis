using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyTrademark("<trademark name>")]
[assembly: AssemblyProduct("NEdifis")]

[assembly: AssemblyDescription("Testing framework with convention tests")]
[assembly: AssemblyCompany("Awesome Incremented")]
[assembly: AssemblyCopyright("Copyright © Awesome Incremented 2015ff")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: NeutralResourcesLanguage("en", UltimateResourceFallbackLocation.MainAssembly)]
