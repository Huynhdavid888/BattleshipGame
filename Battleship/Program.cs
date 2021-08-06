using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipLibrary;
using BattleshipLibrary.Models;


namespace Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            WelcomeMessage();

            PlayerInfoModel activeplayer = CreatePlayer("Player 1"); 
            PlayerInfoModel opponent = CreatePlayer("Player 2"); 
            PlayerInfoModel winner = null;
            
            do
            {
               
                DisplayShotGrid(activeplayer);
                RecordPlayerShot(activeplayer, opponent);

                bool doesGameContinue = GameLogic.PlayerStillActive(opponent);

                if(doesGameContinue == true)
                {
                   

                    (activeplayer, opponent) = (opponent, activeplayer);
                }
                else
                {
                    winner = activeplayer;
                }
            } while (winner == null);

            IdentifyWinner(winner);
            Console.ReadLine();
        }

        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratulations to {winner.usersName} for winning!");
            Console.WriteLine($"{winner.usersName} took {GameLogic.GetShotCount(winner)} shots");
        }

        private static void RecordPlayerShot(PlayerInfoModel activeplayer, PlayerInfoModel opponent)
        {
            bool isValidShot = false;
            string row = "";
            int column = 0;


            do
            {
                string shot = AskForShot(activeplayer);
                try
                {
                    (row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
                    isValidShot = GameLogic.ValidateShot(activeplayer, row, column);
                }
                catch (Exception ex)
                {
                    isValidShot = false;
                }
                if (isValidShot == false)
                {
                    Console.WriteLine("Invalid Shot location. Please try again!");
                }

            } while (isValidShot == false);

            bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);

            GameLogic.MarkShotResult(activeplayer, row, column, isAHit);

            DisplayShotResults(row, column, isAHit);
        }

        private static void DisplayShotResults(string row, int column, bool isAHit)
        {
            if (isAHit)
            {
                Console.WriteLine($"{ row }{ column } is a Hit!");
            }
            else
            {
                Console.WriteLine($"{ row }{ column } is a miss.");
            }

            Console.WriteLine();
        }

        private static string AskForShot(PlayerInfoModel player)
        {
            Console.Write($"{ player.usersName }, please enter your shot selection: ");
            string output = Console.ReadLine();
            return output;
        }

        private static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            string currentRow = activePlayer.ShotGrid[0].spotLetter;

            foreach (var gridSpot in activePlayer.ShotGrid)
            {
                if (gridSpot.spotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.spotLetter;
                }
                if (gridSpot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($"{gridSpot.spotLetter} {gridSpot.spotNumber}");
                }
               else if (gridSpot.Status == GridSpotStatus.hit)
                {
                    Console.Write("X");
                }
                else if (gridSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write("O");
                }
                else
                {
                    Console.Write("?");
                }
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to Battleship!");
            Console.WriteLine("Created by David Huynh");
            Console.WriteLine();
        }
        

        private static PlayerInfoModel CreatePlayer(string playerTitle)
        {
            PlayerInfoModel output = new PlayerInfoModel();

            Console.WriteLine($"Player information for {playerTitle}");

            output.usersName = AskForUsersName();
            

            GameLogic.InitializeGrid(output);

            PlaceShips(output);

            Console.Clear();
           
            return output;






        }
        private static string AskForUsersName()
        {
            Console.Write("What is your name?: ");
            string output = Console.ReadLine();

            return output;

         
        }

        private static void PlaceShips(PlayerInfoModel model)
        {
            do
            {
                Console.Write($"Where do you want to place the ship number {model.ShipLocations.Count + 1 }: ");
                string location = Console.ReadLine();

                bool isValidLocation = false;
                try
                {
                 isValidLocation = GameLogic.PlaceShip(model, location);
                }
                
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                if (isValidLocation == false)
                {
                    Console.WriteLine("that was not a valid location. Please try again.");
                }
            } while (model.ShipLocations.Count < 5);
        }
    }
}
