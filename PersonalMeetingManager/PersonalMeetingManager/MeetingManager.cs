namespace PersonalMeetingManager
{
    /// <summary>
    /// Управление встречами.
    /// </summary>
    internal static class MeetingManager
    {
        /// <summary>
        /// Список встреч.
        /// </summary>
        public static List<Meeting> meetings = new List<Meeting>();

        /// <summary>
        /// Событие при действиях со встречами.
        /// </summary>
        public static event Action<string>? NotifyMessage;


        /// <summary>
        /// Добавить встречу в список.
        /// </summary>
        /// <param name="meeting">Встреча.</param>
        public static void AddMeeting(Meeting meeting)
        {
            meetings.Add(meeting);
            NotifyMessage?.Invoke("Встреча добавлена.");
        }

        /// <summary>
        /// Удалить встречу из списка.
        /// </summary>
        /// <param name="meeting">Встреча.</param>
        public static void RemoveMeeting(Meeting meeting)
        {
            meetings.Remove(meeting);
            NotifyMessage?.Invoke("Встреча удалена.");

        }

        /// <summary>
        /// Создать встречу.
        /// </summary>
        /// <param name="meetingStart">Время начала встречи.</param>
        /// <param name="meetingEnd">Время окончания встречи.</param>
        /// <param name="nameOfMeeting">Название встречи.</param>
        /// <returns>Новую встречу.</returns>
        public static Meeting CreateMeeting(DateTime meetingStart, DateTime meetingEnd, string nameOfMeeting)
        {
            Meeting meeting = new Meeting(meetingStart, meetingEnd, nameOfMeeting);
            return meeting;
        }

        /// <summary>
        /// Найти события на дату.
        /// </summary>
        /// <param name="date">Дата.</param>
        /// <returns>Список событий на определенную дату.</returns>
        public static List<Meeting> FindMeetingsByDate(DateOnly date)
        {
            List<Meeting> meetingsByDate = new List<Meeting>();

            foreach (Meeting meeting in meetings)
                if (DateOnly.FromDateTime(meeting.MeetingStartDateTime) == date)
                    meetingsByDate.Add(meeting);

            return meetingsByDate;
        }

        /// <summary>
        /// Сохранить список событий на определенную дату в файл.
        /// </summary>
        /// <param name="meetingsForTheDay">Список событий на отпределенную дату.</param>
        /// <param name="date">Дата.</param>
        public static void SaveMeetingsToFile(List<Meeting> meetingsForTheDay, DateOnly date)
        {

            string nameOfFile = $"{date.ToString()}.txt";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), nameOfFile);

            using (FileStream file = new FileStream(filePath, FileMode.OpenOrCreate))
            using (StreamWriter sw = new StreamWriter(file))
            {
                foreach (Meeting meeting in meetingsForTheDay)
                {
                    sw.WriteLine($"{meeting.MeetingStartDateTime} - {meeting.MeetingEndDateTime}:" +
                        $"{meeting.NameOfMeeting}");
                }
            }

            NotifyMessage?.Invoke("Данные записаны в файл");
        }

        /// <summary>
        /// Проверить, что список встреч не пустой.
        /// </summary>
        /// <returns>True, если список содержит хотя бы одну встречу, false - список пуст.</returns>
        public static bool IsListOfMeetingsNotEmpty()
        {
            return meetings.Any();
        }

        /// <summary>
        /// Изменить время начала встречи.
        /// </summary>
        /// <param name="meeting">Встреча.</param>
        /// <param name="meetingStart">Время начала встречи.</param>
        public static void ChangeMeetingStart(Meeting meeting, DateTime meetingStart)
        {
            meeting.MeetingStartDateTime = meetingStart;
            NotifyMessage?.Invoke("Время начала встречи изменено");
        }

        /// <summary>
        /// Изменить время окончания встречи.
        /// </summary>
        /// <param name="meeting">Встреча.</param>
        /// <param name="meetingEnd">Время окончания встречи.</param>
        public static void ChangeMeetingEnd(Meeting meeting, DateTime meetingEnd)
        {
            meeting.MeetingEndDateTime = meetingEnd;
            NotifyMessage?.Invoke("Время окончания встречи изменено");
        }

        /// <summary>
        /// Изменить название встречи.
        /// </summary>
        /// <param name="meeting">Встреча.</param>
        /// <param name="nameOfMeeting">Название встречи.</param>
        public static void ChangeNameOfMeeting(Meeting meeting, string nameOfMeeting)
        {
            meeting.NameOfMeeting = nameOfMeeting;
            NotifyMessage?.Invoke("Название встречи изменено");
        }

        /// <summary>
        /// Найти встречу по времени ее начала.
        /// </summary>
        /// <param name="meetingStart">Время начала встречи.</param>
        /// <returns>Встречу с определенным временем начала.</returns>
        public static Meeting FindMeetingByDateTime(DateTime meetingStart) 
        {
            return meetings.Find(item => item.MeetingStartDateTime == meetingStart);
        }
        
        /// <summary>
        /// Установить напоминание о встрече.
        /// </summary>
        /// <param name="meeting">Встреча.</param>
        /// <param name="time">Время, за которое нужно напомнить о встрече.</param>
        public static void StartTimerToReminder(Meeting meeting, TimeSpan time)
        {
            try
            {
                TimeSpan timeToMeeting = meeting.MeetingStartDateTime - DateTime.Now - time;
                if (timeToMeeting < TimeSpan.Zero)
                    throw new Exception("Напоминание не может быть установлено. Время напоминания прошло.");
                else
                {
                    meeting.TimerToReminder = new Timer(meeting.ConsoleReminder, meeting, timeToMeeting, Timeout.InfiniteTimeSpan);
                    NotifyMessage?.Invoke("Напоминание о встрече установлено");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            } 
        }

        /// <summary>
        /// Изменить время напоминания о встрече.
        /// </summary>
        /// <param name="meeting">Встреча.</param>
        /// <param name="time">Время, за которое нужно напомнить о встрече.</param>
        public static void ChangeTimerToReminder(Meeting meeting, TimeSpan time)
        {
            try
            {
                TimeSpan timeToMeeting = meeting.MeetingStartDateTime - DateTime.Now - time;
                if (timeToMeeting < TimeSpan.Zero)
                    throw new Exception("Напоминание не может быть изменено. Время напоминания прошло.");
                else
                {
                    meeting.TimerToReminder.Change(timeToMeeting, Timeout.InfiniteTimeSpan);
                    NotifyMessage?.Invoke("Напоминание о встрече изменено");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Остановить таймер напоминания о встрече.
        /// </summary>
        /// <param name="meeting">Встреча.</param>
        public static void StopTimerToReminder(Meeting meeting)
        {
            meeting.TimerToReminder.Change(Timeout.Infinite, Timeout.Infinite);
            NotifyMessage?.Invoke("Напоминания о встрече не будет");
        }

        /// <summary>
        /// Проверить, что время начала встречи не раньше текущего времени.
        /// </summary>
        /// <param name="dateTime">Время начала встречи.</param>
        /// <returns>True, если время начала не раньше текущего времени, false, если наоборот.</returns>
        public static bool IsStartDateTimeCorrect(DateTime dateTime)
        {
            return DateTime.Now.Subtract(dateTime) < TimeSpan.Zero;
        }

        /// <summary>
        /// Проверить, что время окончания встречи не раньше времени начала.
        /// </summary>
        /// <param name="endDateTime">Время окончания встречи.</param>
        /// <param name="startDateTime">Время начала встречи.</param>
        /// <returns>Nrue, если время окончания встречи не раньше времени начала, false, если наоборот.</returns>
        public static bool IsEndDateTimeCorrect(DateTime endDateTime, DateTime startDateTime)
        {
            return endDateTime.Subtract(startDateTime) > TimeSpan.Zero;
        }

        /// <summary>
        /// Проверить, есть ли пересечения времени начала встречи с другими встречами.
        /// </summary>
        /// <param name="dateTime">Время начала встречи.</param>
        /// <returns>true, если время начала имеет пересечения, false, если наоборот.</returns>
        public static bool IsStartDateTimeCrossing(DateTime dateTime)
        {
            if(!meetings.Any())
                return false;
            var selectedDateTimeMeetings = meetings.Where(item => DateOnly.FromDateTime(item.MeetingStartDateTime) == DateOnly.FromDateTime(dateTime)||
                                                        DateOnly.FromDateTime(item.MeetingEndDateTime) == DateOnly.FromDateTime(dateTime));
            
            if (!selectedDateTimeMeetings.Any())
                return false;

            foreach (var meeting in selectedDateTimeMeetings)
            {
                if (meeting.MeetingStartDateTime == dateTime||meeting.MeetingStartDateTime < dateTime && meeting.MeetingEndDateTime > dateTime)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Проверить, есть ли пересечения времени окончания встречи с другими встречами.
        /// </summary>
        /// <param name="endDateTime">Время окончания встречи.</param>
        /// <param name="startDateTime">Время начала встречи.</param>
        /// <returns>True, если встречи пересекаются, false, если наоборот.</returns>
        public static bool IsEndDateTimeCrossing(DateTime endDateTime, DateTime startDateTime)
        {
            var selectedDateTimeMeetings = meetings.Where(item => DateOnly.FromDateTime(item.MeetingStartDateTime) == DateOnly.FromDateTime(endDateTime) ||
                                            DateOnly.FromDateTime(item.MeetingEndDateTime) == DateOnly.FromDateTime(endDateTime));

            if (!selectedDateTimeMeetings.Any())
                return false;

            foreach (var meeting in selectedDateTimeMeetings)
            {
                if (meeting.MeetingStartDateTime > startDateTime && meeting.MeetingEndDateTime > endDateTime)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Проверить, есть ли пересечения нового времени встречи с другими встречами.
        /// </summary>
        /// <param name="meeting">Встреча.</param>
        /// <param name="newStartDateTime">Новое время встречи.</param>
        /// <returns>True, если встречи пересекаются, false, если наоборот.</returns>
        public static bool IsNewStartDateTimeCorrect(Meeting meeting, DateTime newStartDateTime)
        {
            if(meeting.MeetingEndDateTime <= newStartDateTime || meeting.MeetingStartDateTime == newStartDateTime)
                return false;

            foreach (var item in meetings)
            {
                if (item.MeetingEndDateTime <= meeting.MeetingStartDateTime && item.MeetingEndDateTime > newStartDateTime)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Проверить, есть ли пересечения нового времения окончания встречи с другими встречами.
        /// </summary>
        /// <param name="meeting">Встреча.</param>
        /// <param name="newEndDateTime">Новое время окончания.</param>
        /// <returns>True, если встречи пересекаются, false, если наоборот.</returns>
        public static bool IsNewEndDateTimeCorrect(Meeting meeting, DateTime newEndDateTime)
        {
            if(meeting.MeetingStartDateTime >= newEndDateTime || meeting.MeetingEndDateTime == newEndDateTime)
                return false;

            foreach (var item in meetings)
            {
                if (item.MeetingStartDateTime >= meeting.MeetingEndDateTime && item.MeetingStartDateTime > newEndDateTime)
                    return false;
            }

            return true;
        }
    }
}
