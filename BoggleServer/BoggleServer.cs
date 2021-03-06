﻿// Assignment implementation written by April Martin & Conan Zhang
// for CS3500 Assignment #8. November, 2014.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Timers;
using CustomNetworking;
using MySql.Data.MySqlClient;

namespace BoggleClient
{
    /// <summary>
    /// Server to receive and send boggle information.
    /// </summary>
    public class BoggleServer
    {
        private TcpListener server;

        //length of game
        private int time;
        
        //dictionary to check against and characters used
        private String boardSetup;
        
        // Queue of clients who are waiting to play
        private Queue<Client> clients;
        // List of matches or pairs of clients to iterate over
        private List<Match> matches;

        // Holds words to compare against
        private HashSet<string> dictionary;

        /// <summary>
        /// Two-argument constructor that takes the time limit for the game and the path
        /// for the list of legal words.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="dictionary"></param>
        public BoggleServer(int time, HashSet<String> dictionary)
        {
            this.time = time;
            this.clients = new Queue<Client>();
            this.matches = new List<Match>();
            this.dictionary = dictionary;

            server = new TcpListener(IPAddress.Any, 2000);
            server.Start();
            server.BeginAcceptSocket(AcceptConnection, null);
        }

        /// <summary>
        /// Three-argument constructor that calls the two-argument constructor,
        /// but also initializes the board so that it matches the string provided by the user.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="dictionary"></param>
        /// <param name="boardSetup"></param>
        public BoggleServer(int time, HashSet<String> dictionary, String boardSetup) :
            this(time, dictionary)
        {
            this.boardSetup = boardSetup;
        }


        /// <summary>
        /// Alternate four-argument constructor for the purpose of testing that allows you to pick a different port.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="dictionary"></param>
        /// <param name="boardSetup"></param>
        /// <param name="port"></param>
        public BoggleServer(int time, HashSet<String> dictionary,  String boardSetup, int port)
        {
            this.time = time;
            this.clients = new Queue<Client>();
            this.matches = new List<Match>();
            this.dictionary = dictionary;
            this.boardSetup = boardSetup;

            server = new TcpListener(IPAddress.Any, port);
             server.Start();
            server.BeginAcceptSocket(AcceptConnection, null);
        }

        /// <summary>
        /// Callback for when we receive a connection to accept.
        /// </summary>
        /// <param name="result"></param>
        private void AcceptConnection(IAsyncResult result)
        {
            Socket s = server.EndAcceptSocket(result);
            StringSocket ss = new StringSocket(s, UTF8Encoding.Default);

            // Start a new thread to deal with this client
            ss.BeginReceive(receiveName, ss);

            // Go back to listening for other client connection requests
            server.BeginAcceptSocket(AcceptConnection, null);
        }

        /// <summary>
        /// Callback for when we receive a client's name.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="e"></param>
        /// <param name="ss"></param>
        private void receiveName(String input, Exception e, object ss)
        {
            lock (clients)
            {
                //check if client has disconnected or an exception has been thrown
                if ((input == null && e == null) || e != null)
                {
                    clients.Dequeue();
                    return;
                }

                input = checkFormat("PLAY", input, ss);
                // If the input was valid (i.e., if CheckFormat did not return null),
                // then add the new client to the list of waiting clients.
                if (input != null)
                {
                    clients.Enqueue(new Client(input, (ss as StringSocket)));
                    pairIfPossible();
                    //(ss as StringSocket).BeginReceive(receiveName, ss);
                }
                // Give them another chance to enter their name.
                else
                {
                    (ss as StringSocket).BeginReceive(receiveName, ss);
                }
            }
        }


        /// <summary>
        /// Check if we can make a match and make one if possible.
        /// </summary>
        private void pairIfPossible()
        {
            // End the function without doing anything if there aren't any pairs.
            if (clients.Count < 2)
            {
                return;
            }

            Client p1 = clients.Dequeue();

            Client p2 = clients.Dequeue();

            //set opponents
            p1.opponent = p2;
            p2.opponent = p1;

            BoggleBoard bb;

            //If we were provided a setup
            if (boardSetup != null)
            {
                bb = new BoggleBoard(boardSetup);
            }
            //let the boggle board handle setup
            else
            {
                bb = new BoggleBoard();
            }

            Match m = new Match(p1, p2, time, bb, dictionary);
        }

