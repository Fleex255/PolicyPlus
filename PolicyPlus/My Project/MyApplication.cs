using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace PolicyPlus.My_Project
{
    [System.CodeDom.Compiler.GeneratedCode("MyTemplate", "11.0.0.0")]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]

    /* TODO ERROR: Skipped IfDirectiveTrivia
    #If _MYAPPLICATIONTYPE = "WindowsForms" Then
    */
    internal class MyApplication : Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase
    {
        /* TODO ERROR: Skipped IfDirectiveTrivia
        #If TARGET = "winexe" Then
        */
        [STAThread()]
        [DebuggerHidden()]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static void Main(string[] Args)
        {
            try
            {
                Application.SetCompatibleTextRenderingDefault(UseCompatibleTextRendering);
            }
            finally
            {
            }
            My.MyProject.Application.Run(Args);
        }
        /* TODO ERROR: Skipped EndIfDirectiveTrivia
        #End If
        */
        /* TODO ERROR: Skipped ElifDirectiveTrivia
        #ElseIf _MYAPPLICATIONTYPE = "Windows" Then
        *//* TODO ERROR: Skipped DisabledTextTrivia
                Inherits Global.Microsoft.VisualBasic.ApplicationServices.ApplicationBase
        *//* TODO ERROR: Skipped ElifDirectiveTrivia
        #ElseIf _MYAPPLICATIONTYPE = "Console" Then
        *//* TODO ERROR: Skipped DisabledTextTrivia
                Inherits Global.Microsoft.VisualBasic.ApplicationServices.ConsoleApplicationBase
        *//* TODO ERROR: Skipped EndIfDirectiveTrivia
        #End If '_MYAPPLICATIONTYPE = "WindowsForms"
        */
    }
}