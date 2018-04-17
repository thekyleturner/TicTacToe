using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*  Kyle Turner 
 *  keturner@gmail.com
 *  04.01.18
 *  A simple tic-tac-toe game
 */

/* Game Flow:
 * Check for validity of user input, create board
 * Each player picks a random (unoccupied) spot on the board
 * Loop executes until a player has won and returns the winning player
 */

namespace TTT
{
    class Program
    {
        public class game
        {
            public int dims = 0;                                                                                // for dimensions of board
            public int toWin = 0;                                                                               // number of sequential squares to win
            public int turn = 0;                                                                                // turn number
            public int[,] board = new int[0, 0];                                                                // 0 by 0 board to start
            public bool printMe = false;                                                                        // flag if user wants to print board output
            public int whoseTurn = 1;                                                                           // flip-flop counter for turn
            public List<Tuple<int, int>> hMoves = new List<Tuple<int, int>>();                                  // horizontal tracker
            public List<Tuple<int, int>> vMoves = new List<Tuple<int, int>>();                                  // vertical tracker
            public List<Tuple<int, int>> d1Moves = new List<Tuple<int, int>>();                                 // diagonal (NW-SE) tracker
            public List<Tuple<int, int>> d2Moves = new List<Tuple<int, int>>();                                 // diagonal (NE-SW) tracker
        }

