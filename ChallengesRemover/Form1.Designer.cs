namespace ChallengesRemover
{
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            removeChallengesButton = new Button();
            GreetingLabel = new Label();
            SuspendLayout();
            // 
            // removeChallengesButton
            // 
            removeChallengesButton.Location = new Point(12, 114);
            removeChallengesButton.Name = "removeChallengesButton";
            removeChallengesButton.Size = new Size(260, 70);
            removeChallengesButton.TabIndex = 0;
            removeChallengesButton.Text = "Remove Challenges";
            removeChallengesButton.UseVisualStyleBackColor = true;
            removeChallengesButton.Click += removeChallengesButton_Click;
            // 
            // GreetingLabel
            // 
            GreetingLabel.AutoSize = true;
            GreetingLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            GreetingLabel.Location = new Point(120, 9);
            GreetingLabel.Name = "GreetingLabel";
            GreetingLabel.Size = new Size(51, 21);
            GreetingLabel.TabIndex = 1;
            GreetingLabel.Text = "Hello";
            GreetingLabel.TextAlign = ContentAlignment.MiddleCenter;
            GreetingLabel.Click += GreetingLabel_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 461);
            Controls.Add(GreetingLabel);
            Controls.Add(removeChallengesButton);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_LoadAsync;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button removeChallengesButton;
        private Label GreetingLabel;
    }
}
