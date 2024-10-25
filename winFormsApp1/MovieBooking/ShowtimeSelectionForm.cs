using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using MovieBooking;

public class ShowtimeSelectionForm : Form
{
    private Movie movie;
    private DateTime selectedDate;
    private DateTime selectedShowtime;
    private List<string> selectedSeats = new List<string>();

    // Simulate seat availability (in a real application, this would come from a database)
    private Dictionary<string, bool> seatAvailability = new Dictionary<string, bool>();

    public ShowtimeSelectionForm(Movie movie)
    {
        this.movie = movie;

        // Initialize form properties
        this.Text = "Select Showtime and Seats";
        this.Size = new Size(700, 600);

        InitializeSeatAvailability();
        DisplayShowtimeAndSeats();
    }

    private void InitializeSeatAvailability()
    {
        // Initialize all seats as available (true)
        for (int row = 1; row <= 8; row++)
        {
            for (int col = 1; col <= 8; col++)
            {
                string seatNumber = $"{(char)('A' + row - 1)}{col}";
                seatAvailability[seatNumber] = true; // true means available
            }
        }
    }

    private void DisplayShowtimeAndSeats()
    {
        int y = 10;

        // Date selection
        Label dateLabel = new Label();
        dateLabel.Text = "Select Date:";
        dateLabel.Location = new Point(10, y);
        dateLabel.Size = new Size(100, 30);
        this.Controls.Add(dateLabel);

        DateTimePicker datePicker = new DateTimePicker();
        datePicker.Location = new Point(120, y);
        datePicker.Format = DateTimePickerFormat.Short; // MM/dd/yyyy
        datePicker.ValueChanged += (s, e) =>
        {
            selectedDate = datePicker.Value.Date;
        };
        selectedDate = datePicker.Value.Date; // Set default selected date
        this.Controls.Add(datePicker);

        y += 40;

        // Time slot selection
        Label timeLabel = new Label();
        timeLabel.Text = "Select Time Slot:";
        timeLabel.Location = new Point(10, y);
        timeLabel.Size = new Size(100, 30);
        this.Controls.Add(timeLabel);

        ComboBox timeComboBox = new ComboBox();
        timeComboBox.Location = new Point(120, y);
        timeComboBox.Width = 150;
        for (int hour = 8; hour <= 23; hour++)
        {
            DateTime showtime = DateTime.Today.AddHours(hour);
            timeComboBox.Items.Add(showtime.ToString("HH:00"));
        }
        timeComboBox.SelectedIndexChanged += (s, e) =>
        {
            string selectedTime = timeComboBox.SelectedItem.ToString();
            selectedShowtime = selectedDate.Add(TimeSpan.Parse(selectedTime));
        };
        timeComboBox.SelectedIndex = 0; // Default to the first time slot
        selectedShowtime = selectedDate.AddHours(8); // Set default selected showtime
        this.Controls.Add(timeComboBox);

        y += 50;

        // Confirm button (Moved above the seat grid)
        Button confirmButton = new Button();
        confirmButton.Text = "Confirm";
        confirmButton.Size = new Size(100, 30); // Made the button smaller
        confirmButton.Location = new Point(10, y); // Placed it higher
        confirmButton.Click += ConfirmButton_Click;
        this.Controls.Add(confirmButton);

        // Seat selection grid
        y += 40; // Move y down to position the seat grid below the confirm button

        Label seatLabel = new Label();
        seatLabel.Text = "Select Seats:";
        seatLabel.Location = new Point(10, y);
        seatLabel.Size = new Size(100, 30);
        this.Controls.Add(seatLabel);

        y += 40; // Increment y to position the seat panel

        Panel seatPanel = new Panel();
        seatPanel.Location = new Point(10, y);
        seatPanel.Size = new Size(650, 400);
        seatPanel.AutoScroll = true;
        this.Controls.Add(seatPanel);

        int seatButtonSize = 50;
        int seatButtonMargin = 5;

        for (int row = 1; row <= 8; row++)
        {
            for (int col = 1; col <= 8; col++)
            {
                string seatNumber = $"{(char)('A' + row - 1)}{col}";

                Button seatButton = new Button();
                seatButton.Text = seatNumber;
                seatButton.Size = new Size(seatButtonSize, seatButtonSize);
                seatButton.Location = new Point(
                    (col - 1) * (seatButtonSize + seatButtonMargin),
                    (row - 1) * (seatButtonSize + seatButtonMargin)
                );

                seatButton.BackColor = seatAvailability[seatNumber] ? Color.Green : Color.Red;
                seatButton.Enabled = seatAvailability[seatNumber];

                seatButton.Click += (s, e) =>
                {
                    if (selectedSeats.Contains(seatNumber))
                    {
                        // Deselect seat
                        selectedSeats.Remove(seatNumber);
                        seatButton.BackColor = Color.Green;
                    }
                    else
                    {
                        // Select seat
                        selectedSeats.Add(seatNumber);
                        seatButton.BackColor = Color.Red;
                    }
                };

                seatPanel.Controls.Add(seatButton);
            }
        }
    }

    private void ConfirmButton_Click(object sender, EventArgs e)
    {
        if (selectedShowtime != null && selectedSeats.Count > 0)
        {
            foreach (string seat in selectedSeats)
            {
                // Update seat availability (mark as booked)
                seatAvailability[seat] = false;
            }

            // Create a ticket for each selected seat
            foreach (string seat in selectedSeats)
            {
                Ticket ticket = new Ticket
                {
                    TicketId = TicketManager.GenerateTicketId(), // Use the new method
                    Movie = movie,
                    Date = selectedDate,
                    Showtime = selectedShowtime,
                    Seat = seat
                };
                TicketManager.AddTicket(ticket);

                // Display ticket information
                TicketInfoForm ticketForm = new TicketInfoForm(ticket);
                ticketForm.ShowDialog();
            }

            this.Close();
        }
        else
        {
            MessageBox.Show("Please select a time slot and at least one seat.");
        }
    }
}
