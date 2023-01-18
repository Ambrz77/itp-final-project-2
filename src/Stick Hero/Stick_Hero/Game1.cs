using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Stick_Hero_test


{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    ///
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        enum PlayerStates { state1, state2, state3, state4 }  // must be add few more states..
        enum Menu { MainMenu, Game, }


        //-----------------Rectangle-----------------------------///
        Rectangle stick_rect1;
        Rectangle blockBlack_rect1;
        Rectangle blockRed_rec1;
        Rectangle blockBlack_rect2;
        Rectangle blockred_rect2;
        Rectangle bridge_rect;
        Rectangle PlayButton;
        SoundEffect effect;
        SoundEffect Falling_effect;
        SoundEffect Score_effect;
        Random rand = new Random();
        Vector2 orgin;

        //------------------------------------------------------------------------------
        public bool Collison_detection(int X0, int bridge_H, int Xblock, int Wblock)
        {
            if ((X0 + bridge_H >= Xblock) && (X0 + bridge_H <= Xblock + Wblock))
                return true;
            else

                return false;
        }

        public int Random_W()
        {
            return rand.Next(40, 165);
        }

        public int Random_X()
        {
            return rand.Next(130, 500);
        }


        public bool mouseGet(MouseState mouse, Rectangle rect1)
        {
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);
            if (mouseRectangle.X >= rect1.X && mouseRectangle.X <= rect1.X + rect1.Width && mouseRectangle.Y >= rect1.Y && mouseRectangle.Y <= rect1.Y + rect1.Height && mouse.RightButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public void show(bool flag, Rectangle rect, Texture2D texture, SpriteBatch spriteBatch)
        {
            if (flag)
                spriteBatch.Draw(texture, rect, Color.White);

        }
        //-------------------------------------------------------------------------------

        // Rectangle sprite_rect;


        //Vector2 sprite_position;

        //------------------------------------------------------------
        float rotation = 0;
        public MouseState m;
        static int counter_y = 490, counter_high = 0; // this for high....
        int player_x = 50;
        int player_y = 430;
        int bridge_y = 540, platform_w, platform_x, red_x;
        static float rotation_V = 0.1f;
        bool bridge_showing = false, IsCliked = false;
        int player_speed = 7, counterSoundEffect = 0, counterSoundEffect2 = 0;
        PlayerStates stickHero_states = PlayerStates.state1;
        Menu MenuStates = Menu.MainMenu;
        int Speed_bridge = 5;
        bool ISstanding = true, oneTime = true, ISextending = false, ISwalking = false, ISexchanging = false;
        bool detection = false;
        int ctr = 0;
        int BSpeed = 5;
        bool isdone = false, StatesDone = false, InMenu = true;
        bool InGame = false;

        //----------------------------------------------------------

        const short bridge_x = 95;

        //----------------texture loading--------------------------///
        Texture2D stick_man;
        Texture2D block_texture;
        Texture2D red_block;
        Texture2D PlayButtonT;

        //-----------------------------------------------///
        SpriteBatch spriteBatch;


        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 700;

            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            stick_rect1 = new Rectangle(player_x, player_y, 40, 40);
            blockBlack_rect2 = new Rectangle(390, 470, 100, 500);

            //bridge_rect = new Rectangle(150, bridge_y, 5, 0);

            orgin = new Vector2(0, 0);

            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            spriteBatch = new SpriteBatch(GraphicsDevice);

            //-----------------------------------adresing texture-----------------------------

            stick_man= Content.Load<Texture2D>("Stickman");
            block_texture = Content.Load<Texture2D>("Plat");
            red_block = Content.Load<Texture2D>("Redplat");
            effect = Content.Load<SoundEffect>("Wood_falling");
            Falling_effect = Content.Load<SoundEffect>("Falling");
            Score_effect = Content.Load<SoundEffect>("Score");
            PlayButtonT = Content.Load<Texture2D>("Button Play");

            //--------------------------------------------------------------------------------

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {

        }

        MouseState m1;
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            //------------------------------Menu part..............................

            if (MenuStates == Menu.MainMenu && InMenu)
            {
                IsMouseVisible = true;
                PlayButton = new Rectangle(270, 270, 160, 160);
                m = Mouse.GetState();

                if (mouseGet(m, PlayButton))
                {
                    InMenu = false;
                    InGame = true;
                    MenuStates = Menu.Game;
                }

            }

            else if (MenuStates == Menu.Game && InGame && InMenu == false)
            {
                IsMouseVisible = false;

                if (ctr == 1)  //--->>> or boolean type ...
                {
                    platform_w = Random_W();
                    platform_x = Random_X();
                    red_x = platform_w + platform_x - platform_w / 2 - 3;  //---->> formole red...

                    blockred_rect2 = new Rectangle(red_x, 470, 10, 7);
                    blockBlack_rect2 = new Rectangle(platform_x, 470, platform_w, 500);
                }

                if (ctr == 2)
                {
                    platform_w = Random_W();
                    platform_x = Random_X();
                    red_x = platform_w + platform_x - platform_w / 2 - 3;

                }

                if (ctr == 3)
                {
                    platform_w = Random_W();
                    platform_x = Random_X();
                    red_x = platform_w + platform_x - platform_w / 2 - 3;
                }



                if (ctr == 4)
                {
                    platform_w = Random_W();
                    platform_x = Random_X();
                    red_x = platform_w + platform_x - platform_w / 2 - 3;
                }

                isdone = false;
                rotation += rotation_V;

                Console.WriteLine("i am out :||||");

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////State 1 started.
                if (stickHero_states == PlayerStates.state1 && ISextending == false && ISwalking == false && ISexchanging == false)
                {
                    Console.WriteLine("I am at STATE 1 :D!!");
                    //ISstate1 = true;
                    if (oneTime)
                    {
                        blockBlack_rect1 = new Rectangle(0, 470, 100, 500);
                        blockRed_rec1 = new Rectangle(40, 470, 10, 7);
                    }
                    oneTime = false;
                }
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////State 1 finished.
                m = Mouse.GetState();

                if (m1.LeftButton == ButtonState.Pressed)
                {
                    ISstanding = false;
                    IsCliked = true;  // mouse ...
                    ISextending = true;
                    ISexchanging = false;
                    ISwalking = false;
                    stickHero_states = PlayerStates.state2;
                    Console.WriteLine("inja omade injaaaaaaa");
                }


                //else if (IsCliked == true && m.LeftButton == ButtonState.Released)
                //{
                //momkene inja ghalat bashe.00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
                //}
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////State 2 started.
                if (stickHero_states == PlayerStates.state2 && IsCliked && ISstanding == false && ISextending && ISwalking == false && ISexchanging == false) // state 2
                {

                    Console.WriteLine("I am at State 2 :(( !!");

                    counter_y -= Speed_bridge;
                    counter_high += Speed_bridge;
                    bridge_rect = new Rectangle(bridge_x, counter_y, 5, counter_high);

                    if (m1.LeftButton == ButtonState.Pressed && m.LeftButton == ButtonState.Released && IsCliked)
                    {
                        Console.WriteLine("inja mikhad state 3 she");
                        Speed_bridge = 0;
                        ISstanding = false;
                        ISextending = false;
                        ISwalking = true;
                        ISexchanging = false;
                        IsCliked = false;
                        stickHero_states = PlayerStates.state3;

                    }

                    bridge_showing = true;
                }
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////State 2 finished.
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////State 3 started.
                if (stickHero_states == PlayerStates.state3 && ISextending == false && ISstanding == false && IsCliked == false && ISwalking == true && ISexchanging == false) // state3...
                {
                    Console.WriteLine("I am at state 3.........");
                    bridge_showing = false;

                    if (Collison_detection(bridge_rect.X, bridge_rect.Height, blockBlack_rect2.X, blockBlack_rect2.Width))
                    {
                        detection = true;
                        counterSoundEffect++;

                        if (counterSoundEffect == 1)
                            effect.Play();

                        stick_rect1.X += player_speed;
                        if (stick_rect1.X >= blockBlack_rect2.X + blockBlack_rect2.Width - 50)
                        {
                            player_speed = 0;
                            counterSoundEffect2++;

                            if (counterSoundEffect2 == 1)
                                Score_effect.Play();
                        }

                    }

                    else
                    {
                        stick_rect1.X += player_speed;

                        if ((bridge_showing == false) && (stick_rect1.X > bridge_rect.X + bridge_rect.Width - 20))
                        {
                            //stick_rect1.X += -2;
                            player_speed = 5;

                            if (stick_rect1.X >= bridge_rect.X + bridge_rect.Height - 5) // falling
                            {

                                player_speed = 0;
                                stick_rect1.Y += 14;

                                if (stick_rect1.Y == 850)
                                    Falling_effect.Play();

                            }

                        }
                    }
                }
                if (detection)
                    stickHero_states = PlayerStates.state4;

                if (stickHero_states == PlayerStates.state4)
                {

                    stick_rect1.X += player_speed;

                    if (stick_rect1.X >= blockBlack_rect2.X + blockBlack_rect2.Width - 45)
                    {
                        player_speed = 0;
                        blockBlack_rect1.X -= 700;
                        blockRed_rec1.X -= 700;
                        blockBlack_rect2.X -= BSpeed;
                        blockred_rect2.X -= BSpeed;
                        stick_rect1.X -= BSpeed;
                        bridge_rect = new Rectangle(0, 0, 0, 0);


                        if (blockBlack_rect2.X <= 45 && stick_rect1.X <= 45)
                            BSpeed = 0;

                        isdone = true;
                        if (isdone)
                        {
                            blockBlack_rect1 = new Rectangle(platform_x, 470, platform_w, 500);
                            blockRed_rec1 = new Rectangle(red_x, 470, 10, 7);

                        }

                    }

                    StatesDone = true;
                    isdone = false;
                }

                if (StatesDone)
                {
                    ISstanding = true;
                    ISexchanging = false;
                    ISextending = false;
                    ISwalking = false;
                    stickHero_states = PlayerStates.state1;
                    StatesDone = false;
                }

                ctr++;
            }
            m1 = m;
            ///  counter_y = 
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();

            if (MenuStates == Menu.MainMenu && InGame == false && InMenu)
            {

                spriteBatch.Draw(PlayButtonT, PlayButton, Color.White);
            }

            else if (MenuStates == Menu.Game && InGame && InMenu == false)
            {
                Console.WriteLine("inja to draw");
                spriteBatch.Draw(stick_man, stick_rect1, Color.White);
                spriteBatch.Draw(block_texture, blockBlack_rect1, Color.White);
                spriteBatch.Draw(red_block, blockRed_rec1, Color.White);
                spriteBatch.Draw(block_texture, blockBlack_rect2, Color.White);
                spriteBatch.Draw(red_block, blockred_rect2, Color.White);


                if (bridge_showing == true)
                    spriteBatch.Draw(block_texture, bridge_rect, Color.White);

                if (bridge_showing == false)
                {

                    bridge_rect.Y = 470;

                    spriteBatch.Draw(block_texture, bridge_rect, null, Color.White, 4.712389f, orgin, SpriteEffects.None, 0);
                    //if (stick_Animation.isdone)
                    //stick_Animation.Draw(spriteBatch);
                }
            }
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }

    //public void show(bool flag, Rectangle rect, Texture2D texture)
    //{
    //    if (flag)
    //        SpriteBatch.Draw(texture, rect, Color.White);

    //}

}
