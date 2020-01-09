.assembly extern System.Runtime
{
  .publickeytoken = ( B0 3F 5F 7F 11 D5 0A 3A )
  .ver 4:2:2:0
}

.assembly extern System.Runtime.Extensions
{
  .publickeytoken = ( B0 3F 5F 7F 11 D5 0A 3A )
  .ver 4:2:2:0
}

.assembly extern System.Private.CoreLib
{
  .publickeytoken = ( 7C EC 85 D7 BE A7 79 8E )
  .ver 4:0:0:0
}
.assembly Uuids.CoreLib
{
	.custom instance void [System.Runtime]System.Runtime.CompilerServices.CompilationRelaxationsAttribute::.ctor(int32) = (
		01 00 08 00 00 00 00 00
	)
	.custom instance void [System.Runtime]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::.ctor() = (
		01 00 01 00 54 02 16 57 72 61 70 4e 6f 6e 45 78
		63 65 70 74 69 6f 6e 54 68 72 6f 77 73 01
	)
	.custom instance void [System.Runtime]System.Runtime.Versioning.TargetFrameworkAttribute::.ctor(string) = (
		01 00 18 2e 4e 45 54 43 6f 72 65 41 70 70 2c 56
		65 72 73 69 6f 6e 3d 76 33 2e 31 01 00 54 0e 14
		46 72 61 6d 65 77 6f 72 6b 44 69 73 70 6c 61 79
		4e 61 6d 65 00
	)
	.custom instance void [System.Runtime]System.Reflection.AssemblyCompanyAttribute::.ctor(string) = (
		01 00 0d 55 75 69 64 73 2e 43 6f 72 65 4c 69 62
		00 00
	)
	.custom instance void [System.Runtime]System.Reflection.AssemblyConfigurationAttribute::.ctor(string) = (
		01 00 07 52 65 6c 65 61 73 65 00 00
	)
	.custom instance void [System.Runtime]System.Reflection.AssemblyFileVersionAttribute::.ctor(string) = (
		01 00 07 31 2e 30 2e 33 2e 30 00 00
	)
	.custom instance void [System.Runtime]System.Reflection.AssemblyInformationalVersionAttribute::.ctor(string) = (
		01 00 05 31 2e 30 2e 33 00 00
	)
	.custom instance void [System.Runtime]System.Reflection.AssemblyProductAttribute::.ctor(string) = (
		01 00 0d 55 75 69 64 73 2e 43 6f 72 65 4c 69 62
		00 00
	)
	.custom instance void [System.Runtime]System.Reflection.AssemblyTitleAttribute::.ctor(string) = (
		01 00 0d 55 75 69 64 73 2e 43 6f 72 65 4c 69 62
		00 00
	)
  .custom instance void System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute::.ctor(string) = (
  	01 00 16 53 79 73 74 65 6d 2e 50 72 69 76 61 74
  	65 2e 43 6f 72 65 4c 69 62 00 00
  )
	.hash algorithm 0x00008004 // SHA1
	.ver 1:0:3:0
}

.module Uuids.CoreLib.dll

.class public abstract sealed auto ansi beforefieldinit
  Uuids.CoreLib.Internal
    extends [System.Runtime]System.Object
{

  .method public hidebysig static void
    GetRandomBytes(
      unsigned int8* buffer,
      int32 length
    ) cil managed aggressiveinlining
  {
    .maxstack 8
    ldarg.0      // buffer
    ldarg.1      // length
    call         void [System.Private.CoreLib]Interop::GetRandomBytes(unsigned int8*, int32)
    ret

  } // end of method Uuids.CoreLib.Internal::GetRandomBytes
  
  .method public hidebysig static string
    FastAllocateString(
      int32 length
    ) cil managed aggressiveinlining
  {
    .maxstack 8
    ldarg.0      // length
    call         string System.String::FastAllocateString(int32)
    ret
  } // Uuids.CoreLib.Internal::FastAllocateString
} // end of class Uuids.CoreLib.Internal

.class public auto ansi beforefieldinit
  System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute
    extends [System.Runtime]System.Attribute
{
  .custom instance void [System.Runtime]System.AttributeUsageAttribute::.ctor(valuetype [System.Runtime]System.AttributeTargets)
    = (
      01 00 01 00 00 00 01 00 54 02 0d 41 6c 6c 6f 77 // ........T..Allow
      4d 75 6c 74 69 70 6c 65 01                      // Multiple.
    )
    // int32(1) // 0x00000001
    // property bool 'AllowMultiple' = bool(true)

  .field private initonly string '<AssemblyName>k__BackingField'
  .custom instance void [System.Runtime]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor()
    = (01 00 00 00 )

  .method public hidebysig specialname rtspecialname instance void
    .ctor(
      string assemblyName
    ) cil managed
  {
    .maxstack 8
    ldarg.0      // this
    call         instance void [System.Runtime]System.Attribute::.ctor()
    ldarg.0      // this
    ldarg.1      // assemblyName
    stfld        string System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute::'<AssemblyName>k__BackingField'
    ret
  } // end of method IgnoresAccessChecksToAttribute::.ctor

  .method public hidebysig specialname instance string
    get_AssemblyName() cil managed
  {
    .custom instance void [System.Runtime]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor()
      = (01 00 00 00 )
    .maxstack 8
    ldarg.0      // this
    ldfld        string System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute::'<AssemblyName>k__BackingField'
    ret

  } // end of method IgnoresAccessChecksToAttribute::get_AssemblyName

  .property instance string AssemblyName()
  {
    .get instance string System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute::get_AssemblyName()
  } // end of property IgnoresAccessChecksToAttribute::AssemblyName
} // end of class System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute
