# Event Booking App - Full Stack Project Requirement
Project Name: Event Booking App

## Objective:
Build a local full-stack application that allows users to browse available events and book seats.
Admins can manage events.

## Core Features:
1. User Roles
- There are two roles: User and Admin.
- Role can be selected at the time of login (no authentication needed).

2. For Admin:
- Add a new event with:
- Event name
- Date and time
- Location
- Total available seats
- Description
- Edit or delete an existing event
- View total bookings per event

3. For Users:
- View list of all upcoming events
- Book a seat for a selected event (only if seats are available)
- View their own bookings list

4. Event Details Page:
- Show event description, date/time, location, total seats, available seats
- Book button should be disabled if no seats left

5. Booking Logic:
- When a user books, reduce the available seats by one
- Prevent multiple bookings for the same user on the same event

## Challenging Requirement:
- Implement seat availability control to prevent overbooking if multiple users attempt to book at the
same time.

## Data to Store:
- Users (basic info and role)
- Events (name, date, seats, etc.)
- Bookings (user ID, event ID, timestamp)

## Demo Expectations:
- Local demo working for both User and Admin roles
- Show event list, booking, and booking count updates
- Admin dashboard showing bookings per event
- Clean and simple UI
- Basic validations like required fields and seat availability