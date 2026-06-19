CREATE DATABASE EventSystem;
GO
USE EventSystem;
GO

-- 1. User Table
CREATE TABLE [User] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL
);

-- 2. Main Event Table (Conferences, Festivals, etc.)
CREATE TABLE [Event] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    Location NVARCHAR(200) NOT NULL,
    City NVARCHAR(200) NOT NULL, --for the API weather
    EventType NVARCHAR(50) NOT NULL -- Conference, Festival, Party, etc.
);

-- 3. Sessions Table (Sub-Event, Lectures, Performances)
CREATE TABLE [Session] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL FOREIGN KEY REFERENCES [Event](Id) ON DELETE CASCADE,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    SpeakerName NVARCHAR(100) NULL, -- Speaker, Lecturer or Artist
    StartTime DATETIME NOT NULL,
    EndTime DATETIME NOT NULL,
    RoomName NVARCHAR(50) NULL      -- Specific hall, room, or stage
);

-- 4. Junction Table for [Session] Registrations (Many-to-Many Relationship)
-- The composite primary key strictly prEvent duplicate registrations for the same [Session].
CREATE TABLE SessionRegistration (
    SessionId INT NOT NULL FOREIGN KEY REFERENCES [Session](Id) ON DELETE CASCADE,
    UserId INT NOT NULL FOREIGN KEY REFERENCES [User](Id) ON DELETE CASCADE,
    RegistrationDate DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (SessionId, UserId)
);

-- 5. Seed Data (All Content in English)
INSERT INTO [User] (FullName, Email) VALUES 
('John Doe', 'john.doe@example.com'),
('Jane Smith', 'jane.smith@example.com'),
('Bob Johnson', 'bob.j@example.com'),
('Idan Lin', 'idanlin97@gmail.com'),
('Ofri Lin', 'ofrilin93@gmail.com'),
('Noam Lin', 'noamlin01@gmail.com'),
('Gil Lin', 'gillin64@gmail.com'),
('Debby Lin', 'debbylin65@gmail.com');

INSERT INTO [Event] (Title, Description, StartDate, EndDate, Location, City, EventType) VALUES 
('Modern Tech Conference 2026', 'A comprehensive conference covering software development and artificial intelligence.', '2026-06-15 09:00:00', '2026-06-15 17:00:00', 'Expo Tel Aviv', 'Tel Aviv', 'Conference'),
('Summer Music Festival 2026', 'An outdoor multi-stage music festival featuring international electronic and rock artists.', '2026-07-20 16:00:00', '2026-07-22 23:00:00', 'Yarkon Park Tel Aviv', 'Tel Aviv', 'Festival'),
('React Developers Summit', 'Deep dive into React, Next.js and frontend architecture.', '2026-05-01 09:00:00', '2026-05-02 18:00:00', 'Convention Center', 'Tel Aviv', 'Conference'),
('AI & The Future', 'Exploring the boundaries of Artificial Intelligence.', '2026-06-10 10:00:00', '2026-06-10 16:00:00', 'ICC', 'Jerusalem', 'Workshop'),
('Desert Burning Hackathon', 'A 48-hour coding marathon in the desert.', '2026-08-15 08:00:00', '2026-08-17 20:00:00', 'Desert', 'Idan', 'Hackathon');

INSERT INTO [Session] (EventId, Title, Description, SpeakerName, StartTime, EndTime, RoomName) VALUES 
(1, 'Introduction to AI in Cloud', 'Keynote session exploring modern cloud-based artificial intelligence tools and LLMs.', 'Dr. John Doe', '2026-06-15 09:30:00', '2026-06-15 10:30:00', 'Hall A'),
(1, 'Microservices Architecture', 'A deep dive into server-side microservices design patterns and API gateways.', 'Eng. Ruth Levi', '2026-06-15 10:40:00', '2026-06-15 11:40:00', 'Hall B'),
(2, 'Opening Rock Live Act', 'An energetic live performance to kick off the summer festival lineup.', 'The Code Rockers', '2026-07-20 17:00:00', '2026-07-20 18:30:00', 'Main Stage'),
(3, 'React Best Practices', 'Writing clean code in React components.', 'Dan Abramov', '2026-05-01 13:00:00', '2026-05-01 14:30:00', 'Hall 1'),
(4, 'AI Ethics Panel', 'Discussion on AI safety and regulation.', 'Dr. Ethics', '2026-06-10 14:00:00', '2026-06-10 15:30:00', 'Room 2'),
(3, 'SEO for E-commerce', 'How to rank your shop high on Google.', 'Marketing Expert', '2026-05-01 14:00:00', '2026-05-01 15:30:00', 'Main Stage'),
(5, 'FinTech Security', 'Securing financial transactions.', 'Bank Security Officer', '2026-08-16 10:00:00', '2026-08-16 11:30:00', 'Hall A'),
(2, 'Late Night Jam', 'Improvisational jazz session.', 'Jazz Collective', '2026-07-21 21:00:00', '2026-07-21 23:00:00', 'Stage B'),
(1, 'Serverless Architecture', 'Scaling apps with AWS Lambda.', 'Cloud Architect', '2026-06-15 10:00:00', '2026-06-15 13:00:00', 'Room 101'),
(4, 'Level Design 101', 'Creating engaging game environments.', 'Lead Level Designer', '2026-06-10 15:00:00', '2026-06-10 16:00:00', 'Studio 2'),
(2, 'Future of Protein', 'Sustainable alternatives to meat.', 'Food Scientist', '2026-07-22 12:00:00', '2026-07-22 13:30:00', 'Main Stage');

INSERT INTO SessionRegistration (SessionId, UserId, RegistrationDate) VALUES
(1, 1, '2026-06-15 09:30:00'),
(1, 2, '2026-06-15 09:30:00'),
(1, 3, '2026-06-15 09:30:00'),
(2, 1, '2026-06-15 10:40:00'),
(2, 2, '2026-06-15 10:40:00'),
(2, 5, '2026-06-15 10:40:00');
