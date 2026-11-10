using System;
using System.IO;

namespace Assessment
{
    internal class Program
    {
        // One struct stores the four lines belonging to one contestant.
        public struct Contestant
        {
            public string FirstName;
            public string LastName;
            public string Interest;
            public int Score;
        }

        public static Contestant[] contestants = new Contestant[23];
        public static int contestantCount = 0;
        public static int[] finalists = new int[10];
        public static int playerIndex = -1;
        public static Random rand = new Random();
        public static string filePath = "wheelOfFortune.txt";

        static void Main()
        {

            // Read the original data file before showing the menu.
            if (!File.Exists(filePath))
            {
                filePath = "../../../../../wheelOfFortune.txt";
            }
            reader();
            if (contestantCount == 0)
            {
                Console.WriteLine("No contestants could be loaded. Press Enter to exit.");
                Console.ReadLine();
                return;
            }

            SortContestantsByLastName();
            Menu();
        }

        public static void Menu()
        {
            bool menu = true;
            while (menu)
            {
                char choice;
                Console.WriteLine("Main menu (please select a number)");
                Console.WriteLine("1: Play");
                Console.WriteLine("2: Contestants");
                Console.WriteLine("3: Reset all scores to 0");
                Console.WriteLine("4: Top five scores");
                Console.WriteLine("0: Exit");
                choice = Console.ReadKey().KeyChar;
                Console.Clear();

                switch (choice)
                {
                    case '0':
                        menu = Exit();
                        Console.Clear();
                        break;
                    case '1':
                        Play();
                        Console.Clear();
                        break;
                    case '2':
                        ContestantsMenu();
                        Console.Clear();
                        break;
                    case '3':
                        ResetScores();
                        Console.Clear();
                        break;
                    case '4':
                        TopFive();
                        Pause();
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("You have entered the wrong value.");
                        Pause();
                        Console.Clear();
                        break;
                }
            }
        }

        public static bool Exit()
        {
            Console.WriteLine("Are you sure you want to exit? [Y] / [Any other key to stay]");
            char choice = Console.ReadKey().KeyChar;
            bool meenu;
            if (choice == 'y' || choice == 'Y')
            {
                meenu = false;
            }
            else
            {
                meenu = true;
            }
            return meenu;

        }

        // Loads contestants into an array of structs
        public static void reader()
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Cannot find " + filePath);
                Console.WriteLine("Put wheelOfFortune.txt in the project folder.");
                return;
            }

            StreamReader list = new StreamReader(filePath);
            contestantCount = 0;

            while (!list.EndOfStream && contestantCount < contestants.Length)
            {
                string? first = list.ReadLine();
                string? last = list.ReadLine();
                string? interest = list.ReadLine();
                string? scoreText = list.ReadLine();

                // Stop if the file ends in the middle of someone's details.
                if (first == null || last == null || interest == null || scoreText == null)
                {
                    Console.WriteLine("Warning: Incomplete contestant at the end of the file.");
                    break;
                }

                int score;
                if (!int.TryParse(scoreText, out score))
                {
                    score = 0;
                }

                contestants[contestantCount].FirstName = first;
                contestants[contestantCount].LastName = last;
                contestants[contestantCount].Interest = interest;
                contestants[contestantCount].Score = score;
                contestantCount++;
            }

