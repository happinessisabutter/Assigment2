using System;
using System.Drawing;
using System.Windows.Forms;
using MovieBooking;

public class TicketQueryForm : Form
{
    public TicketQueryForm()
    {
        // Initialize form properties
        this.Text = "Ticket Query";
        this.Size = new Size(400, 300);

        DisplayQueryInterface();
    }

    private void DisplayQueryInterface()
    {
        Label ticketIdLabel = new Label();
        ticketIdLabel.Text = "Enter Ticket ID:";
        ticketIdLabel.Location = new Point(10, 10);
        this.Controls.Add(ticketIdLabel);

        TextBox ticketIdTextBox = new TextBox();
        ticketIdTextBox.Location = new Point(120, 10);
        ticketIdTextBox.Width = 200;
        this.Controls.Add(ticketIdTextBox);

        Button queryButton = new Button();
        queryButton.Text = "Search";
        queryButton.Location = new Point(330, 10);
        queryButton.Click += (s, e) =>
        {
            string ticketId = ticketIdTextBox.Text;
            Ticket ticket = TicketManager.GetTicket(ticketId);
            if (ticket != null)
            {
                DisplayTicketInfo(ticket);
            }
            else
            {
                MessageBox.Show("Ticket not found.");
            }
        };
        this.Controls.Add(queryButton);
    }

    private void DisplayTicketInfo(Ticket ticket)
    {
        // Clear previous controls
        this.Controls.Clear();

        Label movieLabel = new Label();
        movieLabel.Text = "Movie: " + ticket.Movie.Title;
        movieLabel.Location = new Point(10, 50);
        this.Controls.Add(movieLabel);

        Label dateLabel = new Label();
        dateLabel.Text = "Date: " + ticket.Date.ToString("MM/dd/yyyy");
        dateLabel.Location = new Point(10, 80);
        this.Controls.Add(dateLabel);

        Label showtimeLabel = new Label();
        showtimeLabel.Text = "Showtime: " + ticket.Showtime.ToString("HH:mm");
        showtimeLabel.Location = new Point(10, 110);
        this.Controls.Add(showtimeLabel);

        Label seatLabel = new Label();
        seatLabel.Text = "Seat: " + ticket.Seat;
        seatLabel.Location = new Point(10, 140);
        this.Controls.Add(seatLabel);

        Button cancelButton = new Button();
        cancelButton.Text = "Cancel Ticket";
        cancelButton.Location = new Point(10, 170);
        cancelButton.Click += (s, e) =>
        {
            TicketManager.RemoveTicket(ticket.TicketId);
            MessageBox.Show("Ticket has been canceled.");
            this.Controls.Clear();
            DisplayQueryInterface();
        };
        this.Controls.Add(cancelButton);
    }
}
