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
        int minObstacleCount = 2;

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


        List<GameObject> wallOne = new List<GameObject>();
        for (int i = 0; i < 30; i++)
        {
            GameObject temp = new GameObject();
            temp.x = 8;
            temp.y = i;
            temp.color = ConsoleColor.Cyan;

            if (i % 2 == 0)
            {
                temp.c = '\\';
            }
            else
            {
                temp.c = '/';
            }
            wallOne.Add(temp);
        }

        List<GameObject> wallTwo = new List<GameObject>();
        for (int i = 0; i < 30; i++)
        {
            GameObject temp = new GameObject();
            temp.x = 21;
            temp.y = i;
            temp.color = ConsoleColor.Cyan;

            if (i % 2 == 0)
            {
                temp.c = '/';
            }
            else
            {
                temp.c = '\\';
            }
            wallTwo.Add(temp);
        }

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
                bonus.color = ConsoleColor.Yellow;
                bonus.x = randomGenerator.Next(10, playfieldWidht);
                bonus.y = 0;
                bonus.c = 'G';
                obstacles.Add(bonus);
            }
            else if (bonusChance.Next(0, 1000) <= 1)
            {
                GameObject bonusLife = new GameObject();
                bonusLife.color = ConsoleColor.Blue;
                bonusLife.x = randomGenerator.Next(10, playfieldWidht);
                bonusLife.y = 0;
                bonusLife.c = '*';
                obstacles.Add(bonusLife);
            }

            //controls the ammount of rocks
            if (checker > minObstacleCount)
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

                    if (newObstacleB.c == 'G')
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
                minObstacleCount = 2;
                PrintPosition(sperm.x, sperm.y, 'X', ConsoleColor.Red);
            }
            else //If hitted print as X
            {
                PrintPosition(sperm.x, sperm.y, sperm.c, sperm.color);
            }

            PrintStringPosition(0, 32, "Sperms: " + livesCount, ConsoleColor.White);
            PrintStringPosition(0, 33, "Genetic Points: " + geneticPoints, ConsoleColor.White);
            PrintStringPosition(0, 34, "Level: " + level, ConsoleColor.White);

            //moving wall one
            List<GameObject> newWallOne = new List<GameObject>();

            for (int i = 0; i < wallOne.Count; i++)
            {
                GameObject oldWall = wallOne[i];
                GameObject newWall = new GameObject();
                newWall.x = oldWall.x;
                newWall.y = oldWall.y;
                if (oldWall.c == '/')
                {
                    newWall.c = '\\';
                }
                else
                {
                    newWall.c = '/';
                }
                newWall.color = oldWall.color;

                newWallOne.Add(newWall);
                
            }

            wallOne = newWallOne;

            foreach (var brick in wallOne)
            {
                PrintPosition(brick.x, brick.y, brick.c, brick.color);
            }

            //moving wall two
            List<GameObject> newWallTwo = new List<GameObject>();

            for (int i = 0; i < wallTwo.Count; i++)
            {
                GameObject oldWall = wallTwo[i];
                GameObject newWall = new GameObject();
                newWall.x = oldWall.x;
                newWall.y = oldWall.y;
                if (oldWall.c == '/')
                {
                    newWall.c = '\\';
                }
                else
                {
                    newWall.c = '/';
                }
                newWall.color = oldWall.color;

                newWallTwo.Add(newWall);

            }

            wallTwo = newWallTwo;

            foreach (var brick in wallTwo)
            {
                PrintPosition(brick.x, brick.y, brick.c, brick.color);
            }


            Thread.Sleep((int)(200 - speed));

            score = (double)stopwatch.ElapsedMilliseconds * distance / 10000 * speed + 1;
            increment += (int)score;
            geneticPoints += (int)score;

            //Control speed
            if (speed <= 100)
            {
                if (increment >= 50)
                {
                    speed += 10;
                    increment = 0;
                }
                if (speed == 80)
                {
                    minObstacleCount = 3;
                }
                else if (speed == 100)
                {
                    minObstacleCount = 4;
                }
            }

            Console.Clear();

        }



    }
}