using System;
using System.IO;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PacManMonogame.Core;

namespace PacManMonogame
{
    // Enumérations utilisées pour la navigation
    public enum MenuState
    {
        Menu,
        Jouer,
        HowTo,
        Score
    }

    public class Game1 : Game
    {
        // DECLARATION DES VARIABLES NECESSAIRES ------------------------------------------
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public const int WINDOW_WIDTH = 670;
        public const int WINDOW_HEIGHT = 400;
        World world;
        Player player;
        private string playerName = string.Empty;
        private bool isEnteringName = false;

        //Scores
        private List<string> highScores = new List<string>();

        // Temps
        private float elapsedTime = 0f;
        private float LastElapsedTime = 0f;
        private bool isCountingTime = true;

        //Police
        private SpriteFont font;
        private SpriteFont menuFont;

        //Selection menu
        private int selectedOption;
        private MenuState menuState;

        //music
        Song song;

        //Fin de jeu
        private bool isGameOver = false;
        private Vector2 zonedefin = new Vector2(644, 228);
        private Vector2 zonedefin2 = new Vector2(645, 228); 
        private Vector2 zonedefin3 = new Vector2(646, 228);  
        private Vector2 zonedefin4 = new Vector2(647, 228); 
        private Vector2 zonedefin5 = new Vector2(648, 228);  
       
        // FIN DECLARATION VARIABLES --------------------------------------------------------

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            menuState = MenuState.Menu;
            world = new World(new Color(0, 128, 248)); // MEP monde 2D
            player = new Player(8, 13, 13, world);     // Création de "PAC": 8 Images de 13x13 pixels + monde


            base.Initialize();
        }

