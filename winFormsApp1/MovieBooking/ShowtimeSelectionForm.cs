using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using WinFormsApp1.MovieBooking;
using WinFormsApp1.Models;
using WinFormsApp1.Repository;
using WinFormsApp1.Service;
using WinFormsApp1.Data;
using log4net.Config;
using System.Linq;

/// <summary>
/// User is able to select a showtime and seats for a movie
/// also be able to cancel by click on the red button to deselect the seat
/// prompt a message shows "Please select a time slot and at least one seat." if user click confirm button without selecting a time slot or seat
/// prompt a message shows "Are you sure you want to cancel?" if user click cancel button
/// 
/// Back-end:Put seat from database the conponent show should inlcude dataGridview['id']['row']['column']['Isavailable']
/// 
/// </summary>
/*public partial class ShowtimeSelectionForm : Form
{
    private Movie movie;
    private DateTime selectedDate;
    private DateTime selectedShowtime;
    private List<Seat> selectedSeats = new List<Seat>();

    private MovieRepository movieRepository = new MovieRepository(new CinemaAppDbcontext());
    private TicketService ticketService = new TicketService(new TicketRepository(new CinemaAppDbcontext()));
    private RoomRepository roomRepository = new RoomRepository(new CinemaAppDbcontext());
    private ShowingService showingService = new ShowingService(new ShowingRepository(new CinemaAppDbcontext()));
    private SeatService seatService = new SeatService();
    private Room room;
    private Showing showing;
    public ShowtimeSelectionForm()//pass movie from last form
    {
        

        //current room need to be modified after adding showing selection
        this.Load += async (sender, e) => await InitializeAsync();

    }

    private async Task InitializeAsync()
    {
        // Initialize form properties
        this.Text = "Select Showtime and Seats";
        this.Size = new Size(700, 600);
        this.movie = await movieRepository.GetByIdAsync(428);

        room = await roomRepository.CreateRoomWithSeats("Room 1", 64, 8, 8);
        DateTime startTime = new DateTime(2024, 10, 26, 12, 0, 0);
        DateTime endTime = startTime.AddHours(2);
        showing = await showingService.CreateShowingAsync(428, room.RoomId, startTime, endTime);
        DisplayShowtimeAndSeats();
    }
    // assume it is room one if put Showing selection ahead of seat selection 
    private async Task InitializeSeatAvailability()
    {
        Room room = await roomRepository.GetByIdAsync(1);
        var seats = room.Seats;
        // Initialize all seats as available (true)
        for (int row = 1; row <= room.MaximunRow; row++)
        {
            for (int col = 1; col <= room.MaximunCol; col++)
            {
                
                string seatNumber = $"{(char)('A' + row - 1)}{col}";
                
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

        // Time slot selection get available start time from database
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
        selectedShowtime = selectedDate.AddHours(8); // Set default selected showtime use showing 
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

        ///get seat from database here
        Panel seatPanel = new Panel();
        seatPanel.Location = new Point(10, y);
        seatPanel.Size = new Size(650, 400);
        seatPanel.AutoScroll = true;
        this.Controls.Add(seatPanel);

        int seatButtonSize = 50;
        int seatButtonMargin = 5;
        //seats in database
        List<Seat> seats = (List<Seat>) seatService.GetSeatByRoom(room.RoomId).Result;
        Dictionary<string, Seat> seatDictionary = seats.ToDictionary(s => $"{(char)('A' + s.Row - 1)}{s.Column}");

        for (int row = 1; row <= room.MaximunRow; row++)
        {
            for (int col = 1; col <= room.MaximunCol; col++)
            {
                string seatNumber = $"{(char)('A' + row - 1)}{col}";
                if (!seatDictionary.ContainsKey(seatNumber))
                    continue;

                Seat currentSeat = seatDictionary[seatNumber];

                Button seatButton = new Button();
                seatButton.Text = seatNumber;
                seatButton.Size = new Size(seatButtonSize, seatButtonSize);
                seatButton.Location = new Point(
                    (col - 1) * (seatButtonSize + seatButtonMargin),
                    (row - 1) * (seatButtonSize + seatButtonMargin)
                );

                // Store the current seat in the Tag property for later use
                seatButton.Tag = currentSeat;

                // Set the initial color based on availability
                seatButton.BackColor = currentSeat.IsAvailable ? Color.Green : Color.Red;
                seatButton.Enabled = currentSeat.IsAvailable;

                // Event handler for seat selection
                seatButton.Click += (s, e) =>
                {
                    Button clickedButton = s as Button;
                    Seat clickedSeat = (Seat)clickedButton.Tag;

                    if (selectedSeats.Contains(clickedSeat))
                    {
                        // Deselect seat
                        selectedSeats.Remove(clickedSeat);
                        clickedButton.BackColor = Color.Green;
                    }
                    else
                    {
                        // Select seat
                        selectedSeats.Add(clickedSeat);
                        clickedButton.BackColor = Color.Red;
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
            
            // Create a ticket for each selected seat
            foreach (Seat seat in selectedSeats)
            {
                ticketService.CreatTicket(showing.ShowingId, seat.SeatId, seatService.CalculatePriceBySeatLocation(seat.Row, seat.Column, room.MaximunRow, room.MaximunCol));
                
                /*TicketManager.AddTicket(ticket);

                // Display ticket information
                TicketInfoForm ticketForm = new TicketInfoForm(ticket);//put ticket object into the form
                ticketForm.ShowDialog();*/
            /*}

            this.Close();
        }
        else
        {
            MessageBox.Show("Please select a time slot and at least one seat.");
        }
    }
}*/
