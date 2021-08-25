using System;
using System.Collections.Generic;

namespace GameOfLife
{
    //This interface declares all the function memebers for the Game of life.
    interface IGame
    {
        //Empty Game Board
        int[][] genrateEmptyBoard(int row, int column);

        //The initial pattern constitutes the 'seed' of the system.
        //The first generation is created by applying the above rules simultaneously to every cell in the seed
        //Generate random seeds if seedPositions = null
        int[][] insertSeeds(int[][] board, int totalSeeds, List<string> seedPositions = null);

        //Every cell interacts with its eight neighbours, which are the cells that are directly horizontally, vertically,
        //or diagonally adjacent.
        //return the number of alive neighbours
        int countNeighbours(int[][] board, Tuple<int, int> pos);

        //Genrate Next Genration according to the Rules.
        //RULES
        //1. Any live cell with fewer than two live neighbours dies, as if by loneliness.
        //2. Any live cell with more than three live neighbours dies, as if by overcrowding.
        //3. Any dead cell with exactly three live neighbours comes to life.
        //4. Any live cell with two or three live neighbours lives, unchanged, to the next generation.
        int[][] nextGenration(int[][] board);

        //Display board
        void displayBoard(int[][] board);

        //Display coordinates of alive cells
        void displayAliveCellPositions(int[][] board);
    }
}