        /// <summary>
        /// Checks if the command a client sent was valid.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="input"></param>
        /// <param name="ss"></param>
        /// <returns></returns>
        public static string checkFormat(String keyword, string input, object ss)
        {
            //Remove unneccesary whitespace
            input.Trim();

            input = input.ToUpper();
            Regex pattern = new Regex(@"^" + keyword + " ");

            // If the user input does not begin with the keyword then reject it and end the function.
            if (!pattern.IsMatch(input))
            {
                (ss as StringSocket).BeginSend("IGNORING " + input + '\n', (ex, o) => { }, null);
                return null;
            }

            // Otherwise, remove the keyword to isolate the content.
            int index = input.IndexOf(' ');
            return input.Substring(index + 1).Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static int scoreWord(String word)
        {
            if (word.Length <= 4)
            {
                return 1;
            }
            else if (word.Length == 5)
            {
                return 2;
            }
            else if (word.Length == 6)
            {
                return 3;
            }
            else if (word.Length == 7)
            {
                return 5;
            }
            else
            {
                // Jackpot! =D
                return 11;
            }
        }

    }

    /// <summary>
    /// Groups information regarding Clients to store in queue.
    /// </summary>
    internal class Client
    {
        public String name;
        public StringSocket ss;

        //public HashSet<string> words;
        public HashSet<string> legalWords;
        public HashSet<string> illegalWords;

        public Client opponent{get; set;}
        public int score { get; set; }

