Imports System.Runtime.InteropServices

<StructLayout(LayoutKind.Explicit)>
Public Structure ReinterpretableDword
    <FieldOffset(0)> Public Signed As Integer
    <FieldOffset(0)> Public Unsigned As UInteger
End Structure

<StructLayout(LayoutKind.Explicit)>
Public Structure ReinterpretableQword
    <FieldOffset(0)> Public Signed As Long
    <FieldOffset(0)> Public Unsigned As ULong
End Structure