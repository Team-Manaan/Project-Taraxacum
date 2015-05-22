namespace ProjectTaraxacum
{
    using System;

    public class GameObject
    {
        public int x;
        public int y;
        public char c;
        public ConsoleColor color;
                                                         // Exstracted the struct and made it into a class!
        public GameObject()
        {

        }

        public GameObject(int x, int y, char c, ConsoleColor color) // new constructor!
        {
            this.x = x;
            this.y = y;
            this.c = c;
            this.color = color;
        }
    }
}
