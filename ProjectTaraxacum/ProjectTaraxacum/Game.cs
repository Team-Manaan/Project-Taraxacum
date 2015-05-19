using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


struct GameObject
{
    public int x;
    public int y;
    public char c;
    public ConsoleColor color;
}

class Game
{


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

    static void PrintLine()
    {
        for (int i = 0; i < 30; i++)
        {

            PrintPosition(i, 31, '#', ConsoleColor.DarkRed);

        }


    }

    private static void ClearLine(int left, int top)
    {

        int pLeft = Console.CursorLeft;
        int pTop = Console.CursorTop;

        Console.SetCursorPosition(left, top);
        Console.Write(new string(' ', Console.BufferWidth - Console.CursorLeft));

        Console.SetCursorPosition(pLeft, pTop);
    }



    static void Main(string[] args)
    {

        //Resizing the console
        Console.BufferHeight = Console.WindowHeight = 35;
        Console.BufferWidth = Console.WindowWidth = 30;
        int y = 0;
        int x = 0;
        int playfieldWidht = 20;
        double speed = 50.0;
        double defSpeed = 50.0;
        int livesCount = 5;
        int geneticPoints = 0;
        int level = 0;
        int checker = 0;

        //Scorint system
        Stopwatch stopwatch = new Stopwatch();
        double score = 0;
        int distance = 1;
        int increment = 0;

        //Creating the character
        GameObject sperm = new GameObject();
        sperm.x = 10;
        sperm.y = Console.WindowHeight - 6;
        sperm.c = '?';
        sperm.color = ConsoleColor.White;

        //Random generator for falling objects
        Random randomGenerator = new Random();
        List<GameObject> obstacles = new List<GameObject>();


        Random bonusChance = new Random();


        while (true)
        {

            //Creating the obstecles
            GameObject newObstacle = new GameObject();
            newObstacle.color = ConsoleColor.Green;
            newObstacle.x = randomGenerator.Next(10, playfieldWidht);
            newObstacle.y = 0;
            newObstacle.c = 'O';
            obstacles.Add(newObstacle);

            //create bonuses
            if (bonusChance.Next(0, 101) <= 5)
            {
                GameObject bonus = new GameObject();
                bonus.color = ConsoleColor.White;
                bonus.x = randomGenerator.Next(10, playfieldWidht);
                bonus.y = 0;
                bonus.c = '#';
                obstacles.Add(bonus);
            }
            else if (bonusChance.Next(0, 100) <= 1)
            {
                GameObject bonusLife = new GameObject();
                bonusLife.color = ConsoleColor.Blue;
                bonusLife.x = randomGenerator.Next(10, playfieldWidht);
                bonusLife.y = 0;
                bonusLife.c = '*';
                obstacles.Add(bonusLife);
            }

            //controls the ammount of rocks
            if (checker > 2)
            {
                obstacles.Remove(newObstacle);

            }
            if (checker == 8)
            {
                checker = 0;
            }
            checker++;


            bool hitted = false;


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
                GameObject newObstacleB = new GameObject();
                newObstacleB.x = oldObstacle.x;
                newObstacleB.y = oldObstacle.y + 1;
                newObstacleB.c = oldObstacle.c;
                newObstacleB.color = oldObstacle.color;


                //If sperm is hitted substract life
                if (newObstacleB.y == sperm.y && newObstacleB.x == sperm.x)
                {
                    //Calculate distance
                    distance++;
                    if (distance >= Console.BufferHeight)
                    {
                        distance = 0;
                    }

                    if (newObstacleB.c == '#')
                    {
                        geneticPoints += 50;
                        increment += 50;
                    }
                    else if (newObstacleB.c == '*')
                    {
                        livesCount++;
                    }
                    else
                    {
                        livesCount--;
                        hitted = true;
                    }

                    //If live count is bellow zero end the game
                    if (livesCount <= 0)
                    {

                        PrintStringPosition(0, 32, "Sperms: " + livesCount, ConsoleColor.White);
                        PrintStringPosition(8, 7, "GAME OVER", ConsoleColor.Red);
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

            if (hitted) // if hitted print the sperm as an X.
            {
                obstacles.Clear();
                speed = defSpeed;
                PrintPosition(sperm.x, sperm.y, 'X', ConsoleColor.Red);
            }
            else //If hitted print as X
            {
                PrintPosition(sperm.x, sperm.y, sperm.c, sperm.color);
            }
            PrintStringPosition(0, 32, "Sperms: " + livesCount, ConsoleColor.White);
            PrintStringPosition(0, 33, "Genetic Points: " + geneticPoints, ConsoleColor.White);
            PrintStringPosition(0, 34, "Level: " + level, ConsoleColor.White);


            Thread.Sleep((int)(300 - speed));

            //Playfield from 10,0 to 20,30
            //for (int i = 0; i < 30; i++)

            //{
            //    ClearLine(0, i);
            //    for (int j = 10; j < 20; j++)
            //    {
            //        ClearLine(j, i);
            //    }


            //}
            score = (double)stopwatch.ElapsedMilliseconds * distance/10000 * speed + 1;
            increment += (int)score;
            geneticPoints += (int)score;

            //Control speed
            if (speed <= 200)
            {
                if (increment >= 50)
                {
                    speed += 10;
                    increment = 0;
                }
            }

            Console.Clear();


        }



    }
}