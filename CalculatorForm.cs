using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CalculatorApp
{
    /// <summary>
    /// Main calculator form that provides a simple calculator interface
    /// with basic arithmetic operations (addition, subtraction, multiplication, division)
    /// and history tracking functionality.
    /// </summary>
    public partial class CalculatorForm : Form
    {
        // Maximum number of history entries that can be stored in arrays
        private const int MaxHistoryEntries = 50;

        // UI Controls - Input fields
        private TextBox number1TextBox;  // Text box for entering the first number
        private TextBox number2TextBox;  // Text box for entering the second number
        
        // UI Controls - Operation buttons
        private Button addButton;        // Button to perform addition
        private Button subtractButton;   // Button to perform subtraction
        private Button multiplyButton;   // Button to perform multiplication
        private Button divideButton;     // Button to perform division
        
        // UI Controls - Utility buttons
        private Button clearButton;      // Button to clear all inputs and results
        private Button viewHistoryButton;// Button to open the history window
        private Button saveArraysButton; // Button to save array data to file
        
        // UI Controls - Display
        private Label resultLabel;       // Label that displays calculation results or error messages

        // Calculation variables - store the numbers and result for current operation
        private double firstNumber;      // First number entered by user
        private double secondNumber;     // Second number entered by user
        private double calculationResult;// Result of the current calculation
        
        // History tracking - List to store all calculation history with timestamps
        private List<string> calculationHistory;

        // Arrays for the arrays assignment requirements
        // Stores just the numeric result values (e.g., 8.0, 15.5)
        private double[] resultArray;
        // Stores the text representation of the calculation (e.g. "5 + 3 = 8")
        private string[] operationArray;
        // Keeps track of how many entries are currently stored in the arrays
        private int historyEntryCount;

        /// <summary>
        /// Constructor - Initializes the calculator form and sets up data structures
        /// </summary>
        public CalculatorForm()
        {
            // Initialize the list to store calculation history
            calculationHistory = new List<string>();
            // Initialize arrays with maximum capacity
            resultArray = new double[MaxHistoryEntries];
            operationArray = new string[MaxHistoryEntries];
            // Start with zero entries
            historyEntryCount = 0;
            // Set up the UI components
            InitializeComponent();
        }

        /// <summary>
        /// Initializes all UI components and sets up the form layout
        /// </summary>
        private void InitializeComponent()
        {
            // Configure the main form window properties
            this.Text = "Simple Calculator";
            this.Size = new Size(400, 400);
            this.StartPosition = FormStartPosition.CenterScreen;  // Center the window on screen
            this.BackColor = Color.LightGray;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;  // Prevent resizing
            this.MaximizeBox = false;  // Disable maximize button
            this.MinimizeBox = true;   // Allow minimize button

            // Create and configure the first number input text box
            number1TextBox = new TextBox();
            number1TextBox.Location = new Point(50, 30);
            number1TextBox.Size = new Size(150, 25);
            number1TextBox.Font = new Font("Arial", 12);
            number1TextBox.TextAlign = HorizontalAlignment.Center;  // Center-align text
            number1TextBox.PlaceholderText = "Enter first number";
            // Subscribe to text change event to enable/disable buttons dynamically
            number1TextBox.TextChanged += NumberTextBox_TextChanged;
            this.Controls.Add(number1TextBox);

            // Create and configure the second number input text box
            number2TextBox = new TextBox();
            number2TextBox.Location = new Point(200, 30);
            number2TextBox.Size = new Size(150, 25);
            number2TextBox.Font = new Font("Arial", 12);
            number2TextBox.TextAlign = HorizontalAlignment.Center;
            number2TextBox.PlaceholderText = "Enter second number";
            // Subscribe to text change event to enable/disable buttons dynamically
            number2TextBox.TextChanged += NumberTextBox_TextChanged;
            this.Controls.Add(number2TextBox);

            // Create and configure the addition button
            addButton = new Button();
            addButton.Text = "+";
            addButton.Location = new Point(50, 80);
            addButton.Size = new Size(70, 40);
            addButton.Font = new Font("Arial", 16, FontStyle.Bold);
            addButton.BackColor = Color.LightBlue;
            addButton.ForeColor = Color.DarkBlue;
            addButton.Click += AddButton_Click;  // Attach click event handler
            this.Controls.Add(addButton);

            // Create and configure the subtraction button
            subtractButton = new Button();
            subtractButton.Text = "−";
            subtractButton.Location = new Point(130, 80);
            subtractButton.Size = new Size(70, 40);
            subtractButton.Font = new Font("Arial", 16, FontStyle.Bold);
            subtractButton.BackColor = Color.LightGreen;
            subtractButton.ForeColor = Color.DarkGreen;
            subtractButton.Click += SubtractButton_Click;  // Attach click event handler
            this.Controls.Add(subtractButton);

            // Create and configure the multiplication button
            multiplyButton = new Button();
            multiplyButton.Text = "×";
            multiplyButton.Location = new Point(210, 80);
            multiplyButton.Size = new Size(70, 40);
            multiplyButton.Font = new Font("Arial", 16, FontStyle.Bold);
            multiplyButton.BackColor = Color.LightYellow;
            multiplyButton.ForeColor = Color.DarkOrange;
            multiplyButton.Click += MultiplyButton_Click;  // Attach click event handler
            this.Controls.Add(multiplyButton);

            // Create and configure the division button
            divideButton = new Button();
            divideButton.Text = "÷";
            divideButton.Location = new Point(290, 80);
            divideButton.Size = new Size(70, 40);
            divideButton.Font = new Font("Arial", 16, FontStyle.Bold);
            divideButton.BackColor = Color.LightCoral;
            divideButton.ForeColor = Color.DarkRed;
            divideButton.Click += DivideButton_Click;  // Attach click event handler
            this.Controls.Add(divideButton);

            // Create and configure the result display label
            resultLabel = new Label();
            resultLabel.Text = "Result will appear here";
            resultLabel.Location = new Point(50, 150);
            resultLabel.Size = new Size(300, 30);
            resultLabel.Font = new Font("Arial", 12, FontStyle.Bold);
            resultLabel.TextAlign = ContentAlignment.MiddleCenter;  // Center the text
            resultLabel.BackColor = Color.White;
            resultLabel.BorderStyle = BorderStyle.FixedSingle;  // Add border for visibility
            this.Controls.Add(resultLabel);

            // Create and configure the clear button
            clearButton = new Button();
            clearButton.Text = "Clear";
            clearButton.Location = new Point(50, 200);
            clearButton.Size = new Size(120, 35);
            clearButton.Font = new Font("Arial", 11, FontStyle.Bold);
            clearButton.BackColor = Color.LightYellow;
            clearButton.ForeColor = Color.DarkOrange;
            clearButton.Click += ClearButton_Click;  // Attach click event handler
            this.Controls.Add(clearButton);

            // Create and configure the view history button
            viewHistoryButton = new Button();
            viewHistoryButton.Text = "View History";
            viewHistoryButton.Location = new Point(230, 200);
            viewHistoryButton.Size = new Size(120, 35);
            viewHistoryButton.Font = new Font("Arial", 11, FontStyle.Bold);
            viewHistoryButton.BackColor = Color.LightGreen;
            viewHistoryButton.ForeColor = Color.DarkGreen;
            viewHistoryButton.Click += ViewHistoryButton_Click;  // Attach click event handler
            this.Controls.Add(viewHistoryButton);

            // Create and configure the save arrays button
            saveArraysButton = new Button();
            saveArraysButton.Text = "Save Array Data";
            saveArraysButton.Location = new Point(50, 245);
            saveArraysButton.Size = new Size(300, 35);
            saveArraysButton.Font = new Font("Arial", 11, FontStyle.Bold);
            saveArraysButton.BackColor = Color.LightBlue;
            saveArraysButton.ForeColor = Color.DarkBlue;
            saveArraysButton.Click += SaveArraysButton_Click;  // Attach click event handler
            this.Controls.Add(saveArraysButton);

            // Create and configure the title label at the top of the form
            Label titleLabel = new Label();
            titleLabel.Text = "Simple Calculator";
            titleLabel.Location = new Point(50, 5);
            titleLabel.Size = new Size(300, 20);
            titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(titleLabel);
        }

        /// <summary>
        /// Validates that both input text boxes contain valid numeric values
        /// </summary>
        /// <returns>True if both inputs are valid numbers, false otherwise</returns>
        private bool ValidateInputs()
        {
            // Try to parse the first number from the text box
            if (double.TryParse(number1TextBox.Text, out firstNumber))
            {
                // If first number is valid, try to parse the second number
                if (double.TryParse(number2TextBox.Text, out secondNumber))
                {
                    // Both numbers are valid
                    return true;
                }
                else
                {
                    // Second number is invalid - display error message
                    resultLabel.Text = "Error: Second number is invalid. Please enter a valid number.";
                    resultLabel.ForeColor = Color.Red;
                    return false;
                }
            }
            else
            {
                // First number is invalid - display error message
                resultLabel.Text = "Error: First number is invalid. Please enter a valid number.";
                resultLabel.ForeColor = Color.Red;
                return false;
            }
        }

        /// <summary>
        /// Performs addition operation on two numbers
        /// </summary>
        /// <param name="num1">First number</param>
        /// <param name="num2">Second number</param>
        /// <returns>Sum of the two numbers</returns>
        private double PerformAddition(double num1, double num2)
        {
            return num1 + num2;
        }

        /// <summary>
        /// Performs subtraction operation on two numbers
        /// </summary>
        /// <param name="num1">First number (minuend)</param>
        /// <param name="num2">Second number (subtrahend)</param>
        /// <returns>Difference of the two numbers</returns>
        private double PerformSubtraction(double num1, double num2)
        {
            return num1 - num2;
        }

        /// <summary>
        /// Performs multiplication operation on two numbers
        /// </summary>
        /// <param name="num1">First number</param>
        /// <param name="num2">Second number</param>
        /// <returns>Product of the two numbers</returns>
        private double PerformMultiplication(double num1, double num2)
        {
            return num1 * num2;
        }

        /// <summary>
        /// Performs division operation on two numbers
        /// Note: Division by zero should be checked before calling this method
        /// </summary>
        /// <param name="num1">First number (dividend)</param>
        /// <param name="num2">Second number (divisor)</param>
        /// <returns>Quotient of the two numbers</returns>
        private double PerformDivision(double num1, double num2)
        {
            return num1 / num2;
        }

        /// <summary>
        /// Event handler for the addition button click
        /// Performs addition operation and displays the result
        /// </summary>
        private void AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs before performing calculation
                if (ValidateInputs())
                {
                    // Perform the addition operation
                    calculationResult = PerformAddition(firstNumber, secondNumber);
                    // Create formatted string showing the calculation
                    string calculationText = $"{firstNumber} + {secondNumber} = {calculationResult}";
                    // Display the result to the user
                    resultLabel.Text = $"Result: {calculationText}";
                    resultLabel.ForeColor = Color.Black;
                    // Add this calculation to the history
                    AddToHistory(calculationText);
                    // Update button states based on current input
                    EnableCalculatorControls(true);
                }
            }
            catch (OverflowException ex)
            {
                // Handle case where result exceeds maximum value
                resultLabel.Text = $"Error: Calculation result is too large. {ex.Message}";
                resultLabel.ForeColor = Color.Red;
            }
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                resultLabel.Text = $"Error: An unexpected error occurred. {ex.Message}";
                resultLabel.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Event handler for the subtraction button click
        /// Performs subtraction operation and displays the result
        /// </summary>
        private void SubtractButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs before performing calculation
                if (ValidateInputs())
                {
                    // Perform the subtraction operation
                    calculationResult = PerformSubtraction(firstNumber, secondNumber);
                    // Create formatted string showing the calculation
                    string calculationText = $"{firstNumber} − {secondNumber} = {calculationResult}";
                    // Display the result to the user
                    resultLabel.Text = $"Result: {calculationText}";
                    resultLabel.ForeColor = Color.Black;
                    // Add this calculation to the history
                    AddToHistory(calculationText);
                    // Update button states based on current input
                    EnableCalculatorControls(true);
                }
            }
            catch (OverflowException ex)
            {
                // Handle case where result exceeds maximum value
                resultLabel.Text = $"Error: Calculation result is too large. {ex.Message}";
                resultLabel.ForeColor = Color.Red;
            }
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                resultLabel.Text = $"Error: An unexpected error occurred. {ex.Message}";
                resultLabel.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Event handler for the multiplication button click
        /// Performs multiplication operation and displays the result
        /// </summary>
        private void MultiplyButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs before performing calculation
                if (ValidateInputs())
                {
                    // Perform the multiplication operation
                    calculationResult = PerformMultiplication(firstNumber, secondNumber);
                    // Create formatted string showing the calculation
                    string calculationText = $"{firstNumber} × {secondNumber} = {calculationResult}";
                    // Display the result to the user
                    resultLabel.Text = $"Result: {calculationText}";
                    resultLabel.ForeColor = Color.Black;
                    // Add this calculation to the history
                    AddToHistory(calculationText);
                    // Update button states based on current input
                    EnableCalculatorControls(true);
                }
            }
            catch (OverflowException ex)
            {
                // Handle case where result exceeds maximum value
                resultLabel.Text = $"Error: Calculation result is too large. {ex.Message}";
                resultLabel.ForeColor = Color.Red;
            }
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                resultLabel.Text = $"Error: An unexpected error occurred. {ex.Message}";
                resultLabel.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Event handler for the division button click
        /// Performs division operation and displays the result
        /// Includes special handling for division by zero
        /// </summary>
        private void DivideButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs before performing calculation
                if (ValidateInputs())
                {
                    // Check for division by zero before performing the operation
                    if (secondNumber == 0)
                    {
                        throw new DivideByZeroException("Cannot divide by zero. Please enter a non-zero second number.");
                    }
                    // Perform the division operation
                    calculationResult = PerformDivision(firstNumber, secondNumber);
                    // Create formatted string showing the calculation
                    string calculationText = $"{firstNumber} ÷ {secondNumber} = {calculationResult}";
                    // Display the result to the user
                    resultLabel.Text = $"Result: {calculationText}";
                    resultLabel.ForeColor = Color.Black;
                    // Add this calculation to the history
                    AddToHistory(calculationText);
                    // Update button states based on current input
                    EnableCalculatorControls(true);
                }
            }
            catch (DivideByZeroException ex)
            {
                // Handle division by zero error specifically
                resultLabel.Text = $"Error: {ex.Message}";
                resultLabel.ForeColor = Color.Red;
            }
            catch (OverflowException ex)
            {
                // Handle case where result exceeds maximum value
                resultLabel.Text = $"Error: Calculation result is too large. {ex.Message}";
                resultLabel.ForeColor = Color.Red;
            }
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                resultLabel.Text = $"Error: An unexpected error occurred. {ex.Message}";
                resultLabel.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Event handler for the clear button click
        /// Clears all input fields and resets the calculator
        /// </summary>
        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearCalculator();
        }

        /// <summary>
        /// Event handler for the view history button click
        /// Opens a new window to display calculation history
        /// </summary>
        private void ViewHistoryButton_Click(object sender, EventArgs e)
        {
            OpenHistoryForm();
        }

        /// <summary>
        /// Clears all input fields and resets the result display
        /// Also disables operation buttons until new numbers are entered
        /// </summary>
        private void ClearCalculator()
        {
            // Clear both input text boxes
            number1TextBox.Clear();
            number2TextBox.Clear();
            // Reset the result label to default text
            resultLabel.Text = "Result will appear here";
            resultLabel.ForeColor = Color.Black;
            // Disable operation buttons since there's no input
            EnableCalculatorControls(false);
        }

        /// <summary>
        /// Enables or disables the operation buttons based on input state
        /// </summary>
        /// <param name="enabled">If true, buttons are enabled when both text boxes have content. If false, all buttons are disabled.</param>
        private void EnableCalculatorControls(bool enabled)
        {
            if (enabled)
            {
                // Enable buttons only if both text boxes contain non-empty text
                bool hasValidInput = !string.IsNullOrWhiteSpace(number1TextBox.Text) && !string.IsNullOrWhiteSpace(number2TextBox.Text);
                addButton.Enabled = hasValidInput;
                subtractButton.Enabled = hasValidInput;
                multiplyButton.Enabled = hasValidInput;
                divideButton.Enabled = hasValidInput;
            }
            else
            {
                // Disable all operation buttons
                addButton.Enabled = false;
                subtractButton.Enabled = false;
                multiplyButton.Enabled = false;
                divideButton.Enabled = false;
            }
        }

        /// <summary>
        /// Adds a calculation to the history list and arrays
        /// Implements circular buffer behavior - when array is full, it overwrites from the beginning
        /// </summary>
        /// <param name="calculation">Formatted string representation of the calculation (e.g., "5 + 3 = 8")</param>
        private void AddToHistory(string calculation)
        {
            try
            {
                // If we've reached the maximum entries, reset to start overwriting from the beginning
                // This implements a circular buffer pattern
                if (historyEntryCount >= MaxHistoryEntries)
                {
                    historyEntryCount = 0;
                }

                // Store the numeric result in the result array
                resultArray[historyEntryCount] = calculationResult;
                // Store the formatted calculation string in the operation array
                operationArray[historyEntryCount] = calculation;

                // Increment the counter for the next entry
                historyEntryCount++;

                // Add to the list with timestamp for display in history form
                calculationHistory.Add($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {calculation}");
            }
            catch (IndexOutOfRangeException ex)
            {
                // Handle array index errors (shouldn't happen with proper bounds checking)
                resultLabel.Text = $"Error: History array index out of range. {ex.Message}";
                resultLabel.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Event handler for when text changes in either number input text box
        /// Updates the enabled state of operation buttons based on current input
        /// </summary>
        private void NumberTextBox_TextChanged(object sender, EventArgs e)
        {
            // Re-evaluate button states when text changes
            EnableCalculatorControls(true);
        }

        /// <summary>
        /// Event handler for the save arrays button click
        /// Saves the array data to a file in the user's Documents folder
        /// </summary>
        private void SaveArraysButton_Click(object sender, EventArgs e)
        {
            SaveArraysToFile();
        }

        /// <summary>
        /// Saves the calculation arrays (resultArray and operationArray) to a text file
        /// The file is saved in the user's Documents folder as "CalculatorArrayData.txt"
        /// Shows a preview of the first 5 entries in a message box after saving
        /// </summary>
        private void SaveArraysToFile()
        {
            try
            {
                // Get the user's Documents folder path
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                // Create the full file path
                string filePath = Path.Combine(documentsPath, "CalculatorArrayData.txt");

                // Write the array data to the file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Write CSV header
                    writer.WriteLine("Index, Operation, Result");

                    // Write each entry from the arrays
                    for (int i = 0; i < historyEntryCount; i++)
                    {
                        writer.WriteLine($"{i + 1}, {operationArray[i]}, {resultArray[i]}");
                    }
                }

                // Prepare a preview of the first few entries to show in the message box
                int previewCount = Math.Min(historyEntryCount, 5);  // Show up to 5 entries
                string previewText = string.Empty;

                // Build the preview text
                for (int i = 0; i < previewCount; i++)
                {
                    previewText += $"{i + 1}. {operationArray[i]} (Result: {resultArray[i]}){Environment.NewLine}";
                }

                // Create the message to display to the user
                string message;

                if (historyEntryCount > 0)
                {
                    // Show file path and preview if there are entries
                    message = $"Array data saved to:{Environment.NewLine}{filePath}{Environment.NewLine}{Environment.NewLine}" +
                              $"First {previewCount} entries:{Environment.NewLine}{previewText}";
                }
                else
                {
                    // Show file path but indicate no data if arrays are empty
                    message = $"Array data saved to:{Environment.NewLine}{filePath}{Environment.NewLine}{Environment.NewLine}" +
                              "No calculations stored in the arrays yet.";
                }

                // Display success message with preview
                MessageBox.Show(message, "Array Data Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (UnauthorizedAccessException ex)
            {
                // Handle case where user doesn't have permission to write to the file
                MessageBox.Show($"Error: Access denied when writing array data to file.{Environment.NewLine}{ex.Message}",
                    "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                // Handle file I/O errors (e.g., disk full, file locked)
                MessageBox.Show($"Error: Problem writing array data to file.{Environment.NewLine}{ex.Message}",
                    "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Opens a new form to display the calculation history
        /// The history form shows all calculations with timestamps
        /// </summary>
        private void OpenHistoryForm()
        {
            // Create a new history form and pass the calculation history list
            HistoryForm historyForm = new HistoryForm(calculationHistory);
            // Show as a modal dialog (blocks interaction with main form until closed)
            historyForm.ShowDialog();
        }
    }
}