            list.Close();
        }

        // Writes changes back to the same file
        public static void SaveContestants()
        {
            StreamWriter writer = new StreamWriter(filePath);
            for (int i = 0; i < contestantCount; i++)
            {
                writer.WriteLine(contestants[i].FirstName);
                writer.WriteLine(contestants[i].LastName);
                writer.WriteLine(contestants[i].Interest);
                writer.WriteLine(contestants[i].Score);
            }
            writer.Close();
        }

        // Bubble sort
        public static void SortContestantsByLastName()
        {
            for (int i = 0; i < contestantCount - 1; i++)
            {
                for (int j = 0; j < contestantCount - 1 - i; j++)
                {
                    if (contestants[j].LastName.ToLower().CompareTo(contestants[j + 1].LastName.ToLower()) > 0)
                    {
                        Contestant temporary = contestants[j];
                        contestants[j] = contestants[j + 1];
                        contestants[j + 1] = temporary;
                    }
                }
            }
        }

        public static void ContestantsMenu()
        {
            bool inContestantsMenu = true;
            while (inContestantsMenu)
            {
                Console.WriteLine("Contestants menu");
                Console.WriteLine("1: List all contestants (last-name order)");
                Console.WriteLine("2: Find a contestant and edit their interest");
                Console.WriteLine("3: Edit any contestant field (extra feature)");
                Console.WriteLine("0: Back to main menu");
                char choice = Console.ReadKey().KeyChar;
                Console.Clear();

                switch (choice)
                {
                    case '1':
                        ListContestants();
                        Pause();
                        Console.Clear();
                        break;
                    case '2':
                        EditInterest();
                        Console.Clear();
                        break;
                    case '3':
                        EditDetails();
                        Console.Clear();
                        break;
                    case '0':
                        inContestantsMenu = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        Pause();
                        Console.Clear();
                        break;
                }
            }
        }
        public static void ListContestants()
        {
            Console.WriteLine("ALL CONTESTANTS (SORTED BY LAST NAME)");

            Console.WriteLine(
                "No.".PadRight(6) + "First Name".PadRight(20) +  "Last Name".PadRight(25) + "Interest".PadRight(25) + "Score"
            );

            for (int i = 0; i < contestantCount; i++)
            {
                Console.WriteLine(
                    (i + 1).ToString().PadRight(6) + contestants[i].FirstName.PadRight(20) + contestants[i].LastName.PadRight(25) + contestants[i].Interest.PadRight(25) +"$" + contestants[i].Score);
            }
        }

        public static void EditInterest()
        {
            Console.Write("Enter all or part of the first or last name: ");
            string? search = Console.ReadLine();
            if (search == null || search.Trim() == "")
            {
                Console.WriteLine("You must enter a name.");
                Pause();
                return;
            }

            int matchCount = 0;

            Console.WriteLine("No.".PadRight(6) +"First Name".PadRight(20) +"Last Name" );

            for (int i = 0; i < contestantCount; i++)
            {
                if (contestants[i].FirstName.ToLower().Contains(search.ToLower()) || contestants[i].LastName.ToLower().Contains(search.ToLower()))
                {
                    Console.WriteLine(
                        (i + 1).ToString().PadRight(6) + contestants[i].FirstName.PadRight(20) + contestants[i].LastName );

                    matchCount++;
                }
            }
            if (matchCount == 0)
            {
                Console.WriteLine("No matching contestants.");
                Pause();
                return;
            }

            Console.Write("Enter the number of the contestant to edit: ");
            string? numberText = Console.ReadLine();
            int number;
            if (!int.TryParse(numberText, out number) || number < 1 || number > contestantCount)
            {
                Console.WriteLine("Invalid contestant number.");
                Pause();
                return;
            }

            int index = number - 1;
            if (!contestants[index].FirstName.ToLower().Contains(search.ToLower()) && !contestants[index].LastName.ToLower().Contains(search.ToLower()))
            {
                Console.WriteLine("That contestant was not in your search results.");
                Pause();
                return;
            }

            Console.WriteLine("Enter the new interest: ");
            string? newInterest = Console.ReadLine();
            if (newInterest == null || newInterest.Trim() == "")
            {
                Console.WriteLine("Interest cannot be blank.");
                Pause();
                return;
            }

            contestants[index].Interest = newInterest;
            SaveContestants();
            Console.WriteLine("Interest updated and saved to file.");
            Pause();
        }

        public static void EditDetails()
        {
            ListContestants();
            Console.Write("Enter the contestant number to edit (1 to " + contestantCount + "): ");
            int number;
            if (!int.TryParse(Console.ReadLine(), out number) ||
                number < 1 || number > contestantCount)
            {
                Console.WriteLine("Invalid contestant number.");
                Pause();
                return;
            }

            int index = number - 1;
            Console.WriteLine("Editing " + contestants[index].FirstName + " " + contestants[index].LastName);
            Console.WriteLine("1: First name");
            Console.WriteLine("2: Last name");
            Console.WriteLine("3: Interest");
            Console.WriteLine("4: Score");
            Console.WriteLine("0: Cancel");
            char field = Console.ReadKey().KeyChar;
            Console.WriteLine();
            if (field == '0')
            {
                return;
            }
            if (field < '1' || field > '4')
            {
                Console.WriteLine("Invalid field.");
                Pause();
                return;
            }

            Console.Write("Enter the new value: ");
            string? newValue = Console.ReadLine();
            if (newValue == null || newValue.Trim() == "")
            {
                Console.WriteLine("The value cannot be empty.");
                Pause();
                return;
            }

            if (field == '1')
            {
                contestants[index].FirstName = newValue;
            }
            else if (field == '2')
            {
                contestants[index].LastName = newValue;
            }
            else if (field == '3')
            {
                contestants[index].Interest = newValue;
            }
            else if (field == '4')
            {
                int score;
                if (!int.TryParse(newValue, out score))
                {
                    Console.WriteLine("Score must be a whole number.");
                    Pause();
                    return;
                }
                contestants[index].Score = score;
            }

            SortContestantsByLastName();
            SaveContestants();
            Console.WriteLine("Change saved to wheelOfFortune.txt.");
            Pause();
        }

        public static void Play()
        {
            if (contestantCount < 10)
            {
                Console.WriteLine("At least 10 contestants are needed to play.");
                Pause();
                return;
            }

            GenerateFinalists();
            Pause();
            Console.Clear();
            SelectPlayer();
            Pause();
            Console.Clear();
            PlayGame();
            SaveContestants();
            TopFive();
            Pause();
        }

        // Draw 10 unique numbers from 1 to contestantCount.
        public static void GenerateFinalists()
        {
            Console.WriteLine("TEN RANDOM FINALISTS");
            Console.WriteLine("Draw".PadRight(8) +"No.".PadRight(8) +"First Name".PadRight(20) +"Last Name".PadLeft(25));
            for (int i = 0; i < finalists.Length; i++)
            {
                int number;
                bool duplicate;
                do
                {
                    number = rand.Next(1, contestantCount + 1);
                    duplicate = false;
                    for (int j = 0; j < i; j++)
                    {
                        if (finalists[j] == number - 1)
                        {
                            duplicate = true;
                        }
                    }
                }
                while (duplicate);

                // The random draw uses 1..count; array positions use 0..count-1.
                finalists[i] = number - 1;
                Console.WriteLine((i + 1).ToString().PadRight(8) + number.ToString().PadRight(8) + contestants[finalists[i]].FirstName.PadRight(20) + contestants[finalists[i]].LastName.PadLeft(25));
            }
        }

        public static void SelectPlayer()
        {
            int finalistNumber = rand.Next(0, finalists.Length);
            playerIndex = finalists[finalistNumber];
            Console.WriteLine("THE SELECTED PLAYER IS:");
            Console.WriteLine(contestants[playerIndex].FirstName + " " + contestants[playerIndex].LastName);
            Thread.Sleep(500);
        }

        // A 24-segment wheel: 22 cash segments and 2 loss segments.
        public static int SpinWheel()
        {
            int Spin = 0;
            int[] wheel = { 300, 400, 500, 600, 700, 800, 900, 1000,
                            1100, 1200, 1300, 1400, 1500, 1600, 1700,
                            1800, 1900, 2000, 2500, 3000, 4000, 5000,
                            -2000, -3000 };
            for (int i = 0; i < 6; i++)
            {               
                Spin = wheel[rand.Next(wheel.Length)];
                Console.WriteLine("Wheel spin: " + Spin);
                Thread.Sleep(300);
                Console.Clear();
                Console.WriteLine("Wheel spin: " + "");
                Thread.Sleep(200);
                Console.Clear();
            }
            return Spin;
        }

        public static bool IsVowel(char letter)
        {
            return letter == 'A' || letter == 'E' || letter == 'I' || letter == 'O' || letter == 'U';
        }

        // Keep asking until a valid, previously unused letter is entered.
        public static char AskForLetter(string guessedLetters)
        {
            while (true)
            {
                Console.Write("Guess ONE letter: ");
                string? input = Console.ReadLine();
                if (input == null || input.Length != 1)
                {
                    Console.WriteLine("Please enter exactly one letter.");
                    continue;
                }

                char letter = char.ToUpper(input[0]);
                if (letter < 'A' || letter > 'Z')
                {
                    Console.WriteLine("Please enter a letter from A to Z.");
                    continue;
                }
                if (guessedLetters.IndexOf(letter) >= 0)
                {
                    Console.WriteLine("You already guessed that letter.");
                    continue;
                }
                return letter;
            }
        }

        public static void PlayGame()
        {
            // Each single word has at least ten characters
            string[] words = { "CYBERSECURITY", "PROGRAMMING", "NETWORKING",
                               "TECHNOLOGY", "INFORMATION", "DEVELOPMENT",
                               "MOTHERBOARD", "AUTHENTICATION", "ENCRYPTION",
                               "APPLICATION" };
            string word = words[rand.Next(0, words.Length)];
            char[] visible = new char[word.Length];
            string guessedLetters = "";
            int score = 0;

            for (int i = 0; i < word.Length; i++)
            {
                visible[i] = '_';
            }

            bool finished = false;
            while (!finished)
            {
                Console.Clear();
                Console.WriteLine("WHEEL OF FORTUNE");
                Console.WriteLine("Player: " + contestants[playerIndex].FirstName + " " +contestants[playerIndex].LastName);
                Console.WriteLine("Game score: $" + score);
                Console.WriteLine("Letters guessed: " + guessedLetters);
                Console.WriteLine();
                for (int i = 0; i < visible.Length; i++)
                {
                    Console.Write(visible[i] + " ");
                }
                Console.WriteLine("\n");

                int spin = SpinWheel();
                if (spin < 0)
                {
                    score = score + spin;
                    Console.WriteLine("Wheel landed on LOSE $" + (-spin) + "!");
                    Console.WriteLine("Your score is now $" + score);
                    Pause();
                    continue;
                }

                Console.WriteLine("WHEEL OF FORTUNE");
                Console.WriteLine("Player: " + contestants[playerIndex].FirstName + " " + contestants[playerIndex].LastName);
                Console.WriteLine("Game score: $" + score);
                Console.WriteLine("Letters guessed: " + guessedLetters);
                Console.WriteLine();
                for (int i = 0; i < visible.Length; i++)
                {
                    Console.Write(visible[i] + " ");
                }
                Console.WriteLine("\n");
                Console.WriteLine("Wheel landed on $" + spin);
                char letter = AskForLetter(guessedLetters);
                guessedLetters = guessedLetters + letter;

                int matches = 0;
                for (int i = 0; i < word.Length; i++)
                {
                    if (word[i] == letter)
                    {
                        visible[i] = letter;
                        matches++;
                    }
                }

                if (IsVowel(letter))
                {
                    score = score - 250;
                    Console.WriteLine("Vowel cost: $250 (charged once for this guess).");
                }
                else
                {
                    score = score + spin * matches;
                    Console.WriteLine("Consonant appears " + matches + " time(s).");
                    Console.WriteLine("You earned $" + (spin * matches));
                }

                if (matches == 0)
                {
                    Console.WriteLine("That letter is not in the phrase.");
                }

                finished = true;
                for (int i = 0; i < visible.Length; i++)
                {
                    if (visible[i] == '_')
                    {
                        finished = false;
                    }
                }

                if (!finished)
                {
                    Pause();
                }
            }

            Console.Clear();
            Console.WriteLine("PUZZLE COMPLETED: " + word);
            Console.WriteLine("Final game score: $" + score);

            // Each contestant's score stores their most recent completed game.
            contestants[playerIndex].Score = score;
        }

        // Sort indexes rather than moving contestants out of last-name order.
        public static void TopFive()
        {
            int[] positions = new int[contestantCount];
            for (int i = 0; i < contestantCount; i++)
            {
                positions[i] = i;
            }

            for (int i = 0; i < contestantCount - 1; i++)
            {
                for (int j = 0; j < contestantCount - 1 - i; j++)
                {
                    if (contestants[positions[j]].Score < contestants[positions[j + 1]].Score)
                    {
                        int temporary = positions[j];
                        positions[j] = positions[j + 1];
                        positions[j + 1] = temporary;
                    }
                }
            }

            Console.WriteLine("TOP FIVE SCORES");
            Console.WriteLine("Rank".PadRight(8) +"First Name".PadRight(20) +"Last Name".PadRight(25) +"Score");
            int numberToShow = contestantCount;
            if (numberToShow > 5)
            {
                numberToShow = 5;
            }
            for (int i = 0; i < numberToShow; i++)
            {
                int index = positions[i];
                Console.WriteLine((i + 1).ToString().PadRight(8) + contestants[index].FirstName.PadRight(20) + contestants[index].LastName.PadRight(25) + "$" + contestants[index].Score
                );
            }
        }

        public static void ResetScores()
        {
            Console.WriteLine("Reset ALL scores to 0? [Y] / [Any other key to cancel]");
            char choice = Console.ReadKey().KeyChar;
            Console.WriteLine();
            if (choice == 'y' || choice == 'Y')
            {
                for (int i = 0; i < contestantCount; i++)
                {
                    contestants[i].Score = 0;
                }
                SaveContestants();
                Console.Clear();
                Console.WriteLine("All scores reset and saved.");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Reset cancelled.");
            }
            Pause();
        }

        public static void Pause()
        {
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}
