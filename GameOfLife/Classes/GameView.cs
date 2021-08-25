using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GameOfLife
{
    //This class implements general flow of the game and uses Game class methods.
    class GameView  
    {
        public int[][] board { get; private set; } //READ ONLY public property. This property can only be initialized or changed from inside this class only.

        private Game game; //Global Game object   

        //Default Constructor
        public GameView()
        {
            game = new Game(); //initializing the game object     
        }        

        //Main Game Function
        public void play() 
        {
            var seedPosList = new List<string>(); //For storing the input coordinates.
            string xy = ""; //Seed Coordinate
            int row = 0; //universe grid row
            int column = 0; //universe grid column
            bool isCanceled = false; //if the proccess is canceled by invalid input

            Console.WriteLine("Input seeds like x,y. Enter EMPTY input to STOP.");            
            while(true)
            {
                xy = Console.ReadLine();

                if(xy == "")
                {
                    break; //break if empty input
                }               

                //Calculating maximum value of row and column for the universe
                if (checkCoordinates(xy))
                {
                    var pair = xy.Split(',');
                    var pair0 = toInt(pair[0]);
                    var pair1 = toInt(pair[1]);

                    if (pair0 > row)
                    {
                        row = pair0;
                    }

                    if (pair1 > column)
                    {
                        column = pair1;
                    }

                    seedPosList.Add(xy);
                }
                else
                {
                    Console.WriteLine("Invalid input. It should be in the format x,y");
                    isCanceled = true;
                    break;
                }
                
            }

            if (seedPosList.Count > 0 && !isCanceled)
            {
                board = game.genrateEmptyBoard(row + 2, column + 1); //initialize an empty universe with rows and columns.                   
                board = game.insertSeeds(board, seedPosList.Count, seedPosList); //initialize the universe with seeds                            

                Console.WriteLine("Output:");
                board = game.nextGenration(board); //Generate next population according to the Rules.
                game.displayAliveCellPositions(board); //Display coordinates of the alive cells in the universe.
            }
            else
            {
                Console.WriteLine();
            }           
        }

        //Regex For checking the input coordinates
        private bool checkCoordinates(string str)
        {
            string pattern;
            pattern = @"^\d\,\d";      // Define the regular expression pattern for <X,Y> where X and Y are digits.

            Regex rgx = new Regex(pattern);

            if (rgx.IsMatch(str))
            {
                return true;
            }

            return false;
        }

        //String to Int conversion with exception handling
        private int toInt(string str)
        {
            try
            {
                return Convert.ToInt32(str);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return -1;
        }

        //Alternate fun play method
        public void playV2()
        {
            Console.WriteLine("###-GAME-OF-LIFE-###");
            Console.WriteLine("First of all we need to create the game universe. How big would you like to build the universe? Enter lenght of the universe");
            var size = Console.ReadLine();
            board = game.genrateEmptyBoard(Convert.ToInt32(size), Convert.ToInt32(size));
            
            Console.WriteLine("The universe is created!. It looks like this.");
            game.displayBoard(board);
            
            Console.WriteLine("Press any key to continue OR Press 0 to restart");
            
            if(Console.ReadLine() != "0")
            {
                Console.WriteLine("The universe seems empty now. Lets create an inital population which will be the Seeds.");
                Console.WriteLine("We can either create our seeds randomly OR manually for which we have to enter the locations(coordinates x,y) of our seeds.");
                Console.WriteLine("Press 1 for Random seeds OR Press 0 for manual OR Any other key to exit");

                var input = Console.ReadLine();

                if(input == "1" || input == "0")
                {
                    Console.WriteLine("How many seeds do you want to generate?");
                    var totalSeeds = Convert.ToInt32(Console.ReadLine());
                    var isCanceled = false;

                    if (input == "1")
                    {
                        board = game.insertSeeds(board, totalSeeds);
                        nextGeneration();
                    }
                    else
                    {
                        Console.WriteLine("Where do you want to put the seeds? Enter coordinates in X,Y");
                        Console.WriteLine("minimum value = 0,0 | maximum value = " + (board.Length - 1) + "," + (board.Length - 1));
                        var seedPosList = new List<string>();
                        
                        for (int i=0; i<totalSeeds; i++)
                        {
                            var pos = Console.ReadLine();

                            if (checkCoordinates(pos))
                            {
                                var pair = pos.Split(',');
                                if (Convert.ToInt32(pair[1]) < 0 || Convert.ToInt32(pair[1]) >= board.Length ||
                                    Convert.ToInt32(pair[0]) < 0 || Convert.ToInt32(pair[0]) >= board.Length)
                                {
                                    Console.WriteLine("Invalid Coordinate");
                                    break;
                                }
                                else
                                {
                                    seedPosList.Add(pos);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. It should be in the format X,Y");
                                isCanceled = true;
                                break;
                            }
                            
                        }

                        if (seedPosList.Count > 0 && !isCanceled)
                        {
                            board = game.insertSeeds(board, totalSeeds, seedPosList); //Will initialize the board with your positions
                            nextGeneration();
                        }
                    }                    
                }             
                else
                {

                }
            }
            else
            {
                playV2();
            }
        }


        //Generate next generation of cells: For PlayV2 method only
        private void nextGeneration()
        {
            Console.WriteLine("Now our universe looks like this.");
            game.displayBoard(board);

            Console.WriteLine("This universe have some RULES which applies to each individual. The Rules are:");
            Console.WriteLine("1. Any live cell with fewer than two live neighbours dies, as if by loneliness.");
            Console.WriteLine("2. Any live cell with more than three live neighbours dies, as if by overcrowding.");
            Console.WriteLine("3. Any dead cell with exactly three live neighbours comes to life.");
            Console.WriteLine("4. Any live cell with two or three live neighbours lives, unchanged, to the next generation.");

            Console.WriteLine("Do you want to see the next generation?");
            Console.WriteLine("Press any key to continue OR Press 0 to exit");

            if (Console.ReadLine() != "0")
            {
                var genCount = 0;
                do
                {
                    genCount++;
                    board = game.nextGenration(board);

                    Console.WriteLine("After all the rules our population looks like this.");
                    Console.WriteLine("Generation " + genCount);
                    game.displayBoard(board);

                    Console.WriteLine("Coordinates of Alive cells.");
                    game.displayAliveCellPositions(board);

                    Console.WriteLine("Move on to next generation?");
                    Console.WriteLine("Press any key to continue OR Press 0 to exit the game");

                } while (Console.ReadLine() != "0");
            }
        }

    }

}
