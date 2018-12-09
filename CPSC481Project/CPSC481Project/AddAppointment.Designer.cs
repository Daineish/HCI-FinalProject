namespace CPSC481Project
{
    partial class AddAppointment
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
            this.button1 = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.nameLabel = new System.Windows.Forms.Label();
            this.hcLabel = new System.Windows.Forms.Label();
            this.doctorLabel = new System.Windows.Forms.Label();
            this.startTimeLabel = new System.Windows.Forms.Label();
            this.doctorComboBox = new System.Windows.Forms.ComboBox();
            this.startTimePicker = new System.Windows.Forms.DateTimePicker();
            this.endTimeLabel = new System.Windows.Forms.Label();
            this.endTimePicker = new System.Windows.Forms.DateTimePicker();
            this.patientName = new System.Windows.Forms.Label();
            this.patientHCNum = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.AccessibleName = "confirmButton";
            this.button1.Location = new System.Drawing.Point(348, 293);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(65, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Confirm";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.confirmButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.AccessibleDescription = "cancelButton";
            this.cancelButton.Location = new System.Drawing.Point(254, 293);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(65, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // nameLabel
            // 
            this.nameLabel.AccessibleDescription = "nameLabel";
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(14, 31);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(74, 13);
            this.nameLabel.TabIndex = 3;
            this.nameLabel.Text = "Patient Name:";
            // 
            // hcLabel
            // 
            this.hcLabel.AccessibleDescription = "hcLabel";
            this.hcLabel.AutoSize = true;
            this.hcLabel.Location = new System.Drawing.Point(14, 69);
            this.hcLabel.Name = "hcLabel";
            this.hcLabel.Size = new System.Drawing.Size(68, 13);
            this.hcLabel.TabIndex = 4;
            this.hcLabel.Text = "Patient HC#:";
            // 
            // doctorLabel
            // 
            this.doctorLabel.AccessibleDescription = "doctorLabel";
            this.doctorLabel.AutoSize = true;
            this.doctorLabel.Location = new System.Drawing.Point(14, 106);
            this.doctorLabel.Name = "doctorLabel";
            this.doctorLabel.Size = new System.Drawing.Size(42, 13);
            this.doctorLabel.TabIndex = 5;
            this.doctorLabel.Text = "Doctor:";
            // 
            // startTimeLabel
            // 
            this.startTimeLabel.AccessibleDescription = "startTimeLabel";
            this.startTimeLabel.AutoSize = true;
            this.startTimeLabel.Location = new System.Drawing.Point(14, 151);
            this.startTimeLabel.Name = "startTimeLabel";
            this.startTimeLabel.Size = new System.Drawing.Size(58, 13);
            this.startTimeLabel.TabIndex = 6;
            this.startTimeLabel.Text = "Start Time:";
            // 
            // doctorComboBox
            // 
            this.doctorComboBox.AccessibleDescription = "doctorComboBox";
            this.doctorComboBox.FormattingEnabled = true;
            this.doctorComboBox.Items.AddRange(new object[] {
            "Dr. Walter",
            "Dr. Lee",
            "Dr. Payne"});
            this.doctorComboBox.Location = new System.Drawing.Point(108, 106);
            this.doctorComboBox.Name = "doctorComboBox";
            this.doctorComboBox.Size = new System.Drawing.Size(121, 21);
            this.doctorComboBox.TabIndex = 7;
            // 
            // startTimePicker
            // 
            this.startTimePicker.AccessibleDescription = "startTimePicker";
            this.startTimePicker.Location = new System.Drawing.Point(108, 151);
            this.startTimePicker.Name = "startTimePicker";
            this.startTimePicker.Size = new System.Drawing.Size(200, 20);
            this.startTimePicker.TabIndex = 8;
            this.startTimePicker.ValueChanged += new System.EventHandler(this.startTimePicker_ValueChanged);
            // 
            // endTimeLabel
            // 
            this.endTimeLabel.AccessibleDescription = "endTimeLabel";
            this.endTimeLabel.AutoSize = true;
            this.endTimeLabel.Location = new System.Drawing.Point(14, 199);
            this.endTimeLabel.Name = "endTimeLabel";
            this.endTimeLabel.Size = new System.Drawing.Size(55, 13);
            this.endTimeLabel.TabIndex = 9;
            this.endTimeLabel.Text = "End Time:";
            // 
            // endTimePicker
            // 
            this.endTimePicker.AccessibleDescription = "endTimePicker";
            this.endTimePicker.Enabled = false;
            this.endTimePicker.Location = new System.Drawing.Point(108, 199);
            this.endTimePicker.Name = "endTimePicker";
            this.endTimePicker.Size = new System.Drawing.Size(200, 20);
            this.endTimePicker.TabIndex = 10;
            // 
            // patientName
            // 
            this.patientName.AccessibleDescription = "patientName";
            this.patientName.AutoSize = true;
            this.patientName.Location = new System.Drawing.Point(105, 31);
            this.patientName.Name = "patientName";
            this.patientName.Size = new System.Drawing.Size(62, 13);
            this.patientName.TabIndex = 11;
            this.patientName.Text = "placeholder";
            // 
            // patientHCNum
            // 
            this.patientHCNum.AccessibleDescription = "patientHCNum";
            this.patientHCNum.AutoSize = true;
            this.patientHCNum.Location = new System.Drawing.Point(105, 69);
            this.patientHCNum.Name = "patientHCNum";
            this.patientHCNum.Size = new System.Drawing.Size(62, 13);
            this.patientHCNum.TabIndex = 12;
            this.patientHCNum.Text = "placeholder";
            // 
            // AddAppointment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 328);
            this.Controls.Add(this.patientHCNum);
            this.Controls.Add(this.patientName);
            this.Controls.Add(this.endTimePicker);
            this.Controls.Add(this.endTimeLabel);
            this.Controls.Add(this.startTimePicker);
            this.Controls.Add(this.doctorComboBox);
            this.Controls.Add(this.startTimeLabel);
            this.Controls.Add(this.doctorLabel);
            this.Controls.Add(this.hcLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.button1);
            this.Name = "AddAppointment";
            this.Text = "Add Appointment";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label hcLabel;
        private System.Windows.Forms.Label doctorLabel;
        private System.Windows.Forms.Label startTimeLabel;
        private System.Windows.Forms.ComboBox doctorComboBox;
        private System.Windows.Forms.DateTimePicker startTimePicker;
        private System.Windows.Forms.Label endTimeLabel;
        private System.Windows.Forms.DateTimePicker endTimePicker;
        private System.Windows.Forms.Label patientName;
        private System.Windows.Forms.Label patientHCNum;
    }
}