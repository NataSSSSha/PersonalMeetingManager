namespace PersonalMeetingManager
{
    /// <summary>
    /// Встреча.
    /// </summary>
    internal class Meeting
    {
        /// <summary>
        /// Дата и время начала встречи. 
        /// </summary>
        public DateTime MeetingStartDateTime { get; set; }

        /// <summary>
        /// Дата и время окончания встречи.
        /// </summary>
        public DateTime MeetingEndDateTime { get; set; }

        /// <summary>
        /// Название встречи.
        /// </summary>
        public string NameOfMeeting { get; set; }

        /// <summary>
        /// Напоминание о встрече.
        /// </summary>
        public Timer TimerToReminder { get; set; }

        /// <summary>
        /// Вывести сообщение о встрече.
        /// </summary>
        /// <param name="obj">Объект типа Object.</param>
        public void ConsoleReminder(Object obj)
        {
            Console.WriteLine($"Напоминание о встрече: {TimeOnly.FromDateTime(MeetingStartDateTime)} - {TimeOnly.FromDateTime(MeetingEndDateTime)}:" +
                             $"{NameOfMeeting}");
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="meetingStartDateTime">Дата и время начала события.</param>
        /// <param name="meetingEndDateTime">Дата и время окончания события.</param>
        /// <param name="nameOfMeeting">Название события.</param>
        public Meeting(DateTime meetingStartDateTime, DateTime meetingEndDateTime, string nameOfMeeting)
        {
            MeetingStartDateTime = meetingStartDateTime;
            MeetingEndDateTime = meetingEndDateTime;
            NameOfMeeting = nameOfMeeting;
        }
    }
}