        public Client(string name, StringSocket ss)
        {
            this.name = name;
            this.ss = ss;
            this.legalWords = new HashSet<string>();
            this.illegalWords = new HashSet<string>();
            this.opponent = null;
            this.score = 0;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    internal class Match
    {
        private BoggleBoard bb;

        private HashSet<string> dictionary;
        private HashSet<string> commonWords;

        private Client p1;
        private Client p2;

        private int time;
        //used to pass to database
        private int startTime;
        private Timer gameTime;

        // Database to connect to
        private String connectionString = "server=atr.eng.utah.edu;database=cs3500_conanz;uid=cs3500_conanz;password=886456555";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="time"></param>
        /// <param name="bb"></param>
        /// <param name="d"></param>
        public Match(Client p1, Client p2, int time, BoggleBoard bb, HashSet<string> d)
        {
            this.p1 = p1;
            this.p2 = p2;

            this.time = time;
            this.startTime = time;
            this.bb = bb;
            this.dictionary = d;
            this.commonWords = new HashSet<string>();

            gameTime = new Timer(1000);
            gameTime.Elapsed += updateGame;
            gameTime.Enabled = true;

            // Send the start message to both players
            p1.ss.BeginSend("START " + bb.ToString() + " " + time + " " + p2.name +'\n', (e, o) => { }, null);
            p2.ss.BeginSend("START " + bb.ToString() + " " + time + " " + p1.name + '\n', (e, o) => { }, null);
            // Begin listening for input from both players
            p1.ss.BeginReceive(receiveCallback, p1);
            p2.ss.BeginReceive(receiveCallback, p2);
            //TODO: Keeping track of scores and words.
        }

        /// <summary>
        /// Update current match
        /// </summary>
        /// <param name="source"></param>
        /// <param name="ev"></param>
        private void updateGame(Object source, ElapsedEventArgs ev)
        {
            time--;
            p1.ss.BeginSend("TIME " + time + '\n', (e, o) => { }, null);
            p2.ss.BeginSend("TIME " + time + '\n', (e, o) => { }, null);

            // TODO: Check if time has expired, handle accordingly.
            if (time == 0)
            {
                // Send out the final score
                p1.ss.BeginSend("SCORE " + p1.score + " " + p2.score + '\n', (e, o) => { }, null);
                p2.ss.BeginSend("SCORE " + p2.score + " " + p1.score + '\n', (e, o) => { }, null);
                // Send out game summary
                p1.ss.BeginSend("STOP " +
                      p1.legalWords.Count + " " + GetWords(p1.legalWords) + " " +
                      p2.legalWords.Count + " " + GetWords(p2.legalWords) + " " +
                      commonWords.Count + " " + GetWords(commonWords) + " " +
                      p1.illegalWords.Count + " " + GetWords(p1.illegalWords) + " " +
                      p2.illegalWords.Count + " " + GetWords(p2.illegalWords) + '\n',
                      (e, o) => { }, null);
                p2.ss.BeginSend("STOP " +
                      p2.legalWords.Count + " " + GetWords(p2.legalWords) + " " +
                      p1.legalWords.Count + " " + GetWords(p1.legalWords) + " " +
                      commonWords.Count   + " " + GetWords(commonWords)   + " " +
                      p2.illegalWords.Count + " " + GetWords(p2.illegalWords) + " " +
                      p1.illegalWords.Count + " " + GetWords(p1.illegalWords) + '\n',
                      (e, o) => { }, null);

                updateDatabase();
            }
        }

        /// <summary>
        /// Send information to database.
        /// </summary>
        private void updateDatabase()
        {

            // Add all data to the database. =) Cause that'll be easy.
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    HashSet<String> commands = new HashSet<String>();
                    
                    //Get current game ID
                    MySqlCommand getID = new MySqlCommand("select max(Game_ID) AS Game_ID from Games", conn);
                    UInt32 gameID = (UInt32)getID.ExecuteScalar();
                    gameID++;

                    //Update Games table
                    commands.Add("insert into Games (P1_name, P2_name, Date, Time, Setup, Time_limit, P1_score, P2_score) values('" + p1.name + "','" + p2.name + "','" + DateTime.Now.ToString("M/d/yyyy") + "', '" + DateTime.Now.ToString("h:mm:ss tt") + "','" + bb.ToString() + "'," + startTime + "," + p1.score + "," + p2.score + ")");

                    // Update Words table, making a command for each word in the summary
                    foreach (string legalword in p1.legalWords)
                    {
                        commands.Add("insert into Words (Words, Game_ID, Player_name, Legality, Shared) values('" + legalword + "'," + gameID + ",'" + p1.name + "', 'Yes', 'No' )");
                    }
                    foreach (string illegalWord in p1.illegalWords)
                    {
                        commands.Add("insert into Words (Words, Game_ID, Player_name, Legality, Shared) values('" + illegalWord + "'," + gameID + ",'" + p1.name + "', 'No', 'No' )");
                    }
                    foreach (string commonWord in commonWords)
                    {
                        commands.Add("insert into Words (Words, Game_ID, Player_name, Legality, Shared) values('" + commonWord + "'," + gameID + ",'" + p1.name + "', 'Yes', 'Yes' )");
                        commands.Add("insert into Words (Words, Game_ID, Player_name, Legality, Shared) values('" + commonWord + "'," + gameID + ",'" + p2.name + "', 'Yes' , 'Yes')");
                    }
                    foreach (string legalword in p2.legalWords)
                    {
                        commands.Add("insert into Words (Words, Game_ID, Player_name, Legality, Shared) values('" + legalword + "'," + gameID + ",'" + p2.name + "', 'Yes', 'No' )");
                    }
                    foreach (string illegalWord in p2.illegalWords)
                    {
                        commands.Add("insert into Words (Words, Game_ID, Player_name, Legality, Shared) values('" + illegalWord + "'," + gameID + ",'" + p2.name + "', 'No', 'No' )");
                    }

                    // Execute all commands
                    foreach (string s in commands)
                    {
                        MySqlCommand command = new MySqlCommand(s, conn);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        /// <summary>
        /// Compile words from a set of words into a string.
        /// </summary>
        /// <param name="words"></param>
        private string GetWords(HashSet<string> words)
        {
            if (words.Count > 0)
            {
                string wordList = "";
                foreach (string s in words)
                {
                    wordList += s;
                    wordList += " ";
                }
                return wordList;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Callback for when the server receieves words.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="ex"></param>
        /// <param name="p"></param>
        private void receiveCallback(String input, Exception ex, object p)
        {
            // Ignore their crap if they're trying to add words after the match is over.
            if (time == 0)
            {
                return;
            }

            Client player = (p as Client); 
            StringSocket ss = player.ss;

            //Check if player has disconnected
            if ((input == null && ex == null) || ex != null)
            {
                player.opponent.ss.BeginSend("TERMINATED\n", (e, o) => { }, null);
                player.opponent.ss.Close();
                return;
            }

            input = BoggleServer.checkFormat("WORD", input, ss);
            // If   (a) the input was invalid
            // or   (b) they provided a word with less than three characters
            // or   (c) the input is already in the player's list
            // ...Then start listening again for new input and return.
            if (input == null || player.legalWords.Contains(input) || player.illegalWords.Contains(input) || input.Length < 3)
            {
                ss.BeginReceive(receiveCallback, player);
                return;
            }

            // If it is in the dictionary AND it is on the boggle board:
            if (dictionary.Contains(input) && bb.CanBeFormed(input))
            {
                // If it's in the opponent's list
                if (player.opponent.legalWords.Contains(input))
                {
                    player.opponent.legalWords.Remove(input);
                    player.opponent.score -= BoggleServer.scoreWord(input);
                    commonWords.Add(input);
                }
                else
                {
                    player.legalWords.Add(input);
                    player.score += BoggleServer.scoreWord(input);
                }
            }
            // If it's illegal:
            else
            {
                player.illegalWords.Add(input);
                player.score--;
            }
            
            // Finally, send out the updated score to both players.
            ss.BeginSend("SCORE " + player.score + " " + player.opponent.score + '\n', (e, o) => { }, null);
            player.opponent.ss.BeginSend("SCORE " + player.opponent.score + " " + player.score + '\n', (e, o) => { }, null);

            ss.BeginReceive(receiveCallback, player);
        }
    }
}
