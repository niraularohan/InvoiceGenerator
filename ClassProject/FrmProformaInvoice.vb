Imports System.Drawing.Printing

Public Class FrmProformaInvoice
    Inherits Form

    ' === Controls ===
    Private PanelHeader As Panel
    Private contentPanel As Panel
    Private lblCompanyName, lblCompanyDetails, lblInvoiceTitle As Label
    Private cmbInvoiceType As ComboBox
    Private dgvInvoiceItems As DataGridView
    Private txtTotalCost As TextBox
    Private lblTotalCost As Label
    Private btnPrint As Button
    Private lblNote, lblThanks As Label
    Private lblBilledTo, lblAddress, lblInvoiceDate, lblInvoiceSerial As Label
    Private txtBilledTo, txtAddress, txtInvoiceSerial As TextBox
    Private dtpInvoiceDate As DateTimePicker
    Private PrintDocument1 As PrintDocument
    Private PrintPreviewDialog1 As PrintPreviewDialog

    ' New product entry controls
    Private grpProductEntry As GroupBox
    Private lblItemNo As Label
    Private txtItemNo As TextBox
    Private lblDescription As Label
    Private txtDescription As TextBox
    Private lblQty As Label
    Private txtQty As TextBox
    Private lblUnitPrice As Label
    Private txtUnitPrice As TextBox
    Private btnAddItem As Button

    ' New footer controls
    Private btnRemoveLine As Button
    Private btnResetAll As Button

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        ' === Form ===
        Me.Text = "Proforma Invoice"
        Me.ClientSize = New Size(1000, 750)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.BackColor = Color.White

        ' === Header Panel (docked) ===
        PanelHeader = New Panel With {
            .Dock = DockStyle.Top,
            .Height = 120,
            .BackColor = Color.White
        }

        lblCompanyName = New Label With {
            .Text = "MWASID GRANITES",
            .Font = New Font("Segoe UI", 16, FontStyle.Bold),
            .Location = New Point(20, 10),
            .AutoSize = True
        }

        lblCompanyDetails = New Label With {
            .Text = "Nairobi, 00208, Kenya" & vbCrLf &
                    "Contacts: 0790109743  |  Email: mwasidgranite@gmail.com",
            .Font = New Font("Segoe UI", 10),
            .Location = New Point(20, 45),
            .AutoSize = True
        }

        lblInvoiceTitle = New Label With {
            .Text = "PROFORMA INVOICE",
            .Font = New Font("Segoe UI", 14, FontStyle.Bold Or FontStyle.Underline),
            .Location = New Point(720, 40),
            .AutoSize = True
        }

        ' ComboBox to allow editing/selecting invoice type; placed in header
        cmbInvoiceType = New ComboBox With {
            .Location = New Point(520, 40),
            .Width = 180,
            .DropDownStyle = ComboBoxStyle.DropDown ' allow typing custom value
        }
        cmbInvoiceType.Items.AddRange(New Object() {"PROFORMA INVOICE", "TAX INVOICE", "QUOTATION", "CREDIT NOTE", "DEBIT NOTE"})
        cmbInvoiceType.Text = lblInvoiceTitle.Text
        AddHandler cmbInvoiceType.TextChanged, AddressOf cmbInvoiceType_TextChanged

        PanelHeader.Controls.AddRange({lblCompanyName, lblCompanyDetails, lblInvoiceTitle})
        PanelHeader.Controls.Add(cmbInvoiceType)

        ' === Content panel with AutoScroll to allow overlapping elements ===
        contentPanel = New Panel With {
            .Dock = DockStyle.Fill,
            .AutoScroll = True,
            .BackColor = Color.Transparent
        }

        ' === Product Entry Group (will be placed just above the grid) ===
        grpProductEntry = New GroupBox With {
            .Location = New Point(20, 200), ' moved to sit immediately above the DataGridView
            .Size = New Size(960, 50),
            .FlatStyle = FlatStyle.Flat
        }

        lblItemNo = New Label With {.Text = "Item #", .Location = New Point(10, 20), .AutoSize = True}
        txtItemNo = New TextBox With {.Location = New Point(60, 16), .Width = 60}

        lblDescription = New Label With {.Text = "Description", .Location = New Point(130, 20), .AutoSize = True}
        txtDescription = New TextBox With {.Location = New Point(210, 16), .Width = 360}

        lblQty = New Label With {.Text = "Qty", .Location = New Point(580, 20), .AutoSize = True}
        txtQty = New TextBox With {.Location = New Point(610, 16), .Width = 60}

        lblUnitPrice = New Label With {.Text = "Unit Price", .Location = New Point(690, 20), .AutoSize = True}
        txtUnitPrice = New TextBox With {.Location = New Point(760, 16), .Width = 90}

        btnAddItem = New Button With {
            .Text = "Add Item",
            .Location = New Point(860, 13),
            .Size = New Size(80, 25),
            .BackColor = Color.FromArgb(52, 73, 94),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        AddHandler btnAddItem.Click, AddressOf btnAddItem_Click

        grpProductEntry.Controls.AddRange({lblItemNo, txtItemNo, lblDescription, txtDescription, lblQty, txtQty, lblUnitPrice, txtUnitPrice, btnAddItem})

        ' === Client Info (moved to top, above the product entry group) ===
        lblBilledTo = New Label With {
            .Text = "Bill To:",
            .Location = New Point(20, 130),
            .Font = New Font("Segoe UI", 10),
            .AutoSize = True,
            .TextAlign = ContentAlignment.MiddleRight
        }
        txtBilledTo = New TextBox With {
            .Location = New Point(100, 128),
            .Size = New Size(360, 24),
            .Multiline = False,
            .BorderStyle = BorderStyle.FixedSingle,
            .Font = New Font("Segoe UI", 9),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Left
        }

        lblInvoiceDate = New Label With {.Text = "Invoice Date:", .Location = New Point(480, 130), .Font = New Font("Segoe UI", 10)}
        dtpInvoiceDate = New DateTimePicker With {.Location = New Point(600, 128), .Width = 150}

        lblAddress = New Label With {
            .Text = "Address:",
            .Location = New Point(20, 160),
            .Font = New Font("Segoe UI", 10),
            .AutoSize = True,
            .TextAlign = ContentAlignment.MiddleRight
        }
        txtAddress = New TextBox With {
            .Location = New Point(100, 158),
            .Size = New Size(360, 24),
            .Multiline = False,
            .BorderStyle = BorderStyle.FixedSingle,
            .Font = New Font("Segoe UI", 9),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Left
        }

        lblInvoiceSerial = New Label With {.Text = "Invoice Serial:", .Location = New Point(480, 160), .Font = New Font("Segoe UI", 10)}
        txtInvoiceSerial = New TextBox With {.Location = New Point(600, 158), .Width = 150}

        ' === DataGridView (Invoice Table) ===
        dgvInvoiceItems = New DataGridView With {
            .Location = New Point(20, 260), ' placed below the product entry group
            .Size = New Size(960, 260),
            .BackgroundColor = Color.White,
            .AllowUserToAddRows = False,
            .RowHeadersVisible = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
        }

        dgvInvoiceItems.Columns.Add("ItemNo", "ITEM NO.")
        dgvInvoiceItems.Columns.Add("Description", "DESCRIPTION")
        dgvInvoiceItems.Columns.Add("Qty", "QTY")
        dgvInvoiceItems.Columns.Add("UnitPrice", "UNIT PRICE (KSH)")
        dgvInvoiceItems.Columns.Add("Amount", "T. AMOUNT (KSH)")

        ' === Total Section ===
        lblTotalCost = New Label With {
            .Text = "Total Cost (KES):",
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .Location = New Point(650, 520),
            .AutoSize = True
        }

        txtTotalCost = New TextBox With {
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .Location = New Point(790, 516),
            .Width = 180,
            .TextAlign = HorizontalAlignment.Right,
            .Text = "0.00",
            .ReadOnly = True
        }

        ' === Note and Thank You ===
        lblNote = New Label With {
            .Text = "Note: All the Logistics and Transport Cost are included in the total cost.",
            .Font = New Font("Segoe UI", 9, FontStyle.Regular),
            .Location = New Point(20, 560),
            .AutoSize = True
        }

        lblThanks = New Label With {
            .Text = "Thank you for your valuable inquiry.",
            .Font = New Font("Segoe UI", 9, FontStyle.Regular),
            .Location = New Point(20, 580),
            .AutoSize = True
        }

        ' === Footer Buttons: Remove Line, Reset All, Print ===
        btnRemoveLine = New Button With {
            .Text = "Remove Line",
            .Font = New Font("Segoe UI", 9, FontStyle.Regular),
            .Location = New Point(20, 620),
            .Size = New Size(100, 28),
            .BackColor = Color.FromArgb(52, 73, 94),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        AddHandler btnRemoveLine.Click, AddressOf btnRemoveLine_Click

        btnResetAll = New Button With {
            .Text = "Reset All",
            .Font = New Font("Segoe UI", 9, FontStyle.Regular),
            .Location = New Point(130, 620),
            .Size = New Size(100, 28),
            .BackColor = Color.FromArgb(52, 73, 94),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        AddHandler btnResetAll.Click, AddressOf btnResetAll_Click

        btnPrint = New Button With {
            .Text = "Print",
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .Location = New Point(880, 620),
            .Size = New Size(80, 30),
            .BackColor = Color.FromArgb(52, 73, 94),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        AddHandler btnPrint.Click, AddressOf btnPrint_Click

        ' === Print Setup ===
        PrintDocument1 = New PrintDocument()
        PrintPreviewDialog1 = New PrintPreviewDialog() With {.Document = PrintDocument1, .Width = 800, .Height = 600}
        AddHandler PrintDocument1.PrintPage, AddressOf PrintDocument1_PrintPage

        ' === Add Controls to contentPanel ===
        contentPanel.Controls.AddRange({lblBilledTo, txtBilledTo, lblAddress, txtAddress, lblInvoiceDate, dtpInvoiceDate, lblInvoiceSerial, txtInvoiceSerial, grpProductEntry, dgvInvoiceItems, lblTotalCost, txtTotalCost, lblNote, lblThanks, btnRemoveLine, btnResetAll, btnPrint})

        ' === Add header and contentPanel to Form ===
        Me.Controls.AddRange({PanelHeader, contentPanel})

        ' === Calculation Events ===
        AddHandler dgvInvoiceItems.CellValueChanged, AddressOf dgvInvoiceItems_CellValueChanged
        AddHandler dgvInvoiceItems.RowsAdded, AddressOf dgvInvoiceItems_RowsAdded
        AddHandler dgvInvoiceItems.UserDeletedRow, AddressOf dgvInvoiceItems_UserDeletedRow
    End Sub

    ' === Auto Calculation ===
    Private Sub RecalculateTotal()
        Dim total As Decimal = 0
        For Each row As DataGridViewRow In dgvInvoiceItems.Rows
            If Not row.IsNewRow Then
                Dim qty, price As Decimal
                Decimal.TryParse(Convert.ToString(row.Cells("Qty").Value), qty)
                Decimal.TryParse(Convert.ToString(row.Cells("UnitPrice").Value), price)
                Dim amount As Decimal = qty * price
                row.Cells("Amount").Value = amount.ToString("N2")
                total += amount
            End If
        Next
        txtTotalCost.Text = total.ToString("N2")
    End Sub

    Private Sub dgvInvoiceItems_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then RecalculateTotal()
    End Sub

    Private Sub dgvInvoiceItems_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs)
        RecalculateTotal()
    End Sub

    Private Sub dgvInvoiceItems_UserDeletedRow(sender As Object, e As DataGridViewRowEventArgs)
        RecalculateTotal()
    End Sub

    ' === Add Item button handler ===
    Private Sub btnAddItem_Click(sender As Object, e As EventArgs)
        ' Try to parse values
        Dim itemNoText = txtItemNo.Text.Trim()
        Dim description = txtDescription.Text.Trim()
        Dim qty As Decimal = 0D
        Dim unitPrice As Decimal = 0D
        Decimal.TryParse(txtQty.Text.Trim(), qty)
        Decimal.TryParse(txtUnitPrice.Text.Trim(), unitPrice)

        If String.IsNullOrWhiteSpace(description) Then
            MessageBox.Show("Please enter a description.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If qty <= 0 OrElse unitPrice < 0 Then
            MessageBox.Show("Please enter valid quantity and unit price.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim amount = qty * unitPrice

        Dim itemNo As String = itemNoText
        If String.IsNullOrWhiteSpace(itemNo) Then
            itemNo = (dgvInvoiceItems.Rows.Count + 1).ToString()
        End If

        dgvInvoiceItems.Rows.Add(itemNo, description, qty.ToString(), unitPrice.ToString("N2"), amount.ToString("N2"))
        RecalculateTotal()

        ' Clear inputs
        txtItemNo.Clear()
        txtDescription.Clear()
        txtQty.Clear()
        txtUnitPrice.Clear()
        txtDescription.Focus()
    End Sub

    ' === Remove Line ===
    Private Sub btnRemoveLine_Click(sender As Object, e As EventArgs)
        If dgvInvoiceItems.SelectedRows.Count > 0 Then
            For Each r As DataGridViewRow In dgvInvoiceItems.SelectedRows
                If Not r.IsNewRow Then dgvInvoiceItems.Rows.Remove(r)
            Next
            RecalculateTotal()
        Else
            MessageBox.Show("Select a row to remove.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ' === Reset All ===
    Private Sub btnResetAll_Click(sender As Object, e As EventArgs)
        If MessageBox.Show("Clear all invoice items?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            dgvInvoiceItems.Rows.Clear()
            txtTotalCost.Text = "0.00"
        End If
    End Sub

    ' === Print Logic ===
    Private Sub btnPrint_Click(sender As Object, e As EventArgs)
        PrintPreviewDialog1.ShowDialog()
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs)
        Dim g As Graphics = e.Graphics
        Dim fontHeader As New Font("Segoe UI", 12, FontStyle.Bold)
        Dim fontNormal As New Font("Segoe UI", 10)
        Dim y As Integer = 50

        ' Header
        g.DrawString(lblCompanyName.Text, fontHeader, Brushes.Black, 50, y) : y += 25
        g.DrawString(lblCompanyDetails.Text, fontNormal, Brushes.Black, 50, y) : y += 40
        ' print the dynamic invoice title from lblInvoiceTitle
        g.DrawString(lblInvoiceTitle.Text, fontHeader, Brushes.Black, 600, 50)
        y += 10

        ' Client Info (moved to top)
        g.DrawString("Billed To: " & txtBilledTo.Text, fontNormal, Brushes.Black, 50, y)
        g.DrawString("Invoice Date: " & dtpInvoiceDate.Value.ToShortDateString(), fontNormal, Brushes.Black, 600, y)
        y += 20
        g.DrawString("Address: " & txtAddress.Text, fontNormal, Brushes.Black, 50, y)
        g.DrawString("Invoice Serial: " & txtInvoiceSerial.Text, fontNormal, Brushes.Black, 600, y)
        y += 30

        ' Table Header
        g.DrawString("ITEM NO.", fontNormal, Brushes.Black, 50, y)
        g.DrawString("DESCRIPTION", fontNormal, Brushes.Black, 120, y)
        g.DrawString("QTY", fontNormal, Brushes.Black, 400, y)
        g.DrawString("UNIT PRICE", fontNormal, Brushes.Black, 480, y)
        g.DrawString("AMOUNT", fontNormal, Brushes.Black, 600, y)
        y += 20

        ' Items
        For Each row As DataGridViewRow In dgvInvoiceItems.Rows
            If Not row.IsNewRow Then
                g.DrawString(Convert.ToString(row.Cells("ItemNo").Value), fontNormal, Brushes.Black, 50, y)
                g.DrawString(Convert.ToString(row.Cells("Description").Value), fontNormal, Brushes.Black, 120, y)
                g.DrawString(Convert.ToString(row.Cells("Qty").Value), fontNormal, Brushes.Black, 400, y)
                g.DrawString(Convert.ToString(row.Cells("UnitPrice").Value), fontNormal, Brushes.Black, 480, y)
                g.DrawString(Convert.ToString(row.Cells("Amount").Value), fontNormal, Brushes.Black, 600, y)
                y += 20
            End If
        Next

        y += 30
        g.DrawString("Total Cost (KES): " & txtTotalCost.Text, fontHeader, Brushes.Black, 480, y)
        y += 40
        g.DrawString(lblNote.Text, fontNormal, Brushes.Black, 50, y) : y += 20
        g.DrawString(lblThanks.Text, fontNormal, Brushes.Black, 50, y)
    End Sub

    ' Sync ComboBox text into the header invoice title label
    Private Sub cmbInvoiceType_TextChanged(sender As Object, e As EventArgs)
        If cmbInvoiceType IsNot Nothing AndAlso lblInvoiceTitle IsNot Nothing Then
            lblInvoiceTitle.Text = cmbInvoiceType.Text
        End If
    End Sub
End Class
