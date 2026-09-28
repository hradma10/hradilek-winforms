namespace XMLDataViewer;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        tableLayoutPanel = new TableLayoutPanel();
        panelTop = new Panel();
        cancelButton = new Button();
        loadButton = new Button();
        salesView = new DataGridView();
        tableLayoutPanel.SuspendLayout();
        panelTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)salesView).BeginInit();
        SuspendLayout();
        // 
        // tableLayoutPanel
        // 
        tableLayoutPanel.ColumnCount = 1;
        tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel.Controls.Add(panelTop, 0, 0);
        tableLayoutPanel.Controls.Add(salesView, 0, 1);
        tableLayoutPanel.Dock = DockStyle.Fill;
        tableLayoutPanel.Location = new Point(0, 0);
        tableLayoutPanel.Name = "tableLayoutPanel";
        tableLayoutPanel.RowCount = 2;
        tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel.Size = new Size(800, 450);
        tableLayoutPanel.TabIndex = 0;
        salesView.CellFormatting += salesView_CellFormatting;
        salesView.Paint += SalesView_Paint;
        // 
        // panelTop
        // 
        panelTop.Controls.Add(cancelButton);
        panelTop.Controls.Add(loadButton);
        panelTop.Dock = DockStyle.Fill;
        panelTop.Location = new Point(3, 3);
        panelTop.Name = "panelTop";
        panelTop.Size = new Size(794, 44);
        panelTop.TabIndex = 0;
        // 
        // cancelButton
        // 
        cancelButton.Enabled = false;
        cancelButton.Location = new Point(120, 9);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(100, 30);
        cancelButton.TabIndex = 1;
        cancelButton.Text = "Zrušit";
        cancelButton.UseVisualStyleBackColor = true;
        cancelButton.Click += cancelButton_Click;
        // 
        // loadButton
        // 
        loadButton.Location = new Point(9, 9);
        loadButton.Name = "loadButton";
        loadButton.Size = new Size(100, 30);
        loadButton.TabIndex = 0;
        loadButton.Text = "Načíst";
        loadButton.UseVisualStyleBackColor = true;
        loadButton.Click += loadButton_Click;
        // 
        // salesView
        // 
        salesView.AllowUserToAddRows = false;
        salesView.AllowUserToDeleteRows = false;
        salesView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        salesView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        salesView.Dock = DockStyle.Fill;
        salesView.Location = new Point(3, 53);
        salesView.MultiSelect = false;
        salesView.Name = "salesView";
        salesView.ReadOnly = true;
        salesView.RowHeadersVisible = false;
        salesView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        salesView.Size = new Size(794, 394);
        salesView.TabIndex = 1;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(tableLayoutPanel);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "XML Data Viewer";
        tableLayoutPanel.ResumeLayout(false);
        panelTop.ResumeLayout(false);
        panelTop.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)salesView).EndInit();
        ResumeLayout(false);
    }

    private TableLayoutPanel tableLayoutPanel;
    private Panel panelTop;
    private Button loadButton;
    private Button cancelButton;
    private DataGridView salesView;
}