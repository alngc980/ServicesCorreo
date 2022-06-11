<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class InformeVentas
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InformeVentas))
        Me.dgvdia = New System.Windows.Forms.DataGridView()
        Me.lblmensaje = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        CType(Me.dgvdia, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvdia
        '
        Me.dgvdia.AllowUserToAddRows = False
        Me.dgvdia.AllowUserToDeleteRows = False
        Me.dgvdia.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvdia.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvdia.Location = New System.Drawing.Point(12, 34)
        Me.dgvdia.Name = "dgvdia"
        Me.dgvdia.ReadOnly = True
        Me.dgvdia.RowHeadersVisible = False
        Me.dgvdia.RowHeadersWidth = 70
        Me.dgvdia.RowTemplate.Height = 40
        Me.dgvdia.Size = New System.Drawing.Size(251, 265)
        Me.dgvdia.TabIndex = 0
        '
        'lblmensaje
        '
        Me.lblmensaje.AutoSize = True
        Me.lblmensaje.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.lblmensaje.Font = New System.Drawing.Font("Arial Narrow", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblmensaje.ForeColor = System.Drawing.Color.White
        Me.lblmensaje.Location = New System.Drawing.Point(12, 9)
        Me.lblmensaje.Name = "lblmensaje"
        Me.lblmensaje.Size = New System.Drawing.Size(95, 20)
        Me.lblmensaje.TabIndex = 1
        Me.lblmensaje.Text = "Conectando..."
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'InformeVentas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.ClientSize = New System.Drawing.Size(275, 310)
        Me.Controls.Add(Me.lblmensaje)
        Me.Controls.Add(Me.dgvdia)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(291, 349)
        Me.MinimumSize = New System.Drawing.Size(291, 349)
        Me.Name = "InformeVentas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "INFORME VENTAS"
        CType(Me.dgvdia, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvdia As DataGridView
    Friend WithEvents lblmensaje As Label
    Friend WithEvents Timer1 As Timer
End Class
