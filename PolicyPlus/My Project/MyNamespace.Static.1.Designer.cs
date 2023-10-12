// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic;

/* TODO ERROR: Skipped IfDirectiveTrivia
#If TARGET = "module" AndAlso _MYTYPE = "" Then
*//* TODO ERROR: Skipped DisabledTextTrivia
#Const _MYTYPE="Empty"
*//* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
/* TODO ERROR: Skipped IfDirectiveTrivia
#If _MYTYPE = "WindowsForms" Then
*/
/* TODO ERROR: Skipped DefineDirectiveTrivia
#Const _MYFORMS = True
*//* TODO ERROR: Skipped DefineDirectiveTrivia
#Const _MYWEBSERVICES = True
*//* TODO ERROR: Skipped DefineDirectiveTrivia
#Const _MYUSERTYPE = "Windows"
*//* TODO ERROR: Skipped DefineDirectiveTrivia
#Const _MYCOMPUTERTYPE = "Windows"
*//* TODO ERROR: Skipped DefineDirectiveTrivia
#Const _MYAPPLICATIONTYPE = "WindowsForms"
*/
/* TODO ERROR: Skipped ElifDirectiveTrivia
#ElseIf _MYTYPE = "WindowsFormsWithCustomSubMain" Then
*//* TODO ERROR: Skipped DisabledTextTrivia

#Const _MYFORMS = True
#Const _MYWEBSERVICES = True
#Const _MYUSERTYPE = "Windows"
#Const _MYCOMPUTERTYPE = "Windows"
#Const _MYAPPLICATIONTYPE = "Console"

*//* TODO ERROR: Skipped ElifDirectiveTrivia
#ElseIf _MYTYPE = "Windows" OrElse _MYTYPE = "" Then
*//* TODO ERROR: Skipped DisabledTextTrivia

#Const _MYWEBSERVICES = True
#Const _MYUSERTYPE = "Windows"
#Const _MYCOMPUTERTYPE = "Windows"
#Const _MYAPPLICATIONTYPE = "Windows"

*//* TODO ERROR: Skipped ElifDirectiveTrivia
#ElseIf _MYTYPE = "Console" Then
*//* TODO ERROR: Skipped DisabledTextTrivia

#Const _MYWEBSERVICES = True
#Const _MYUSERTYPE = "Windows"
#Const _MYCOMPUTERTYPE = "Windows"
#Const _MYAPPLICATIONTYPE = "Console"

*//* TODO ERROR: Skipped ElifDirectiveTrivia
#ElseIf _MYTYPE = "Web" Then
*//* TODO ERROR: Skipped DisabledTextTrivia

#Const _MYFORMS = False
#Const _MYWEBSERVICES = False
#Const _MYUSERTYPE = "Web"
#Const _MYCOMPUTERTYPE = "Web"

*//* TODO ERROR: Skipped ElifDirectiveTrivia
#ElseIf _MYTYPE = "WebControl" Then
*//* TODO ERROR: Skipped DisabledTextTrivia

#Const _MYFORMS = False
#Const _MYWEBSERVICES = True
#Const _MYUSERTYPE = "Web"
#Const _MYCOMPUTERTYPE = "Web"

*//* TODO ERROR: Skipped ElifDirectiveTrivia
#ElseIf _MYTYPE = "Custom" Then
*//* TODO ERROR: Skipped DisabledTextTrivia

*//* TODO ERROR: Skipped ElifDirectiveTrivia
#ElseIf _MYTYPE <> "Empty" Then
*//* TODO ERROR: Skipped DisabledTextTrivia

#Const _MYTYPE = "Empty"

*//* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
/* TODO ERROR: Skipped IfDirectiveTrivia
#If _MYTYPE <> "Empty" Then
*/
namespace PolicyPlus.My
{

