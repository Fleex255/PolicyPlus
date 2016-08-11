Public Class EditSetting
    Public CurrentSetting As PolicyPlusPolicy
    Public CurrentSection As AdmxPolicySection
    Public AdmxWorkspace As AdmxBundle
    Public CompPolSource, UserPolSource As IPolicySource
    Dim ElementControls As Dictionary(Of String, Control)
    Dim CurrentSource As IPolicySource
    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Close()
    End Sub
    Private Sub EditSetting_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        SettingNameLabel.Text = CurrentSetting.DisplayName
        SupportedTextbox.Text = CurrentSetting.SupportedOn.DisplayName
        HelpTextbox.Text = CurrentSetting.DisplayExplanation
        If CurrentSetting.RawPolicy.Section = AdmxPolicySection.Both Then
            SectionDropdown.Enabled = True
            CurrentSection = IIf(CurrentSection = AdmxPolicySection.Both, AdmxPolicySection.Machine, CurrentSection)
        Else
            SectionDropdown.Enabled = False
            CurrentSection = CurrentSetting.RawPolicy.Section
        End If
        PreparePolicyElements()
        SectionDropdown.Text = IIf(CurrentSection = AdmxPolicySection.Machine, "Computer", "User")
        PreparePolicyState()
    End Sub
    Sub PreparePolicyElements()
        For n = ExtraOptionsTable.RowCount - 1 To 0 Step -1 ' Go backwards because Dispose changes the indexes
            Dim ctl = ExtraOptionsTable.GetControlFromPosition(0, n)
            If ctl IsNot Nothing Then ctl.Dispose()
        Next
        ExtraOptionsTable.Controls.Clear()
        ExtraOptionsTable.RowCount = 0
        Dim addControl = Sub(ID As String, Control As Control, Label As String)
                             ExtraOptionsTable.RowStyles.Add(New RowStyle(SizeType.AutoSize))
                             If Label = "" Then ' Just a single control
                                 If Control.AutoSize Then Control.MaximumSize = New Size(ExtraOptionsTable.ClientSize.Width, 0)
                                 ExtraOptionsTable.Controls.Add(Control, 0, ExtraOptionsTable.RowStyles.Count - 1)
                             Else ' Has a label attached
                                 Dim flowPanel As New FlowLayoutPanel With {.WrapContents = True, .AutoSize = True, .AutoSizeMode = AutoSizeMode.GrowAndShrink}
                                 flowPanel.MaximumSize = New Size(ExtraOptionsTable.ClientRectangle.Width, 0)
                                 flowPanel.Margin = New Padding(0)
                                 ExtraOptionsTable.Controls.Add(flowPanel, 0, ExtraOptionsTable.RowStyles.Count - 1)
                                 Dim labelControl As New Label With {.AutoSize = True, .Text = Label}
                                 labelControl.Anchor = AnchorStyles.Left
                                 Control.Anchor = AnchorStyles.Left
                                 flowPanel.Controls.Add(labelControl)
                                 flowPanel.Controls.Add(Control)
                             End If
                             If ID <> "" Then ElementControls.Add(ID, Control)
                         End Sub
        ExtraOptionsTable.RowStyles.Clear()
        ElementControls = New Dictionary(Of String, Control)
        ' The "soft" attribute is NYI because no default ADMX file uses it
        If CurrentSetting.RawPolicy.Elements IsNot Nothing And CurrentSetting.Presentation IsNot Nothing Then
            Dim elemDict = CurrentSetting.RawPolicy.Elements.ToDictionary(Function(e) e.ID)
            For Each pres In CurrentSetting.Presentation.Elements
                Select Case pres.ElementType
                    Case "text" ' A plain label
                        Dim textPres As LabelPresentationElement = pres
                        Dim label As New Label With {.Text = textPres.Text, .AutoSize = True}
                        label.Margin = New Padding(3, 6, 3, 0)
                        addControl(textPres.ID, label, "")
                    Case "decimalTextBox"
                        Dim decimalTextPres As NumericBoxPresentationElement = pres
                        Dim numeric As DecimalPolicyElement = elemDict(pres.ID)
                        Dim newControl As Control
                        If decimalTextPres.HasSpinner Then
                            Dim nud As New NumericUpDown
                            nud.Minimum = numeric.Minimum
                            nud.Maximum = numeric.Maximum
                            nud.Increment = decimalTextPres.SpinnerIncrement
                            nud.Value = decimalTextPres.DefaultValue
                            newControl = nud
                        Else
                            Dim text As New TextBox
                            AddHandler text.TextChanged, Sub()
                                                             If Not Integer.TryParse(text.Text, 0) Then text.Text = decimalTextPres.DefaultValue
                                                             Dim curNum As Integer = text.Text
                                                             If curNum > numeric.Maximum Then text.Text = numeric.Maximum
                                                             If curNum < numeric.Minimum Then text.Text = numeric.Minimum
                                                         End Sub
                            AddHandler text.KeyPress, Sub(Sender As Object, EventArgs As KeyPressEventArgs)
                                                          If Not (Char.IsControl(EventArgs.KeyChar) Or Char.IsDigit(EventArgs.KeyChar)) Then EventArgs.Handled = True
                                                      End Sub
                            text.Text = decimalTextPres.DefaultValue
                            newControl = text
                        End If
                        addControl(pres.ID, newControl, decimalTextPres.Label)
                    Case "textBox"
                        Dim textboxPres As TextBoxPresentationElement = pres
                        Dim text As TextPolicyElement = elemDict(pres.ID)
                        Dim textbox As New TextBox
                        textbox.Width = ExtraOptionsTable.Width * 0.75
                        textbox.Text = textboxPres.DefaultValue
                        textbox.MaxLength = text.MaxLength
                        addControl(pres.ID, textbox, textboxPres.Label)
                    Case "checkBox"
                        Dim checkPres As CheckBoxPresentationElement = pres
                        Dim checkbox As New CheckBox With {.TextAlign = ContentAlignment.MiddleLeft}
                        checkbox.Text = checkPres.Text
                        checkbox.Width = ExtraOptionsTable.ClientSize.Width
                        Using g = checkbox.CreateGraphics ' Figure out how tall it should be
                            Dim size = g.MeasureString(checkbox.Text, checkbox.Font, checkbox.Width)
                            checkbox.Height = (size.Height + checkbox.Padding.Vertical + checkbox.Margin.Vertical)
                        End Using
                        checkbox.Checked = checkPres.DefaultState
                        addControl(pres.ID, checkbox, "")
                    Case "comboBox" ' Not tested because it's not used in any default ADML
                        Dim comboPres As ComboBoxPresentationElement = pres
                        Dim text As TextPolicyElement = elemDict(pres.ID)
                        Dim combobox As New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDown}
                        combobox.MaxLength = text.MaxLength
                        combobox.Width = ExtraOptionsTable.Width * 0.75
                        combobox.Text = comboPres.DefaultText
                        combobox.Sorted = Not comboPres.NoSort
                        For Each suggestion In comboPres.Suggestions
                            combobox.Items.Add(suggestion)
                        Next
                        addControl(pres.ID, combobox, comboPres.Label)
                    Case "dropdownList"
                        Dim dropdownPres As DropDownPresentationElement = pres
                        Dim combobox As New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList}
                        combobox.Sorted = Not dropdownPres.NoSort
                        Dim enumElem As EnumPolicyElement = elemDict(pres.ID)
                        Dim itemId As Integer = 0
                        Using g = combobox.CreateGraphics ' Figure out how wide it should be, and add entries
                            Dim maxWidth = combobox.Width
                            For Each entry In enumElem.Items
                                Dim map = New DropdownPresentationMap With {.ID = itemId, .DisplayName = AdmxWorkspace.ResolveString(entry.DisplayCode, CurrentSetting.RawPolicy.DefinedIn)}
                                Dim width = g.MeasureString(map.DisplayName, combobox.Font).Width * 1.2
                                If width > maxWidth Then maxWidth = width
                                combobox.Items.Add(map)
                                If itemId = dropdownPres.DefaultItemID.GetValueOrDefault(-1) Then combobox.SelectedItem = map
                                itemId += 1
                            Next
                            combobox.Width = maxWidth
                        End Using
                        addControl(pres.ID, combobox, dropdownPres.Label)
                    Case "listBox"
                        Dim listPres As ListPresentationElement = pres
                        Dim list As ListPolicyElement = elemDict(pres.ID)
                        Dim button As New Button
                        button.UseVisualStyleBackColor = True
                        button.Tag = list
                        button.Text = "Edit..."
                        AddHandler button.Click, Sub()
                                                     ' TODO: Show a grid view
                                                 End Sub
                        addControl(pres.ID, button, listPres.Label)
                End Select
            Next
        End If
    End Sub
    Sub PreparePolicyState()
        Select Case PolicyProcessing.GetPolicyState(CurrentSource, CurrentSetting)
            Case PolicyState.Disabled
                DisabledOption.Checked = True
            Case PolicyState.Enabled
                EnabledOption.Checked = True
                Dim optionStates = PolicyProcessing.GetPolicyOptionStates(CurrentSource, CurrentSetting)
                For Each kv In optionStates
                    Dim uiControl As Control = ElementControls(kv.Key)
                    If TypeOf kv.Value Is UInteger Then ' Numeric box
                        If TypeOf uiControl Is TextBox Then
                            CType(uiControl, TextBox).Text = kv.Value.ToString
                        Else
                            CType(uiControl, NumericUpDown).Value = kv.Value
                        End If
                    ElseIf TypeOf kv.Value Is String Then ' Text box or combo box
                        If TypeOf uiControl Is ComboBox Then
                            CType(uiControl, ComboBox).Text = kv.Value
                        Else
                            CType(uiControl, TextBox).Text = kv.Value
                        End If
                    ElseIf TypeOf kv.Value Is Integer Then ' Dropdown list
                        CType(uiControl, ComboBox).SelectedIndex = kv.Value
                    ElseIf TypeOf kv.Value Is Boolean Then ' Check box
                        CType(uiControl, CheckBox).Checked = kv.Value
                    Else ' List box (pop-out button)
                        uiControl.Tag = kv.Value
                    End If
                Next
            Case Else
                NotConfiguredOption.Checked = True
        End Select
    End Sub
    Private Sub StateRadiosChanged(sender As Object, e As EventArgs) Handles DisabledOption.CheckedChanged, EnabledOption.CheckedChanged, NotConfiguredOption.CheckedChanged
        Dim allowOptions = EnabledOption.Checked
        For Each kv In ElementControls
            kv.Value.Enabled = allowOptions
        Next
    End Sub
    Private Sub SectionDropdown_SelectedIndexChanged(sender As Object, e As EventArgs) Handles SectionDropdown.SelectedIndexChanged
        CurrentSource = IIf(SectionDropdown.Text = "User", UserPolSource, CompPolSource)
        PreparePolicyState()
    End Sub
    Private Class DropdownPresentationMap
        Public ID As Integer
        Public DisplayName As String
        Public Overrides Function ToString() As String
            Return DisplayName
        End Function
    End Class
End Class