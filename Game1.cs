using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using System.Reflection.Metadata;


namespace DutarGame;
//
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    SpriteBatch _spriteBatch;
    List<SoundEffect> soundEffects;
    SoundEffectInstance
        soundInstanceA,
        soundInstanceASBF,
        soundInstanceB,
        soundInstanceC,
        soundInstanceCSDF,
        soundInstanceD,
        soundInstanceDSEF,
        soundInstanceE,
        soundInstanceF,
        soundInstanceFSGF,
        soundInstanceG,
        soundInstanceGSAF;
    List<SoundEffect> sounds;
    SoundEffectInstance song;
    Texture2D texture;
    Vector2 texturePos;
    RenderTarget2D renderTarget;
    MouseState _currentMouseState;
    MouseState _previousMouseState;
    KeyboardState _currentKeyboardState;
    KeyboardState _previousKeyboardState;
    
    private GameStates _gameState;
    private SpriteFont font, menuFont, helpFont;
    private int score = 0;
    // Button setup
    private Rectangle exitButtonMM, infoOnDutarButton, freePlayDutarButton, exitButtonFPD, exitButtonFPDOutline, line, line2, line3, popupFPD, popupFPDOutline, popupFPDStay, popupFPDReturn, helpButton, helpButtonOutline, backgroundRectangle, backgroundRectangleOutline;
    private bool isMenu, isHelp;

    // new
    float pitch = 1.0f;
    float pitch2 = 0.0f;

    Stack<SpriteBatch> screenSpriteBatches = new Stack<SpriteBatch>();

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        soundEffects = new List<SoundEffect>();
        sounds = new List<SoundEffect>();
        texturePos = new Vector2(0, 50);
    }

    public enum GameStates
    {
        Menu,
        Dutar,
        Info,
        Pause
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        // Button setup
        // 800x400 res.
        freePlayDutarButton = new Rectangle(300, 250, 200, 50);
        infoOnDutarButton =   new Rectangle(300, 325, 200, 50);
        exitButtonMM =        new Rectangle(300, 400, 200, 50);

        exitButtonFPD = new Rectangle(10, 5, 20, 20);
        exitButtonFPDOutline = new Rectangle(9, 4, 22, 22);
        line = new Rectangle(15, 8, 10, 3);
        line2 = new Rectangle(15, 13, 10, 3);
        line3 = new Rectangle(15, 18, 10, 3);

        helpButtonOutline = new Rectangle(769, 4, 22, 22);
        helpButton = new Rectangle(770, 5, 20, 20);

        popupFPDOutline = new Rectangle(140, 50, 500, 300);
        popupFPD =        new Rectangle(142, 52, 496, 296);
        popupFPDStay = new Rectangle(180, 260, 200, 50);
        popupFPDReturn = new Rectangle(400, 260, 200, 50);

        backgroundRectangle = new Rectangle(77, 32, 646, 396);
        backgroundRectangleOutline = new Rectangle(75, 30, 650, 400);


        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        soundEffects.Add(Content.Load<SoundEffect>("Dutar_Sounds/dutar_A"));
        soundEffects.Add(Content.Load<SoundEffect>("Dutar_Sounds/dutar_ASBF"));
        soundEffects.Add(Content.Load<SoundEffect>("Dutar_Sounds/dutar_B"));
        soundEffects.Add(Content.Load<SoundEffect>("Dutar_Sounds/dutar_C"));
        soundEffects.Add(Content.Load<SoundEffect>("Dutar_Sounds/dutar_CSDF"));
        soundEffects.Add(Content.Load<SoundEffect>("Dutar_Sounds/dutar_D"));
        soundEffects.Add(Content.Load<SoundEffect>("Dutar_Sounds/dutar_DSEF"));
        soundEffects.Add(Content.Load<SoundEffect>("Dutar_Sounds/dutar_E"));
        soundEffects.Add(Content.Load<SoundEffect>("Dutar_Sounds/dutar_F"));
        soundEffects.Add(Content.Load<SoundEffect>("Dutar_Sounds/dutar_FSGF"));
        soundEffects.Add(Content.Load<SoundEffect>("Dutar_Sounds/dutar_G"));
        soundEffects.Add(Content.Load<SoundEffect>("Dutar_Sounds/dutar_GSAF"));

        soundInstanceA = soundEffects[0].CreateInstance();
        soundInstanceASBF = soundEffects[1].CreateInstance();
        soundInstanceB = soundEffects[2].CreateInstance();
        soundInstanceC = soundEffects[3].CreateInstance();
        soundInstanceCSDF = soundEffects[4].CreateInstance();
        soundInstanceD = soundEffects[5].CreateInstance();
        soundInstanceDSEF = soundEffects[6].CreateInstance();
        soundInstanceE = soundEffects[7].CreateInstance();
        soundInstanceF = soundEffects[8].CreateInstance();
        soundInstanceFSGF = soundEffects[9].CreateInstance();
        soundInstanceG = soundEffects[10].CreateInstance();
        soundInstanceGSAF = soundEffects[11].CreateInstance();

        sounds.Add(Content.Load<SoundEffect>("Turkmen Dutar instrumental performance by Oghlan Bakhshi"));
        song = sounds[0].CreateInstance();

        song.IsLooped = true;
        song.Play();

        font = Content.Load<SpriteFont>("File");
        menuFont = Content.Load<SpriteFont>("MenuFonts");
        helpFont = Content.Load<SpriteFont>("HelpFonts");

        texture = this.Content.Load<Texture2D>("dutar3");

        var semitone = Math.Pow(2, 1.0 / 12);
        var upOneTone = semitone * semitone;

        //instance.IsLooped = true;
        //instance.Play();
        // TODO: use this.Content to load your game content here
    }

    protected override void UnloadContent()
    {
        base.UnloadContent();
        _spriteBatch.Dispose();
        
    }

    void UpdateMenu(GameTime gameTime)
    {
        // TODO
        var mouseState = Mouse.GetState();

        // Button setup
        if (exitButtonMM.Contains(_currentMouseState.X, _currentMouseState.Y) && (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released))
        {
            //Console.WriteLine("Exit Button clicked!");
            Exit();
        }
        else if (infoOnDutarButton.Contains(_currentMouseState.X, _currentMouseState.Y) && (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released))
        {
            //Console.WriteLine("Info On Dutar Button clicked!");
            _gameState = GameStates.Info;
        }
        else if (freePlayDutarButton.Contains(_currentMouseState.X, _currentMouseState.Y) && (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released))
        {
            //Console.WriteLine("Play Dutar clicked!");
            song.Pause();
            _gameState = GameStates.Dutar;
        }
    }

    void UpdateDutar(GameTime gameTime)
    {
        // TODO
        var mouseState = Mouse.GetState();
        // Button setup

        // Return to main menu button
        if (exitButtonFPD.Contains(_currentMouseState.X, _currentMouseState.Y) && (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released))
        {
            //Console.WriteLine("Exit Button clicked!");
            //_gameState = GameStates.Menu;
            if (isMenu == false && isHelp == false)
            {
                isMenu = true;
            }
            else if (isMenu == true)
            {
                isMenu = false;
            }
        }

        // Help Button
        if (helpButton.Contains(_currentMouseState.X, _currentMouseState.Y) && (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released))
        {
            if (isHelp == false && isHelp == false)
            {
                isHelp = true;
            }
            else if (isHelp == true)
            {
                isHelp = false;
            }
        }

        if (isMenu == true)
        {
            if (popupFPDReturn.Contains(_currentMouseState.X, _currentMouseState.Y) && (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released))
            {
                //Console.WriteLine("Exit Button clicked!");
                isMenu = false;
                _gameState = GameStates.Menu;
                song.Resume();
                //isMenu = false;
            }
            else if (popupFPDStay.Contains(_currentMouseState.X, _currentMouseState.Y) && (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released))
            {
                //Console.WriteLine("Exit Button clicked!");
                isMenu = false;
            }
        }

        var keyboardState = Keyboard.GetState();
        // A
        if (_currentKeyboardState.IsKeyDown(Keys.X) && _previousKeyboardState.IsKeyUp(Keys.X))
        {
            if (_currentKeyboardState.IsKeyDown(Keys.OemMinus))
            {
                soundInstanceA.Pitch = -0.5f;
                soundInstanceA.Play();
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.OemPlus))
            {
                soundInstanceA.Pitch = 0.5f;
                soundInstanceA.Play();
            }
            else
            {
                soundInstanceA.Pitch = 0.0f;
                soundInstanceA.Play();
            }
        }
        else if (_currentKeyboardState.IsKeyUp(Keys.X))
        {
            soundInstanceA.Stop();
        }

        // A-sharp/B-flat
        if (_currentKeyboardState.IsKeyDown(Keys.C) && _previousKeyboardState.IsKeyUp(Keys.C))
        {
            if (_currentKeyboardState.IsKeyDown(Keys.OemMinus))
            {
                soundInstanceASBF.Pitch = -0.5f;
                soundInstanceASBF.Play();
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.OemPlus))
            {
                soundInstanceASBF.Pitch = 0.5f;
                soundInstanceASBF.Play();
            }
            else
            {
                soundInstanceASBF.Pitch = 0.0f;
                soundInstanceASBF.Play();
            }
        }
        else if (_currentKeyboardState.IsKeyUp(Keys.C))
        {
            soundInstanceASBF.Stop();
        }

        // B
        if (_currentKeyboardState.IsKeyDown(Keys.V) && _previousKeyboardState.IsKeyUp(Keys.V))
        {
            if (_currentKeyboardState.IsKeyDown(Keys.OemMinus))
            {
                soundInstanceB.Pitch = -0.5f;
                soundInstanceB.Play();
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.OemPlus))
            {
                soundInstanceB.Pitch = 0.5f;
                soundInstanceB.Play();
            }
            else
            {
                soundInstanceB.Pitch = 0.0f;
                soundInstanceB.Play();
            }
        }
        else if (_currentKeyboardState.IsKeyUp(Keys.V))
        {
            soundInstanceB.Stop();
        }

        // C
        if (_currentKeyboardState.IsKeyDown(Keys.Q) && _previousKeyboardState.IsKeyUp(Keys.Q))
        {
            if (_currentKeyboardState.IsKeyDown(Keys.OemMinus))
            {
                soundInstanceC.Pitch = -0.5f;
                soundInstanceC.Play();
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.OemPlus))
            {
                soundInstanceC.Pitch = 0.5f;
                soundInstanceC.Play();
            }
            else
            {
                soundInstanceC.Pitch = 0.0f;
                soundInstanceC.Play();
            }
        }
        else if (_currentKeyboardState.IsKeyUp(Keys.Q))
        {
            soundInstanceC.Stop();
        }

        // C-sharp/D-flat
        if (_currentKeyboardState.IsKeyDown(Keys.W) && _previousKeyboardState.IsKeyUp(Keys.W))
        {
            if (_currentKeyboardState.IsKeyDown(Keys.OemMinus))
            {
                soundInstanceCSDF.Pitch = -0.5f;
                soundInstanceCSDF.Play();
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.OemPlus))
            {
                soundInstanceCSDF.Pitch = 0.5f;
                soundInstanceCSDF.Play();
            }
            else
            {
                soundInstanceCSDF.Pitch = 0.0f;
                soundInstanceCSDF.Play();
            }
        }
        else if (_currentKeyboardState.IsKeyUp(Keys.W))
        {
            soundInstanceCSDF.Stop();
        }

        // D
        if (_currentKeyboardState.IsKeyDown(Keys.E) && _previousKeyboardState.IsKeyUp(Keys.E))
        {
            if (_currentKeyboardState.IsKeyDown(Keys.OemMinus))
            {
                soundInstanceD.Pitch = -0.5f;
                soundInstanceD.Play();
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.OemPlus))
            {
                soundInstanceD.Pitch = 0.5f;
                soundInstanceD.Play();
            }
            else
            {
                soundInstanceD.Pitch = 0.0f;
                soundInstanceD.Play();
            }
        }
        else if (_currentKeyboardState.IsKeyUp(Keys.E))
        {
            soundInstanceD.Stop();
        }

        // D-sharp/E-flat
        if (_currentKeyboardState.IsKeyDown(Keys.R) && _previousKeyboardState.IsKeyUp(Keys.R))
        {
            if (_currentKeyboardState.IsKeyDown(Keys.OemMinus))
            {
                soundInstanceDSEF.Pitch = -0.5f;
                soundInstanceDSEF.Play();
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.OemPlus))
            {
                soundInstanceDSEF.Pitch = 0.5f;
                soundInstanceDSEF.Play();
            }
            else
            {
                soundInstanceDSEF.Pitch = 0.0f;
                soundInstanceDSEF.Play();
            }
        }
        else if (_currentKeyboardState.IsKeyUp(Keys.R))
        {
            soundInstanceDSEF.Stop();
        }

        // E
        if (_currentKeyboardState.IsKeyDown(Keys.A) && _previousKeyboardState.IsKeyUp(Keys.A))
        {
            if (_currentKeyboardState.IsKeyDown(Keys.OemMinus))
            {
                soundInstanceE.Pitch = -0.5f;
                soundInstanceE.Play();
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.OemPlus))
            {
                soundInstanceE.Pitch = 0.5f;
                soundInstanceE.Play();
            }
            else
            {
                soundInstanceE.Pitch = 0.0f;
                soundInstanceE.Play();
            }
        }
        else if (_currentKeyboardState.IsKeyUp(Keys.A))
        {
            soundInstanceE.Stop();
        }

        // F
        if (_currentKeyboardState.IsKeyDown(Keys.S) && _previousKeyboardState.IsKeyUp(Keys.S))
        {
            if (_currentKeyboardState.IsKeyDown(Keys.OemMinus))
            {
                soundInstanceF.Pitch = -0.5f;
                soundInstanceF.Play();
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.OemPlus))
            {
                soundInstanceF.Pitch = 0.5f;
                soundInstanceF.Play();
            }
            else
            {
                soundInstanceF.Pitch = 0.0f;
                soundInstanceF.Play();
            }
        }
        else if (_currentKeyboardState.IsKeyUp(Keys.S))
        {
            soundInstanceF.Stop();
        }

        // F-sharp/G-flat
        if (_currentKeyboardState.IsKeyDown(Keys.D) && _previousKeyboardState.IsKeyUp(Keys.D))
        {
            if (_currentKeyboardState.IsKeyDown(Keys.OemMinus))
            {
                soundInstanceFSGF.Pitch = -0.5f;
                soundInstanceFSGF.Play();
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.OemPlus))
            {
                soundInstanceFSGF.Pitch = 0.5f;
                soundInstanceFSGF.Play();
            }
            else
            {
                soundInstanceFSGF.Pitch = 0.0f;
                soundInstanceFSGF.Play();
            }
        }
        else if (_currentKeyboardState.IsKeyUp(Keys.D))
        {
            soundInstanceFSGF.Stop();
        }

        // G
        if (_currentKeyboardState.IsKeyDown(Keys.F) && _previousKeyboardState.IsKeyUp(Keys.F))
        {
            if (_currentKeyboardState.IsKeyDown(Keys.OemMinus))
            {
                soundInstanceG.Pitch = -0.5f;
                soundInstanceG.Play();
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.OemPlus))
            {
                soundInstanceG.Pitch = 0.5f;
                soundInstanceG.Play();
            }
            else
            {
                soundInstanceG.Pitch = 0.0f;
                soundInstanceG.Play();
            }
        }
        else if (_currentKeyboardState.IsKeyUp(Keys.F))
        {
            soundInstanceG.Stop();
        }

        // G-sharp/A-flat
        if (_currentKeyboardState.IsKeyDown(Keys.Z) && _previousKeyboardState.IsKeyUp(Keys.Z))
        {
            if (_currentKeyboardState.IsKeyDown(Keys.OemMinus))
            {
                soundInstanceGSAF.Pitch = -0.5f;
                soundInstanceGSAF.Play();
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.OemPlus))
            {
                soundInstanceGSAF.Pitch = 0.5f;
                soundInstanceGSAF.Play();
            }
            else
            {
                soundInstanceGSAF.Pitch = 0.0f;
                soundInstanceGSAF.Play();
            }
        }
        else if (_currentKeyboardState.IsKeyUp(Keys.Z))
        {
            soundInstanceGSAF.Stop();
        }
    }

    void UpdateInfo(GameTime gameTime)
    {
        // TODO
        var mouseState = Mouse.GetState();

        var keyboardState = Keyboard.GetState();

        if (exitButtonFPD.Contains(_currentMouseState.X, _currentMouseState.Y) && (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released))
        {
            //Console.WriteLine("Exit Button clicked!");
            //_gameState = GameStates.Menu;
            if (isMenu == false && isHelp == false)
            {
                isMenu = true;
            }
            else if (isMenu == true)
            {
                isMenu = false;
            }
        }

        if (isMenu == true)
        {
            if (popupFPDReturn.Contains(_currentMouseState.X, _currentMouseState.Y) && (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released))
            {
                //Console.WriteLine("Exit Button clicked!");
                isMenu = false;
                _gameState = GameStates.Menu;
                //isMenu = false;
            }
            else if (popupFPDStay.Contains(_currentMouseState.X, _currentMouseState.Y) && (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released))
            {
                //Console.WriteLine("Exit Button clicked!");
                isMenu = false;
            }
        }
    }

    void UpdatePause(GameTime gameTime)
    {
        // TODO
        var mouseState = Mouse.GetState();

        var keyboardState = Keyboard.GetState();
    }

    protected override void Update(GameTime gameTime)
    {
        _currentKeyboardState = Keyboard.GetState();
        _currentMouseState = Mouse.GetState();
        pitch = MathHelper.Clamp(pitch, 0.5f, 2.0f);    // new
        pitch2 = MathHelper.Clamp(pitch, 0.5f, 2.0f);    // new 

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
        switch (_gameState)
        {
            case GameStates.Menu:
                UpdateMenu(gameTime);
                break;
            case GameStates.Dutar:
                UpdateDutar(gameTime);
                break;
            case GameStates.Info:
                UpdateInfo(gameTime);
                break;
            case GameStates.Pause:
                UpdatePause(gameTime);
                break;
        }


        base.Update(gameTime);
        _previousKeyboardState = _currentKeyboardState;
        _previousMouseState = _currentMouseState;
    }

    void DrawMenu(GameTime gameTime)
    {
        // TODO

        _spriteBatch.Begin();

        // Button setup
        _spriteBatch.Draw(GetButtonTexture(), exitButtonMM, Color.Brown);
        _spriteBatch.Draw(GetButtonTexture(), infoOnDutarButton, Color.Brown);
        _spriteBatch.Draw(GetButtonTexture(), freePlayDutarButton, Color.Brown);

        _spriteBatch.DrawString(font, "Free Play Dutar", new Vector2(345, 265), Color.Black);
        _spriteBatch.DrawString(font, "Info On Dutar",   new Vector2(350, 340), Color.Black);
        _spriteBatch.DrawString(font, "Exit",            new Vector2(380, 415), Color.Black);

        // title
        _spriteBatch.DrawString(menuFont, "Dutar Simulator", new Vector2(300, 100), Color.Black);

        // music credit
        _spriteBatch.DrawString(font, "Music: Turkmen Dutar instrumental performance by Oghlan Bakhshi ", new Vector2(10, 450), Color.LightGray);


        _spriteBatch.End();

    }

    void DrawDutar(GameTime gameTime)
    {
        // TODO
        _spriteBatch.Begin();        
        _spriteBatch.Draw(texture, texturePos, Color.White);
        // Button setup
        _spriteBatch.Draw(GetButtonTexture(), exitButtonFPDOutline, Color.Black);
        _spriteBatch.Draw(GetButtonTexture(), exitButtonFPD, Color.WhiteSmoke);
        _spriteBatch.Draw(GetButtonTexture(), line, Color.Black);
        _spriteBatch.Draw(GetButtonTexture(), line2, Color.Black);
        _spriteBatch.Draw(GetButtonTexture(), line3, Color.Black);

        _spriteBatch.Draw(GetButtonTexture(), helpButtonOutline, Color.Black);
        _spriteBatch.Draw(GetButtonTexture(), helpButton, Color.WhiteSmoke);
        // 769, 4, 22, 22
        _spriteBatch.DrawString(font, "?", new Vector2(774, 5), Color.Black);
        _spriteBatch.End();

        if (isMenu == true)
        {
            // draw popup menu
            _spriteBatch.Begin();
            _spriteBatch.Draw(GetButtonTexture(), popupFPDOutline, Color.Black);
            _spriteBatch.Draw(GetButtonTexture(), popupFPD, Color.WhiteSmoke);
            _spriteBatch.Draw(GetButtonTexture(), popupFPDStay, Color.Brown);
            _spriteBatch.Draw(GetButtonTexture(), popupFPDReturn, Color.Brown);
            _spriteBatch.DrawString(font, "Stay", new Vector2(260, 273), Color.Black);
            _spriteBatch.DrawString(font, "Main Menu", new Vector2(460, 273), Color.Black);
            _spriteBatch.DrawString(menuFont, "Return to Main Menu?", new Vector2(260, 150), Color.Black);
            _spriteBatch.End();
        }

        if (isHelp == true)
        {
            // draw help popup
            _spriteBatch.Begin();
            _spriteBatch.Draw(GetButtonTexture(), popupFPDOutline, Color.Black);
            _spriteBatch.Draw(GetButtonTexture(), popupFPD, Color.WhiteSmoke);
            _spriteBatch.DrawString(menuFont, "How to Play", new Vector2(310, 110), Color.Black);
            _spriteBatch.DrawString(font, "The 'Dutar' is tuned to a G3. The notes are tied to the following keys:", new Vector2(155, 200), Color.Black);
            _spriteBatch.DrawString(font, "q: C, w: C-sharp, e: D, r: D-sharp, a: E, s: F, d: F-sharp,", new Vector2(155, 220), Color.Black);
            _spriteBatch.DrawString(font, "f: G, z: G-sharp, x: A, c: A-sharp v: B", new Vector2(155, 240), Color.Black);
            _spriteBatch.DrawString(font, "To shift the sound down or up, use the minus '-' and plus '+' keys", new Vector2(155, 260), Color.Black);

            _spriteBatch.End();
        }
    }

    void DrawInfo(GameTime gameTime)
    {
        // TODO
        _spriteBatch.Begin();

        // Main Menu button setup
        _spriteBatch.Draw(GetButtonTexture(), exitButtonFPDOutline, Color.Black);
        _spriteBatch.Draw(GetButtonTexture(), exitButtonFPD, Color.WhiteSmoke);
        _spriteBatch.Draw(GetButtonTexture(), line, Color.Black);
        _spriteBatch.Draw(GetButtonTexture(), line2, Color.Black);
        _spriteBatch.Draw(GetButtonTexture(), line3, Color.Black);

        // background
        _spriteBatch.Draw(GetButtonTexture(), backgroundRectangleOutline, Color.Black);
        _spriteBatch.Draw(GetButtonTexture(), backgroundRectangle, Color.WhiteSmoke);
        _spriteBatch.DrawString(menuFont, "What Is A Dutar?", new Vector2(280, 50), Color.Black);
        _spriteBatch.DrawString(font, "Dutar is a two stringed plucked instrument that originated in Iran but can be found all over", new Vector2(85, 90), Color.Black);
        _spriteBatch.DrawString(font, "in Central Asia. Ranging in cultural popularity, it can be seen in countries such as Iran,", new Vector2(85, 110), Color.Black);
        _spriteBatch.DrawString(font, "Afghanistan Turkmenistan, Tajikistan, Uzbekistan and parts of China. As such, many", new Vector2(85, 130), Color.Black);
        _spriteBatch.DrawString(font, "different methods for playing dutar have emerged with some choosing to pluck, strum or", new Vector2(85, 150), Color.Black);
        _spriteBatch.DrawString(font, "some combination. Take for instance Turkmens who both strum and pluck: within their", new Vector2(85, 170), Color.Black);
        _spriteBatch.DrawString(font, "musical traditions for dutar one can find two major types of performance, dutarchy", new Vector2(85, 190), Color.Black);
        _spriteBatch.DrawString(font, "(purely instrumental music), and bagshy (playing and singing which ranges from poetry to", new Vector2(85, 210), Color.Black);
        _spriteBatch.DrawString(font, "narrations). Each of these has differenttechniques used by the performers for creating", new Vector2(85, 230), Color.Black);
        _spriteBatch.DrawString(font, "a variety of auditory textures. Another feature that is not just found in Turkmen musical", new Vector2(85, 250), Color.Black);
        _spriteBatch.DrawString(font, "tradition but many other too is the use of improvisation. Many ethnic regions in Central", new Vector2(85, 270), Color.Black);
        _spriteBatch.DrawString(font, "Asia (and other parts of the world) have the musical texture of heterophony. It is achieved", new Vector2(85, 290), Color.Black);
        _spriteBatch.DrawString(font, "by having multiple instruments and or vocalists simultaneously vary a single melody.", new Vector2(85, 310), Color.Black);
        _spriteBatch.DrawString(font, "Some other common techniques are tapping the dutar for percussive sounds, pulsating the", new Vector2(85, 330), Color.Black);
        _spriteBatch.DrawString(font, "pitch of a string by quickly rocking the finger (similar to vibrato but still very much a", new Vector2(85, 350), Color.Black);
        _spriteBatch.DrawString(font, "unique, separate technique), and quickly strumming with a downward than immediate", new Vector2(85, 370), Color.Black);
        _spriteBatch.DrawString(font, "upwards motion which can be combined with plucking.", new Vector2(85, 390), Color.Black);

        _spriteBatch.End();

        if (isMenu == true)
        {
            // draw popup menu
            _spriteBatch.Begin();
            _spriteBatch.Draw(GetButtonTexture(), popupFPDOutline, Color.Black);
            _spriteBatch.Draw(GetButtonTexture(), popupFPD, Color.WhiteSmoke);
            _spriteBatch.Draw(GetButtonTexture(), popupFPDStay, Color.Brown);
            _spriteBatch.Draw(GetButtonTexture(), popupFPDReturn, Color.Brown);
            _spriteBatch.DrawString(font, "Stay", new Vector2(260, 273), Color.Black);
            _spriteBatch.DrawString(font, "Main Menu", new Vector2(460, 273), Color.Black);
            _spriteBatch.DrawString(menuFont, "Return to Main Menu?", new Vector2(260, 150), Color.Black);
            _spriteBatch.End();
        }
    }

    void DrawPause(GameTime gameTime)
    {
        // TODO
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        // TODO: Add your drawing code here

        base.Draw(gameTime);

        switch (_gameState)
        {
            case GameStates.Menu:
                DrawMenu(gameTime);
                break;
            case GameStates.Dutar:
                DrawDutar(gameTime);
                break;
            case GameStates.Info:
                DrawInfo(gameTime);
                break;
            case GameStates.Pause:
                DrawPause(gameTime);
                break;
        }
    }

    // Button setup
    private Texture2D GetButtonTexture()
    {
        Texture2D texture = new Texture2D(GraphicsDevice, 1, 1);
        texture.SetData(new[] { Color.White });

        return texture;
    }
}