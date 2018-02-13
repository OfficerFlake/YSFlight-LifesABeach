namespace LifesABeach
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Color1_Form = new System.Windows.Forms.ColorDialog();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.Color1_Button = new System.Windows.Forms.Button();
            this.Color2_Button = new System.Windows.Forms.Button();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.Color2_Display = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.Color1_Display = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.Color2_Form = new System.Windows.Forms.ColorDialog();
            this.TextBox_BeachSize = new System.Windows.Forms.TextBox();
            this.Label_BeachSize = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Color1_Form
            // 
            this.Color1_Form.Color = System.Drawing.Color.DimGray;
            // 
            // StatusLabel
            // 
            this.StatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusLabel.BackColor = System.Drawing.Color.Transparent;
            this.StatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusLabel.ForeColor = System.Drawing.Color.White;
            this.StatusLabel.Location = new System.Drawing.Point(0, 46);
            this.StatusLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(288, 187);
            this.StatusLabel.TabIndex = 1;
            this.StatusLabel.Text = "Drop Input FLD Here When Ready!";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Color1_Button
            // 
            this.Color1_Button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Color1_Button.BackColor = System.Drawing.Color.Transparent;
            this.Color1_Button.Location = new System.Drawing.Point(12, 12);
            this.Color1_Button.Name = "Color1_Button";
            this.Color1_Button.Size = new System.Drawing.Size(128, 23);
            this.Color1_Button.TabIndex = 2;
            this.Color1_Button.Text = "Color1";
            this.Color1_Button.UseVisualStyleBackColor = false;
            this.Color1_Button.Click += new System.EventHandler(this.Color1_Button_Click);
            // 
            // Color2_Button
            // 
            this.Color2_Button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Color2_Button.BackColor = System.Drawing.Color.Transparent;
            this.Color2_Button.Location = new System.Drawing.Point(145, 12);
            this.Color2_Button.Name = "Color2_Button";
            this.Color2_Button.Size = new System.Drawing.Size(127, 23);
            this.Color2_Button.TabIndex = 3;
            this.Color2_Button.Text = "Color2";
            this.Color2_Button.UseVisualStyleBackColor = false;
            this.Color2_Button.Click += new System.EventHandler(this.Color2_Button_Click);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.Color2_Display,
            this.Color1_Display});
            this.shapeContainer1.Size = new System.Drawing.Size(284, 262);
            this.shapeContainer1.TabIndex = 4;
            this.shapeContainer1.TabStop = false;
            // 
            // Color2_Display
            // 
            this.Color2_Display.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Color2_Display.BackColor = System.Drawing.Color.Gray;
            this.Color2_Display.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Opaque;
            this.Color2_Display.BorderColor = System.Drawing.Color.Transparent;
            this.Color2_Display.FillColor = System.Drawing.Color.DimGray;
            this.Color2_Display.Location = new System.Drawing.Point(142, 0);
            this.Color2_Display.Name = "Color2_Display";
            this.Color2_Display.Size = new System.Drawing.Size(142, 46);
            // 
            // Color1_Display
            // 
            this.Color1_Display.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Color1_Display.BackColor = System.Drawing.Color.DimGray;
            this.Color1_Display.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Opaque;
            this.Color1_Display.BorderColor = System.Drawing.Color.Transparent;
            this.Color1_Display.FillColor = System.Drawing.Color.DimGray;
            this.Color1_Display.Location = new System.Drawing.Point(0, 0);
            this.Color1_Display.Name = "Color1_Display";
            this.Color1_Display.Size = new System.Drawing.Size(142, 46);
            // 
            // Color2_Form
            // 
            this.Color2_Form.Color = System.Drawing.Color.Gray;
            // 
            // TextBox_BeachSize
            // 
            this.TextBox_BeachSize.BackColor = System.Drawing.Color.Black;
            this.TextBox_BeachSize.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox_BeachSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox_BeachSize.ForeColor = System.Drawing.Color.White;
            this.TextBox_BeachSize.Location = new System.Drawing.Point(97, 246);
            this.TextBox_BeachSize.Name = "TextBox_BeachSize";
            this.TextBox_BeachSize.Size = new System.Drawing.Size(184, 13);
            this.TextBox_BeachSize.TabIndex = 5;
            this.TextBox_BeachSize.Text = "50";
            this.TextBox_BeachSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Label_BeachSize
            // 
            this.Label_BeachSize.AutoSize = true;
            this.Label_BeachSize.ForeColor = System.Drawing.Color.White;
            this.Label_BeachSize.Location = new System.Drawing.Point(3, 245);
            this.Label_BeachSize.Name = "Label_BeachSize";
            this.Label_BeachSize.Size = new System.Drawing.Size(88, 13);
            this.Label_BeachSize.TabIndex = 6;
            this.Label_BeachSize.Text = "Shore Length (m)";
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.Label_BeachSize);
            this.Controls.Add(this.TextBox_BeachSize);
            this.Controls.Add(this.Color2_Button);
            this.Controls.Add(this.Color1_Button);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.shapeContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "LifesABeach - © OfficerFlake 2015";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog Color1_Form;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Button Color1_Button;
        private System.Windows.Forms.Button Color2_Button;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape Color1_Display;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape Color2_Display;
        private System.Windows.Forms.ColorDialog Color2_Form;
        private System.Windows.Forms.TextBox TextBox_BeachSize;
        private System.Windows.Forms.Label Label_BeachSize;


    }
}

