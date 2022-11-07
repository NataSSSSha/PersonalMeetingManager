
namespace PersonalMeetingManager
{
    /// <summary>
    /// Консольное меню.
    /// </summary>
    internal static class ConsoleMenu
    {
        /// <summary>
        /// Вывести сообщение.
        /// </summary>
        /// <param name="message"></param>
        public static void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }
        /// <summary>
        /// Вывести в консоль стартовое сообщение.
        /// </summary>
        public static void StartMessage()
        {
            Console.WriteLine("Привет! Эта программа поможет тебе распланировать твое время!\n" +
                "В ней ты можешь создавать встречи, изменять или удалять их, а также добавлять напоминания о встречах.\n" +
                "Давай начнем!\n");
        }

        /// <summary>
        /// Вывести в консоль меню.
        /// </summary>
        public static void ShowMenu()
        {
            Console.WriteLine("Меню:\n" +
                "1 - добавить встречу\n" +
                "2 - изменить/удалить встречу\n" +
                "3 - посмотреть встречи на определенный день\n" +
                "4 - завершить программу.\n");
        }

        /// <summary>
        /// Начать программу.
        /// </summary>
        public static void ManageMeetings()
        {
            MeetingManager.NotifyMessage += message => Console.WriteLine(message);

            StartMessage();

            int numberOfMenu;

            const string DefaultNameOfMeeting = "Секретная встреча";

            do
            {
                ShowMenu();

                bool result = int.TryParse(Console.ReadLine(), out numberOfMenu);

                if(result)
                    switch (numberOfMenu)
                    {
                        case 1:
                            {
                                Console.WriteLine("Ок, давай создадим встречу!");
                                try
                                {
                                    Console.WriteLine("Введи дату и время начала будущей встречи (Например, 01.01.2001 01:00:00)");
                                    DateTime startDateTime = DateTime.Parse(Console.ReadLine());

                                    if(!MeetingManager.IsStartDateTimeCorrect(startDateTime))
                                        throw new Exception("Дата начала события не может быть раньше текущей");
                                    if(MeetingManager.IsStartDateTimeCrossing(startDateTime))
                                        throw new Exception("Дата начала события имеет пересечения с другими событиями");
 
                                    Console.WriteLine("Введи название встречи (Например, Встреча с Марией)");
                                    string nameOfMeeting = Console.ReadLine() ?? DefaultNameOfMeeting;

                                    Console.WriteLine("Введи дату и время примерного окончания будущей встречи (Например, 01.01.2001 02:00:00)");
                                    DateTime endDateTime = DateTime.Parse(Console.ReadLine());

                                    if (!MeetingManager.IsEndDateTimeCorrect(endDateTime, startDateTime))
                                        throw new Exception("Дата окончания события не может быть раньше его начала");          
                                    
                                    if (MeetingManager.IsEndDateTimeCrossing(endDateTime, startDateTime))
                                        throw new Exception("Дата начала события имеет пересечения с другими событиями");

                                    Meeting meeting = (MeetingManager.CreateMeeting(startDateTime, endDateTime, nameOfMeeting));
                                    MeetingManager.AddMeeting(meeting);

                                    Console.WriteLine("Добавить напоминание о встрече?\n" +
                                                      "1 - да, за 15 мин\n" +
                                                      "2 - да, за 1 час\n" +
                                                      "3 - да, за день\n" +
                                                      "4 - нет\n");

                                    int numberOfReminder;

                                    bool resultOfChangeMeeting = int.TryParse(Console.ReadLine(), out numberOfReminder);

                                    if (resultOfChangeMeeting)
                                        switch (numberOfReminder)
                                    {
                                        case 1:
                                        {
                                            TimeSpan timeToRemind = TimeSpan.Parse("0.00:15:00");
                                            MeetingManager.StartTimerToReminder(meeting, timeToRemind);
                                            break;
                                        }

                                        case 2:
                                        {
                                            TimeSpan timeToRemind = TimeSpan.Parse("0.01:00:00");
                                            MeetingManager.StartTimerToReminder(meeting, timeToRemind);
                                            break;
                                        }

                                        case 3:
                                        {
                                            TimeSpan timeToRemind = TimeSpan.Parse("1.00:00:00");
                                            MeetingManager.StartTimerToReminder(meeting, timeToRemind);
                                            break;
                                        }

                                        case 4:
                                        {
                                            Console.WriteLine("Хорошо, напоминания не будет");
                                            break;
                                        }
                                        default:
                                        {
                                            Console.WriteLine("Неверный ввод");
                                            break;
                                        }
                            }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                break;
                            }

                        case 2:
                            {
                                if (!MeetingManager.IsListOfMeetingsNotEmpty())
                                {
                                    Console.WriteLine("Кажется, встреч пока нет, нажми 1 для создания встречи");
                                    break;
                                }

                                Console.WriteLine("Сначала определим, какую встречу ты хочешь изменить или удалить.");
                                try
                                {
                                    Console.WriteLine("Введи дату и время начала существующей встречи (Например, 01.01.2001 01:00:00)");
                                    DateTime startDateTime = DateTime.Parse(Console.ReadLine());

                                    Meeting meetingForChange = MeetingManager.FindMeetingByDateTime(startDateTime);

                                    if (meetingForChange == null)
                                    {
                                        Console.WriteLine("Кажется, такой встречи пока нет, нажми 1 для создания встречи");
                                        break;
                                    }

                                    Console.WriteLine("Что в этой встрече ты хотел бы изменить?\n" +
                                                      "1 - начало встречи\n" +
                                                      "2 - окончание встречи\n" +
                                                      "3 - название встречи\n" +
                                                      "4 - изменить время напоминания о встрече\n" +
                                                      "5 - удалить встречу\n" +
                                                      "6 - передумал менять встречу\n");

                                    int numberOfMenuForChangeEvent;

                                    bool resultOfChangeEvent = int.TryParse(Console.ReadLine(), out numberOfMenuForChangeEvent);

                                    if (resultOfChangeEvent)
                                        switch (numberOfMenuForChangeEvent)
                                        {
                                            case 1:
                                                {
                                                    Console.WriteLine("Введи дату и время нового начала встречи (Например, 01.01.2001 01:00:00");
                                                    DateTime newStartDateTime = DateTime.Parse(Console.ReadLine());

                                                    if (MeetingManager.IsNewStartDateTimeCorrect(meetingForChange, newStartDateTime))
                                                        throw new Exception("Некорректная дата начала");
                                                    else
                                                        MeetingManager.ChangeMeetingStart(meetingForChange, newStartDateTime);

                                                    break;
                                                }

                                            case 2:
                                                {
                                                    Console.WriteLine("Введи дату и время нового окончания встречи (Например, 01.01.2001 01:00:00");
                                                    DateTime newEndDateTime = DateTime.Parse(Console.ReadLine());

                                                    if (MeetingManager.IsNewEndDateTimeCorrect(meetingForChange, newEndDateTime))
                                                        throw new Exception("Некорректная дата окончания");
                                                    else

                                                        MeetingManager.ChangeMeetingEnd(meetingForChange, newEndDateTime);

                                                    break;
                                                }

                                            case 3:
                                                {
                                                    Console.WriteLine("Введи новое название встречи (Например, Встреча с Пашей");
                                                    string newNameOfMeeting = Console.ReadLine() ?? DefaultNameOfMeeting;

                                                    MeetingManager.ChangeNameOfMeeting(meetingForChange, newNameOfMeeting);

                                                    break;
                                                }

                                            case 4:
                                                {
                                                    Console.WriteLine("Когда напомнить о встрече?\n" +
                                                                      "1 - за 15 мин\n" +
                                                                      "2 - за 1 час\n" +
                                                                      "3 - за день\n" +
                                                                      "4 - не надо напоминать\n");

                                                    int numberOfReminder;

                                                    bool resultOfChangeReminder = int.TryParse(Console.ReadLine(), out numberOfReminder);

                                                    if (resultOfChangeReminder)
                                                        switch (numberOfReminder)
                                                        {
                                                            case 1:
                                                                {
                                                                    TimeSpan timeToRemind = TimeSpan.Parse("0.00:15:00");
                                                                    MeetingManager.StartTimerToReminder(meetingForChange, timeToRemind);
                                                                    break;
                                                                }

                                                            case 2:
                                                                {
                                                                    TimeSpan timeToRemind = TimeSpan.Parse("0.01:00:00");
                                                                    MeetingManager.StartTimerToReminder(meetingForChange, timeToRemind);
                                                                    break;
                                                                }
                                                            case 3:
                                                                {
                                                                    TimeSpan timeToRemind = TimeSpan.Parse("1.00:00:00");
                                                                    MeetingManager.StartTimerToReminder(meetingForChange, timeToRemind);
                                                                    break;
                                                                }
                                                            case 4:
                                                                {
                                                                    Console.WriteLine("Хорошо, напоминания не будет");
                                                                    break;
                                                                }
                                                            default:
                                                                {
                                                                    Console.WriteLine("Неверный ввод");
                                                                    break;
                                                                }

                                                        }

                                                    break;
                                                }


                                            case 5:
                                                {
                                                    MeetingManager.RemoveMeeting(meetingForChange);
                                                    break;
                                                }

                                            case 6:
                                                {
                                                    Console.WriteLine("Хорошо, ничего не меняем");
                                                    break;
                                                }

                                            default:
                                                {
                                                    Console.WriteLine("Неверный ввод");
                                                    break;
                                                }
                                        }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }

                                break;
                            }

                        case 3:
                            {
                                if (!MeetingManager.IsListOfMeetingsNotEmpty())
                                {
                                    Console.WriteLine("Кажется, встреч пока нет, нажми 1 для создания встречи");
                                    break;
                                }

                                Console.WriteLine("Введи дату, чтобы посмотреть все встречи на этот день (Например, 01.01.2001)");
                                try
                                {
                                    DateOnly? date = DateOnly.Parse(Console.ReadLine());
                                    List<Meeting> meetingsForTheDay = MeetingManager.FindMeetingsByDate((DateOnly)date);

                                    foreach (Meeting meeting in meetingsForTheDay)
                                        Console.WriteLine($"{meeting.MeetingStartDateTime} - {meeting.MeetingEndDateTime}: {meeting.NameOfMeeting}");

                                    Console.WriteLine("Если хочешь сохранить данные в файл, нажми 1, если нет, любую клавишу");
                                    string? choiseToSaveToFile = Console.ReadLine();
                                    if (choiseToSaveToFile == "1")
                                        MeetingManager.SaveMeetingsToFile(meetingsForTheDay, (DateOnly)date);
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("Неверный формат даты");
                                }

                                break;
                            }

                        case 4:
                            {
                                Console.WriteLine("Программа завершена");
                                break;
                            }

                        default:
                            {
                                Console.WriteLine("Неверный ввод");
                                break;
                            }
                    }
            }
            while (numberOfMenu != 4);
        }
    }
}
