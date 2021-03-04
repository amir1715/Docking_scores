<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class options
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.fitnessBX = New System.Windows.Forms.TextBox
        Me.identifierBX = New System.Windows.Forms.TextBox
        Me.HbondBx = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'fitnessBX
        '
        Me.fitnessBX.Location = New System.Drawing.Point(115, 109)
        Me.fitnessBX.Name = "fitnessBX"
        Me.fitnessBX.Size = New System.Drawing.Size(154, 20)
        Me.fitnessBX.TabIndex = 0
        Me.fitnessBX.Text = "GoldscoreFitness"
        '
        'identifierBX
        '
        Me.identifierBX.Location = New System.Drawing.Point(115, 135)
        Me.identifierBX.Name = "identifierBX"
        Me.identifierBX.Size = New System.Drawing.Size(154, 20)
        Me.identifierBX.TabIndex = 1
        Me.identifierBX.Text = "Identifier"
        '
        'HbondBx
        '
        Me.HbondBx.Location = New System.Drawing.Point(115, 161)
        Me.HbondBx.Name = "HbondBx"
        Me.HbondBx.Size = New System.Drawing.Size(154, 20)
        Me.HbondBx.TabIndex = 3
        Me.HbondBx.Text = "GoldscoreExternalHBond"
        '
        'options
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.Controls.Add(Me.HbondBx)
        Me.Controls.Add(Me.identifierBX)
        Me.Controls.Add(Me.fitnessBX)
        Me.Name = "options"
        Me.Text = "options"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents fitnessBX As System.Windows.Forms.TextBox
    Friend WithEvents identifierBX As System.Windows.Forms.TextBox
    Friend WithEvents HbondBx As System.Windows.Forms.TextBox
End Class
