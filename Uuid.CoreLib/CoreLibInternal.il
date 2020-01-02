.assembly extern System.Runtime
{
  .publickeytoken = ( B0 3F 5F 7F 11 D5 0A 3A )
  .ver 4:0:0:0
}

.assembly extern System.Private.CoreLib
{
  .publickeytoken = ( 7C EC 85 D7 BE A7 79 8E )
  .ver 4:0:0:0
}

.assembly Uuid.CoreLib
{
  .ver 1:0:0:0

  .custom instance void System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute::.ctor(string) = (
  	01 00 16 53 79 73 74 65 6d 2e 50 72 69 76 61 74
  	65 2e 43 6f 72 65 4c 69 62 00 00
  )
}

.module Uuid.CoreLib.dll

.class public abstract sealed auto ansi beforefieldinit
  Uuid.CoreLib.Internal
    extends [System.Runtime]System.Object
{

  .method public hidebysig static void
    GetRandomBytes(
      unsigned int8* buffer,
      int32 length
    ) cil managed
  {
    .maxstack 8
    ldarg.0      // buffer
    ldarg.1      // length
    call         void [System.Private.CoreLib]Interop::GetRandomBytes(unsigned int8*, int32)
    ret

  } // end of method Uuid.CoreLib.Internal::GetRandomBytes
  
  .method public hidebysig static string
    FastAllocateString(
      int32 length
    ) cil managed
  {
    .maxstack 8
    ldarg.0      // length
    call         string System.String::FastAllocateString(int32)
    ret
  } // Uuid.CoreLib.Internal::FastAllocateString
} // end of class Uuid.CoreLib.Internal

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
