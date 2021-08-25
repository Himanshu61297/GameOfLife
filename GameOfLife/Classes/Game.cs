using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfLife
{
    //This Game class inherits from IGame interface and will implement all of it's members.
    class Game : IGame
    {
        public Game() { }
        
        //Empty Game Board
        public int[][] genrateEmptyBoard(int m, int n)
        {
            int[][] board = new int[m][];
            for (int i = 0; i < m; i++)
            {
                int[] rows = new int[n];
                for (int j = 0; j < n; j++)
                {
                    rows[j] = 0;
                }
                board[i] = rows;
            }

            return board;
        }

        //The initial pattern constitutes the 'seed' of the system.
        //The first generation is created by applying the above rules simultaneously to every cell in the seed
        public int[][] insertSeeds(int[][] board, int totalSeeds, List<string> seedPositions = null) //Genrate random if null
        {
            if (seedPositions == null)
            {
                var random = new Random();

                for (int i = 0; i < totalSeeds; i++)
                {
                    board[random.Next(board.Length)][random.Next(board.Length)] = 1;
                }
            }
            else
            {
                foreach (var pos in seedPositions) // <pos> is in format <X,Y> string
                {
                    var vals = pos.Split(",");
                    board[Convert.ToInt32(vals[0])][Convert.ToInt32(vals[1])] = 1;
                }
            }

            return board;
        }

        //Every cell interacts with its eight neighbours, which are the cells that are directly horizontally, vertically,
        //or diagonally adjacent.
        public int countNeighbours(int[][] board, Tuple<int, int> pos)
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

            foreach (var pair in checksList)
            {
                var i = pos.Item1 + pair.Item1;
                var j = pos.Item2 + pair.Item2;

                if (i >= 0 && j >= 0 && i < board.Length && j < board[0].Length) //Checking for the out of bounds of the array
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
        public int[][] nextGenration(int[][] board)
        {
            var next = genrateEmptyBoard(board.Length, board[0].Length); //Initialize with all 0's, 0 - dead, 1 - alive

            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board[0].Length; j++)
                {
                    var count = countNeighbours(board, Tuple.Create(i, j));

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
        public void displayBoard(int[][] board)
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
        public void displayAliveCellPositions(int[][] board)
        {
            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board[0].Length; j++)
                {
                    if (board[i][j] == 1)
                    {
                        Console.WriteLine(i + "," + j);
                    }
                }
            }
        }
    }
}
