using System;

namespace GameOfLife
{
    class Program
    {  
        //Main Function
        static void Main(string[] args)
        {
            var gameView = new GameView(); //initializing gameView class object           

            do
            {
                Console.WriteLine("Press 1 for gameView.play() method OR \nPress 2 for customized gameView.playV2() method OR \nPress 0 to exit");
                var input = Console.ReadLine();

                if (input == "1")
                {
                    gameView.play(); //Simple input output
                }
                else if (input == "2")
                {
                    gameView.playV2(); //customized input and output                    
                }
                else
                {
                    break;
                }

                Console.WriteLine("Press Any key to continue OR Press 0 to exit the program");
            } while (Console.ReadLine() != "0");

        }
    }
}
