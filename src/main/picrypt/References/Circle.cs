// Create a new form
Form myForm = new Form();

// Create a group box to contain the radio buttons
GroupBox groupBox = new GroupBox();
groupBox.Text = "Select an option";
groupBox.Location = new Point(10, 10);
groupBox.Size = new Size(200, 100);

// Create the radio buttons and add them to the group box
RadioButton textRadioButton = new RadioButton();
textRadioButton.Text = "Text";
textRadioButton.Location = new Point(10, 20);
textRadioButton.Checked = true;
groupBox.Controls.Add(textRadioButton);

RadioButton imageRadioButton = new RadioButton();
imageRadioButton.Text = "Image";
imageRadioButton.Location = new Point(10, 50);
groupBox.Controls.Add(imageRadioButton);

// Add the group box to the form
myForm.Controls.Add(groupBox);

// Show the form
myForm.ShowDialog();

