# KinoCentre

KinoCentre is a ticket booking API for a cinema. It allows users to purchase tickets, validate them, and send ticket details via email in PDF format.

## Features

- **Ticket Management**: 
  - Purchase tickets for a specific session.
  - Validate tickets by their ID or by ID and phone number.
  - Retrieve all tickets for a session or a specific ticket by ID.

- **Email Notifications**:
  - Sends purchased tickets as PDF attachments to the user's email.

- **Database Integration**:
  - Uses MongoDB for storing ticket and session data.

## Technologies Used

- **Backend**: ASP.NET Core
- **Database**: MongoDB
- **Email Service**: MailKit
- **PDF Generation**: Custom PDF generator
- **Dependency Injection**: Unit of Work pattern

## Project Structure

- `KinoCentre.API`: Contains the API controllers and email sender logic.
- `KinoCentre.DB`: Contains database entities and repositories.
- `KinoCentre.API/Controllers/TicketsController.cs`: Handles ticket-related API endpoints.
- `KinoCentre.API/EmailSender.cs`: Handles email sending functionality.
- `KinoCentre.DB/Entities/Ticket.cs`: Defines the `Ticket` entity.

## Endpoints

### Tickets

- **Get all tickets for a session**  
  `GET /api/tickets/{sessionId}/taken`

- **Get ticket by ID**  
  `GET /api/tickets/{id}`

- **Validate ticket by last 4 digits of ID**  
  `GET /api/tickets/validate/{last4TicketIdDigits}`

- **Validate ticket by last 4 digits of ID and phone**  
  `GET /api/tickets/validate/{last4TicketIdDigits}/{last4PhoneDigits}`

- **Purchase a ticket**  
  `POST /api/tickets`  
  Request Body:
  ```json
  {
    "sessionId": "string",
    "seatRow": 1,
    "seatNumber": 1,
    "email": "string",
    "phone": "string",
    "originalPrice": 0.0,
    "finalPrice": 0.0
  }
