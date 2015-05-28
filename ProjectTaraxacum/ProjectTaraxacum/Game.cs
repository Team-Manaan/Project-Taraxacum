namespace ProjectTaraxacum
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Threading;
    using System.IO;
    
    class Game
    {
        static void Main(string[] args)
        {
            Console.BufferHeight = Console.WindowHeight = 20;
            Console.BufferWidth = Console.WindowWidth = 30;
            int playfieldWidht = 20;
            bool intro = false;
            Intro(intro);
            MenuLoad(intro);
            intro = true;
            GameObject spermIntro = new GameObject(3, Console.WindowHeight - 8, '?', ConsoleColor.White);
            GameObject spermIntroLocation = new GameObject(); //where is the cursor

            while (true) //Intro
            {
                Console.BufferHeight = Console.WindowHeight = 20;
                Console.BufferWidth = Console.WindowWidth = 30;
                Intro(intro);
                MenuLoad(intro);


                if (Console.KeyAvailable)
                {
                    //Move the character
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                    while (Console.KeyAvailable) Console.ReadKey(true);
                    if (pressedKey.Key == ConsoleKey.UpArrow)
                    {
                        if (spermIntro.y - 1 >= 12)
                        {
                            spermIntro.y = spermIntro.y - 1;

                        }
                    }

                    else if (pressedKey.Key == ConsoleKey.DownArrow)
                    {
                        if (spermIntro.y + 1 < playfieldWidht - 5)
                        {
                            spermIntro.y = spermIntro.y + 1;
                        }
                    }

                    else if (pressedKey.Key == ConsoleKey.Enter && spermIntro.y == 13)
                    {
                        Console.Clear();
                        Console.BufferHeight = Console.WindowHeight = 25;
                        Console.BufferWidth = Console.WindowWidth = 41;
                        PrintStringPosition(0, 0, "-You are a spermatozoid.", ConsoleColor.Gray);
                        PrintStringPosition(0, 2, "-Your goal is to reach the egg", ConsoleColor.Gray);
                        PrintStringPosition(0, 3, "..Or die trying", ConsoleColor.Gray);
                        PrintStringPosition(0, 5, "-Run from the falling objects.", ConsoleColor.Gray);
                        PrintStringPosition(0, 7, "-Collect Genetic points (G) to ", ConsoleColor.Gray);
                        PrintStringPosition(0, 8, "ensure your baby will be great", ConsoleColor.Gray);
                        PrintStringPosition(0, 10, "-Collect lives (*) so that you wont die", ConsoleColor.Gray);
                        PrintStringPosition(0, 12, "Collect minus signs to keep the proper ", ConsoleColor.Gray);
                        PrintStringPosition(0, 13, "tempreture (37-40)", ConsoleColor.Gray);
                        PrintStringPosition(0, 14, "To pass a level you should have the right", ConsoleColor.Gray);
                        PrintStringPosition(0, 15, "tempreture and enough genetic points", ConsoleColor.Gray);
                        PrintStringPosition(0, 17, "If the temperture is too high ", ConsoleColor.Gray);
                        PrintStringPosition(0, 18, "The genetics points will be reseted", ConsoleColor.Gray);

                        PrintStringPosition(0, 21, "Press enter to go back to the menu", ConsoleColor.Red);

                        Console.ReadLine();
                    }

                    else if (pressedKey.Key == ConsoleKey.Enter)
                    {
                        spermIntroLocation = spermIntro;
                        break;
                    }

                }


                PrintPosition(spermIntro.x, spermIntro.y, spermIntro.c, spermIntro.color);





                Thread.Sleep(200);
                Console.Clear();
            }


            if (spermIntroLocation.y == 12)
            {


                ////Resizing the console
                Console.BufferHeight = Console.WindowHeight = 35;
                Console.BufferWidth = Console.WindowWidth = 30;

                double speed = 50.0;
                double defSpeed = 50.0;
                int livesCount = 5;
                int geneticPoints = 0;
                int level = 0;
                double temperature = 37;// temperature for passing the level
                double minTemp = 36;    // the temperature must be betwen 37-42
                double maxTemp = 42;
                double initialLevelScore = 0; //In this variable save the genetic score when the temperature fall in range!
                bool isTempInRange = true; // Remember if the temeprature is betwen 37-42!
                int scoreToLevelUp = 300;  //Score needed to pass the level during the time when the temperature was in range!
                int newObstacleChance = 9; //Generic obsticle with chance 1/8;

                //Scorint system
                Stopwatch stopwatch = new Stopwatch();
                double score = 0;
                int distance = 1;
                int increment = 0;

                //Creating the character
                GameObject sperm = new GameObject(10, Console.WindowHeight - 6, 'o', ConsoleColor.White);

                //Random generator for falling objects
                Random randomGenerator = new Random();
                List<GameObject> obstacles = new List<GameObject>();


                Random bonusChance = new Random();


                List<GameObject> wallOne = new List<GameObject>();
                for (int i = 0; i < 30; i++)
                {
                    GameObject temp = new GameObject(8, i, '/', ConsoleColor.Cyan);

                    if (i % 2 == 0)
                    {
                        temp.c = '\\';
                    }

                    wallOne.Add(temp);
                }

                List<GameObject> wallTwo = new List<GameObject>();
                for (int i = 0; i < 30; i++)
                {
                    GameObject temp = new GameObject(21, i, '\\', ConsoleColor.Cyan);

                    if (i % 2 == 0)
                    {
                        temp.c = '/';
                    }

                    wallTwo.Add(temp);
                }



                while (true)
                {

                    if (isTempInRange && geneticPoints >= initialLevelScore + scoreToLevelUp) // Check if the temperature is in range and genetic points
                    {                                                                         // are enought to pass the level! 
                        level++;
                        if (level == 7)
                        {
                            PrintGameWonMessage(geneticPoints);// Gamer Won print!
                            scoreToLevelUp = 300;              // Reset the value of all variables!
                            geneticPoints = 0;
                            level = 0;
                            initialLevelScore = geneticPoints;
                            obstacles.Clear();
                            temperature = 37;
                            newObstacleChance = 9;
                            //livesCount = 5;
                        }
                        else
                        {
                            PrintNextLevelMessage(level, geneticPoints); //Passed level!
                            scoreToLevelUp += 100;                       // Set the values for the next level! Every next level +100 g.p.
                            initialLevelScore = geneticPoints;
                            obstacles.Clear();
                            temperature = 37;
                            newObstacleChance = 9;
                        }
                    }
                    //Creating the obstecles
                    if (bonusChance.Next(0, newObstacleChance) <= 1)
                    {
                        GameObject newObstacle = new GameObject(randomGenerator.Next(10, playfieldWidht), 0, 'O', ConsoleColor.Red);
                        obstacles.Add(newObstacle);
                    }

                    //create bonuses
                    if (bonusChance.Next(0, 101) <= 1)
                    {
                        int bonusX = randomGenerator.Next(10, playfieldWidht);  // generate new bonus X coordinate
                        while (obstacles.Any(o => o.x == bonusX && o.y == 0))   // check if there is obstacle on X for the new bonus
                        {
                            bonusX = randomGenerator.Next(10, playfieldWidht);  // if obstacle exists on bonusX, generate new X
                        }

                        GameObject bonus = new GameObject(bonusX, 0, 'G', ConsoleColor.Yellow);
                        obstacles.Add(bonus);
                    }
                    if (bonusChance.Next(0, 1000) <= 1)
                    {
                        int bonusLifeX = randomGenerator.Next(10, playfieldWidht);  // generate new bonusLife X coordinate
                        while (obstacles.Any(o => o.x == bonusLifeX && o.y == 0))   // check if there is obstacle on X for the new bonusLife
                        {
                            bonusLifeX = randomGenerator.Next(10, playfieldWidht);  // if obstacle exists on bonusLifeX, generate new X
                        }

                        GameObject bonusLife = new GameObject(bonusLifeX, 0, '*', ConsoleColor.Blue);
                        obstacles.Add(bonusLife);
                    }
                    if (bonusChance.Next(0, 31) <= 1)
                    {
                        int tempReduceX = randomGenerator.Next(10, playfieldWidht); //generate new tempReduceX coordinate
                        while (obstacles.Any(o => o.x == tempReduceX && o.y == 0))
                        {
                            tempReduceX = randomGenerator.Next(10, playfieldWidht);

                        }
                        GameObject tempReduce = new GameObject(tempReduceX, 0, '-', ConsoleColor.Green);
                        obstacles.Add(tempReduce);
                    }

                    //controls the ammount of rocks




                    bool isHit = false;


                    if (Console.KeyAvailable)
                    {
                        //Move the character
                        ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                        while (Console.KeyAvailable) Console.ReadKey(true);
                        if (pressedKey.Key == ConsoleKey.LeftArrow)
                        {
                            if (sperm.x - 1 >= 10)
                            {
                                sperm.x = sperm.x - 1;

                            }
                        }

                        else if (pressedKey.Key == ConsoleKey.RightArrow)
                        {
                            if (sperm.x + 1 < playfieldWidht)
                            {
                                sperm.x = sperm.x + 1;
                            }
                        }
                    }

                    List<GameObject> newList = new List<GameObject>();

                    //Move the falling obstecles
                    for (int i = 0; i < obstacles.Count; i++)
                    {
                        GameObject oldObstacle = obstacles[i];
                        GameObject newObstacleB = new GameObject(oldObstacle.x, oldObstacle.y + 1, oldObstacle.c, oldObstacle.color);
                        
                        //If sperm is hitted substract life
                        if (newObstacleB.y == sperm.y && newObstacleB.x == sperm.x)
                        {

                            //Calculate distance
                            distance++;
                            if (distance >= Console.BufferHeight)
                            {
                                distance = 0;
                            }

                            if (newObstacleB.c == 'G')
                            {
                                geneticPoints += 50;
                                increment += 50;
                            }
                            else if (newObstacleB.c == '*')
                            {
                                livesCount++;
                            }
                            else if (newObstacleB.c == '-') //If the player catches '-' , reduce the temperature but not below minTemp
                            {
                                if (temperature - 1 < minTemp)
                                {
                                    temperature = minTemp;
                                }
                                else
                                {
                                    temperature--;
                                }

                            }
                            else
                            {
                                livesCount--;
                                isHit = true;
                            }

                            //If live count is bellow zero end the game
                            if (livesCount <= 0)
                            {

                                PrintStringPosition(0, 32, "Sperms: " + livesCount, ConsoleColor.White);
                                PrintStringPosition(8, 7, "GAME OVER", ConsoleColor.Red);
                                Console.WriteLine();
                                PrintHighScores(geneticPoints);
                                Console.ReadLine();
                                return;

                            }
                        }
                        //Make the rocks disappear at a certain point
                        if (newObstacleB.y < Console.WindowHeight - 5)
                        {
                            newList.Add(newObstacleB);
                        }
                    }

                    obstacles = newList;

                    //Print obstacles
                    foreach (var obstacle in obstacles)
                    {
                        PrintPosition(obstacle.x, obstacle.y, obstacle.c, obstacle.color);
                    }


                    //Print spermm

                    if (isHit) // if hitted print the sperm as an X.
                    {
                        obstacles.Clear();
                        speed = defSpeed;
                        temperature = 37;
                        newObstacleChance = 9;
                        PrintPosition(sperm.x, sperm.y, 'X', ConsoleColor.Red);
                    }
                    else //If hitted print as X
                    {
                        PrintPosition(sperm.x, sperm.y, sperm.c, sperm.color);
                        PrintPosition(sperm.x, sperm.y + 1, '|', sperm.color);
                    }

                    PrintStringPosition(0, 32, "Level: " + level, ConsoleColor.White);
                    PrintStringPosition(13, 32, "Sperms: " + livesCount, ConsoleColor.White);
                    PrintStringPosition(0, 33, string.Format("Temp: {0:f0}", temperature), ConsoleColor.White); //Prrint the temperature
                    if (isTempInRange)
                    {
                        PrintStringPosition(13, 33, "Level up in: " + (initialLevelScore + scoreToLevelUp - geneticPoints), ConsoleColor.White);
                    }
                    else
                    {
                        PrintStringPosition(13, 33, "Level up in: " + scoreToLevelUp, ConsoleColor.White);  //Score to Level Up
                    }
                    PrintStringPosition(0, 34, "Genetic Points: " + geneticPoints, ConsoleColor.White);

                    //moving wall one
                    for (int i = 0; i < wallOne.Count; i++)
                    {
                        GameObject oldWall = wallOne[i];
                        if (oldWall.c == '/')
                        {
                            oldWall.c = '\\';
                        }
                        else
                        {
                            oldWall.c = '/';
                        }
                    }

                    foreach (var brick in wallOne)
                    {
                        PrintPosition(brick.x, brick.y, brick.c, brick.color);
                    }

                    //moving wall two
                    for (int i = 0; i < wallTwo.Count; i++)
                    {
                        GameObject oldWall = wallTwo[i];
                        if (oldWall.c == '/')
                        {
                            oldWall.c = '\\';
                        }
                        else
                        {
                            oldWall.c = '/';
                        }
                    }

                    foreach (var brick in wallTwo)
                    {
                        PrintPosition(brick.x, brick.y, brick.c, brick.color);
                    }


                    Thread.Sleep((int)(200 - speed));

                    score = (double)stopwatch.ElapsedMilliseconds * distance / 10000 * speed + 1;
                    temperature += score / 33;
                    increment += (int)score;
                    geneticPoints += (int)score;                     //Chek if the the temperature is in range 
                    if (temperature < 37 || temperature > maxTemp)
                    {
                        isTempInRange = false;
                    }
                    if (!isTempInRange && temperature >= 37 && temperature <= maxTemp)
                    {
                        isTempInRange = true;
                        initialLevelScore = geneticPoints;
                    }
                    //Control speed
                    if (speed <= 100)
                    {
                        if (increment >= 50)
                        {
                            speed += 2;
                            increment = 0;
                        }
                        if (level > 4)                          // Spawn  new obstacles more often!
                        {
                            newObstacleChance = 7;
                        }
                        else if (level > 2)
                        {
                            newObstacleChance = 8;
                        }
                    }

                    Console.Clear();
                }
            }




            else if (spermIntroLocation.y == 14)
            {

                Console.Clear();
                PrintStringPosition(0, 4, "    You chose an abortion.\n    Thats too bad :(\n\n", ConsoleColor.Gray);
                Environment.Exit(0);
            }



        }


        static void Intro(bool intro)   // Void method about "Text intro"
        {
            
            string[] text =  
                {
                    "Team", "\"Manaan\" ",
                    "Proudly", "Presents",
                    "\n",
                "Project" ,"T","a","r","a","x","a","CUM"
                };

            if (intro == false)
            {
                for (int i = 0; i < text.Length; i++)
                {




                    PrintStringPosition(5, 4, text[i], ConsoleColor.White); i++;
                    Console.Beep(622, 354); //D#
                    PrintStringPosition(10, 4, text[i], ConsoleColor.White); i++;
                    Thread.Sleep(300);
                    Console.Beep(932, 328); // A#
                    PrintStringPosition(19, 4, text[i], ConsoleColor.White); i++;
                    Thread.Sleep(250);


                    Console.Beep(932, 320); //A#
                    PrintStringPosition(7, 5, text[i], ConsoleColor.White); i++;
                    Thread.Sleep(50);
                    Console.Beep(830, 320);//G#
                    PrintStringPosition(16, 5, text[i], ConsoleColor.White); i++;
                    Thread.Sleep(50);
                    Console.Beep(932, 320);//A#
                    PrintStringPosition(16, 5, text[i], ConsoleColor.White); i++;

                    Thread.Sleep(200);

                    PrintStringPosition(9, 7, text[i], ConsoleColor.White); i++;
                    Thread.Sleep(140);
                    PrintStringPosition(10, 7, text[i], ConsoleColor.White); i++;
                    Console.Beep(932, 320); //A#
                    Thread.Sleep(50);
                    PrintStringPosition(11, 7, text[i], ConsoleColor.White); i++;
                    Console.Beep(830, 320);//G#
                    Thread.Sleep(50);
                    PrintStringPosition(12, 7, text[i], ConsoleColor.White); i++;
                    Console.Beep(932, 320);//A#

                    Thread.Sleep(250);

                    //Second Part
                    PrintStringPosition(13, 7, text[i], ConsoleColor.White); i++;
                    Console.Beep(932, 400); //A#
                    Thread.Sleep(50);
                    PrintStringPosition(14, 7, text[i], ConsoleColor.White); i++;
                    Console.Beep(830, 400);//G#
                    Thread.Sleep(50);
                    Console.Beep(739, 400);//F#
                    Thread.Sleep(50);
                    PrintStringPosition(15, 7, text[i], ConsoleColor.White); i++;
                    Console.Beep(830, 609);//G#
                    Console.WriteLine();
                }
            } //END OF IF

            else
	        {

	        
                PrintStringPosition(5, 4, text[0], ConsoleColor.White); 
                PrintStringPosition(10, 4, text[1], ConsoleColor.White);
                PrintStringPosition(19, 4, text[2], ConsoleColor.White);
                PrintStringPosition(7, 5, text[3], ConsoleColor.White); 
                PrintStringPosition(16, 5, text[4], ConsoleColor.White);
                PrintStringPosition(16, 5, text[5], ConsoleColor.White);
                PrintStringPosition(9, 7, text[6], ConsoleColor.White); 
                PrintStringPosition(10, 7, text[7], ConsoleColor.White);
                PrintStringPosition(11, 7, text[8], ConsoleColor.White);
                PrintStringPosition(12, 7, text[9], ConsoleColor.White);
                PrintStringPosition(13, 7, text[10], ConsoleColor.White);
                PrintStringPosition(14, 7, text[11], ConsoleColor.White);
                PrintStringPosition(15, 7, text[12], ConsoleColor.White);
                Console.WriteLine();
            }



        }
        static void MenuLoad(bool intro)
        {

            if (intro == false)
            {
                Console.Beep(622, 354); //D#
                PrintStringPosition(5, 10, "******************", ConsoleColor.Yellow);
                Thread.Sleep(300);
                PrintStringPosition(5, 12, "Start Game", ConsoleColor.Red);
                Console.Beep(932, 328); // A#
                Thread.Sleep(250);



                Console.Beep(932, 320); //A#
                Thread.Sleep(50);
                Console.Beep(830, 320);//G#
                Thread.Sleep(50);
                Console.Beep(932, 320);//A#

                Thread.Sleep(200);

                PrintStringPosition(5, 13, "Instructions", ConsoleColor.Red);
                Thread.Sleep(140);
                Console.Beep(932, 320); //A#
                Thread.Sleep(50);
                Console.Beep(830, 320);//G#
                Thread.Sleep(50);
                Console.Beep(932, 320);//A#

                Thread.Sleep(250);
                PrintStringPosition(5, 14, "Exit", ConsoleColor.Red);
                //Second Part
                Console.Beep(932, 400); //A#
                Thread.Sleep(50);
                Console.Beep(830, 400);//G#
                Thread.Sleep(50);
                Console.Beep(739, 400);//F#
                Thread.Sleep(50);
                Console.Beep(830, 609);//G#
            }


            else
            {

                PrintStringPosition(5, 10, "******************", ConsoleColor.Yellow);
                PrintStringPosition(5, 12, "Start Game", ConsoleColor.Red);
                PrintStringPosition(5, 13, "Instructions", ConsoleColor.Red);
                PrintStringPosition(5, 14, "Exit", ConsoleColor.Red);
            }
        }
         
               


        static void PrintHighScores(double score)
        {
            string[] oneLine = new string[10];
            var reader = new StreamReader("high-score.txt");
            using (reader)
            {
                string line = reader.ReadLine();
                oneLine = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 1; i < oneLine.Length; i += 2)
                {
                    if (score > int.Parse(oneLine[i]))
                    {
                        for (int j = oneLine.Length - 1; j >= i + 2; j -= 2)
                        {
                            oneLine[j] = oneLine[j - 2];
                        }
                        oneLine[i] = score.ToString();
                        break;
                    }
                }

                Console.WriteLine("High Scores:");
                for (int i = 0; i < oneLine.Length - 1; i += 2)
                {
                    Console.WriteLine("{0} {1}",
                                      oneLine[i],
                                      oneLine[i + 1]);
                }
            }

            var writer = new StreamWriter("high-score.txt");
            using (writer)
            {
                string line = string.Join(" ", oneLine);
                writer.Write(line);
            }
        }
        
        static void PrintPosition(int x, int y, char c, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(c);
        }


        static void PrintStringPosition(int x, int y, string str, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(str);
        }

        //private static void ClearLine(int left, int top)    
        //{

        //    int pLeft = Console.CursorLeft;
        //    int pTop = Console.CursorTop;

        //    Console.SetCursorPosition(left, top);
        //    Console.Write(new string(' ', Console.BufferWidth - Console.CursorLeft));

        //    Console.SetCursorPosition(pLeft, pTop);
        //}



        



        private static void PrintGameWonMessage(int geneticPoints)       //Method about printing that the  player won !
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            string wonMsg = "YOU WON";
            Console.SetCursorPosition(Console.WindowWidth / 2 - (wonMsg.Length / 2), Console.WindowHeight / 2 - 3);
            Console.WriteLine(wonMsg);

            int badGenesPercentage = (geneticPoints - 4200) / geneticPoints * 100;   // Baby profile depends on bad genetic percentage!
            string pointsGathered = string.Format("Bad genes: {0}%", badGenesPercentage);
            Console.SetCursorPosition(Console.WindowWidth / 2 - (pointsGathered.Length / 2), Console.WindowHeight / 2 - 1);
            Console.WriteLine(pointsGathered);

            bool isMale = true;
            Random rand = new Random();          // Sex of the baby is random! It's DESTINY!!!!!
            if (rand.Next(0, 101) % 2 == 0)
            {
                isMale = false;
            }

            string babyDescription;

            if (isMale)
            {
                babyDescription = "It's a boy. ";
            }
            else
            {
                babyDescription = "It's a girl. ";
            }

            if (badGenesPercentage <= 10)            //Print baby profile depends of bad genes percentage!
            {
                babyDescription += "Perfect baby";
            }
            else if (badGenesPercentage <= 20)
            {
                babyDescription += "Average baby";
            }
            else if (badGenesPercentage <= 30)
            {
                babyDescription += "Not so clever baby";
            }
            else if (badGenesPercentage <= 40)
            {
                babyDescription += "The Baby is gay.";
            }
            else if (badGenesPercentage <= 50)
            {
                babyDescription += "Beautifull but stupid baby";
            }
            else if (badGenesPercentage <= 60)
            {
                babyDescription += "Ugly and stupid baby";
            }
            else if (badGenesPercentage <= 70)
            {
                babyDescription += "Baby with three hands";
            }
            else if (badGenesPercentage <= 80)
            {
                babyDescription += "Chernobyl baby";
            }
            else if (badGenesPercentage <= 90)
            {
                babyDescription += "Charming crocodile";
            }
            else if (badGenesPercentage <= 100)
            {
                babyDescription += "...crocodile";
            }
            else
            {
                babyDescription += "but the baby is an idiot";
            }

            Console.SetCursorPosition(Console.WindowWidth / 2 - (babyDescription.Length / 2), Console.WindowHeight / 2 + 1);
            Console.WriteLine(babyDescription);

            string pressKey = "Press 'P' to PLAY AGAIN";   // Option for new game!
            Console.SetCursorPosition(Console.WindowWidth / 2 - (pressKey.Length / 2), Console.WindowHeight / 2 + 3);
            Console.WriteLine(pressKey);

            while (!Console.KeyAvailable)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                if (pressedKey.Key == ConsoleKey.P)
                {
                    break;
                }
            }

            Console.Clear();
        }

        private static void PrintNextLevelMessage(int level, int geneticPoints)  // Print level passed!
        {
            Console.Clear();

            string newLevelMsg = string.Format("LEVEL {0}", level);
            Console.SetCursorPosition(Console.WindowWidth / 2 - (newLevelMsg.Length / 2), Console.WindowHeight / 2 - 3);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(newLevelMsg);

            int baseScore = 300;     
            int minScoreForLevel = 300;      //Calculate the least points to pass!
            for (int i = 1; i < level; i++)
            {
                minScoreForLevel += baseScore + i * 100;
            }

            int badGenesPercentage = (int)((geneticPoints - minScoreForLevel) / (double)geneticPoints * 100); //Calculate the percent of bad genes!
            string pointsGathered = string.Format("Bad genes so far: {0}%", badGenesPercentage);
            Console.SetCursorPosition(Console.WindowWidth / 2 - (pointsGathered.Length / 2), Console.WindowHeight / 2 - 1);
            Console.WriteLine(pointsGathered);

            string babyDescription = "The baby might be ";   // Guess the baby profile so far depend of bad genes percentage! 

            if (badGenesPercentage <= 10)
            {
                babyDescription += "Perfect ";
            }
            else if (badGenesPercentage <= 20)
            {
                babyDescription += "Average ";
            }
            else if (badGenesPercentage <= 30)
            {
                babyDescription += "Not so clever ";
            }
            else if (badGenesPercentage <= 40)
            {
                babyDescription += "Gay ";
            }
            else if (badGenesPercentage <= 50)
            {
                babyDescription += "Beautifull but stupid ";
            }
            else if (badGenesPercentage <= 60)
            {
                babyDescription += "Ugly and stupid ";
            }
            else if (badGenesPercentage <= 70)
            {
                babyDescription += "With three hands ";
            }
            else if (badGenesPercentage <= 80)
            {
                babyDescription += "A Chernobyl baby ";
            }
            else if (badGenesPercentage <= 90)
            {
                babyDescription += "Charming crocodile ";
            }
            else if (badGenesPercentage <= 100)
            {
                babyDescription += "...crocodile";
            }
            else
            {
                babyDescription += " an idiot";
            }

            Console.SetCursorPosition(Console.WindowWidth / 2 - (babyDescription.Length / 2), Console.WindowHeight / 2 + 1);
            Console.WriteLine(babyDescription);

            string pressKey = "Press 'Y' to continue...";   // Continue to next level by pressing 'Y' key! 
            Console.SetCursorPosition(Console.WindowWidth / 2 - (pressKey.Length / 2), Console.WindowHeight / 2 + 3);
            Console.WriteLine(pressKey);

            while (!Console.KeyAvailable)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                if (pressedKey.Key == ConsoleKey.Y)
                {
                    break;
                }
            }

            Console.Clear();
        }
    }
}
