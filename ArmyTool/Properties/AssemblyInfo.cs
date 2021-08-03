// bacteriamage.wordpress.com

using System.Reflection;
using System.Runtime.InteropServices;

// The assembly title appears as the "File description" in the details tab of
// the properties popup in Windows Explorer. This is the name that applies to
// the specific assembly as opposed to an entire product composed by individual
// assemblies.
[assembly: AssemblyTitle("Ogre Battle MOTBQ Import/Export Utility")]

// The name of the product that this assembly belongs to. multiple assemblies
// that compose a single product can share the same value for this attribute.
[assembly: AssemblyProduct("Ogre Battle Army Tool")]

// the company, organization, or entity that developed the assembly and product
[assembly: AssemblyCompany("bacteriamage.wordpress.com")]

// information about the copyright for the assembly and product
[assembly: AssemblyCopyright("None")]

// The assembly configuration attribute records whether the assembly was built
// in debug or release mode. set this based on whether the debug symbol is set.
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

// The informational version appears in the details tab of the properties popup
// and is only used for user facing informational purposes. This can be a user
// friendly version number.
[assembly: AssemblyInformationalVersion("21.8.0")]

// This is the internal version number that .NET looks at when determining
// whether to use the assembly. This will also be the file version when the 
// AssemblyFileVersion attribute is not specified. 
[assembly: AssemblyVersion("1.0.*")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  The default is true.
[assembly: ComVisible(false)]