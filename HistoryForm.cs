using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CalculatorApp
{
    /// <summary>
    /// Form that displays the calculation history in a list box
    /// Allows users to view all past calculations with timestamps
    /// Provides functionality to clear the history
    /// </summary>
    public partial class HistoryForm : Form
    {
        // UI Controls
        private ListBox historyListBox;      // Displays the list of calculations
        private Button clearHistoryButton;  // Button to clear all history entries
        private Button closeButton;          // Button to close this form
        private Label titleLabel;            // Title label at the top of the form
        
        // Data - Reference to the calculation history list from the main form
        private List<string> calculationHistory;

        /// <summary>
        /// Constructor - Initializes the history form with the provided history list
        /// </summary>
        /// <param name="history">List of calculation history entries with timestamps</param>
        public HistoryForm(List<string> history)
        {
            // Use provided history or create empty list if null
            calculationHistory = history ?? new List<string>();
            // Set up the UI components
            InitializeComponent();
            // Load and display the history
            LoadHistory();
        }

        /// <summary>
        /// Initializes all UI components and sets up the form layout
        /// </summary>
        private void InitializeComponent()
        {
            // Configure the main form window properties
            this.Text = "Calculation History";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;  // Center the window on screen
            this.BackColor = Color.LightGray;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;  // Prevent resizing
            this.MaximizeBox = false;  // Disable maximize button

            // Create and configure the title label
            titleLabel = new Label();
            titleLabel.Text = "Calculation History";
            titleLabel.Location = new Point(50, 10);
            titleLabel.Size = new Size(400, 30);
            titleLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(titleLabel);

            // Create and configure the list box to display history
            historyListBox = new ListBox();
            historyListBox.Location = new Point(50, 50);
            historyListBox.Size = new Size(400, 250);
            historyListBox.Font = new Font("Arial", 10);
            this.Controls.Add(historyListBox);

            // Create and configure the clear history button
            clearHistoryButton = new Button();
            clearHistoryButton.Text = "Clear History";
            clearHistoryButton.Location = new Point(50, 310);
            clearHistoryButton.Size = new Size(150, 35);
            clearHistoryButton.Font = new Font("Arial", 11, FontStyle.Bold);
            clearHistoryButton.BackColor = Color.LightCoral;
            clearHistoryButton.ForeColor = Color.DarkRed;
            clearHistoryButton.Click += ClearHistoryButton_Click;  // Attach click event handler
            this.Controls.Add(clearHistoryButton);

            // Create and configure the close button
            closeButton = new Button();
            closeButton.Text = "Close";
            closeButton.Location = new Point(300, 310);
            closeButton.Size = new Size(150, 35);
            closeButton.Font = new Font("Arial", 11, FontStyle.Bold);
            closeButton.BackColor = Color.LightBlue;
            closeButton.ForeColor = Color.DarkBlue;
            closeButton.Click += CloseButton_Click;  // Attach click event handler
            this.Controls.Add(closeButton);
        }

        /// <summary>
        /// Loads the calculation history and displays it in the list box
        /// </summary>
        private void LoadHistory()
        {
            RefreshHistoryDisplay();
        }

        /// <summary>
        /// Event handler for the clear history button click
        /// Removes all entries from the calculation history
        /// </summary>
        private void ClearHistoryButton_Click(object sender, EventArgs e)
        {
            ClearHistoryList();
        }

        /// <summary>
        /// Event handler for the close button click
        /// Closes the history form window
        /// </summary>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            CloseHistoryForm();
        }

        /// <summary>
        /// Clears all entries from the calculation history list
        /// Updates the display to show the empty list
        /// </summary>
        private void ClearHistoryList()
        {
            // Remove all entries from the history list
            calculationHistory.Clear();
            // Refresh the display to show the empty list
            RefreshHistoryDisplay();
        }

        /// <summary>
        /// Closes the history form window
        /// </summary>
        private void CloseHistoryForm()
        {
            this.Close();
        }

        /// <summary>
        /// Refreshes the list box display with current calculation history
        /// Numbers each entry sequentially starting from 1
        /// </summary>
        private void RefreshHistoryDisplay()
        {
            // Clear existing items in the list box
            historyListBox.Items.Clear();
            // Add each history entry with a number prefix
            for (int i = 0; i < calculationHistory.Count; i++)
            {
                historyListBox.Items.Add($"{i + 1}. {calculationHistory[i]}");
            }
        }
    }
}