        private void LoadHighScores()
        {
            string filePath = "score.txt";

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                highScores.AddRange(lines);
                highScores.Sort((s1, s2) =>
                {
                    int score1 = int.Parse(s1.Split(':')[1].Trim());
                    int score2 = int.Parse(s2.Split(':')[1].Trim());
                    return score2.CompareTo(score1);
                });

                if (highScores.Count > 10)
                {
                    highScores = highScores.Take(10).ToList();
                }
            }
        }

        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            // 0.0f is silent, 1.0f is full volume
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(song);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Musique
            this.song = Content.Load<Song>("music");
            MediaPlayer.Play(song);
            //  Uncomment the following line will also loop the song
            //  MediaPlayer.IsRepeating = true;
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;


            // Charger la police
            font = Content.Load<SpriteFont>("Police");
            menuFont = Content.Load<SpriteFont>("Police");

            // TODO: use this.Content to load your game content here
            // Chargement du monde
            world.Texture = Content.Load<Texture2D>("world22");
            world.Position = new Vector2(0, 0);

            // Différenciation des couleurs
            world.colorTab = new Color[world.Texture.Width * world.Texture.Height];
            world.Texture.GetData<Color>(world.colorTab);

            //Chargement de "PAC"
            player.Texture = Content.Load<Texture2D>("pacman");
            player.Position = new Vector2(7, 7);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (menuState == MenuState.Menu)
            {
                // Quitter le jeu si la touche Échap est enfoncée
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                // Sélectionner une option du menu en appuyant sur les touches numériques
                if (Keyboard.GetState().IsKeyDown(Keys.D1) || Keyboard.GetState().IsKeyDown(Keys.NumPad1))
                    selectedOption = 1;
                else if (Keyboard.GetState().IsKeyDown(Keys.D2) || Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                    selectedOption = 2;
                else if (Keyboard.GetState().IsKeyDown(Keys.D4) || Keyboard.GetState().IsKeyDown(Keys.NumPad4))
                    selectedOption = 4; 
                else if (Keyboard.GetState().IsKeyDown(Keys.D3) || Keyboard.GetState().IsKeyDown(Keys.NumPad3))
                    selectedOption = 3;

                // Passer à l'état "Score" si l'option "Score" est sélectionnée
                if (selectedOption == 2 && Keyboard.GetState().IsKeyDown(Keys.Enter))
                    menuState = MenuState.Score;
                //Quitter le jeu
                if (selectedOption == 4 && Keyboard.GetState().IsKeyDown(Keys.Enter))
                    Exit();
                // Si jouer
                if (selectedOption == 1 && Keyboard.GetState().IsKeyDown(Keys.Enter))
                    menuState = MenuState.Jouer; 
                // Si howto
                if (selectedOption == 3 && Keyboard.GetState().IsKeyDown(Keys.Enter))
                    menuState = MenuState.HowTo;

            }
            else if (menuState == MenuState.Score)
            {
                // Revenir au menu de sélection en appuyant sur la touche "0"
                if (Keyboard.GetState().IsKeyDown(Keys.D0) || Keyboard.GetState().IsKeyDown(Keys.NumPad0))
                    menuState = MenuState.Menu;
            }
            else if (menuState == MenuState.HowTo)
            {
                // Revenir au menu de sélection en appuyant sur la touche "0"
                if (Keyboard.GetState().IsKeyDown(Keys.D0) || Keyboard.GetState().IsKeyDown(Keys.NumPad0))
                    menuState = MenuState.Menu;
            }
            else if (menuState == MenuState.Jouer)
            {
                // Verifier si le personnage a atteint la zone de fin
                if (player.Position == zonedefin || player.Position == zonedefin2 || player.Position == zonedefin3 || player.Position == zonedefin4 || player.Position == zonedefin5)
                {
                    isCountingTime = false;
                    isGameOver = true;


                    isEnteringName = true;
                    // Écouter les entrées clavier
                    KeyboardState keyboardState = Keyboard.GetState();


                    // Récupérer la saisie du clavier
                    Keys[] pressedKeys = keyboardState.GetPressedKeys();

                    // Vérifier si une touche est enfoncée
                    if (pressedKeys.Length > 0)
                    {
                        Keys pressedKey = pressedKeys[0];

                        // Ajouter la lettre à la chaîne du nom du joueur
                        if (pressedKey >= Keys.A && pressedKey <= Keys.Z)
                        {
                            playerName += pressedKey.ToString();
                        }
                        else if (pressedKey == Keys.Space)
                        {
                            // Si la touche Entrée est enfoncée, terminer la saisie du nom du joueur
                            isEnteringName = false;

                            // Enregistrer le score avec le nom du joueur dans un fichier texte
                            string scoreText = "Nom : " + playerName + ", Score : " + LastElapsedTime;

                            string filePath = "score.txt";

                            using (StreamWriter writer = new StreamWriter(filePath, true))
                            {
                                writer.WriteLine(scoreText);
                            }
                        }
                    }
                }

                // Mettre à jour le temps écoulé
                if (isCountingTime)
                {
                    elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }



                player.Move(Keyboard.GetState());       // Récupération des entrées clavier
                player.UpdateFrame(gameTime);           // UPDATE FRAME DE PAC
            }

            // TODO: Add your update logic here
            _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            switch(menuState)
            {
                case MenuState.Jouer:
                    GraphicsDevice.Clear(Color.Black);
                    world.Draw(_spriteBatch);

                    // Afficher le temps écoulé en bas de la fenêtre
                    string timerText = "Temps ecoule : " + elapsedTime.ToString("0.00");
                    // PErmet d'afficher la position en temps réel du personnage
                    //string timerText = player.Position.ToString();
                    Vector2 timerPosition = new Vector2(10, GraphicsDevice.Viewport.Height - 30);
                    _spriteBatch.DrawString(font, timerText, timerPosition, Color.White);

                    // Fin de jeu 
                    if (isGameOver)
                    {
                        // Afficher un écran de fin de jeu
                        // Score temps
                        LastElapsedTime = elapsedTime;
                        string gameOverText = "Bravo, vous avez gagne en " + LastElapsedTime + " secondes \n Appuyez sur ECHAP pour quitter";
                        Vector2 gameOverPosition = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
                        Vector2 gameOverOrigin = font.MeasureString(gameOverText) / 2;
                        _spriteBatch.DrawString(font, gameOverText, gameOverPosition, Color.Red, 0f, gameOverOrigin, 1f, SpriteEffects.None, 0f);

                        // Afficher l'interface utilisateur pour la saisie du nom du joueur
                        if (isEnteringName)
                        {
                            string promptText = "Entrez votre nom :";
                            string inputText = playerName;

                            Vector2 promptPosition = new Vector2((GraphicsDevice.Viewport.Width - font.MeasureString(promptText).X) / 2, 100);
                            Vector2 inputPosition = new Vector2((GraphicsDevice.Viewport.Width - font.MeasureString(inputText).X) / 2, 150);

                            _spriteBatch.DrawString(font, promptText, promptPosition, Color.White);
                            _spriteBatch.DrawString(font, inputText, inputPosition, Color.White);


                        }
                    }
                    break;

                case MenuState.Menu:
                    GraphicsDevice.Clear(Color.Pink);

                    // Dessiner les options du menu
                    DrawTitle("LABYRINTH", new Vector2(100, 50));
                    DrawMenuOption("1 - Jouer", 1);
                    DrawMenuOption("2 - Score", 2);
                    DrawMenuOption("3 - Comment Jouer", 3);
                    DrawMenuOption("4 - Quitter", 4);
                    break;

                case MenuState.Score:
                    // Dessiner la page orange des scores
                    GraphicsDevice.Clear(Color.Orange);
                    // Afficher les meilleurs scores
                    DrawTitle("High scores - 0 pour quitter", new Vector2(100, 100));
                    Vector2 position = new Vector2(100, 100);
                    float lineHeight = font.LineSpacing;

                    for (int i = 0; i < highScores.Count; i++)
                    {
                        _spriteBatch.DrawString(font, highScores[i], position + new Vector2(0, i * lineHeight), Color.White);
                    }
                    break;

                case MenuState.HowTo:
                    // Dessiner la page orange des scores
                    GraphicsDevice.Clear(Color.Green);
                    // Afficher le tuto
                    DrawTitle("Comment jouer\n\n Le but est de sortir du labyrinthe le plus rapidement possible \n DEPLACEMENTS: \nZ: Haut\nQ: Gauche\nS: Bas\nD: Droite", new Vector2(340, 50));
                   
                    break;


            }

            _spriteBatch.End();

            player.DrawAnimation(_spriteBatch);

            base.Draw(gameTime);
        }

        private void DrawMenuOption(string optionText, int optionNumber)
        {
            Vector2 optionPosition = new Vector2(100, 100 + optionNumber * 50);
            Color optionColor = (optionNumber == selectedOption) ? Color.Yellow : Color.White;

            _spriteBatch.DrawString(menuFont, optionText, optionPosition, optionColor);
        }

        private void DrawTitle(string titleText, Vector2 position)
        {
            Vector2 titleSize = menuFont.MeasureString(titleText);
            Vector2 titlePosition = position - new Vector2(titleSize.X / 2, 0);
            Color titleColor = Color.White;

            _spriteBatch.DrawString(menuFont, titleText, titlePosition, titleColor);
        }


    }
}