        static void Main(string[] args)
        {
            Random rand = new Random();                                                                         // random number generator                    
            game g = new game();                                                                                // make new game

            if (args.Length < 2)                                                                                // checks for user input
            {
                Console.WriteLine("Please enter dimensions of board and number of sequential squares needed to win");
                Environment.Exit(1);
            }
            else
            {
                try
                {
                    g.dims = int.Parse(args[0]);                                                                // assigns input to class object
                    g.toWin = int.Parse(args[1]);
                }
                catch
                {
                    Console.WriteLine("Board size and number of squares needed to win must be integers");       // if inputs are not ints
                    Environment.Exit(1);
                }
                if (args.Length >= 3 && args[2] != "v")                                                         // v for verbose (prints board)
                {
                    Console.WriteLine("To print the board, please only use flag v\n");
                }
                else if (args.Length >= 3 && args[2] == "v")
                {
                    g.printMe = true;
                }
                if (g.dims < g.toWin)                                                                           // makes sure input is valid
                {
                    Console.WriteLine("Number of sequential squares needed to win cannot exceed dimensions of board");
                    Environment.Exit(1);
                }
                else
                {
                    g.board = new int[g.dims, g.dims];                                                          // modify board to size
                }
            }

            while (g.turn < (g.dims * g.dims))                                                                  // START MAIN GAME LOOP
            {
                int chooseX = rand.Next(g.dims);                                                                // pick random place
                int chooseY = rand.Next(g.dims);
                int winner = 0;                                                                                 // for (potential) winner

                while (occupied(g, chooseX, chooseY))                                                           // make sure not occupied
                {
                    chooseX = rand.Next(g.dims);                                                                // if so pick new
                    chooseY = rand.Next(g.dims);
                }

                if (g.whoseTurn % 2 != 0)                                                                       // player 1
                {
                    g.board[chooseX, chooseY] = 1;                                                              // place on board

                    if (winCheck(g))
                    {
                        winner = 1;
                    }
                    else
                    {
                        g.whoseTurn++;                                                                          // switch turns
                    }                                                        
                }
                else                                                                                            // player 2 (same as above)
                {
                    g.board[chooseX, chooseY] = 2;

                    if (winCheck(g))
                    {
                        winner = 2;
                    }
                    else
                    {
                        g.whoseTurn--;
                    }
                }
                
                if (winner == 0 && g.turn != ((g.dims * g.dims) - 1))                                           // not winner but not last turn
                {
                    g.turn++;                                                                                   // advance turn
                }
                else                                                                                            // either have a winner or no winner and last turn
                {
                    declareWinner(winner);
                    break;
                }
            }                                                                                                   // END MAIN GAME LOOP

            if (g.printMe == true)                                                                              // for printing output of game
            {                                                                                                   // only called once so no need for method
                string op = "";
                for (int i = 0; i < g.dims; i++)
                {
                    for (int j = 0; j < g.dims; j++)
                    {
                        op = op + g.board[i, j];
                    }
                    Console.WriteLine(op);
                    op = "";
                }
            }

            void declareWinner(int winner)                                                                      // determines winner if any
            {
                if (winner == 0)                                                                                // reached last turn with no winner-tie game
                {
                    Console.WriteLine("There is no winner, please try again");
                }
                else                                                                                            // there is a winner
                {
                    Console.WriteLine("The winner is " + winner.ToString());
                }
            }

            bool occupied(game game, int x, int y)                                                              // check if a space is occupied
            {
                if (game.board[x, y] == 0)                                                                      // still 0 (the initial value) so not occupied
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            bool isSequential(List<Tuple<int,int>> moves)                                                       // check if spaces are sequential
            {
                int yChk = moves[0].Item1;                                                                      // first number in list
                int xChk = moves[0].Item2;
                bool yRet = true;                                                                               // return value checkers
                bool xRet = true;

                for (int i = 1;i < moves.Count; i++)                                                            // start at index 1, check if is +-1 from (or = to) chk
                {
                    if (moves[i].Item1-yChk == 1 | moves[i].Item1+yChk == 1 | moves[i].Item1 == yChk)
                    {
                        yChk = moves[i].Item1;
                    }
                    else
                    {
                        yRet = false;
                    }
                    if (moves[i].Item2 - xChk == 1 | moves[i].Item2 + xChk == 1 | moves[i].Item2 == xChk |      // same as above
                        moves[i].Item2 - xChk == -1)                                                            // this additional case is necessary for NE-SW checking
                    {
                        xChk = moves[i].Item2;
                    }
                    else
                    {
                        xRet = false;
                    }
                }

                if (yRet && xRet)                                                                               // if everything checks out
                {
                    return true;
                }
                else
                {
                    return false;
                }   
            }

            bool winCheck(game game)                                                                            // returns 0 if no winner, 1 if player 1, 2 if player 2, 3 if a draw
            {
                int backCnt = game.dims-1;                                                                      // counter for NE to SW diagonal
                int whoseTurn = game.whoseTurn;                                                                 // store as int

                for (int y = 0; y < game.dims; y++)                                                             // outer loop for cols
                {
                    for (int x = 0; x < game.dims; x++)                                                         // inner loop for rows
                    {
                        if (game.board[y, x] == whoseTurn)                                                      // check horizontal
                        {
                            game.hMoves.Add(Tuple.Create(y, x));
                        }
                        if (game.board[x, y] == whoseTurn)                                                      // check vertical
                        {
                            game.vMoves.Add(Tuple.Create(x, y));
                        }
                        if (game.board[x, x] == whoseTurn)                                                      // check diagonal NW to SE
                        {
                            game.d1Moves.Add(Tuple.Create(x, x));
                        }
                        if (game.board[x, backCnt] == whoseTurn)                                                // check diagonal NE to SW
                        {
                            game.d2Moves.Add(Tuple.Create(x, backCnt));
                            backCnt--;
                        }

                        game.hMoves = game.hMoves.Distinct().ToList();                                          // remove duplicates
                        game.vMoves = game.vMoves.Distinct().ToList();
                        game.d1Moves = game.d1Moves.Distinct().ToList();
                        game.d2Moves = game.d2Moves.Distinct().ToList();

                        if (game.hMoves.Count() == game.toWin && isSequential(game.hMoves))                     // check horizontal for win
                        {
                            return true;
                        }
                        if (game.vMoves.Count() == game.toWin && isSequential(game.vMoves))                     // check vertical for win
                        {
                            return true;
                        }
                        if (game.d1Moves.Count() == game.toWin && isSequential(game.d1Moves))                   // check NW-SE for win
                        {
                            return true;
                        }
                        if (game.d2Moves.Count() == game.toWin && isSequential(game.d2Moves))                   // check NE-SW for win
                        {
                            return true;
                        }
                    }

                    backCnt = game.dims-1;                                                                      // reset counter and trackers
                    game.hMoves = new List<Tuple<int, int>>();
                    game.vMoves = new List<Tuple<int, int>>();
                    game.d1Moves = new List<Tuple<int, int>>();
                    game.d2Moves = new List<Tuple<int, int>>();
                }
                return false;                                                                                   // default return, no winner
            }
        }
    }
}