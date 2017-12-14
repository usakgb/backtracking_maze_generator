using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maze_generator
{
    class Program
    {
        /// <summary>
        /// Cell direction representation
        /// </summary>
        enum Direction
        {
            L,
            R,
            U,
            D
        }

        // Width and height of the cell grid
        private static int _width = 25;
        private static int _height = 25;

        private static readonly Random _rand = new Random();

        private static Cell[,] _cells;

        static void Main(string[] args)
        {
             _cells = new Cell[_width,_height];

            DiscoverNewCell(0, 0);

            // Output top horizontal wall
            for(var i = 0; i < _width; i++)
                Console.Write(" __");


            for (var y = 0; y < _height; y++)
            {
                Console.WriteLine("");
                // Output left vertical wall for each row
                Console.Write("|");


                for (var x = 0; x < _width; x++)
                {
                    // Only care about the right and the bottom wall of each cell

                    // See if the cell has the bottom wall or if the cell under has the top wall
                    if (_cells[x,y].Directions.Contains(Direction.D) || ( y < _height - 1 && _cells[x, y+1].Directions.Contains(Direction.U)))
                        Console.Write("  ");
                    else
                        Console.Write("__");

                    // See if the cell has the right wall or if the cell to the right has the left wall
                    if (_cells[x, y].Directions.Contains(Direction.R) || (x < _width - 1 && _cells[x+1, y].Directions.Contains(Direction.L)))
                        Console.Write(" ");
                    else
                        Console.Write("|");
                }
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Recursive method of descovering new cells from the current x and y
        /// </summary>
        /// <param name="x">x of the cell to discover the next cells from</param>
        /// <param name="y">y of the cell to discover the next cells from</param>
        static void DiscoverNewCell(int x, int y)
        {
            // Compute a list of random directions
            var randDirections = new List<Direction>();

            var directions = new List<Direction>() {Direction.L, Direction.R, Direction.U, Direction.D};

            for (var i = 0; i < 4; i++)
            {
                var randIndex = _rand.Next(0, directions.Count);
                var randDirection = directions[randIndex];
                directions.RemoveAt(randIndex);

                randDirections.Add(randDirection);
            }

            // Try to step from the current cell into each direction
            foreach (var direction in randDirections)
            {
                var nx = x;
                var ny = y;
                
                switch (direction)
                {
                    case Direction.L:
                        nx = x - 1;
                        break;
                    case Direction.R:
                        nx = x + 1;
                        break;
                    case Direction.U:
                        ny = y - 1;
                        break;
                    case Direction.D:
                        ny = y + 1;
                        break;
                }

                // If the next cell is within the grid and is undiscovered (null)
                if (nx >= 0 && nx < _width && ny >= 0 && ny < _height && _cells[nx, ny] == null)
                {
                    // Assign the direction to the current cell
                    if (_cells[x, y] == null)
                        _cells[x, y] = new Cell() {Directions = new List<Direction>() {direction}};
                    else
                        _cells[x, y].Directions.Add(direction);

                    // Assign the opposite direction to the next cell
                    if (_cells[nx, ny] == null)
                        _cells[nx, ny] = new Cell() { Directions = new List<Direction>() { Opposite(direction) } };
                    else
                        _cells[nx, ny].Directions.Add(Opposite(direction));
                    DiscoverNewCell(nx, ny);
                }
            }
        }

        /// <summary>
        /// Get opposite direction
        /// </summary>
        /// <param name="direction">direction</param>
        /// <returns></returns>
        static Direction Opposite(Direction direction)
        {
            switch (direction)
            {
                case Direction.L:
                    return Direction.R;
                case Direction.R:
                    return Direction.L;
                case Direction.U:
                    return Direction.D;
                default:
                    return Direction.U;
            }
        }


        /// <summary>
        /// Cell representation
        /// </summary>
        class Cell
        {
            public List<Direction> Directions { get; set; }
        }
    }
}
