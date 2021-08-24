using System;
using System.Collections.Generic;

namespace GameOfLife
{
    //This class contains all the fuctions related to the game logic
    public static class Helpers
    { 
        //Empty Game Board
        public static int[][] genrateEmptyBoard(int m, int n)
        {
            int[][] board = new int[m][];
            for(int i=0; i<m; i++)
            {
                int[] rows = new int[n];
                for(int j=0; j<n; j++)
                {
                    rows[j] = 0;
                }
                board[i] = rows;
            }

            return board;
        }

        //The initial pattern constitutes the 'seed' of the system.
        //The first generation is created by applying the above rules simultaneously to every cell in the seed
        public static int[][] insertSeeds(int[][] board, List<string> seedPositions = null) //Genrate random if null
        {
            if (seedPositions == null)
            {
                var random = new Random();                

                for (int i = 0; i< (board.Length*2); i++)
                {
                    board[random.Next(board.Length)][random.Next(board.Length)] = 1;
                }                
            }
            else
            {
                foreach(var pos in seedPositions) // <pos> is in format <X,Y> string
                {
                    var vals = pos.Split(",");
                    board[Convert.ToInt32(vals[0])][Convert.ToInt32(vals[1])] = 1;
                }
            }

            return board;
        }

        //Every cell interacts with its eight neighbours, which are the cells that are directly horizontally, vertically,
        //or diagonally adjacent.
        public static int countNeighbours(int[][] board, Tuple<int, int> pos)
        {
            //For calculation of 8 neighbours
            var checksList = new List<(int, int)>();
            checksList.Add((-1, -1));
            checksList.Add((-1, 0));
            checksList.Add((-1, 1));
            checksList.Add((0, -1));
            checksList.Add((0, 1));
            checksList.Add((1, -1));
            checksList.Add((1, 0));
            checksList.Add((1, 1));

            int count = 0;

            foreach(var pair in checksList)
            {
                var i = pos.Item1 + pair.Item1;
                var j = pos.Item2 + pair.Item2;

                if (i>=0 && j>=0 && i<board.Length && j<board[0].Length) //Checking for the out of bounds of the array
                {
                    if (board[i][j] == 1)
                    {
                        count++;
                    }                   
                } 
            }

            return count;
        }

        //Genrate Next Genration according to the Rules.
        public static int[][] nextGenration(int[][] board)
        {
            var next = genrateEmptyBoard(board.Length, board[0].Length); //Initialize with all 0's, 0 - dead, 1 - alive

            for(int i=0; i<board.Length; i++)
            {
                for(int j=0; j<board[0].Length; j++)
                {
                    var count  = countNeighbours(board, Tuple.Create(i, j));
                    
                    //RULES
                    //1. Any live cell with fewer than two live neighbours dies, as if by loneliness.
                    //2. Any live cell with more than three live neighbours dies, as if by overcrowding.
                    if (count < 2 || count > 3)
                    {
                        next[i][j] = 0;
                    }
                    //3. Any dead cell with exactly three live neighbours comes to life.
                    else if (count == 3 && board[i][j] == 0)
                    {
                        next[i][j] = 1;
                    }
                    //4. Any live cell with two or three live neighbours lives, unchanged, to the next generation.
                    else if (count == 2 || count == 3)
                    {
                        next[i][j] = board[i][j];
                    }                                     
                }
            }          
            return next;
        }

        //For Displaying array
        public static void displayBoard(int[][] board)
        {
            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board[0].Length; j++)
                {
                    Console.Write(board[i][j] + " ");
                }
                Console.WriteLine();
            }            
        }

        //For Displaying positons
        public static void displayAliveCellPositions(int[][] board)
        {
            Console.WriteLine("Positions..");
            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board[0].Length; j++)
                {
                    if(board[i][j] == 1)
                    {
                        Console.WriteLine(i + "," + j);
                    }
                }               
            }
        }
    }

    class Program
    {  
        static void Main(string[] args)
        {
            Console.WriteLine("Game Of Life!");

            Console.WriteLine("Enter board size. Board is a Square matrix (m x m)");
            var size = Console.ReadLine();
            var board = Helpers.genrateEmptyBoard(Convert.ToInt32(size), Convert.ToInt32(size));

            Console.WriteLine("1. Genrate Random Seed Board");
            Console.WriteLine("2. Enter Seed Positions (X,Y)");

            switch (Convert.ToInt32(Console.ReadLine()))
            {
                case 1:                    
                    board = Helpers.insertSeeds(board); //Will initialize the board with Random seeds
                    Console.WriteLine("Initial Board");
                    Helpers.displayBoard(board);
                    do
                    {
                        board = Helpers.nextGenration(board);
                        Console.WriteLine("Next Genration");
                        Helpers.displayBoard(board);
                        Helpers.displayAliveCellPositions(board);
                        Console.WriteLine("Press 1 for next genration! Any other key for exit.");
                    } while (Console.ReadLine() == "1");                    
                    break;
                case 2:
                    Console.WriteLine("Enter Total number of seeds");
                    var totalSeeds = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter seed positions <x,y>. example: 0,2");
                    Console.WriteLine("minimum position = 0,0 | maximum position = " + (board.Length-1) + "," + (board.Length - 1));
                    var seedPosList = new List<string>();                    

                    for (int i = 0; i < totalSeeds; i++)
                    {
                        var pos = Console.ReadLine();
                        var pair = pos.Split(',');
                        if (Convert.ToInt32(pair[1]) < 0 || Convert.ToInt32(pair[1]) >= board.Length ||
                            Convert.ToInt32(pair[0]) < 0 || Convert.ToInt32(pair[0]) >= board.Length)
                        {
                            Console.WriteLine("Invalid");
                            break;
                        }
                        else
                        {
                            seedPosList.Add(pos);
                        }                        
                    }

                    if (seedPosList.Count > 0)
                    {
                        board = Helpers.insertSeeds(board, seedPosList); //Will initialize the board with your positions
                        Console.WriteLine("Initial Board");
                        Helpers.displayBoard(board);
                        do
                        {
                            board = Helpers.nextGenration(board);
                            Console.WriteLine("Next Genration");
                            Helpers.displayBoard(board);
                            Helpers.displayAliveCellPositions(board);
                            Console.WriteLine("Press 1 for next genration! Any other key for exit.");
                        } while (Console.ReadLine() == "1");
                    }
                    break;
                default: 
                    Console.WriteLine("Invalid input!. Exiting"); 
                    break;
            }
        }
    }
}