    /* TODO ERROR: Skipped IfDirectiveTrivia
    #If _MYAPPLICATIONTYPE = "WindowsForms" OrElse _MYAPPLICATIONTYPE = "Windows" OrElse _MYAPPLICATIONTYPE = "Console" Then
    */
    [System.CodeDom.Compiler.GeneratedCode("MyTemplate", "11.0.0.0")]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]

    /* TODO ERROR: Skipped IfDirectiveTrivia
    #If _MYAPPLICATIONTYPE = "WindowsForms" Then
    */
    internal partial class MyApplication : Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase
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
            MyProject.Application.Run(Args);
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

    /* TODO ERROR: Skipped EndIfDirectiveTrivia
    #End If '#If _MYAPPLICATIONTYPE = "WindowsForms" Or _MYAPPLICATIONTYPE = "Windows" or _MYAPPLICATIONTYPE = "Console"
    */
    /* TODO ERROR: Skipped IfDirectiveTrivia
    #If _MYCOMPUTERTYPE <> "" Then
    */
    [System.CodeDom.Compiler.GeneratedCode("MyTemplate", "11.0.0.0")]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]

    /* TODO ERROR: Skipped IfDirectiveTrivia
    #If _MYCOMPUTERTYPE = "Windows" Then
    */
    internal partial class MyComputer : Microsoft.VisualBasic.Devices.Computer
    {
        /* TODO ERROR: Skipped ElifDirectiveTrivia
        #ElseIf _MYCOMPUTERTYPE = "Web" Then
        *//* TODO ERROR: Skipped DisabledTextTrivia
                Inherits Global.Microsoft.VisualBasic.Devices.ServerComputer
        *//* TODO ERROR: Skipped EndIfDirectiveTrivia
        #End If
        */
        [DebuggerHidden()]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public MyComputer() : base()
        {
        }
    }
    /* TODO ERROR: Skipped EndIfDirectiveTrivia
    #End If
    */
    [HideModuleName()]
    [System.CodeDom.Compiler.GeneratedCode("MyTemplate", "11.0.0.0")]
    internal static partial class MyProject
    {

        /* TODO ERROR: Skipped IfDirectiveTrivia
        #If _MYCOMPUTERTYPE <> "" Then
        */
        [System.ComponentModel.Design.HelpKeyword("My.Computer")]
        internal static MyComputer Computer
        {
            [DebuggerHidden()]
            get
            {
                return m_ComputerObjectProvider.GetInstance;
            }
        }

        private readonly static ThreadSafeObjectProvider<MyComputer> m_ComputerObjectProvider = new ThreadSafeObjectProvider<MyComputer>();
        /* TODO ERROR: Skipped EndIfDirectiveTrivia
        #End If
        */
        /* TODO ERROR: Skipped IfDirectiveTrivia
        #If _MYAPPLICATIONTYPE = "Windows" Or _MYAPPLICATIONTYPE = "WindowsForms" Or _MYAPPLICATIONTYPE = "Console" Then
        */
        [System.ComponentModel.Design.HelpKeyword("My.Application")]
        internal static MyApplication Application
        {
            [DebuggerHidden()]
            get
            {
                return m_AppObjectProvider.GetInstance;
            }
        }
        private readonly static ThreadSafeObjectProvider<MyApplication> m_AppObjectProvider = new ThreadSafeObjectProvider<MyApplication>();
        /* TODO ERROR: Skipped EndIfDirectiveTrivia
        #End If
        */
        /* TODO ERROR: Skipped IfDirectiveTrivia
        #If _MYUSERTYPE = "Windows" Then
        */
        [System.ComponentModel.Design.HelpKeyword("My.User")]
        internal static Microsoft.VisualBasic.ApplicationServices.User User
        {
            [DebuggerHidden()]
            get
            {
                return m_UserObjectProvider.GetInstance;
            }
        }
        private readonly static ThreadSafeObjectProvider<Microsoft.VisualBasic.ApplicationServices.User> m_UserObjectProvider = new ThreadSafeObjectProvider<Microsoft.VisualBasic.ApplicationServices.User>();
        /* TODO ERROR: Skipped ElifDirectiveTrivia
        #ElseIf _MYUSERTYPE = "Web" Then
        *//* TODO ERROR: Skipped DisabledTextTrivia
                <Global.System.ComponentModel.Design.HelpKeyword("My.User")> _
                Friend ReadOnly Property User() As Global.Microsoft.VisualBasic.ApplicationServices.WebUser
                    <Global.System.Diagnostics.DebuggerHidden()> _
                    Get
                        Return m_UserObjectProvider.GetInstance()
                    End Get
                End Property
                Private ReadOnly m_UserObjectProvider As New ThreadSafeObjectProvider(Of Global.Microsoft.VisualBasic.ApplicationServices.WebUser)
        *//* TODO ERROR: Skipped EndIfDirectiveTrivia
        #End If
        */
        /* TODO ERROR: Skipped IfDirectiveTrivia
        #If _MYFORMS = True Then
        */
        /* TODO ERROR: Skipped DefineDirectiveTrivia
        #Const STARTUP_MY_FORM_FACTORY = "My.MyProject.Forms"
        */
        [System.ComponentModel.Design.HelpKeyword("My.Forms")]
        internal static MyForms Forms
        {
            [DebuggerHidden()]
            get
            {
                return m_MyFormsObjectProvider.GetInstance;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [MyGroupCollection("System.Windows.Forms.Form", "Create__Instance__", "Dispose__Instance__", "My.MyProject.Forms")]
        internal sealed partial class MyForms
        {
            [DebuggerHidden()]
            private static T Create__Instance__<T>(T Instance) where T : Form, new()
            {
                if (Instance is null || Instance.IsDisposed)
                {
                    if (m_FormBeingCreated is not null)
                    {
                        if (m_FormBeingCreated.ContainsKey(typeof(T)) == true)
                        {
                            throw new InvalidOperationException(Microsoft.VisualBasic.CompilerServices.Utils.GetResourceString("WinForms_RecursiveFormCreate"));
                        }
                    }
                    else
                    {
                        m_FormBeingCreated = new Hashtable();
                    }
                    m_FormBeingCreated.Add(typeof(T), null);
                    try
                    {
                        return new T();
                    }
                    catch (System.Reflection.TargetInvocationException ex) when (ex.InnerException is not null)
                    {
                        string BetterMessage = Microsoft.VisualBasic.CompilerServices.Utils.GetResourceString("WinForms_SeeInnerException", ex.InnerException.Message);
                        throw new InvalidOperationException(BetterMessage, ex.InnerException);
                    }
                    finally
                    {
                        m_FormBeingCreated.Remove(typeof(T));
                    }
                }
                else
                {
                    return Instance;
                }
            }

            [DebuggerHidden()]
            private void Dispose__Instance__<T>(ref T instance) where T : Form
            {
                instance.Dispose();
                instance = null;
            }

            [DebuggerHidden()]
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            public MyForms() : base()
            {
            }

            [ThreadStatic()]
            private static Hashtable m_FormBeingCreated;

            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            public override bool Equals(object o)
            {
                return base.Equals(o);
            }
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            internal new Type GetType()
            {
                return typeof(MyForms);
            }
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            public override string ToString()
            {
                return base.ToString();
            }
        }

        private static ThreadSafeObjectProvider<MyForms> m_MyFormsObjectProvider = new ThreadSafeObjectProvider<MyForms>();

        /* TODO ERROR: Skipped EndIfDirectiveTrivia
        #End If
        */
        /* TODO ERROR: Skipped IfDirectiveTrivia
        #If _MYWEBSERVICES = True Then
        */
        [System.ComponentModel.Design.HelpKeyword("My.WebServices")]
        internal static MyWebServices WebServices
        {
            [DebuggerHidden()]
            get
            {
                return m_MyWebServicesObjectProvider.GetInstance;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [MyGroupCollection("System.Web.Services.Protocols.SoapHttpClientProtocol", "Create__Instance__", "Dispose__Instance__", "")]
        internal sealed class MyWebServices
        {

            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            [DebuggerHidden()]
            public override bool Equals(object o)
            {
                return base.Equals(o);
            }
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            [DebuggerHidden()]
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            [DebuggerHidden()]
            internal new Type GetType()
            {
                return typeof(MyWebServices);
            }
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            [DebuggerHidden()]
            public override string ToString()
            {
                return base.ToString();
            }

            [DebuggerHidden()]
            private static T Create__Instance__<T>(T instance) where T : new()
            {
                if (instance is null)
                {
                    return new T();
                }
                else
                {
                    return instance;
                }
            }

            [DebuggerHidden()]
            private void Dispose__Instance__<T>(ref T instance)
            {
                instance = default;
            }

            [DebuggerHidden()]
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            public MyWebServices() : base()
            {
            }
        }

        private readonly static ThreadSafeObjectProvider<MyWebServices> m_MyWebServicesObjectProvider = new ThreadSafeObjectProvider<MyWebServices>();
        /* TODO ERROR: Skipped EndIfDirectiveTrivia
        #End If
        */
        /* TODO ERROR: Skipped IfDirectiveTrivia
        #If _MYTYPE = "Web" Then
        *//* TODO ERROR: Skipped DisabledTextTrivia

                <Global.System.ComponentModel.Design.HelpKeyword("My.Request")> _
                Friend ReadOnly Property Request() As Global.System.Web.HttpRequest
                    <Global.System.Diagnostics.DebuggerHidden()> _
                    Get
                        Dim CurrentContext As Global.System.Web.HttpContext = Global.System.Web.HttpContext.Current
                        If CurrentContext IsNot Nothing Then
                            Return CurrentContext.Request
                        End If
                        Return Nothing
                    End Get
                End Property

                <Global.System.ComponentModel.Design.HelpKeyword("My.Response")> _
                Friend ReadOnly Property Response() As Global.System.Web.HttpResponse
                    <Global.System.Diagnostics.DebuggerHidden()> _
                    Get
                        Dim CurrentContext As Global.System.Web.HttpContext = Global.System.Web.HttpContext.Current
                        If CurrentContext IsNot Nothing Then
                            Return CurrentContext.Response
                        End If
                        Return Nothing
                    End Get
                End Property

                <Global.System.ComponentModel.Design.HelpKeyword("My.Application.Log")> _
                Friend ReadOnly Property Log() As Global.Microsoft.VisualBasic.Logging.AspLog
                    <Global.System.Diagnostics.DebuggerHidden()> _
                    Get
                        Return m_LogObjectProvider.GetInstance()
                    End Get
                End Property

                Private ReadOnly m_LogObjectProvider As New ThreadSafeObjectProvider(Of Global.Microsoft.VisualBasic.Logging.AspLog)

        *//* TODO ERROR: Skipped EndIfDirectiveTrivia
        #End If  '_MYTYPE="Web"
        */
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Runtime.InteropServices.ComVisible(false)]
        internal sealed class ThreadSafeObjectProvider<T> where T : new()
        {
            internal T GetInstance
            {
                /* TODO ERROR: Skipped IfDirectiveTrivia
                #If TARGET = "library" Then
                *//* TODO ERROR: Skipped DisabledTextTrivia
                                <Global.System.Diagnostics.DebuggerHidden()> _
                                Get
                                    Dim Value As T = m_Context.Value
                                    If Value Is Nothing Then
                                        Value = New T
                                        m_Context.Value() = Value
                                    End If
                                    Return Value
                                End Get
                *//* TODO ERROR: Skipped ElseDirectiveTrivia
                #Else
                */
                [DebuggerHidden()]
                get
                {
                    if (m_ThreadStaticValue is null)
                        m_ThreadStaticValue = new T();
                    return m_ThreadStaticValue;
                }
                /* TODO ERROR: Skipped EndIfDirectiveTrivia
                #End If
                */
            }

            [DebuggerHidden()]
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            public ThreadSafeObjectProvider() : base()
            {
            }

            /* TODO ERROR: Skipped IfDirectiveTrivia
            #If TARGET = "library" Then
            *//* TODO ERROR: Skipped DisabledTextTrivia
                        Private ReadOnly m_Context As New Global.Microsoft.VisualBasic.MyServices.Internal.ContextValue(Of T)
            *//* TODO ERROR: Skipped ElseDirectiveTrivia
            #Else
            */
            [System.Runtime.CompilerServices.CompilerGenerated()]
            [ThreadStatic()]
            private static T m_ThreadStaticValue;
            /* TODO ERROR: Skipped EndIfDirectiveTrivia
            #End If
            */
        }
    }
}
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/