using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1.Data;
using WinFormsApp1.Models;
using WinFormsApp1.Repository;
using WinFormsApp1.Service;

namespace WinFormsApp1.MovieBooking
{
    /// <summary>
    /// getAllShowByMovieId
    /// </summary>
    public partial class SeatSelection : Form
    {
        private Movie movie;
        
        private List<SeatDto> selectedSeats = new List<SeatDto>();

        private MovieRepository movieRepository = new MovieRepository(new CinemaAppDbcontext());
        private ShowingRepository showingRepository = new ShowingRepository(new CinemaAppDbcontext());
        private TicketService ticketService = new TicketService(new TicketRepository(new CinemaAppDbcontext()));
        private RoomRepository roomRepository = new RoomRepository(new CinemaAppDbcontext());
        private ShowingService showingService = new ShowingService(new ShowingRepository(new CinemaAppDbcontext()));
        private SeatService seatService = new SeatService();
        private Room room;
        private Showing showing;
        
        public SeatSelection()
        {
            InitializeComponent();
            
            //current room need to be modified after adding showing selection
            this.Load += async (sender, e) => await InitializeAsync();
        }
        private async Task InitializeAsync()
        {
            // Initialize form properties
            this.Text = "Select Showtime and Seats";
            this.Size = new Size(600, 700);
            this.movie = await movieRepository.GetByIdAsync(428);
            this.room = await roomRepository.GetByIdAsync(1);
            this.showing = await showingRepository.GetByIdAsync(1);
            ShowDateAndSeats();
        }

        private void displayShowTime(int movieId, int y, DateTime selectDate)
        {
            // Clear previous time slot ComboBox
            var oldComboBox = this.Controls.OfType<ComboBox>().FirstOrDefault(c => c.Tag as string == "showTimeCombo");
            if (oldComboBox != null)
            {
                this.Controls.Remove(oldComboBox);
                oldComboBox.Dispose();
            }

            Label timeLabel = new Label();
            timeLabel.Text = "Select Time Slot:";
            timeLabel.Location = new Point(10, y);
            timeLabel.Size = new Size(150, 30);  
            this.Controls.Add(timeLabel);

            ComboBox timeComboBox = new ComboBox();
            timeComboBox.Location = new Point(170, y);
            timeComboBox.Width = 200;
            timeComboBox.Tag = "showTimeCombo";  // Name of ComboBox
            this.Controls.Add(timeComboBox);


            // Fetch and filter showings
            List<Showing> allShowings = (List<Showing>) showingService.GetShowingByMovieAndDate(movieId, selectDate);
            
            if (allShowings.Any())
            {
                foreach (var showing in allShowings)
                {
                    string timeSlot = $"{showing.StartTime:HH:mm} - {showing.EndTime:HH:mm}";
                    timeComboBox.Items.Add(new KeyValuePair<int, string>(showing.ShowingId, timeSlot));
                }
            }
            else
            {
                timeComboBox.Items.Add("<no showing at that date>");
            }

            timeComboBox.DisplayMember = "Value";
            timeComboBox.ValueMember = "Key";
            timeComboBox.SelectedIndex = 0;
            timeComboBox.Update();
            
        }


        private void datePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker picker = sender as DateTimePicker;
            if (picker == null) return;

            DateTime selectedDate = picker.Value.Date;

            
            var oldComboBox = this.Controls.OfType<ComboBox>().FirstOrDefault(c => c.Tag as string == "showTimeCombo");
            if (oldComboBox != null)
            {
                int oldY = oldComboBox.Location.Y;
                this.Controls.Remove(oldComboBox);
                oldComboBox.Dispose();
                
                displayShowTime(movie.MovieId, oldY, selectedDate);
                
            }
            else
            {
                
                displayShowTime(movie.MovieId, 50, selectedDate);
            }
        }




        private void ShowDateAndSeats()
        {
            int y = 10;

            // Date selection
            Label dateLabel = new Label()
            {
                Text = "Select Date:",
                Location = new Point(10, y),
                Size = new Size(150, 30)
            };
            this.Controls.Add(dateLabel);

            DateTimePicker datePicker = new DateTimePicker()
            {
                Location = new Point(170, y),
                Format = DateTimePickerFormat.Short, // Format as MM/dd/yyyy
                Tag = "datePicker" // Tag to identify the control
            };

            datePicker.ValueChanged += datePicker_ValueChanged;
            this.Controls.Add(datePicker);

            y += 40; // Increment y to place the next control

            // Initialize the first display of showtimes
            displayShowTime(movie.MovieId, y, datePicker.Value.Date);

            y += 50; // Adjust y for additional controls like buttons

            // Example of adding a confirm button, adjust y based on your needs
            Button confirmButton = new Button()
            {
                Text = "Confirm",
                Size = new Size(100, 30),
                Location = new Point(10, y)
            };
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
                seatPanel.Size = new Size(550, 700);
                seatPanel.AutoScroll = true;
                this.Controls.Add(seatPanel);

                int seatButtonSize = 50;
                int seatButtonMargin = 5;
                //seats in database
                List<SeatDto> seats = (List<SeatDto>)seatService.GetSeatByRoom(room.RoomId);
                Dictionary<string, SeatDto> seatDictionary = seats.ToDictionary(s => $"{(char)('A' + s.Row - 1)}{s.Column}");

            for (int row = 1; row <= room.MaximunRow; row++)
            {
                for (int col = 1; col <= room.MaximunCol; col++)
                {
                    string seatNumber = $"{(char)('A' + row - 1)}{col}";
                    if (!seatDictionary.ContainsKey(seatNumber))
                        continue;

                    SeatDto currentSeat = seatDictionary[seatNumber];

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
                        SeatDto clickedSeat = (SeatDto)clickedButton.Tag;

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


        /// <summary>
        /// use seatDto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ConfirmButton_Click(object sender, EventArgs e)
        {
            // Retrieve the ComboBox by tag
            ComboBox timeComboBox = this.Controls.OfType<ComboBox>().FirstOrDefault(c => c.Tag as string == "showTimeCombo");
            //value is the Key Part.
            // whether is null
            if (timeComboBox == null || timeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please choose a valid period. ");
                return; 
            }
            int selectedShowingId = 0;
            // Check if the selected item is a KeyValuePair(default value is string)
            if (timeComboBox.SelectedItem is KeyValuePair<int, string>)
            {
                // 安全地将 SelectedValue 转换为整数
                selectedShowingId = ((KeyValuePair<int, string>)timeComboBox.SelectedItem).Key;
               
            }
            else
            {
                MessageBox.Show("Please choose a valid period. ");
                return;
            }

            if (!selectedSeats.Any())
            {
                MessageBox.Show("Please select at least one seat.");
                return; // Exit if no seats are selected
            }

            // Proceed with ticket creation as both time slot and seats are valid
            
            List<Ticket> createdTickets = new();

            try
            {
                foreach (SeatDto seat in selectedSeats)
                {
                    Ticket ticket = await ticketService.CreatTicket(selectedShowingId, seat.SeatId);
                    createdTickets.Add(ticket);
                }

                // Display ticket information in a new form
                TicketInfoForm ticketForm = new TicketInfoForm(createdTickets);
                ticketForm.ShowDialog();

                this.Close(); // Close the form only after all operations are successful
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to create ticket: " + ex.Message);
            }
        }

    }
}
